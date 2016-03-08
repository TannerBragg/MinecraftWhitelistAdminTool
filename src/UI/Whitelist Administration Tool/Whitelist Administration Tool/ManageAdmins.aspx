<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageAdmins.aspx.cs" Inherits="Whitelist_Administration_Tool.ManageAdmins" EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <center>
            <img alt="Herd of Squirrels!" src="Images/HoS%20Logo%20Type.png" style="width: 784px; height: 177px" />
            <center><h1>Manage Admins</h1></center>
        </center>
    </div>
        <form>
            <center>
                <div class="panel panel-default">
                    <h2 style="text-align: center">Logged In As</h2>
                        <div class="well">
                            <div class="form-group">
                                <table>
                                    <tr>
                                        <td>
                                            <table ID="infoTable" runat="server">
                                                <tr>
                                                    <td><label>User Name:</label></td>
                                                    <td><label id="labelUsername" runat="server"></label></td>
                                                </tr>
                                                <tr>
                                                    <td><label>Power Level:</label></td>
                                                    <td><label id="labelPowerLevel" runat="server"></label></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                <div class="well">
                    <div id="formGroup" class="form-group" runat="server">
                        <table>
                            <tr>
                                <td>
                                    <table ID="tableAdd" runat="server">
                                        <tr>
                                            <td><label>User Name:</label></td>
                                            <td><asp:TextBox class="form-control" ID="txtAddUser" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td><label>Password:</label></td>
                                            <td><asp:TextBox class="form-control" TextMode="Password" ID="txtPassword" runat="server"/></td>
                                        </tr>
                                        <tr>
                                            <td><label>Retype Password:</label></td>
                                            <td><asp:TextBox class="form-control" TextMode="Password" ID="txtPasswordRepeat" runat="server"/></td>
                                        </tr>
                                        <tr>
                                            <td><label>Power (1-10):</label></td>
                                            <td><asp:TextBox class="form-control" ID="txtPower" runat="server" /></td>
                                        </tr>
                                    </table>
                                    <div>
                                        <br />
                                        <asp:Button class="btn btn-default btn-sm" ID="buttonAdd" runat="server" Text="Add" OnClick="OnButtonAddClicked"/>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </center>
        </form>
    <p id="message" class="alert-warning" style="text-align: center" runat="server"></p>
        <div class="panel panel-default">
            <h2 style="text-align: center">Current Admins</h2>
            <asp:Table class="table table-condensed table-striped" ID="adminsTable" runat="server" Width="100%" style="text-align: left">
        </asp:Table>
        </div>
    
</asp:Content>
