<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SubmitApplication.aspx.cs" Inherits="Whitelist_Administration_Tool.SubmitApplication" %>
<asp:Content ID="ApplicationContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <table>
            <td>
                <img src="Images/HoS%20Logo%20Mark.png" style="width: 262px; height: 168px" />
            </td>
            <td>
                <center>
                    <h1 style="text-align: center">Submit Application</h1>
                    <p class="lead" style="text-align: center"><label id="message" runat="server">Please enter your ScreenName and uuid for whitelisting.</label></p>
                </center>
            </td>
            <td>
                <img src="Images/HoS%20Logo%20Mark.png" style="width: 262px; height: 168px" />
        </table>
    </div>
    
    <div class="well">
        <center>
            <p id="messageSubmitReport" class="alert-warning" runat="server"></p>
             <table ID="tableSubmit" runat="server">
                <tr>
                    <td><label>Screen Name:</label></td>
                    <td><asp:TextBox class="form-control" ID="txtUserName" runat="server" /></td>
                </tr>
                <tr>
                    <td><label>UUID (Optional):</label></td>
                    <td><asp:TextBox class="form-control" ID="txtUid" runat="server" /></td>
                </tr>
                 <tr>
                    <td><label>Email (Optional):</label></td>
                    <td><asp:TextBox class="form-control" ID="txtEmail" runat="server" /></td>
                </tr>
            </table>
            <div>
                <br />
                <asp:Button class="btn btn-primary btn-lg" ID="buttonSubmit" runat="server" Text="Submit" OnClick="OnSubmitButtonClicked"/>
            </div>
        </center>
    </div>

    </asp:Content>
