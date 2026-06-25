using System;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;

namespace IOTController
{
    class Db
    {
        private readonly string _connectionString;

        public Db()
        {
            _connectionString = ConfigurationManager.AppSettings["Db"] ?? ConfigurationManager.AppSettings["Data"];
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                throw new InvalidOperationException("Missing appSettings key 'Db' (or 'Data') in App.config.");
            }

            EnsureSchema();
        }

        public DataTable GetCustomers()
        {
            return Query("select cId, cName, metno, con_type, units, Email from tblCustomer order by cName");
        }

        public bool TryGetCustomerIdByMeter(string meterNo, out int cid)
        {
            cid = 0;
            if (string.IsNullOrWhiteSpace(meterNo))
            {
                return false;
            }

            DataTable tab = Query(
                "select cId from tblCustomer where metno = @metno limit 1",
                new MySqlParameter("@metno", meterNo.Trim()));

            if (tab.Rows.Count == 0)
            {
                return false;
            }

            return int.TryParse(tab.Rows[0]["cId"].ToString(), out cid);
        }

        public bool TryGetMeterNoByCustomerId(int cid, out string meterNo)
        {
            meterNo = null;
            DataTable tab = Query(
                "select metno from tblCustomer where cId = @cid limit 1",
                new MySqlParameter("@cid", cid));

            if (tab.Rows.Count == 0)
            {
                return false;
            }

            string m = tab.Rows[0]["metno"].ToString();
            if (string.IsNullOrWhiteSpace(m))
            {
                return false;
            }

            meterNo = m.Trim();
            return true;
        }

        public void InsertReading(int cid, double value)
        {
            try
            {
                ExecuteNonQuery(
                    "insert into readings(cid, data) values (@cid, @value)",
                    new MySqlParameter("@cid", cid),
                    new MySqlParameter("@value", value));
                Logger.Info(string.Format("DB insert success: readings(cid={0}, data={1:0.###})", cid, value));
            }
            catch (Exception ex)
            {
                Logger.Error("InsertReading failed", ex);
            }
        }

        public void UpdateData(int cid, double eval)
        {
            if (eval == 0) return;
            try
            {
                ExecuteNonQuery(
                    "update tblCustomer set units = ifnull(units,0) + @eval where cId = @cid",
                    new MySqlParameter("@eval", eval),
                    new MySqlParameter("@cid", cid));

                ExecuteNonQuery(
                    @"delete from readings 
                      where cid = @cid and id not in (
                        select id from (
                          select id from readings where cid = @cid order by created_at desc limit 100
                        ) x
                      )",
                    new MySqlParameter("@cid", cid));
            }
            catch (Exception ex)
            {
                Logger.Error("UpdateData failed", ex);
            }
        }

        public bool GenerateBill(int cid)
        {
            try
            {
                DataTable rates = Query("select con_type, cost from tblRates");
                DataTable custBuffer = Query(
                    "select * from tblCustomer where cId = @cid",
                    new MySqlParameter("@cid", cid));

                if (custBuffer.Rows.Count == 0)
                {
                    return false;
                }

                DataRow row = custBuffer.Rows[0];
                string metno = row["metno"].ToString();
                string name = row["cName"].ToString();
                string contype = row["con_type"].ToString().ToLowerInvariant();
                string email = row["Email"].ToString();
                double units = Convert.ToDouble(row["units"]);

                if (units <= 0)
                {
                    // Demo mode: allow bill generation even when live units are zero.
                    units = 1.0;
                    Logger.Info("GenerateBill: customer had zero units, applied demo minimum units = 1.0");
                }

                double rate = ResolveRate(rates, contype);
                double total = units * rate;

                ExecuteNonQuery(
                    "insert into tblBill(metno, amount, status, BillDate) values (@metno, @amount, 'N', NOW())",
                    new MySqlParameter("@metno", metno),
                    new MySqlParameter("@amount", total));

                ExecuteNonQuery(
                    "update tblCustomer set units = 0 where cId = @cid",
                    new MySqlParameter("@cid", cid));

                RapidMailSender.Send(
                    email,
                    metno,
                    total.ToString("0.00"),
                    units.ToString("0.000"),
                    name,
                    contype);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("GenerateBill failed", ex);
                return false;
            }
        }

        public bool DisconnectService(int cid)
        {
            try
            {
                int disconnectMinutes;
                if (!int.TryParse(ConfigurationManager.AppSettings["BillDueDisconnectMinutes"], out disconnectMinutes))
                {
                    disconnectMinutes = 2;
                }

                DataTable buffer = Query(
                    "select b.* from tblBill b join tblCustomer c on b.metno = c.metno " +
                    "where c.cId = @cid and b.status = 'N' and TIMESTAMPDIFF(MINUTE, b.BillDate, NOW()) >= @mins",
                    new MySqlParameter("@cid", cid),
                    new MySqlParameter("@mins", disconnectMinutes));
                return buffer.Rows.Count > 0;
            }
            catch (Exception ex)
            {
                Logger.Error("DisconnectService failed", ex);
                return false;
            }
        }

