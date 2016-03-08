<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminControl.ascx.cs" Inherits="Whitelist_Administration_Tool.AdminControl" %>

<style type="text/css">
    .auto-style1 {
        width: 326px;
    }
    .auto-style2 {
        width: 366px;
    }
</style>

<form>
    <center>
        <p id="messageSubmitReport" class="alert-warning" runat="server"></p>
        <div class="well">
            <div class="form-group">
                <table>
                    <tr>
                        <td class="auto-style2">
                            <table ID="tableAdd" runat="server">
                                <tr>
                                    <td><label>User Name:</label></td>
                                    <td><asp:TextBox class="form-control" ID="txtAddUser" runat="server" /></td>
                                </tr>
                                <tr>
                                    <td><label>UID:</label></td>
                                    <td><asp:TextBox class="form-control" ID="txtAddUid" runat="server" /></td>
                                </tr>
                            </table>
                            <div>
                                <br />
                                <asp:Button class="btn btn-default btn-sm" ID="buttonAdd" runat="server" Text="Add" OnClick="OnButtonAddClicked"/>
                            &nbsp;<asp:Button class="btn btn-default btn-sm" ID="button" runat="server" Text="Remove" OnClick="OnButtonRemoveClicked"/>
                            </div>
                        </td>
                        <td class="auto-style1">
                            <asp:Button class="btn btn-default btn-lg" ID="btnManageAdmins" runat="server" Text="Manage Admins" Width="100%" Height="100%" OnClick="OnManageAdminsButtonClicked"/>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </center>
</form>

<div class="panel panel-default">
  <h2 style="text-align: center">Pending Applications</h2>
<asp:Table class="table table-condensed table-striped" ID="pendingTable" runat="server" Width="100%" style="text-align: left">
</asp:Table>
    </div>

<div class="panel panel-default">
  <h2 style="text-align: center">Current Whitelist</h2>
<asp:Table class="table table-condensed table-striped" ID="playersTable" runat="server" Width="100%" style="text-align: left">
</asp:Table>
    </div>


