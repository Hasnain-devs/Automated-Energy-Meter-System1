using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace electricity
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RadioButtonList1.SelectedItem.Text == "ADMIN")
            {
                Response.Redirect("AdminLog.aspx");
            }
            else if (RadioButtonList1.SelectedItem.Text == "CUSTOMER")
            {
                Response.Redirect("CustomorLog.aspx");
            }

        }
    }
}