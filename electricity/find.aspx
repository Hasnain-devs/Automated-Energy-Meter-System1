<%@ Page Title="" Language="C#" MasterPageFile="~/admin.Master" AutoEventWireup="true" CodeBehind="find.aspx.cs" Inherits="electricity.find" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


   <div class="page-container sidebar-collapsed-back">
    <div class="left-content">
   <div class="inner-content">
    <div class="content">
     <div class="women_main">
     <div class="grids">
					<div class="progressbar-heading grids-heading">
						<h2></h2>
					</div>
					<div class="panel panel-widget forms-panel">
						<div class="forms">
							<div class="form-grids widget-shadow" data-example-id="basic-forms"> 
								<div class="form-title">
									<h4>Find Customer</h4>
								</div>
						<div class="form-body">
							<form id="Form2" role="form" runat="server"> 
                                 <div class="form-group">
                                   <asp:Label ID="lblCourse" runat="server" Text="">Meter number</asp:Label>
                                     <asp:TextBox ID="txtmet" class="form-control" runat="server" placeholder="meter number"></asp:TextBox>
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                         ErrorMessage="Enter the meter number to search " ControlToValidate="txtmet" 
                                         ForeColor="Red" ValidationGroup="A"></asp:RequiredFieldValidator>
                                  </div>
                         
                          
                                  
                            <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
                                   <div class="pull-right">
                                  <asp:Button ID="btnSubmit" class="btn btn-default" style="padding:0px 12px" runat="server" Text="Submit" OnClick="btnSubmit_Click"
                                    ValidationGroup="A" />
                                
                                 </div>
                                     
                                 
								</div>
                                <div>
                               
                            </div>
							</div>
						</div>
					</div>
				</div>
	<!-- end content -->
    
    <asp:GridView ID="GridView1" 
                                        class="table table-striped table_border table-hover" runat="server" 
                                        AutoGenerateColumns="False">
                                       <Columns>
                                           <asp:BoundField DataField="metno" HeaderText="Meter Number" />
                                           <asp:BoundField DataField="cName" HeaderText="Name/company" />
                                           <asp:BoundField DataField="Email" HeaderText="Email" />
                                           <asp:BoundField DataField="phone" HeaderText="Phone Number" />
                                           <asp:TemplateField HeaderText="Edit">
                                               <ItemTemplate>
                                                   <asp:LinkButton ID="lnkedit" runat="server" 
                                                       CommandArgument='<%# Eval("cid") %>' onclick="lnkedit_Click">Click to View and Edit Details</asp:LinkButton>
                                               </ItemTemplate>
                                           </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Delete">
                                               <ItemTemplate>
                                                   <asp:LinkButton ID="lnkdelete" runat="server" 
                                                       CommandArgument='<%# Eval("cid") %>' onclick="LinkButton1_Click">Delete</asp:LinkButton>
                                               </ItemTemplate>
                                           </asp:TemplateField>
                                       </Columns>
                            </asp:GridView>
                            </form>
    </div>
</div>
</div>
</div>
</div>
</asp:Content>
