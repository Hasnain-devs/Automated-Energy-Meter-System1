using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace electricity
{
    public partial class find : System.Web.UI.Page
    {
        common obj = new common();
        DataTable tab = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
            }

          
        }
        public void Loaddata()
        {
            try
            {
                string metno = txtmet.Text;
                string query = string.Format("select * from tblCustomer where metno='{0}'", metno);
                tab = obj.nontransq(query);
                if (tab != null)
                {
                    GridView1.DataSource = tab;
                    GridView1.DataBind();
                }
                else
                {
                    GridView1.Controls.Clear();
                    lblMsg.Text = "No Records Found...!";

                }
            }
            catch
            {
 
            }
 
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Loaddata();

        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
           
            LinkButton lnkd = (LinkButton)sender;
            int id = int.Parse(lnkd.CommandArgument.ToString());
            string query = string.Format("delete from tblCustomer where cId={0}", id);
            int res = obj.transq(query);
            if (res > 0)
            {
                lblMsg.Text="Record delete";
            }
            else
            {
                lblMsg.Text = "error in Deleting  Record";
            }
            Response.Redirect("find.aspx");

        }

        protected void lnkedit_Click(object sender, EventArgs e)
        {
            LinkButton lnk = (LinkButton)sender;
            int cid = int.Parse(lnk.CommandArgument.ToString());
            Response.Redirect("add.aspx?Id=" + cid);

           

        }

      
    }
}