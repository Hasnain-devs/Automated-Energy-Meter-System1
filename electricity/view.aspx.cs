using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace electricity
{
    public partial class view : System.Web.UI.Page
    {
        common obj = new common();
        protected void Page_Load(object sender, EventArgs e)
        {
            string query = "SELECT metno AS meterno , cname AS customer, phone , units AS consumed FROM tblCustomer ORDER BY units DESC LIMIT 3";
            DataTable summary = new DataTable();
            summary = obj.nontransq(query);

            query = "select * from tblCustomer";
            DataTable buf = new DataTable();
            buf = obj.nontransq(query);
            if (buf != null)
            {
                dyntab.GridLines = GridLines.Both;
                dyntab.BackColor = System.Drawing.Color.Azure;
                dyntab.ForeColor = System.Drawing.Color.Blue;
                dyntab.Font.Bold = true;
                dyntab.BorderColor = System.Drawing.Color.DarkRed;

                dyntab.Controls.Clear();
                TableHeaderRow tr = new TableHeaderRow();
                TableHeaderCell th1 = new TableHeaderCell();
                th1.Text = "Meter no";
                tr.Controls.Add(th1);
                TableHeaderCell th2 = new TableHeaderCell();
                th2.Text = "Name/company";
                tr.Controls.Add(th2);
                TableHeaderCell th3 = new TableHeaderCell();
                th3.Text = "Address";
                tr.Cells.Add(th3);
                TableHeaderCell th4 = new TableHeaderCell();
                th4.Text = "Email ID";
                tr.Controls.Add(th4);
                TableHeaderCell tp = new TableHeaderCell();
                tp.Text = "Phone";
                tr.Controls.Add(tp);
                TableHeaderCell tc5 = new TableHeaderCell();
                tc5.Text = "Type of Connection";
                tr.Controls.Add(tc5);
                TableHeaderCell tc6 = new TableHeaderCell();
                tc6.Text = "Edit";
                tr.Controls.Add(tc6);
                TableHeaderCell tc7 = new TableHeaderCell();
                tc6.Text = "Manipulations";
                tr.Controls.Add(tc7);
                dyntab.Controls.Add(tr);
                for (int i = 0; i < buf.Rows.Count; i++)
                {
                    TableRow tbr = new TableRow();
                    Label lblid = new Label();
                    lblid.Text = buf.Rows[i]["metno"].ToString();
                    TableCell tcid = new TableCell();
                    tcid.Controls.Add(lblid);
                    tbr.Controls.Add(tcid);

                    Label lblname = new Label();
                    lblname.Text = buf.Rows[i]["cName"].ToString();
                    TableCell tcname = new TableCell();
                    tcname.Controls.Add(lblname);
                    tbr.Controls.Add(tcname);

                    Label lblsub = new Label();
                    lblsub.Text = buf.Rows[i]["Address"].ToString();
                    TableCell tcsub = new TableCell();
                    tcsub.Controls.Add(lblsub);
                    tbr.Controls.Add(tcsub);

                    Label lblemail = new Label();
                    lblemail.Text = buf.Rows[i]["Email"].ToString();
                    TableCell tcemail = new TableCell();
                    tcemail.Controls.Add(lblemail);
                    tbr.Controls.Add(tcemail);


                    Label lblphone = new Label();
                    lblphone.Text = buf.Rows[i]["phone"].ToString();
                    TableCell tcphone = new TableCell();
                    tcphone.Controls.Add(lblphone);
                    tbr.Controls.Add(tcphone);


                    Label lblcon = new Label();
                    lblcon.Text = buf.Rows[i]["con_type"].ToString();
                    TableCell tccon = new TableCell();
                    tccon.Controls.Add(lblcon);
                    tbr.Controls.Add(tccon);
                    dyntab.Controls.Add(tbr);

                    LinkButton lnkedit = new LinkButton();
                    lnkedit.Text = "Edit";
                    lnkedit.ID = "Edit" + i.ToString();
                    lnkedit.CommandArgument = buf.Rows[i]["cid"].ToString();
                    lnkedit.Click += new EventHandler(lnkedit_Click);
                    TableCell tcedit = new TableCell();
                    tcedit.Controls.Add(lnkedit);
                    tbr.Controls.Add(tcedit);

                    LinkButton lnkdelete = new LinkButton();
                    lnkdelete.Text = "Delete";
                    lnkdelete.ID = "Delete" + i.ToString();
                    lnkdelete.CommandArgument = buf.Rows[i]["cid"].ToString();
                    lnkdelete.Click += new EventHandler(lnkdelete_Click);
                    TableCell tcdelete = new TableCell();
                    tcdelete.Controls.Add(lnkdelete);
                    tbr.Controls.Add(tcdelete);
                }
            }
            else
            {
                TableRow tr = new TableRow();
                TableHeaderCell tc7 = new TableHeaderCell();
                tc7.Text = "No Rcords Found!";
                tr.Controls.Add(tc7);
                dyntab.Controls.Add(tr);
            }
            GridView1.DataSource = summary;
            GridView1.DataBind();
        }
       

        void lnkdelete_Click(object sender, EventArgs e)
        {
            LinkButton lnkd = (LinkButton)sender;
            int id = int.Parse(lnkd.CommandArgument.ToString());
            string query = string.Format("delete from tblCustomer where cId={0}", id);
            int res = obj.transq(query);
            if (res > 0)
            {
                Response.Write("<script>alert('Record delete')</script>");
            }
            else
            {
                Response.Write("<script>alert('Failed to Delete')</script>");
            }
            Response.Redirect("view.aspx");
        }

        void lnkedit_Click(object sender, EventArgs e)
        {
            LinkButton lnkd = (LinkButton)sender;
            int id = int.Parse(lnkd.CommandArgument.ToString());
            Response.Redirect("add.aspx?Id=" + id);

        }
    }
}