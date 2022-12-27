<%@ Page Title="" Language="C#"  MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="frontRegister.aspx.cs" Inherits="Day19.frontRegister" %>


<asp:Content ID="Content_Index" ContentPlaceHolderID="MainContent" runat="server">  

    <div class="contentFrontLogin container">
        <div class="loginBox">
            <p class="h1 text-center">會員註冊</p>
            <div class="loginTest">
                <!-- Email input -->
                <div class="form-outline mb-4">
                    <asp:TextBox type="email" ID="account" runat="server" class="account form-control input-lg"  TabIndex="1" placeholder="電子郵件"></asp:TextBox>
                </div> 
                <!-- Name input -->
                <div class="form-outline mb-4">
                    <asp:TextBox type="text" ID="name" runat="server" class="name form-control input-lg"  TabIndex="1" placeholder="名字"></asp:TextBox>
                </div> 
                <!-- Password input -->
                <div class="form-outline mb-4">
                    <input id="password" runat="server" type="password" class="password form-control input-lg"  placeholder="密碼" />
                </div>
                <!-- Confirmpassword input -->
                    <div class="form-outline mb-4">
                    <input id="Confirmpassword" runat="server" type="password" class="Confirmpassword form-control input-lg"  placeholder="確認密碼" />
                </div>
                <div class="form-outline mb-4">
                    <asp:Button ID="registerBtn" class="registerBtn btn btn-dark" runat="server" Text="註冊" OnClientClick="return false;" />
                    <a class="nav-link login" href="./frontlogin.aspx">已經是會員?</a>
                    <br />
                </div>
            </div>
        </div>
    </div>
    <script src="assets/js/frontRegister.js"></script>
    
</asp:Content>
