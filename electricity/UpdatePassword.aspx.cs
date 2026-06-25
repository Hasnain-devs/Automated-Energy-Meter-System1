using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;                       
using System.Data;
using System.Web.Mail;

namespace electricity
{
    public partial class UpdatePassword : System.Web.UI.Page
    {
        common obj;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtoldpwd0.Text = txtnewpwd.Text = txtconfirm.Text = "";
            }
        }

        protected void lnkForgotPassword_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["Email"] == null)
                {
                    Response.Redirect("CustomorLog.aspx");
                    return;
                }
                SendEmail_SMTP.SMTPEmailSend objEmail = new SendEmail_SMTP.SMTPEmailSend();
               
                DataTable tab = new DataTable();
                obj = new common();
                string email = Session["Email"].ToString().Replace("'", "''");
                tab = obj.nontransq("select password from tblCustomer where Email='" + email + "'");
                if (tab == null || tab.Rows.Count == 0)
                {
                    Response.Write("<Script>alert('Email not found')</script>");
                    return;
                }
                string pwd = tab.Rows[0][0].ToString();
                objEmail.sendEmail("mailserviceforproject@gmail.com", Session["Email"].ToString(), "Unlock2018", "Password Recovery","["+ pwd + "] Use this Password To Recover Your Password  ");
                
                
                Response.Write("<Script>alert('Use The Password sent in your Email To Reset Password')</script>");
            }
            catch
            {
                Response.Write("<Script>alert('Something Went Wrong')</script>"); 
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["Email"] == null)
                {
                    Response.Redirect("CustomorLog.aspx");
                    return;
                }
                string email = Session["Email"].ToString();
                obj = new common();
                if (obj.aggregate("select count(*) from tblCustomer where Email='"+ email.Replace("'", "''") +"' and password='" + txtoldpwd0.Text.Replace("'", "''") + "'") == 1)
                {
                    int res = obj.transq("update tblCustomer set password='" + txtnewpwd.Text.Replace("'", "''") + "' where Email='" + email.Replace("'", "''") + "'");
                    if (res > 0)
                    { 
                        Response.Write("<script>alert('Password Updated Successfully')</script>");
                    }
                    else
                    {
                        Response.Write("<script>alert('Error in Password Updation')</script>");
                    }
                }
                else
                {
                    msg.Text = "Old password doesn't match please enter the correct old password";
                    msg.ForeColor = System.Drawing.Color.Red;

                }
            }
            catch
            {
                Response.Write("<script>alert('Error in Password Updation')</script>");
            }
        }
    }
}
