<%@ Page Title="首頁" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Day18.Index" %>


<asp:Content ID="Content_Index" ContentPlaceHolderID="MainContent" runat="server">
    
    <a class="nav-link" href="#"><%=Session["username"] %> Hello~~</a><br />
    權限:<a class="nav-link" ><%=Session["permissions"] %></a>
    <div class="navbar-nav">
        <% if (Session["permissions"].ToString().Contains('R'))
        {%>
            <a class="nav-link login" href="./member.aspx">會員清單</a>

            <% if (Session["permissions"].ToString().Contains('C')){%>
                <a class="nav-link login" href="./member.aspx">會員新增</a>
            <%}%>
        <%}%>                    


    </div>
</asp:Content>