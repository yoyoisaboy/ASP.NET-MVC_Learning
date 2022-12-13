# ASP .NET Web應用程式上手(.NET Framework) Day 17 (ASP.NET Framework內建的CRUD與驗證登入)

###### tags: `ASP .NET Web` `C#` `.NET Framework`

github:[Day17](https://github.com/yoyoisaboy/ASP.NET-MVC_Learning/tree/main/Day17)
## 前言
本例子使用asp的標籤用法，快速開發新增(Create)、讀取(Read)、修改(Update)、刪除(Delete)。

## 新增本地資料庫
新增App_Data->加入->SQL Server資料庫，取名dbmember.mdf
![](https://i.imgur.com/NqvtUOq.png)

![](https://i.imgur.com/NoHVwSX.png)
### 新增Table
加入新的資料表
![](https://i.imgur.com/UWJSTD6.png)
看你要手動新增還是新增查詢，這邊用新增查詢，輸入新增table的SQL指令
![](https://i.imgur.com/jx6kMgG.png)

## Site.Master母框
簡單建一個母框，方便我們直接切換分頁
![](https://i.imgur.com/VWBuCxy.png)
```
<%@ Master Language="C#" AutoEventWireup="true" CodeBehind="Site1.master.cs" Inherits="Day17.Site1" %>

<!DOCTYPE html>

<html lang="zh">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>yoyo會員平台 - <%: Page.Title %></title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-wEmeIV1mKuiNpC+IOBjI7aAzPcEZeedi5yW5f2yOq55WWLwNGmvvx4Um1vskeMj0" crossorigin="anonymous">
    <!--<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.3.1/dist/css/bootstrap.min.css" integrity="sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T" crossorigin="anonymous">-->
</head>
<body>
    <form id="form1" runat="server">
        
        <section class="headerBg">
<%--            <div class="content container">--%>
            <div class="header">
                <nav class="navbar navbar-expand-lg navbar-light bg-light">
                  <div class="container-fluid">
                    <%--logo--%>
                    <a class="navbar-brand logo" href="./index.aspx">
                        <img class="logoImg" src="./assets/images/home.png" alt="Logo" width=10% height=10% />
                    </a>
                    <%--一般選單--%>
                    <a class="nav-link login" href="./frontLogin.aspx">會員登入</a>
                    <a class="nav-link login" href="./Index.aspx">會員編輯</a>
                  </div>
                </nav>
            </div>
        </section>
        <%---------------------------- 主要內容 ----------------------------%>
        <section class="contentBg">
            <asp:ContentPlaceHolder ID="MainContent" runat="server">

            </asp:ContentPlaceHolder>
        </section>
       <%---------------------------- footer 區域 ----------------------------%>
        <section class="footerBg">
           <footer class="content container text-center footer">
             <p class="footerText">&copy; yoyo中心</p>
           </footer>
        </section>
    </form>

    <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js" integrity="sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/popper.js@1.14.7/dist/umd/popper.min.js" integrity="sha384-UO2eT0CpHqdSJQ6hJty5KVphtPhzWj9WO1clHTMGa3JDZwrnQq4sF86dIHNDz0W1" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.3.1/dist/js/bootstrap.min.js" integrity="sha384-JjSmVgyd0p3pXB1rRibZUAYoIIy6OrQ6VrjIEaFf/nJGzIxFDsf4x0xIM+B07jRM" crossorigin="anonymous"></script>
</body>
</html>

```
* `<asp:ContentPlaceHolder ID="MainContent" runat="server">` 放每一個分頁的內容
* 引入需要的jquery、bootstrap
* 之後每個分頁都需要寫 `MasterPageFile="~/Site1.Master"`、`ContentPlaceHolderID="MainContent"`
* 
## Index.aspx
* 在專案右鍵->加入->新增項目->Web表單->取名Index.aspx。
![](https://i.imgur.com/7f56gcw.png)
* 套用母框
![](https://i.imgur.com/THjicX1.png)
:::success
可以用設計介面設計版面，標籤要到檢視找工具箱
![](https://i.imgur.com/1jeGUUp.png)
:::
```
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

    <div class="loginBox">
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
</div>
```

1. ID是與後端傳值的物件名稱。ex: ID.屬性
2. GridView 表單方便觀察我們的CRUD，後台管理也會用到
3. TextBox輸入帳號密碼
4. 可以發現 asp 標籤裡都要有 runat="server" ，要加入才能串後端，我故意用TextBox和Input當例子，有<asp:>的取值方式要TextBox.Text，沒有的是Input.Value ([補充](https://ithelp.ithome.com.tw/articles/10220220))
5. CommandField產生ShowEditButton、ShowDeleteButton
6. SqlDataSource是重點:
    * `"<%$ ConnectionStrings:ConnectionString %>"` => `Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dbmember.mdf;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework` : 呼叫資料庫
    * `SelectCommand="SELECT * FROM [Member]"`:顯示資料SQL指令
    * `UpdateCommand="update Member set Account=@Account,Password=@Password where id=@id"`:顯示更新資料SQL指令
    * `DeleteCommand="delete from Member Where id=@id"`:顯示刪除資料SQL指令
    *  @xxx : xxx要跟 DataField="xxx"一樣

## Index.aspx.cs
在設計界面，在按鈕左鍵點擊兩下
```
protected void Button1_Click(object sender, EventArgs e)
{
    //撰寫觸發按鈕的功能
    Member member = new Member();
    member.Account = TextBox1.Text;
    member.Password = Password1.Value;

    db.Member.Add(member);
    db.SaveChanges();

    Response.Redirect("Index.aspx");
}
```
:::success
* db.[table name].Add([物件]) : 將[物件]資料寫進資料庫，此方法是LINQ用法。 ([ASP .NET MVC 上手 Day06(Model模型應用_LINQ)](https://hackmd.io/0TeXAgttT1u8RihV331VNQ))
:::
## 結果
![](https://i.imgur.com/QEXlEme.png)

## 登入功能

### frontlogin.aspx
* 帳號密碼輸入，取得欄位的值用LINQ確認有沒有這個人，有就跳轉到Index.aspx
* 可以點註冊新增帳號
```
<%@ Page Language="C#"  AutoEventWireup="true" CodeBehind="frontlogin.aspx.cs" Inherits="Day17.frontlogin" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>會員登入 </title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-wEmeIV1mKuiNpC+IOBjI7aAzPcEZeedi5yW5f2yOq55WWLwNGmvvx4Um1vskeMj0" crossorigin="anonymous">
    
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

```
### frontlogin.aspx.cs
* LINQ查詢
```
dbmemberEntities1 db = new dbmemberEntities1();
...
protected void Button1_Click(object sender, EventArgs e)
{

    var member = db.Member.Where(m => m.Account == TextBox1.Text && m.Password == Password1.Value).FirstOrDefault(); //查詢
    if (member != null)
    {
        Response.Redirect("Index.aspx");
    }
    else
    {
        Response.Write(@"<script>alert(""登入失敗"");</script>");
    }

}
```
## 註冊功能
* 新增aspx(不用套用母框)
* 記得做防呆: 帳號已存在 、 密碼不一致 、 註冊失敗
### frontRegister.aspx
```
<%@ Page Language="C#"  AutoEventWireup="true" CodeBehind="frontRegister.aspx.cs" Inherits="Day17.frontRegister" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>會員登入 </title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-wEmeIV1mKuiNpC+IOBjI7aAzPcEZeedi5yW5f2yOq55WWLwNGmvvx4Um1vskeMj0" crossorigin="anonymous" />
    
</head>
<body>
    <form id="form1" runat="server">
       
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

                        <asp:Button ID="Button1" class="btn btn-dark" runat="server" Text="新增"  OnClick="Button1_Click" />
                        <a class="nav-link login" href="../frontlogin.aspx">上一頁</a>
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

```
### frontRegister.aspx.cs
```
protected void Button1_Click(object sender, EventArgs e)
{
    if ( TextBox1 != null && !string.IsNullOrWhiteSpace(TextBox1.Text) && Password1 != null && !string.IsNullOrWhiteSpace(Password1.Value) && Password2 != null && !string.IsNullOrWhiteSpace(Password2.Value) )
    {
        String password1 = Password1.Value;
        String password2 = Password2.Value;
        if (password1== password2)
        {
            String account = TextBox1.Text;
            var member_db = db.Member.Where(m => m.Account == account).FirstOrDefault(); //查詢
            if (member_db == null)
            {
                Member member = new Member();
                member.Account = account;
                member.Password = password1;
                db.Member.Add(member);
                db.SaveChanges();
                Response.Redirect("frontlogin.aspx");
            }
            else Response.Write(@"<script>alert(""帳號已存在"");</script>");
        }
        else Response.Write(@"<script>alert(""密碼不一致"");</script>");
    }
    else Response.Write(@"<script>alert(""註冊失敗"");</script>");
}
```

## 驗證
簡單加個驗證的機制，需登入才能進入Index.aspx
### Web.config : Authenticate 事件
```
<system.web>
    <!--啟用驗證功能-->
    <authentication mode="Forms">
      <forms loginUrl="frontlogin.aspx" defaultUrl ="Index.aspx"></forms>
    </authentication>
    <authorization>
      <deny users="?"></deny>
    </authorization>
</system.web>
```
* loginUrl="頁面.aspx" : 沒登入導入這
* defaultUrl = "頁面.aspx" : 登入後到這
* deny是禁止，這代表拒絕沒有通過驗證的使用者來存取我們的網站。註：「?」代表未授權的使用者，「*」代表所有的使用者。

### frontlogin.aspx.cs
新增一行 FormsAuthentication.RedirectFromLoginPage : 使用為表單驗證 Cookie 指定的 Cookie 路徑，將已驗證的使用者重新導向回到原先要求的 URL 或預設 URL。
```
protected void Button1_Click(object sender, EventArgs e)
{
    var member = db.Member.Where(m => m.Account == TextBox1.Text && m.Password == Password1.Value).FirstOrDefault(); //查詢
    if (member != null)
    {
        //登入驗證
        FormsAuthentication.RedirectFromLoginPage(TextBox1.Text, true);
        Response.Redirect("Index.aspx");
    }
    else
    {
        Response.Write(@"<script>alert(""登入失敗"");</script>");
    }
}
```
cookie記住登入資訊，下次再執行還會保持登入狀態。把它移除舊輝物乘虛登入才能到Index.aspx了
![](https://i.imgur.com/10wztPr.png)
## 登出
多做一個登出，不用再移除cookie
### Site.Master
LoginStatus : 偵測使用者的驗證狀態，並切換登入或登出網站的連結狀態。未登入不出現。
```
<asp:LoginStatus ID="LoginStatus1" runat="server" class="nav-link logout" LoginText="" LogoutText="登出" /> 
<a class="nav-link login" href="./Index.aspx">會員編輯</a>
```
![](https://i.imgur.com/cr2mRfO.png)

## 痾....有個地方怪怪的，註冊也要驗證登入???
阿對齁，這地方要注意，驗證登入是Web.config管同一路徑的網站(這跟[IIS](https://learn.microsoft.com/zh-tw/aspnet/core/host-and-deploy/iis/web-config?view=aspnetcore-7.0)有很大的關係)，因此註冊需要多一個資料夾(Registration)來放，在這資料夾放"frontRegister.aspx"、"Web.config"，並設定授權。

### Registration/Web.config
```
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <system.web>
    <!--啟用授權功能-->
    <authorization>
      <allow users="*" />
    </authorization>
  </system.web>
</configuration>
```
* allow : 允許
這樣就允許所有未登錄的人進來這頁面
![](https://i.imgur.com/PWrv2KC.png)

### frontlogin.aspx
```
<a class="nav-link login" href="./Registration/frontRegister.aspx">註冊</a><br />
```
這裡路徑記得改新的。
## 結果
本日簡單開發ASP. Net Framework內建的CRUD功能、登入、註冊與登入驗證的功能，注意一些運作模式，不管是<asp:xxx>、web.config中寫驗證，他們背後是如何運作的，為什麼大眾比較沒用這寫法，過時還是不安全，我給自己的解釋是不通用，假如你換一份工作妳只會這套，換一個框架那不都要重學這框架的用法。
因此，本篇是帶大家知道這框架有這功能，但建議是知道就好，要開發平台最好是用業界主流的寫法會比較好。



## 補充
* [Bootstrap Buttons](https://bootstrap5.hexschool.com/docs/5.0/components/buttons/)
* [Bootstrap login 版面](https://mdbootstrap.com/docs/standard/extended/login/)
* [原來工具箱有Login都寫好的< asp:Login >標籤，哭阿沒看到，補這個的驗證](https://dotblogs.com.tw/sywang/2010/07/30/16905)
* [[ASP.NET MVC] 使用 AOP 驗證系統功能執行權限](https://blog.hungwin.com.tw/aspnet-mvc-aop-authorize/)
* [什麼是 Forms 認證](http://vito-note.blogspot.com/2012/12/form.html)
* [web config 官方介紹](https://learn.microsoft.com/zh-tw/aspnet/core/host-and-deploy/iis/web-config?view=aspnetcore-7.0)
