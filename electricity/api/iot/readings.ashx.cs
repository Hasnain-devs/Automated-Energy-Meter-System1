using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using MySql.Data.MySqlClient;

namespace electricity.api.iot
{
    public class readings : IHttpHandler
    {
        private sealed class IotReadingRequest
        {
            public string meterId { get; set; }
            public decimal unitsDelta { get; set; }
            public string readingAtUtc { get; set; }
            public long sequenceNo { get; set; }
        }

        public bool IsReusable { get { return false; } }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            if (!string.Equals(context.Request.HttpMethod, "POST", StringComparison.OrdinalIgnoreCase))
            {
                WriteJson(context, 405, "method_not_allowed", "Use POST.");
                return;
            }

            string body;
            using (var reader = new StreamReader(context.Request.InputStream))
            {
                body = reader.ReadToEnd();
            }

            if (string.IsNullOrWhiteSpace(body))
            {
                WriteJson(context, 400, "invalid_payload", "Request body is empty.");
                return;
            }

            IotReadingRequest req;
            try
            {
                req = new JavaScriptSerializer().Deserialize<IotReadingRequest>(body);
            }
            catch
            {
                WriteJson(context, 400, "invalid_json", "Body must be valid JSON.");
                return;
            }

            if (req == null || string.IsNullOrWhiteSpace(req.meterId) || string.IsNullOrWhiteSpace(req.readingAtUtc) || req.sequenceNo <= 0)
            {
                WriteJson(context, 400, "invalid_payload", "meterId, readingAtUtc and sequenceNo are required.");
                return;
            }

            if (req.unitsDelta <= 0)
            {
                WriteJson(context, 400, "invalid_payload", "unitsDelta must be greater than zero.");
                return;
            }

            DateTime readingAtUtc;
            if (!DateTime.TryParse(req.readingAtUtc, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out readingAtUtc))
            {
                WriteJson(context, 400, "invalid_payload", "readingAtUtc must be ISO-8601 UTC.");
                return;
            }

            var nowUtc = DateTime.UtcNow;
            if (readingAtUtc > nowUtc.AddMinutes(5) || readingAtUtc < nowUtc.AddDays(-7))
            {
                WriteJson(context, 400, "invalid_payload", "readingAtUtc is outside accepted range.");
                return;
            }

            string deviceKey = context.Request.Headers["X-Device-Key"];
            string signature = context.Request.Headers["X-Signature"];
            if (string.IsNullOrWhiteSpace(deviceKey) || string.IsNullOrWhiteSpace(signature))
            {
                WriteJson(context, 403, "unauthorized", "Missing X-Device-Key or X-Signature header.");
                return;
            }

            string connStr = ConfigurationManager.AppSettings["Data"] ?? ConfigurationManager.AppSettings["data"];
            if (string.IsNullOrWhiteSpace(connStr))
            {
                WriteJson(context, 500, "config_error", "Missing database connection in appSettings key Data.");
                return;
            }

            try
            {
                using (var con = new MySqlConnection(connStr))
                {
                    con.Open();

                    int meterPk;
                    string sharedSecret;
                    if (!TryGetMeterCredentials(con, req.meterId.Trim(), deviceKey.Trim(), out meterPk, out sharedSecret))
                    {
                        WriteJson(context, 403, "unauthorized", "Unknown meter/device credentials.");
                        return;
                    }

                    if (!IsValidSignature(body, sharedSecret, signature.Trim()))
                    {
                        WriteJson(context, 403, "unauthorized", "Invalid signature.");
                        return;
                    }

                    bool inserted = InsertReading(con, meterPk, req.sequenceNo, req.unitsDelta, readingAtUtc, body, signature.Trim());
                    UpdateMeterLastSeen(con, meterPk);
                    if (inserted)
                    {
                        MirrorToLegacyBillingTables(con, req.meterId.Trim(), req.unitsDelta);
                    }

                    if (!inserted)
                    {
                        WriteJson(context, 200, "duplicate", "Reading already processed.");
                        return;
                    }

                    WriteJson(context, 202, "accepted", "Reading accepted.");
                }
            }
            catch (Exception ex)
            {
                WriteJson(context, 500, "server_error", ex.Message);
            }
        }

        private static bool TryGetMeterCredentials(MySqlConnection con, string meterId, string deviceKey, out int meterPk, out string sharedSecret)
        {
            meterPk = 0;
            sharedSecret = null;

            const string sql = @"SELECT MeterPk, SharedSecret
                                 FROM Meters
                                 WHERE MeterId = @meterId AND DeviceKey = @deviceKey AND IsActive = 1
                                 LIMIT 1";

            using (var cmd = new MySqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@meterId", meterId);
                cmd.Parameters.AddWithValue("@deviceKey", deviceKey);

                using (var rd = cmd.ExecuteReader())
                {
                    if (!rd.Read())
                    {
                        return false;
                    }

                    meterPk = rd.GetInt32("MeterPk");
                    sharedSecret = rd.GetString("SharedSecret");
                    return true;
                }
            }
        }

