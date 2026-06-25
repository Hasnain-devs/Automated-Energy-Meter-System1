<%@ Page Title="" Language="C#" MasterPageFile="~/admin.Master" AutoEventWireup="true" CodeBehind="view.aspx.cs" Inherits="electricity.view" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="page-container sidebar-collapsed-back">
    <div class="left-content">
   <div class="inner-content">
    <div class="content">
     <div class="women_main">
	  <!-- start content -->
	   <div class="grids">
					<div class="progressbar-heading grids-heading">
						<h2></h2>
					</div>
					<div class="panel panel-widget forms-panel">
						<div class="forms">
							<div class="form-grids widget-shadow" data-example-id="basic-forms"> 
								<div class="form-title">
									<h4>Customer Details</h4>
								</div>
						<div class="form-body">
							<form id="Form2" role="form" runat="server">
                            <asp:Table ID="dyntab" class="table table-bordered table-hover table-responsive " runat="server">
                            </asp:Table> 
                            &nbsp;&nbsp;&nbsp; High units of consumption by following top 3 consumers<asp:GridView 
                                ID="GridView1" runat="server" BackColor="#DEBA84" BorderColor="#DEBA84" 
                                BorderStyle="None" BorderWidth="1px" CellPadding="3" CellSpacing="2">
                                <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
                                <HeaderStyle BackColor="#A55129" Font-Bold="True" ForeColor="White" />
                                <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                                <RowStyle BackColor="#FFF7E7" ForeColor="#8C4510" />
                                <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                                <SortedAscendingCellStyle BackColor="#FFF1D4" />
                                <SortedAscendingHeaderStyle BackColor="#B95C30" />
                                <SortedDescendingCellStyle BackColor="#F1E5CE" />
                                <SortedDescendingHeaderStyle BackColor="#93451F" />
                            </asp:GridView>
                                 </form>
								</div>
							</div>
						</div>
					</div>
				</div>
	<!-- end content -->
</div>
</div>
</div>
</div>
</div>
</asp:Content>