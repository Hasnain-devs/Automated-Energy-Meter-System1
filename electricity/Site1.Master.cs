using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace electricity
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        common obj = new common();
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        //protected void Button1_Click(object sender, EventArgs e)
        //{
        //    if (Request.QueryString["Id"] != null)
        //    {

        //        string s = "Update tblBill set status='Y' where Id=" + Request.QueryString["Id"];
        //        int res = obj.transq(s);
        //        if (res > 0)
        //        {
        //            Response.Write("<scripe>alert('Payment Done Successfully')</script>");
        //        }
        //        else
        //        {
        //            Response.Write("<script>alert('Error in Payment')</script>");
        //        }
        //    }
        //    else
        //    {
        //        string em=Session["Email"].ToString();
        //        string s = string.Format("Update tblBill set status='Y' where metno=(select metno from tblCustomer where Email={0})",em);
        //        int res=obj.transq(s);
        //        if (res > 0)
        //        {
        //            Response.Write("<scripe>alert('Payment Done Successfully')</script>");
        //        }
        //        else
        //        {
        //            Response.Write("<script>alert('Error in Payment')</script>");
        //        }
        //    }

        //}

        protected void Button1_Click1(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["Id"] != null)
                {
                    int billId;
                    if (!int.TryParse(Request.QueryString["Id"], out billId))
                    {
                        Response.Write("<script>alert('Invalid bill id')</script>");
                        return;
                    }

                    string s = "Update tblBill set status='Y', PaidDate='" + DateTime.Now.Date.ToString() + "' where Id=" + billId;
                    int res = obj.transq(s);
                    if (res > 0)
                    {

                        Response.Write("<script>alert('Payment Done Successfully')</script>");
                        Response.Redirect("Bills.aspx");
                    }
                    else
                    {
                        Response.Write("<script>alert('Error in Payment')</script>");
                    }
                }
                else
                {
                    if (Session["Email"] == null)
                    {
                        Response.Redirect("CustomorLog.aspx");
                        return;
                    }
                    string em = Session["Email"].ToString();
                    string s = string.Format("Update tblBill set status='Y', PaidDate='{0}' where metno=(select metno from tblCustomer where Email='{1}')", DateTime.Now.Date.ToString(), em.Replace("'", "''"));
                    int res = obj.transq(s);
                    if (res > 0)
                    {
                        Response.Write("<script>alert('Payment Done Successfully')</script>");
                        Response.Redirect("Bills.aspx");
                    }
                    else
                    {
                        Response.Write("<script>alert('Error in Payment')</script>");
                    }
                }
            }
            catch
            {
                Response.Write("<script>alert('Error in Payment')</script>");
            }

        }

        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("bills.aspx");
        }
    }
}