        private static bool InsertReading(MySqlConnection con, int meterPk, long sequenceNo, decimal unitsDelta, DateTime readingAtUtc, string rawPayload, string signature)
        {
            const string sql = @"INSERT INTO MeterReadings
                                (MeterPk, SequenceNo, UnitsDelta, ReadingAtUtc, ReceivedAtUtc, RawPayload, Signature)
                                VALUES
                                (@meterPk, @sequenceNo, @unitsDelta, @readingAtUtc, UTC_TIMESTAMP(), @rawPayload, @signature)";

            try
            {
                using (var cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@meterPk", meterPk);
                    cmd.Parameters.AddWithValue("@sequenceNo", sequenceNo);
                    cmd.Parameters.AddWithValue("@unitsDelta", unitsDelta);
                    cmd.Parameters.AddWithValue("@readingAtUtc", readingAtUtc);
                    cmd.Parameters.AddWithValue("@rawPayload", rawPayload);
                    cmd.Parameters.AddWithValue("@signature", signature);
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062)
                {
                    return false;
                }
                throw;
            }
        }

        private static void UpdateMeterLastSeen(MySqlConnection con, int meterPk)
        {
            const string sql = "UPDATE Meters SET LastSeenAtUtc = UTC_TIMESTAMP() WHERE MeterPk = @meterPk";
            using (var cmd = new MySqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@meterPk", meterPk);
                cmd.ExecuteNonQuery();
            }
        }

        private static void MirrorToLegacyBillingTables(MySqlConnection con, string meterId, decimal unitsDelta)
        {
            int customerId;
            if (!TryGetLegacyCustomerId(con, meterId, out customerId))
            {
                return;
            }

            const string insertLegacyReading = "INSERT INTO readings(cid, data) VALUES(@cid, @data)";
            using (var cmd = new MySqlCommand(insertLegacyReading, con))
            {
                cmd.Parameters.AddWithValue("@cid", customerId);
                cmd.Parameters.AddWithValue("@data", unitsDelta);
                cmd.ExecuteNonQuery();
            }

            const string updateUnits = "UPDATE tblCustomer SET units = IFNULL(units,0) + @delta WHERE cId = @cid";
            using (var cmd = new MySqlCommand(updateUnits, con))
            {
                cmd.Parameters.AddWithValue("@delta", unitsDelta);
                cmd.Parameters.AddWithValue("@cid", customerId);
                cmd.ExecuteNonQuery();
            }
        }

        private static bool TryGetLegacyCustomerId(MySqlConnection con, string meterId, out int customerId)
        {
            customerId = 0;
            const string sql = "SELECT cId FROM tblCustomer WHERE metno = @meterId LIMIT 1";
            using (var cmd = new MySqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@meterId", meterId);
                object value = cmd.ExecuteScalar();
                if (value == null || value == DBNull.Value)
                {
                    return false;
                }

                int parsed;
                if (!int.TryParse(value.ToString(), out parsed))
                {
                    return false;
                }

                customerId = parsed;
                return true;
            }
        }

        private static bool IsValidSignature(string body, string sharedSecret, string receivedSignature)
        {
            if (string.IsNullOrWhiteSpace(sharedSecret))
            {
                return false;
            }

            string expected = ComputeHmacHex(body, sharedSecret);
            return ConstantTimeEquals(expected, receivedSignature.ToLowerInvariant());
        }

        private static string ComputeHmacHex(string message, string secret)
        {
            var key = Encoding.UTF8.GetBytes(secret);
            var msg = Encoding.UTF8.GetBytes(message);
            using (var h = new HMACSHA256(key))
            {
                byte[] hash = h.ComputeHash(msg);
                var sb = new StringBuilder(hash.Length * 2);
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        private static bool ConstantTimeEquals(string a, string b)
        {
            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }

            int diff = 0;
            for (int i = 0; i < a.Length; i++)
            {
                diff |= a[i] ^ b[i];
            }
            return diff == 0;
        }

        private static void WriteJson(HttpContext context, int statusCode, string code, string message)
        {
            context.Response.StatusCode = statusCode;
            context.Response.TrySkipIisCustomErrors = true;
            var suppressRedirectProp = context.Response.GetType().GetProperty("SuppressFormsAuthenticationRedirect");
            if (suppressRedirectProp != null && suppressRedirectProp.CanWrite)
            {
                suppressRedirectProp.SetValue(context.Response, true, null);
            }
            var payload = new
            {
                status = code,
                message = message,
                serverUtc = DateTime.UtcNow.ToString("o")
            };
            context.Response.Write(new JavaScriptSerializer().Serialize(payload));
        }
    }
}
