using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MySql.Data.MySqlClient;

namespace electricity
{
    public partial class add : System.Web.UI.Page
    {
        private static string SqlSafe(string value)
        {
            return (value ?? string.Empty).Replace("'", "''").Trim();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            common obj = new common();
            if (this.IsPostBack == false)
            {
                int id;
                if (int.TryParse(Convert.ToString(Request.QueryString["Id"]), out id))
                {
                    string query1 = "select*from tblCustomer where cId=" + id;
                    DataTable tab = new DataTable();
                    tab = obj.nontransq(query1);
                    if (tab != null && tab.Rows.Count > 0)
                    {
                        txtmet.Text = tab.Rows[0]["metno"].ToString();
                        txtname.Text = tab.Rows[0]["cName"].ToString();
                        txtadress.Text = tab.Rows[0]["Address"].ToString();
                        txtemail.Text = tab.Rows[0]["Email"].ToString();
                        txtphone.Text = tab.Rows[0]["phone"].ToString();
                        string item = tab.Rows[0]["con_type"].ToString();
                        ListItem l = new ListItem(item);
                        int index = ddlcon.Items.IndexOf(l);
                        ddlcon.SelectedIndex = index;


                    }
                    btnSubmit.Text = "update";
                }
            }
        }

        public void clear()
        {
            txtmet.Text = "";
            txtname.Text = "";
            txtadress.Text = "";
            txtemail.Text = "";
            txtphone.Text = "";
            ddlcon.SelectedIndex = 0;
            

        }
        public void emailSending(string pwd)
        {
            try
            {
            SendEmail_SMTP.SMTPEmailSend objemail = new SendEmail_SMTP.SMTPEmailSend();
            objemail.sendEmail("mailserviceforproject@gmail.com",txtemail.Text,"Unlock2018","Password","Your Meter Number is = "+txtmet.Text+"\nYour Paasword is = "+pwd);
            
            }
            catch
            {

            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            common obj = new common();

            if (btnSubmit.Text != "update")
            {
                string met = SqlSafe(txtmet.Text);
                string name = SqlSafe(txtname.Text);
                string address = SqlSafe(txtadress.Text);
                string email = SqlSafe(txtemail.Text);
                string phone = SqlSafe(txtphone.Text);
                string con = SqlSafe(ddlcon.SelectedValue);
                if (con == "-select-")
                {
                    Response.Write("<script>alert('Please select connection type')</script>");
                    return;
                }
                Random r = new Random();
                int pwd = r.Next(1000, 9999);
                string query = string.Format("insert into tblCustomer (cName,metno,Email,Address,phone,con_type,password) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", name, met, email, address, phone, con, pwd);
                try
                {
                    int result = obj.transq(query);
                    if (result > 0)
                    {
                        emailSending(pwd.ToString());
                        Response.Write("<script>alert('Record inserted and password is " + pwd + "')</script>");
                    }
                    else
                    {
                        Response.Write("<script>alert('Failed to insert')</script>");
                    }
                }
                catch (MySqlException ex)
                {
                    if (ex.Message.IndexOf("Duplicate entry", StringComparison.OrdinalIgnoreCase) >= 0 &&
                        ex.Message.IndexOf("Email", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        Response.Write("<script>alert('Email already exists')</script>");
                    }
                    else if (ex.Message.IndexOf("Duplicate entry", StringComparison.OrdinalIgnoreCase) >= 0 &&
                             (ex.Message.IndexOf("metno", StringComparison.OrdinalIgnoreCase) >= 0 ||
                              ex.Message.IndexOf("Meter", StringComparison.OrdinalIgnoreCase) >= 0))
                    {
                        Response.Write("<script>alert('Meter Number Duplication')</script>");
                    }
                    else
                    {
                        Response.Write("<script>alert('Insert failed')</script>");
                    }
                }
            }

            else
            {
                string conn = SqlSafe(ddlcon.SelectedValue);
                if (conn == "-select-")
                {
                    Response.Write("<script>alert('Please select connection type')</script>");
                    return;
                }

                string emailDupQuery = string.Format(
                    "select count(*) from tblCustomer where Email='{0}' and cId<>{1}",
                    SqlSafe(txtemail.Text),
                    SqlSafe(Request.QueryString["Id"]));
                double emailDup = obj.aggregate(emailDupQuery);
                if (emailDup > 0)
                {
                    Response.Write("<script>alert('Email already exists')</script>");
                    return;
                }

                string query12 = string.Format(
                    "update tblCustomer set metno='{0}' , cName='{1}', Address='{2}', Email='{3}', phone='{4}', con_type='{5}' where cid={6}",
                    SqlSafe(txtmet.Text),
                    SqlSafe(txtname.Text),
                    SqlSafe(txtadress.Text),
                    SqlSafe(txtemail.Text),
                    SqlSafe(txtphone.Text),
                    conn,
                    SqlSafe(Request.QueryString["Id"]));
                try
                {
                    int result = obj.transq(query12);
                    if (result > 0)
                    {
                        Response.Write("<script>alert('updated Successfully')</script>");
                        Response.Redirect("view.aspx");
                        clear();
                    }
                    else
                    {
                        Response.Write("<script>alert('eRROR IN UPDATION')</script>");
                        clear();
                    }
                }
                catch (MySqlException ex)
                {
                    if (ex.Message.IndexOf("Duplicate entry", StringComparison.OrdinalIgnoreCase) >= 0 &&
                        ex.Message.IndexOf("Email", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        Response.Write("<script>alert('Email already exists')</script>");
                    }
                    else if (ex.Message.IndexOf("Duplicate entry", StringComparison.OrdinalIgnoreCase) >= 0 &&
                             (ex.Message.IndexOf("metno", StringComparison.OrdinalIgnoreCase) >= 0 ||
                              ex.Message.IndexOf("Meter", StringComparison.OrdinalIgnoreCase) >= 0))
                    {
                        Response.Write("<script>alert('Meter Number Duplication')</script>");
                    }
                    else
                    {
                        Response.Write("<script>alert('Update failed')</script>");
                    }
                }

            }

        }
    }

}
