<%@ Page Title="" Language="C#" MasterPageFile="~/CustomerHome.Master" AutoEventWireup="true" CodeBehind="Customer.aspx.cs" Inherits="electricity.Customer" %>
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
									<h4>Update Profile</h4>
								</div>
						<div class="form-body">
							<form id="Form2" role="form" runat="server"> 
                                 <%--<div class="form-group">
                                   <asp:Label ID="lblCourse" runat="server" Text="">Meter number</asp:Label>
                                     <asp:TextBox ID="txtmet" class="form-control" runat="server" placeholder="meter number"></asp:TextBox>
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                         ErrorMessage="meter number cannot be blank " ControlToValidate="txtmet" 
                                         ForeColor="Red" ValidationGroup="A"></asp:RequiredFieldValidator>
                                  </div>--%> 
                                  <div class="form-group">
                                   <asp:Label ID="lblname" runat="server" Text="">Name/company</asp:Label>
                                     <asp:TextBox ID="txtname" class="form-control" runat="server" placeholder="name"></asp:TextBox>
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                         ErrorMessage="name field Can't Be Empty" ControlToValidate="txtname" 
                                         ForeColor="Red" ValidationGroup="A"></asp:RequiredFieldValidator>
                                  </div> 
                                 <div class="form-group">
                                   <asp:Label ID="lblSemister" runat="server" Text="">Addres</asp:Label>
                                     <asp:TextBox ID="txtadress" class="form-control" runat="server" 
                                         placeholder="Address" TextMode="MultiLine"></asp:TextBox>
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                         ErrorMessage="Address Field Can't Be Empty" ControlToValidate="txtadress" 
                                         ForeColor="Red" ValidationGroup="A"></asp:RequiredFieldValidator>
                                  </div> 
                                  <div class="form-group">
                                   <asp:Label ID="lblDuration" runat="server" Text="">Email</asp:Label>
                                     <asp:TextBox ID="txtemail" class="form-control" runat="server" placeholder="Email"></asp:TextBox>
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                                         ErrorMessage="Email Field Can't Be Empty" ControlToValidate="txtemail" 
                                         ForeColor="Red" ValidationGroup="A"></asp:RequiredFieldValidator>
                                      <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                          ErrorMessage="Email format Invalid" ControlToValidate="txtemail" 
                                          ForeColor="Red" 
                                          ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                          ValidationGroup="A"></asp:RegularExpressionValidator>
                                  </div> 
                                  <div class="form-group">
                                   <asp:Label ID="lblAdmission" runat="server" Text="">phone number</asp:Label>
                                     <asp:TextBox ID="txtphone" class="form-control" runat="server" placeholder="Phone Number"></asp:TextBox>
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                                         ErrorMessage="Phone number Field Can't Be Empty" ControlToValidate="txtphone" 
                                         ForeColor="Red" ValidationGroup="A"></asp:RequiredFieldValidator>
                                  </div> 
                                 <%-- <div class="form-group">
                                   <asp:Label ID="lblExaminationFee" runat="server" Text="">Type of connection</asp:Label>
                                      <asp:DropDownList ID="ddlcon" class="form-control" runat="server" 
                                          DataTextField="none">
                                          <asp:ListItem>-select-</asp:ListItem>
                                          <asp:ListItem>none</asp:ListItem>
                                          <asp:ListItem>Industrial</asp:ListItem>
                                          <asp:ListItem>Domestic</asp:ListItem>
                                          <asp:ListItem Value="Commercial"></asp:ListItem>
                                      </asp:DropDownList> 
                                   
                                  </div> --%>
                                            <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
                                   <div class="pull-right">
                                  <asp:Button ID="btnSubmit" class="btn btn-default" style="padding:0px 12px" runat="server" Text="Update" OnClick="btnSubmit_Click"
                                    ValidationGroup="A" />
                                   <button type="reset" style="padding:0px 12px" class="btn btn-default">
                                   Cancel</button>
                                 </div>
                                 </form>
                                           </div>
                                           </div>
                                           </div>
                                           </div>
                                           </div>

</asp:Content>
