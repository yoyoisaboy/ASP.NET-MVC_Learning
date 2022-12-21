<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="frontlogin.aspx.cs" Inherits="Day19.frontlogin" %>

<asp:Content ID="Content_Index" ContentPlaceHolderID="MainContent" runat="server">
        <%---------------------------- 主要內容 ----------------------------%>
        <div class="contentFrontLogin container">    
            <div class="loginBox">
                <h3>會員登入</h3>
                <div class="loginTest">
                        <input class="account form-control" type="email" name="name" value="" placeholder="電子郵件" required/>
                        <div class="passwordBox">
                        <input class="password form-control" type="password" name="name" value="" placeholder="密碼" required/>
                        </div>
                </div>
                <div class="loginNoticeFlex">
                    <div class="loginNotice">
                    
                    </div>
                </div>
                <asp:Button ID="Button1" CssClass="loginBtn btn btn-primary" runat="server" Text="登入" OnClientClick="return false;" />
                <p>不是會員?<a href="./frontRegister.aspx">立即註冊</a></p>
            </div>
        </div>
    <script src="./assets/js/frontLogin.js"></script>
</asp:Content>



