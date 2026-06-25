<%@ Page Title="" Language="C#" MasterPageFile="~/landing.Master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="electricity.index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<%--<!-- modal -->
<div class="modal fade" id="myModal1" tabindex="-1" role="dialog">
	<!-- Modal1 -->
	<div class="modal-dialog">
		<!-- Modal content-->
		
			<div class="modal-header">
				<button type="button" class="close" data-dismiss="modal">&times;</button>
				<h4>LOGIN AS</h4>
                </div>
                <div class="modal-content">
                 <form id="form1" runat="server">
                <asp:RadioButtonList class="form-control" ID="RadioButtonList1" runat="server" 
        AppendDataBoundItems="True" 
        onselectedindexchanged="RadioButtonList1_SelectedIndexChanged" AutoPostBack="True">
        <asp:ListItem>ADMIN</asp:ListItem>
        <asp:ListItem>CUSTOMER</asp:ListItem>
     
    </asp:RadioButtonList>
    </form>
				
			</div>
		</div>
	</div>
<!-- //Modal1 -->--%>
<div id="myModal1" class="modal fade" role="dialog">
  <div class="modal-dialog">

    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal">&times;</button>
        <h4 class="modal-title">LOGIN AS</h4>
      </div>
      <div class="modal-body">
    

        
        <form id="form1" runat="server">
        <%--<div class="form-group">--%>
                <asp:RadioButtonList class="form-control" ID="RadioButtonList1" runat="server" 
        AppendDataBoundItems="True" 
        onselectedindexchanged="RadioButtonList1_SelectedIndexChanged" AutoPostBack="True">
        <asp:ListItem>ADMIN</asp:ListItem>
        <asp:ListItem>CUSTOMER</asp:ListItem>
     
    </asp:RadioButtonList>
    <%--</div>--%>
    </form>
   
    </div>
     
      <div class="modal-footer">
        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
      </div>
       </div>
    </div>

  </div>


</asp:Content>
