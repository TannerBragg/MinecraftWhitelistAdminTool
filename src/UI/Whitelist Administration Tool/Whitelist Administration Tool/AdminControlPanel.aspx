<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminControlPanel.aspx.cs" Inherits="Whitelist_Administration_Tool.AdminControlPanel" EnableEventValidation="false" %>

<%@ Register Src="~/AdminControl.ascx" TagPrefix="uc1" TagName="AdminControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <center>
            <img alt="Herd of Squirrels!" src="Images/HoS%20Logo%20Type.png" style="width: 784px; height: 177px" />
            <center><h1>Admin Control Panel</h1></center>
        </center>
    </div>
    <div>
        <asp:Panel ID="adminPanel" runat="server">
    </asp:Panel>
    </div>

</asp:Content>



