<%@ Page Title="" Language="C#" MasterPageFile="~/CustomerHome.Master" AutoEventWireup="true" CodeBehind="pay.aspx.cs" Inherits="electricity.pay" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-container sidebar-collapsed-back">
        <div class="left-content">
            <div class="inner-content">
                <div class="content">
                    <div class="women_main">
                        <div class="grids">
                            <div class="progressbar-heading grids-heading">
                                <h2>Pay Bills</h2>
                            </div>
                            <div class="panel panel-widget forms-panel">
                                <div class="forms">
                                    <div class="form-grids widget-shadow" data-example-id="basic-forms">
                                        <div class="form-title">
                                            <h4>Pay All Unpaid Bills</h4>
                                        </div>
                                        <div class="form-body">
                                            <form id="FormPay" role="form" runat="server">
                                                <asp:GridView ID="GridBills" runat="server"
                                                    class="table table-bordered table-hover"
                                                    AutoGenerateColumns="False"
                                                    DataKeyNames="Id"
                                                    OnSelectedIndexChanged="GridBills_SelectedIndexChanged"
                                                    Width="100%">
                                                    <Columns>
                                                        <asp:BoundField DataField="Id" HeaderText="Bill ID" />
                                                        <asp:BoundField DataField="BillDate" HeaderText="Bill Date" />
                                                        <asp:BoundField DataField="amount" HeaderText="Amount" />
                                                        <asp:BoundField DataField="status" HeaderText="Status" />
                                                    </Columns>
                                                </asp:GridView>

                                                <asp:Label ID="LblSelected" runat="server" Text="" />
                                                <br />
                                                <asp:Button ID="BtnPaySelected" runat="server"
                                                    Text="Pay All"
                                                    CssClass="btn btn-success"
                                                    OnClick="BtnPaySelected_Click" />
                                                &nbsp;
                                                <asp:Button ID="BtnBackBills" runat="server"
                                                    Text="Back to Bills"
                                                    CssClass="btn btn-default"
                                                    OnClick="BtnBackBills_Click" />
                                                <br /><br />
                                                <asp:Label ID="LblMsg" runat="server" Text="" />
                                            </form>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
