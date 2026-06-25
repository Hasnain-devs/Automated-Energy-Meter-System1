using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace electricity
{
    public partial class customerLogin : System.Web.UI.MasterPage
    {
        common obj = new common();
        private static string SqlSafe(string input)
        {
            return (input ?? string.Empty).Replace("'", "''").Trim();
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string email = SqlSafe(TextBox1.Text);
            string pass = SqlSafe(TextBox2.Text);
            double check = obj.aggregate(string.Format("select count(*) from tblCustomer where Email='{0}' and password='{1}'", email, pass));
            if (check == 1)
            {
                Session["Email"] = email;
                Response.Redirect("CustomerWeb.aspx");
            }
            else
            {
                Response.Write("<script>alert('Error Email/Password Incorrect')</script>");
 
            }
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            
        }

        protected void btnForGot_Click(object sender, EventArgs e)
        {
            try
            {
                SendEmail_SMTP.SMTPEmailSend objEmail = new SendEmail_SMTP.SMTPEmailSend();

                DataTable tab = new DataTable();
                obj = new common();
                string email = SqlSafe(TextBox1.Text);
                tab = obj.nontransq("select password from tblCustomer where Email='" + email + "'");
                if (tab == null || tab.Rows.Count == 0)
                {
                    Response.Write("<Script>alert('Email not found')</script>");
                    return;
                }
                string pwd = tab.Rows[0][0].ToString();
                objEmail.sendEmail("mailserviceforproject@gmail.com", email, "Unlock2018", "Password Recovery", "[" + pwd + "] Use this Password To Recover Your Password  ");


                Response.Write("<Script>alert('Use The Password sent to your mail For Loging in ')</script>");
            }
            catch
            {
                Response.Write("<Script>alert('Something Went Wrong')</script>");
            }
        }

      

       


    }
}
