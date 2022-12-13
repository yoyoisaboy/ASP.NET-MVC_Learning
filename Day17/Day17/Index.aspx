<%@ Page Title="首頁" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Day17.Index" %>


<asp:Content ID="Content_Index" ContentPlaceHolderID="MainContent" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-wEmeIV1mKuiNpC+IOBjI7aAzPcEZeedi5yW5f2yOq55WWLwNGmvvx4Um1vskeMj0" crossorigin="anonymous">

     <div class="contentFrontLogin container">
        <p class="h1 text-center">會員新增</p>
        <!-- Email input -->
        <div class="form-outline mb-4">
            <asp:TextBox type="email" ID="TextBox1" runat="server" class="form-control input-lg"  placeholder="電子郵件"></asp:TextBox>
        </div>   
        <!-- Password input -->
        <div class="form-outline mb-4">
            <input id="Password1" runat="server" type="password" class="form-control input-lg"  placeholder="密碼" />
        </div>
        <!-- Submit button -->
        <asp:Button ID="Button1" class="btn btn-primary btn-block mb-4" runat="server" Text="新增"  OnClick="Button1_Click" />
    </div>

    <div class="form-outline mb-4">
        <asp:GridView ID="GridView1" class="table table-bordered table-condensed table-responsive table-hover " runat="server" AutoGenerateColumns="False" DataKeyNames="Id" DataSourceID="SqlDataSource1">
        <Columns>
            <asp:BoundField DataField="Id" HeaderText="Id" InsertVisible="False" ReadOnly="True" SortExpression="Id" />
            <asp:BoundField DataField="Account" HeaderText="Account" SortExpression="Account" />
            <asp:BoundField DataField="Password" HeaderText="Password" SortExpression="Password" />
            <asp:CommandField ShowEditButton="true" ButtonType="Button" ControlStyle-CssClass="btn btn-success"  HeaderText="Edit" ShowHeader="True" />
            <asp:CommandField ShowDeleteButton="true" ButtonType="Button" ControlStyle-CssClass="btn btn-danger"  HeaderText="Delete" ShowHeader="True" />
        </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT * FROM [Member]" UpdateCommand="update Member set Account=@Account,Password=@Password where id=@id" DeleteCommand="delete from Member Where id=@id"></asp:SqlDataSource>
        <p>&nbsp;</p>
    </div>
        
</asp:Content>