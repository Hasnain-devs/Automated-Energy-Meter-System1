using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace electricity
{
    public partial class AdminLogin : System.Web.UI.MasterPage
    {
        common obj = new common();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                TextBox1.Text = TextBox2.Text = "";
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (TextBox1.Text == "Admin" && TextBox2.Text == "123")
            {
                Response.Redirect("AdminWeb.aspx");
            }
            else
            {
                Response.Write("<script>alert('Error Id/Password Incorrect')</script>");
            }
        }

        protected void Button12_Click(object sender, EventArgs e)
        {
            string email = TextBox1.Text;
            string pass = TextBox2.Text;
            double check = obj.aggregate(string.Format("select count(*) from tblCustomer where Email='{0}' and password='{1}'",email,pass));
            if (check == 1)
            {
                Response.Redirect("Customer.aspx");
            }
            else
            {
                Response.Write("<script>alert('Error Email/Password Incorrect')</script>");
 
            }
        }
    }
}
