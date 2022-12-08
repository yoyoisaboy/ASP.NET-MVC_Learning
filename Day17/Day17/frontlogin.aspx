<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="frontlogin.aspx.cs" Inherits="Day17.frontlogin" %>
<asp:Content ID="Content_frontlogin" ContentPlaceHolderID="MainContent" runat="server">

    <div class="contentFrontLogin container">
        <div class="loginBox">
            <p class="h1 text-center">會員登入</p>
            <div class="loginTest">
                <!-- Email input -->
                <div class="form-outline mb-4">
                    <asp:TextBox type="email" ID="TextBox1" runat="server" class="form-control input-lg"  TabIndex="1" placeholder="電子郵件"></asp:TextBox>
                </div> 
                <!-- Password input -->
                <div class="form-outline mb-4">
                    <input id="Password1" runat="server" type="password" class="form-control input-lg"  placeholder="密碼" />
                </div>
                <!-- Submit button -->
                <div class="form-outline mb-4">
                    <asp:Button ID="Button1" class="btn btn-dark" runat="server" Text="新增"  OnClick="Button1_Click" />
                    <!-- Register button -->
                     <a class="nav-link login" href="./frontRegister.aspx">註冊</a>
                </div>
                

           </div>
        </div>
    </div>
</asp:Content>
