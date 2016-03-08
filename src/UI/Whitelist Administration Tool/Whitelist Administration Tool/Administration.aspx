<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Administration.aspx.cs" Inherits="Whitelist_Administration_Tool.Administration" %>
<asp:Content ID="LoginContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <table>
            <td>
                <img src="Images/HoS%20Logo%20Mark.png" style="width: 262px; height: 168px" />
            </td>
            <td>
                <center>
                    <h1 style="text-align: center">Administration</h1>
                    <p class="lead" style="text-align: center"><label id="labelMessage" runat="server">Please log in to begin adminstration on your whitelist.</label></p>
                </center>
            </td>
            <td>
                <img src="Images/HoS%20Logo%20Mark.png" style="width: 262px; height: 168px" />
        </table>
    </div>
    
    <div class="well">
        <center>
             <table ID="tableLogin" runat="server">
                <tr>
                    <td><label>User Name:</label></td>
                    <td><asp:TextBox class="form-control" ID="txtUser" runat="server" /></td>
                </tr>
                <tr>
                    <td><label>Password:</label></td>
                    <td><asp:TextBox class="form-control" TextMode="Password" ID="txtPassword" runat="server" /></td>
                </tr>
            </table>
            <div>
                <br />
                <asp:Button class="btn btn-primary btn-lg" ID="buttonLogin" runat="server" Text="Login" OnClick="OnLoginButtonClicked"/>
            </div>
        </center>
    </div>

    </asp:Content>
