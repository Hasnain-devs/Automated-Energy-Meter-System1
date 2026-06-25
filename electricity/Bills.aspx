<%@ Page Title="" Language="C#" MasterPageFile="~/CustomerHome.Master" AutoEventWireup="true" CodeBehind="Bills.aspx.cs" Inherits="electricity.Bills" %>
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
									<h4>Your Unpaid Bills</h4>
								</div>
						<div class="form-body">
							<form id="Form2" role="form" runat="server">
                            <asp:GridView ID="GridView1" 
                                class="table table-bordered table-hover" runat="server" 
                                Height="16px" Width="579px" AutoGenerateColumns="False" CellPadding="4" 
                                ForeColor="#333333" GridLines="None" 
                               >
                                <AlternatingRowStyle BackColor="White" />
                                <Columns>
                                    <asp:BoundField DataField="BillDate" HeaderText="date" 
                                        DataFormatString="{0:MMMM d, yyyy}" />
                                    <asp:BoundField DataField="amount" HeaderText="Amount" />
                                    <asp:TemplateField HeaderText="Pay" Visible=false>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkpay"  runat="server" CommandArgument='<%# Eval("Id") %>' 
                                                onclick="lnkpay_Click" target="_blank" Visible=false>Pay Now</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                                <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
                                <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                                <SortedAscendingCellStyle BackColor="#FDF5AC" />
                                <SortedAscendingHeaderStyle BackColor="#4D0000" />
                                <SortedDescendingCellStyle BackColor="#FCF6C0" />
                                <SortedDescendingHeaderStyle BackColor="#820000" />
                            </asp:GridView>
                            <asp:Label ID="lblmsg" runat="server" Text=""></asp:Label>
                             <div class="pull-right">

                                  <asp:Button ID="btnSubmit" class="btn btn-danger" style="padding:0px 12px" 
                                      runat="server" Text="Pay All" onclick="btnSubmit_Click"  />
                                       <asp:Button ID="btnConsuption" class="btn btn-warning" style="padding:0px 12px" 
                                      runat="server" Text="View Units Consumed" onclick="btnConsuption_Click"/>
                                       <asp:Button ID="btnhistory" class="btn btn-success" style="padding:0px 12px" 
                                      runat="server" Text="View Payments" onclick="btnhistory_Click"  />
                                      </div>
                                 <br />
                            <br />
                            <br />
                            <br />
                            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" 
                                CellPadding="3" GridLines="Vertical" Width="100%" BackColor="White" 
                                BorderColor="#999999" BorderStyle="None" BorderWidth="1px" Height="182px">
                                <AlternatingRowStyle BackColor="#DCDCDC" />
                                <Columns>
                                    <asp:BoundField DataField="BillDate" HeaderText="Generated Date" 
                                        DataFormatString="{0:MMMM d, yyyy}" />
                                    <asp:BoundField DataField="amount" HeaderText="Amount" />
                                    <asp:BoundField DataField="PaidDate" HeaderText="paid Date" />
                                </Columns>
                                <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                <SortedAscendingHeaderStyle BackColor="#0000A9" />
                                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                <SortedDescendingHeaderStyle BackColor="#000065" />
                            </asp:GridView>
                                 <br />
                            <asp:Label ID="lblmsg0" runat="server" Text=""></asp:Label>
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
