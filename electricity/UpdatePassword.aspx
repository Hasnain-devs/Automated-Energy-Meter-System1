<%@ Page Title="" Language="C#" MasterPageFile="~/CustomerHome.Master" AutoEventWireup="true" CodeBehind="UpdatePassword.aspx.cs" Inherits="electricity.UpdatePassword" %>
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
									<h4>Update Password</h4>
								</div>
						<div class="form-body">
							<form id="Form1" role="form" runat="server"> 
                                 <div class="form-group">
                                   <asp:Label ID="lblCourse" runat="server" Text="">Enter old Password</asp:Label>
                                     <asp:TextBox ID="txtoldpwd0" class=" form-control" runat="server" 
                                         TextMode="Password"></asp:TextBox> 
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                        ErrorMessage="password can't be blank" ControlToValidate="txtoldpwd0" 
                                        ForeColor="Red" InitialValue="--Select--" ValidationGroup="A"></asp:RequiredFieldValidator>
                                  </div>
                                  <div class="form-group">
                                   <asp:Label ID="Label1" runat="server" Text="">Enter New password</asp:Label>
                                     <asp:TextBox ID="txtnewpwd" class=" form-control" runat="server" 
                                          TextMode="Password"></asp:TextBox> 
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                        ErrorMessage="password can't be blank" ControlToValidate="txtnewpwd" 
                                        ForeColor="Red" InitialValue="--Select--" ValidationGroup="A"></asp:RequiredFieldValidator>
                                  </div>
                                  <div class="form-group">
                                   <asp:Label ID="Label2" runat="server" Text="">Confirm Password</asp:Label>
                                     <asp:TextBox ID="txtconfirm" class=" form-control" runat="server" 
                                          TextMode="Password"></asp:TextBox> 
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                        ErrorMessage="please confirm yout password" ControlToValidate="txtconfirm" 
                                        ForeColor="Red" InitialValue="--Select--" ValidationGroup="A"></asp:RequiredFieldValidator>
                                      &nbsp;&nbsp;&nbsp;
                                      <asp:CompareValidator ID="CompareValidator1" runat="server" 
                                          ErrorMessage="Password dont matck" ClientIDMode="Inherit" 
                                          ControlToValidate="txtconfirm" ControlToCompare="txtnewpwd" ForeColor="#CC0000"></asp:CompareValidator>
                                      <br />
                                      
                                  </div>  
                                  <div class="form-group">
                                      <asp:Label ID="msg" runat="server" Text=""></asp:Label>
                                  </div>
                                   <div class="form-group">
                                   <div class=" pull-right">
                                       <asp:Button ID="Button1" runat="server" class=" btn btn-default" Text="Update Password" 
                                           onclick="Button1_Click" />
                                            <button type="reset" style="padding:0px 12px" class="btn btn-default">
                                   Cancel</button>
                                       </div>
                                   </div>
                                 
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