        private void EnsureSchema()
        {
            try
            {
                ExecuteNonQuery("ALTER TABLE tblBill ADD COLUMN BillDate TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP");
            }
            catch
            {
                // ignore if column exists
            }

            EnsureTableRates();
            EnsureTableReadings();
            EnsureMeterMappings();

            DataTable count = Query("select count(*) as cnt from tblRates");
            if (count.Rows.Count > 0 && count.Rows[0]["cnt"].ToString() == "0")
            {
                double d = ReadDoubleSetting("BillRateDomestic", 1.0);
                double c = ReadDoubleSetting("BillRateCommercial", 2.0);
                double i = ReadDoubleSetting("BillRateIndustrial", 3.0);
                ExecuteNonQuery(
                    "insert into tblRates(con_type, cost) values ('domestic', @d), ('commercial', @c), ('industrial', @i) " +
                    "on duplicate key update cost = values(cost)",
                    new MySqlParameter("@d", d),
                    new MySqlParameter("@c", c),
                    new MySqlParameter("@i", i));
            }
        }

        private void EnsureMeterMappings()
        {
            // Keep legacy tblCustomer.metno and normalized Meters.MeterId aligned.
            ExecuteNonQuery(
                "INSERT INTO Meters (MeterId, UserId, DeviceKey, SharedSecret, IsActive) " +
                "SELECT c.metno, u.UserId, CONCAT('dev-', c.metno), LOWER(SHA2(CONCAT('secret-', c.metno), 256)), 1 " +
                "FROM tblCustomer c " +
                "LEFT JOIN Users u ON u.LegacyCustomerId = c.cId " +
                "LEFT JOIN Meters m ON m.MeterId = c.metno " +
                "WHERE m.MeterPk IS NULL");
        }

        private void EnsureTableRates()
        {
            if (TableExists("tblRates"))
            {
                return;
            }

            ExecuteNonQuery("CREATE TABLE tblRates (con_type VARCHAR(50) NOT NULL PRIMARY KEY, cost DOUBLE NOT NULL)");
        }

        private void EnsureTableReadings()
        {
            if (TableExists("readings"))
            {
                return;
            }

            // Keep startup schema creation minimal to avoid driver-specific DDL issues.
            ExecuteNonQuery(
                "CREATE TABLE readings (" +
                "id INT AUTO_INCREMENT PRIMARY KEY, " +
                "cid INT NOT NULL, " +
                "data DOUBLE NOT NULL, " +
                "created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP, " +
                "INDEX idx_readings_cid (cid), " +
                "INDEX idx_readings_created (created_at)" +
                ")");
        }

        private bool TableExists(string tableName)
        {
            DataTable tab = Query(
                "SELECT COUNT(*) AS cnt FROM information_schema.tables WHERE table_schema = DATABASE() AND table_name = @name",
                new MySqlParameter("@name", tableName));

            if (tab.Rows.Count == 0)
            {
                return false;
            }

            int cnt;
            return int.TryParse(tab.Rows[0]["cnt"].ToString(), out cnt) && cnt > 0;
        }

        private double ResolveRate(DataTable rates, string contype)
        {
            foreach (DataRow row in rates.Rows)
            {
                if (string.Equals(row["con_type"].ToString(), contype, StringComparison.OrdinalIgnoreCase))
                {
                    return Convert.ToDouble(row["cost"]);
                }
            }
            if (contype == "industrial") return ReadDoubleSetting("BillRateIndustrial", 3.0);
            if (contype == "commercial") return ReadDoubleSetting("BillRateCommercial", 2.0);
            return ReadDoubleSetting("BillRateDomestic", 1.0);
        }

        private double ReadDoubleSetting(string key, double fallback)
        {
            double val;
            return double.TryParse(ConfigurationManager.AppSettings[key], out val) ? val : fallback;
        }

        private DataTable Query(string query, params MySqlParameter[] parameters)
        {
            DataTable tab = new DataTable();
            using (var con = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand(query, con))
            using (var da = new MySqlDataAdapter(cmd))
            {
                if (parameters != null && parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                con.Open();
                da.Fill(tab);
            }
            return tab;
        }

        private void ExecuteNonQuery(string query, params MySqlParameter[] parameters)
        {
            using (var con = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand(query, con))
            {
                if (parameters != null && parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
