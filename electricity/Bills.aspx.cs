using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace electricity
{
    public partial class Bills : System.Web.UI.Page
    {
        common obj = new common();

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
                loaddata();
            }
        }
        public void LoadHistory()
        {
            try
            {
                DataTable tab = new DataTable();
                int id;
                if (!TryGetCustomerId(out id))
                {
                    Response.Redirect("CustomorLog.aspx");
                    return;
                }
                String sql = string.Format("Select * from tblBill where status = 'Y' and metno=(select metno from tblCustomer where cId = {0} )", id);
                tab = obj.nontransq(sql);
                if (tab != null && tab.Rows.Count > 0)
                {
                    GridView2.DataSource = tab;
                    GridView2.DataBind();
                }
                else
                {
                    GridView2.Controls.Clear();
                    lblmsg0.Text = "You Don't Have Any Payment History";
                    lblmsg0.ForeColor = System.Drawing.Color.Red;
                    lblmsg0.Font.Bold = true;
                }
            }
            catch
            {
                lblmsg0.Text = "Unable to load payment history";
                lblmsg0.ForeColor = System.Drawing.Color.Red;
                lblmsg0.Font.Bold = true;
            }
        }
        public void loaddata()
        {
            DataTable tab = new DataTable();
            int id;
            if (!TryGetCustomerId(out id))
            {
                Response.Redirect("CustomorLog.aspx");
                return;
            }
            string sql = string.Format("select * from tblBill where status='N' and metno = (select metno from tblCustomer where cId = {0})", id);
            double sum = 0;
            string s = string.Format("select sum(amount) from tblBill where status='N' and metno in (select metno from  tblCustomer where cId = {0})", id);

            tab = obj.nontransq(sql);
            if (tab != null && tab.Rows.Count > 0)
            {
                btnSubmit.Visible = true;
                btnSubmit.Enabled = true;
                sum = obj.aggregate(s);
                GridView1.DataSource = tab;
                GridView1.DataBind();
                lblmsg.Text = string.Format("THE TOTAL AMOUNT TO BE PAID IS {0:f4}/-",sum);
                lblmsg.ForeColor = System.Drawing.Color.Blue;
                lblmsg.Font.Bold = true;

            }
            else
            {
                btnSubmit.Visible = true;
                btnSubmit.Enabled = false;
                lblmsg.Text = "Yo have no unpaied Bills";
                lblmsg.ForeColor = System.Drawing.Color.Red;
                lblmsg.Font.Bold = true;
            }

        }

        protected void lnkpay_Click(object sender, EventArgs e)
        {
            //LinkButton lnk = (LinkButton)sender;
            //int id = int.Parse(lnk.CommandArgument);
            //Response.Redirect("~/pay.aspx");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/pay.aspx");
        }

        protected void btnhistory_Click(object sender, EventArgs e)
        {
            LoadHistory();
        }

        public void loadUnits()
        {
            try
            {
                GridView1.Controls.Clear();
                GridView2.Controls.Clear();
                int id;
                if (!TryGetCustomerId(out id))
                {
                    Response.Redirect("CustomorLog.aspx");
                    return;
                }
                string sql = "Select units from tblCustomer where cId=" + id;
                DataTable tab =new DataTable();
                tab=obj.nontransq(sql);
                if (tab != null && tab.Rows.Count > 0)
                {
                    lblmsg.Text = "The Total units Consumed till now is " + tab.Rows[0][0].ToString();
                }
                else
                {
                    lblmsg.Text = "No consumption data found";
                }
                lblmsg.ForeColor = System.Drawing.Color.Blue;
                lblmsg.Font.Bold = true;
            }
            catch
            {
                lblmsg.Text = "Unable to load consumption data";
                lblmsg.ForeColor = System.Drawing.Color.Red;
                lblmsg.Font.Bold = true;
            }
        }

        protected void btnConsuption_Click(object sender, EventArgs e)
        {
            loadUnits();
        }

       
    }
}





