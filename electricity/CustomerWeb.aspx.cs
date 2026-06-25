using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace electricity
{
    public partial class CustomerWeb : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["Email"] == null)
                {
                    Response.Redirect("CustomorLog.aspx");
                    return;
                }
                common obj = new common();
                string email = Session["Email"].ToString().Replace("'", "''");
                string sql = "select cId,cName from tblCustomer where Email='" + email + "'";
                DataTable tab = new DataTable();
                tab = obj.nontransq(sql);
                if (tab == null || tab.Rows.Count == 0)
                {
                    Response.Redirect("CustomorLog.aspx");
                    return;
                }
                lblname.Text = tab.Rows[0]["cName"].ToString();
                Session["Id"] = tab.Rows[0]["cId"].ToString();
            }
            catch
            {
 
            }
        } 
    }
}
