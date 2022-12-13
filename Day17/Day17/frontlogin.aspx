<%@ Page Language="C#"  AutoEventWireup="true" CodeBehind="frontlogin.aspx.cs" Inherits="Day17.frontlogin" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>會員登入 </title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-wEmeIV1mKuiNpC+IOBjI7aAzPcEZeedi5yW5f2yOq55WWLwNGmvvx4Um1vskeMj0" crossorigin="anonymous"/>
    
</head>
<body>
    <form id="form1" runat="server">
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
                     <a class="nav-link login" href="./Registration/frontRegister.aspx">註冊</a><br />
                    <br />
                </div>
           </div>
        </div>
    </div>
    </form>
    <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js" integrity="sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/popper.js@1.14.7/dist/umd/popper.min.js" integrity="sha384-UO2eT0CpHqdSJQ6hJty5KVphtPhzWj9WO1clHTMGa3JDZwrnQq4sF86dIHNDz0W1" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.3.1/dist/js/bootstrap.min.js" integrity="sha384-JjSmVgyd0p3pXB1rRibZUAYoIIy6OrQ6VrjIEaFf/nJGzIxFDsf4x0xIM+B07jRM" crossorigin="anonymous"></script>
</body>
</html>
