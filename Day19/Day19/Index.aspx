<%@ Page Title="首頁" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Day19.Index" %>


<asp:Content ID="Content_Index" ContentPlaceHolderID="MainContent" runat="server"> 
    <div class="navbar-nav">
        <% if (Session["permissions"]!=null && Session["permissions"].ToString().Contains("R"))
        {%>
            <a class="nav-link" href="#"><%=Session["username"] %> Hello~~</a><br />
            權限:<a class="nav-link" ><%=Session["permissions"] %></a>
            <a class="nav-link login" href="./member.aspx">會員清單</a>

            <% if (Session["permissions"].ToString().Contains('C')){%>
                <a class="nav-link login" href="./member.aspx">會員新增</a>
            <%}%>
        <%}%>  
         <% else{%>
            <a href="./frontlogin.aspx" class="nav-link">請先登入</a>
         <%}%>
    </div>
</asp:Content>