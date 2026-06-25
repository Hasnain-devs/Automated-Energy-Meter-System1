using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace electricity
{
    public partial class pay : System.Web.UI.Page
    {
        common obj = new common();
        private const string ControlMeterNo = "7700";

        private bool TryGetCustomerId(out int id)
        {
            id = 0;
            object sessionId = Session["Id"];
            return sessionId != null && int.TryParse(sessionId.ToString(), out id);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int id;
                if (!TryGetCustomerId(out id))
                {
                    Response.Redirect("CustomorLog.aspx");
                    return;
                }

                BindUnpaidBills(id);
                LblSelected.Text = "Pay All mode";
            }
        }

        private void BindUnpaidBills(int customerId)
        {
            string sql = string.Format(
                "select Id, metno, amount, status, BillDate from tblBill " +
                "where status='N' and metno=(select metno from tblCustomer where cId={0}) " +
                "order by BillDate desc", customerId);

            DataTable tab = obj.nontransq(sql);
            GridBills.DataSource = tab;
            GridBills.DataBind();

            if (tab == null || tab.Rows.Count == 0)
            {
                LblMsg.Text = "No unpaid bills found.";
                LblMsg.ForeColor = System.Drawing.Color.Blue;
                BtnPaySelected.Enabled = false;
            }
            else
            {
                LblMsg.Text = "";
                BtnPaySelected.Enabled = true;
            }
        }

        protected void GridBills_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Kept only to preserve page event wiring. Pay flow is "Pay All".
            LblSelected.Text = "Pay All mode";
        }

        protected void BtnPaySelected_Click(object sender, EventArgs e)
        {
            int customerId;
            if (!TryGetCustomerId(out customerId))
            {
                Response.Redirect("CustomorLog.aspx");
                return;
            }

            string paidDate = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            string metSql = string.Format("select metno from tblCustomer where cId={0}", customerId);
            DataTable mtab = obj.nontransq(metSql);
            if (mtab == null || mtab.Rows.Count == 0)
            {
                LblMsg.Text = "Customer meter not found.";
                LblMsg.ForeColor = System.Drawing.Color.Red;
                return;
            }
            string metno = mtab.Rows[0][0].ToString().Replace("'", "''");

            string update = string.Format(
                "update tblBill set status='Y', PaidDate='{0}' " +
                "where metno='{1}' and status='N'",
                paidDate, metno);

            int rows = obj.transq(update);

            if (rows > 0)
            {
                TryPublishImmediateReconnect();
                LblMsg.Text = "Payment successful. Bill marked as PAID.";
                LblMsg.ForeColor = System.Drawing.Color.Green;
                BtnPaySelected.Enabled = false;
                LblSelected.Text = "No bill selected";
                BindUnpaidBills(customerId);
            }
            else
            {
                LblMsg.Text = "Payment not applied. Bill may already be paid.";
                LblMsg.ForeColor = System.Drawing.Color.OrangeRed;
            }
        }

        protected void BtnBackBills_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Bills.aspx");
        }

        private void TryPublishImmediateReconnect()
        {
            string primary = ConfigurationManager.AppSettings["MqttServer"] ?? "broker.emqx.io";
            string fallback = ConfigurationManager.AppSettings["MqttServerFallbackIp"] ?? "35.172.255.228";
            int port;
            if (!int.TryParse(ConfigurationManager.AppSettings["MqttPort"], out port))
            {
                port = 1883;
            }

            string topic = "energy/control/" + ControlMeterNo;
            byte[] payload = Encoding.UTF8.GetBytes("0");
            string[] hosts = new[] { primary, fallback };

            foreach (string h in hosts)
            {
                if (string.IsNullOrWhiteSpace(h)) continue;
                try
                {
                    MqttClient c = new MqttClient(h.Trim(), port, false, null);
                    string cid = "WebPay-" + Guid.NewGuid().ToString("N").Substring(0, 10);
                    c.Connect(cid);
                    c.Publish(topic, payload, MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
                    c.Disconnect();
                    LblMsg.Text += " Power restore command sent.";
                    return;
                }
                catch
                {
                    // Best effort only; controller auto-loop will still send reconnect.
                }
            }
        }
    }
}
