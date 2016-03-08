<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Whitelist_Administration_Tool._Default" %>



<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <p class="lead" style="text-align: center">
            <img alt="Herd Of Squirrels!" src="Images/HoS%20Logo%20Horizontal%202.png" style="width: 784px; height: 177px" /></p>
        <center>
            <h1 style="text-align: center">The Whitelist Administration Tool</h1>
            <p class="lead" style="text-align: center; width: 683px;">Welcome to the Whitelist Administration Tool!  Please login to continue to the administration page.</p>
            <a href="Administration.aspx" class="btn btn-primary btn-lg">Login &raquo;</a> &nbsp;&nbsp; <a href="SubmitApplication.aspx" class="btn btn-primary btn-lg">Apply &raquo;</a>
        </center>
    </div>

    </asp:Content>
