using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace electricity
{
    public partial class Customer : System.Web.UI.Page
    {
        common obj = new common();
        private static string SqlSafe(string input)
        {
            return (input ?? string.Empty).Replace("'", "''").Trim();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loaddata();
            }

        }
        public void loaddata()
        {
            if (Session["Email"] == null)
            {
                Response.Redirect("CustomorLog.aspx");
                return;
            }
            string email = SqlSafe(Session["Email"].ToString());
            DataTable tab = new DataTable();
            string q = "select * from tblCustomer where Email='" + email + "'";
            tab = obj.nontransq(q);
            if (tab == null || tab.Rows.Count == 0)
            {
                Response.Redirect("CustomorLog.aspx");
                return;
            }
            int id=int.Parse(tab.Rows[0]["cId"].ToString());
            Session["Id"] = id;
            if (tab.Rows.Count > 0)
            {
                txtname.Text = tab.Rows[0]["cName"].ToString();
                txtadress.Text = tab.Rows[0]["Address"].ToString();
                txtphone.Text = tab.Rows[0]["phone"].ToString();
                txtemail.Text = tab.Rows[0]["Email"].ToString();
              
            }
            
           
            

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Session["Id"] == null)
            {
                Response.Redirect("CustomorLog.aspx");
                return;
            }
            int id = Convert.ToInt32(Session["Id"]);
            string q = string.Format(
                "update tblCustomer set cName='{0}', Address='{1}', Email='{2}', phone='{3}'  where cid={4}",
                SqlSafe(txtname.Text),
                SqlSafe(txtadress.Text),
                SqlSafe(txtemail.Text),
                SqlSafe(txtphone.Text),
                id);
            int res=obj.transq(q);
            if (res > 0)
            {
                loaddata();
                lblMsg.Text = "Updated Successfully";
     
                

            }
            else
            {
                loaddata();
                lblMsg.Text = "Updated Successfully";
            }
        }
    }
}
