<%@ Page Title="" Language="C#"  MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="frontRegister.aspx.cs" Inherits="Day18.frontRegister" %>


<asp:Content ID="Content_Index" ContentPlaceHolderID="MainContent" runat="server">  
        <div class="contentFrontLogin container">
            <div class="loginBox">
                <p class="h1 text-center">會員註冊</p>
                <div class="loginTest">
                    <!-- Email input -->
                    <div class="form-outline mb-4">
                        <asp:TextBox type="email" ID="TextBox1" runat="server" class="form-control input-lg"  TabIndex="1" placeholder="電子郵件"></asp:TextBox>
                    </div> 
                    <!-- Password input -->
                    <div class="form-outline mb-4">
                        <input id="Password1" runat="server" type="password" class="form-control input-lg"  placeholder="密碼" />
                    </div>
                     <div class="form-outline mb-4">
                        <input id="Password2" runat="server" type="password" class="form-control input-lg"  placeholder="確認密碼" />
                    </div>
                    <div class="form-outline mb-4">

                        <asp:Button ID="Button1" class="btn btn-dark" runat="server" Text="新增"  />
                        <a class="nav-link login" href="./frontlogin.aspx">上一頁</a>
                        <br />
                    </div>
               </div>

            </div>
        </div>
    <script src="./assets/js/frontRegister.js"></script>
</asp:Content>
