ASP .NET Web應用程式上手(.NET Framework) Day 18 網路服務Web API asmx Part1(SQL執行&Ajax取傳值)
===
###### tags: `ASP .NET Web` `C#` `.NET Framework`

github : [Day18](https://github.com/yoyoisaboy/ASP.NET-MVC_Learning/tree/main/Day18)

## 前言
###
* [ASP .NET MVC 上手 Day12(網路服務Web API_非同步會員管理系統)](https://hackmd.io/wy2RRI9USKGfEVBCD0MgCw)是我們之前講解過的例子，透過Ajax非同步的方式，但是用URL傳值，雖然有加密安全性還是有疑慮。
* 本篇透過asmx建Web API 處理資料庫CRUD，此方法有別於之前的介紹，比較通用，因為都自己刻的。
* 資料庫我故意不用.mdf建資料庫，xampp模擬用連線的方式須輸入帳密才能用資料庫。


| 套件 | 版本 |來源|
| -------- | -------- |-------- |
| pagination     | 2.4.2     | [連結](https://pagination.js.org/dist/2.4.2/pagination.css)|
| modernizr     | 2.8.3     | NuGet載|
| jquery     | 3.4.1     |NuGet載|
| bootstrap     | 4.3.1    | [連結](https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js)|
| popper     | 1.14.7   | [連結](https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js)|

## XAMPP建置資料庫 
### 1. 安裝XAMPP : ([連結](https://www.apachefriends.org/zh_tw/download.html))
![](https://i.imgur.com/UgjP3tm.png)
:::danger
記得要開著不然連不進資料庫喔!!
:::
### 2. 建置資料庫，新增會員table
![](https://i.imgur.com/kw1mENH.png)

* State : True(1)沒停權，False(0)停權
* permissions : CRUD 先分成3種角色
    * 管理者 : CRUD
    * 一般會員 : R
    * 廠商 : CR

### 3. 新增權限
![](https://i.imgur.com/zjn4gXy.png)
####
![](https://i.imgur.com/6hIvI0W.png)
* 記住帳號密碼

### 4. Web.config : 設定資料庫連線帳密
####
```
<connectionStrings>
    <add name="dbConnectionString" connectionString="datasource=127.0.0.1;port=3306;username=yoyo;password=yoyo;database=dbMember;" />
</connectionStrings>
<system.web>
```
> * name="dbConnectionString" : 連線名稱取名dbConnectionString
> * connectionString : 連線資料庫字串
> * datasource= xxx : localhost的ip
> * port : 可以再Xampp上看到MySQL有
> ![](https://i.imgur.com/lUVThjD.png)
> * username= xxx;password= xxx : 帳密
> * database= xxx : 資料庫名稱


### 5. 先來撈資料看看
#### test.aspx
```
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="Day18.test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
        <div id="mainDiv" runat="server">
            <!--塞值區域-->
        </div>
    </form>
</body>
</html>
```
#### test.aspx.cs
```
string connectionString = WebConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;
string query = "SELECT * FROM tmember";
string str = "<table>";
DataTable dt = new DataTable();
MySqlConnection databaseConnection = new MySqlConnection(connectionString);
MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
commandDatabase.CommandText = query;
commandDatabase.CommandTimeout = 60;
try{ 
    commandDatabase.Connection = databaseConnection;
    using (MySqlDataAdapter MySqla = new MySqlDataAdapter(commandDatabase))
    {
        MySqla.Fill(dt);
    }
    //每一行
    for (int i=0; i < dt.Rows.Count;i++)
    {
        str += "<tr><td style='color:red'>";
        //欄位數量
        for (int j = 0; j < dt.Columns.Count; j++)
        {
            str += "<td style='color:red'>" + dt.Rows[i][j].ToString() + "</td>";
        }
        str+= "</td></tr>";
    }
    str += "</table>";
    //<div id="mainDiv" runat="server">
    mainDiv.InnerHtml = str;
    //databaseConnection.Close();
    Label1.Text = "success";
    commandDatabase.Connection.Close();
    commandDatabase.Connection.Dispose();
}
catch (Exception ex)
{
    Label1.Text = ex.Message;
}
```
#### 結果:
![](https://i.imgur.com/sSIKxWm.png)

OK，有連到了~~
> * 每個SQL連的方式不太一樣，我們是用 using MySql.Data.MySqlClient，用內建DB的或MSSQL用using System.Data.SqlClient;
> * 連線流程架構
    > 1.MySqlConnection databaseConnection
    > 2.MySqlCommand commandDatabase 
    > 3.commandDatabase.CommandText = SQL指令
    > 4.commandDatabase.Connection 
    > 5.MySqlDataAdapter取值放到Datatable
    > 6.commandDatabase.Connection.Close();
    > 7.commandDatabase.Connection.Dispose();
---

## dbCommand : 後端串資料庫指令執行程序
###
因為呼叫資料庫是蠻常做的把這功能寫成一個類別，負責MySqlCommand(執行sql指令)、MySqlConnection(資料庫連線)，主要寫接收 MySqlCommand 或 sql字串，執行完最後一定鐵定鋼釘鑽石定要把 SqlCommand 釋放與關掉，不然會超級究級無敵麻煩。

### 新增類別 dbCommand.cs
![](https://i.imgur.com/9GN3mbg.png)
#### ExecuteSQL : 執行SQL
```
using MySql.Data.MySqlClient;
...
namespace Day18.App_Code
{
    public class dbCommand
    {
        public dbCommand()
        {
            //
            //  在此加入建構函式的程式碼
            //
        }
        public static MySqlConnection conn()
        {
            return new MySqlConnection(WebConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString);
        }
        //接收Command，回傳執行SQL成功與失敗，通常用這個比較多
         public static bool ExecuteSQL(MySqlCommand command)
        {
            bool state = false;
            if (command.ToString() != "" && command.ToString() != null)
            {
                using (MySqlConnection sqlconnection = new MySqlConnection(WebConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                {
                    try
                    {
                        command.Connection = sqlconnection;
                        command.Connection.Open();
                        command.ExecuteNonQuery();
                        state = true;
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                    command.Connection.Close();
                    command.Connection.Dispose();
                }
            }
            return state;
        }
        //接收sql指令，回傳執行SQL成功與失敗
        public static void ExecuteSQL(string queryString)
        {
            if (queryString != "" && queryString != null)
            {
                using (MySqlConnection sqlconnection = new MySqlConnection(WebConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                {
                    using (MySqlCommand command = new MySqlCommand(queryString, sqlconnection))
                    {
                        command.Connection.Open();
                        command.ExecuteNonQuery();
                        command.Connection.Close();
                        command.Connection.Dispose();
                    }
                }
            }
        }
    }
}
```

#### GetTable : 取資料庫內容
```
// 根據SQL取資料庫內容
public static DataTable GetTable(MySqlCommand command)
{
    DataTable data = new DataTable();
    using(MySqlConnection sqlconnection = new MySqlConnection(WebConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
    {
        command.Connection = sqlconnection;
        using (MySqlDataAdapter MySqla = new MySqlDataAdapter(command))
        {
            MySqla.Fill(data);
        }
        command.Connection.Close();
        command.Connection.Dispose();
    }
    return data;
}
public static DataTable GetTable(string sql)
{
    DataTable data = new DataTable();
    if (sql != "" && sql != null)
    {
        using (MySqlConnection sqlconnection = new MySqlConnection(WebConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
        {
            using (MySqlDataAdapter MySqla = new MySqlDataAdapter(sql, sqlconnection))
            {
                MySqla.Fill(data);
            }
            sqlconnection.Close();
            sqlconnection.Dispose();
        }
    }
    return data;
}
```
:::success
* [DataTable基礎認識](https://www.dotblogs.com.tw/chjackiekimo/2014/04/03/144606)
:::
## Web 服務 asmx : CRUD ㄌㄨㄥ裡加

寫好了與資料庫的連線，再來是寫個Web API，有CRUD的功能都往這裡寫。
### 1. 新增 asmx
![](https://i.imgur.com/ScBZlEO.png)

### 2. 介紹asmx
我們要用非同步的方式進行，所以會用Ajax來呼叫這裡，記得把這裡的註解用掉
```
// 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
[System.Web.Script.Services.ScriptService]
```
> 以下看一下，比較知道他跟IIS的關係是甚麼...
> * [Asp .NET 神奇的WebMethod](https://dotblogs.com.tw/justforgood/2016/10/21/172354)
> * [http://tempuri.org 是什麼？](https://www.796t.com/content/1550507059.html)
> * [web service介面 wsdl和asmx有什麼區別](https://www.796t.com/content/1542725346.html)
> * [WebService-(asmx)發布至IIS記錄(VS2019)](https://www.cnblogs.com/dirtyboy/p/15743895.html)

### 3. WebService1.asmx : LoginMember 登入會員
###
```
#region 登入會員
[WebMethod(EnableSession = true)]
public void LoginMember(string Account, string Password)
{
    string mMsg = "";
    MySqlCommand Cmd = new MySqlCommand();
    //注意WHERE後面的=不要有空格
    //第一個@是字串特殊字元不用加\，剩下的@是增加可讀性
    Cmd.CommandText = @"SELECT ID,Account,Name,Permissions FROM tmember WHERE Account=@Account AND Password=@Password AND State=@State";
    Cmd.Parameters.AddWithValue("@Account", Account);
    Cmd.Parameters.AddWithValue("@Password", Password);
    Cmd.Parameters.AddWithValue("@State", 1);
    DataTable dt = dbCommand.GetTable(Cmd);
    if (dt.Rows.Count > 0)
    {
        HttpContext.Current.Session.Add("userid", dt.Rows[0][0].ToString()); ////User.Identity.Name
        HttpContext.Current.Session.Add("username", dt.Rows[0][2].ToString());
        HttpContext.Current.Session.Add("permissions", dt.Rows[0][3].ToString());
        mMsg = "success";
    }
    else mMsg = "fail";   
    Context.Response.Write(new JavaScriptSerializer().Serialize(mMsg));
    Context.Response.End();
}
#endregion
```
> * [SessionID.cookie,Session傻傻分不清楚??](https://dotblogs.com.tw/daniel/2017/04/08/110915)
> * [Asp.net Session (Session核心原理)](https://dotblogs.com.tw/daniel/2017/11/07/220532)
> * [WebMethod(EnableSession=true)??)](https://www.cnblogs.com/weck0736/archive/2008/01/16/1041340.html)
> * [ASP.NET在webservice中使用session和cookie](http://vcsos.com/article/pageSource/140725/20140725200519.shtml)

:::    danger
[usin Day18.App_Code引入問題!!](https://learn.microsoft.com/zh-tw/previous-versions/0c6xyb66(v=vs.110)?redirectedfrom=MSDN#建置動作屬性)
![](https://i.imgur.com/ayUf6LI.png)
78哩，找超久原來是這個在搞
:::

### 4. frontLogin.js : Ajax呼叫API
#### 
* 登入防呆機制確認
* 登入成功可以到 Index 頁面
```
$(document).ready(function () {
    $(".loginBtn").click(function () {
        var temp = true;
        if (!$('.account').val()) {
            alert('信箱欄位不能為空');
            temp = false;
            return;
        }
        if (!checkEmail($('.account').val())) {
            alert('信箱格式錯誤');
            temp = false;
            return;
        }
        if (!$('.password').val()) {
            alert('密碼欄位不能為空');
            temp = false;
            return;
        }
        if (temp) {
            $.ajax({
                type: "POST",
                url: "/WebService1.asmx/LoginMember",
                data: {
                    Account: $('.account').val(),
                    Password: $('.password').val(),
                },
                contentType: 'application/x-www-form-urlencoded',
                dataType: 'text',
                success: function (response) {
                    //防雙引號""
                    response = response.replace(/^\"|\"$/g, '');       
                    if (response == "success") {
                        location.href = "/index.aspx"
                    } else {
                        alert("登入失敗");
                    }
                },
                error: function (err) {
                    console.log(err);
                }
            });
        }
    });
});

function checkEmail(email) {
    //正則表達示
    var emailRegxp = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/; //簡單的驗證
    if (emailRegxp.test(email) != true)
        return false;
    else 
        return true;
}
```
> * [嚴謹的信箱驗證](https://ithelp.ithome.com.tw/articles/10094951)，[這裡測試](https://www.runoob.com/try/try.php?filename=tryjsref_regexp5)

### 5. frontlogin.aspx
```
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
```
* 主要用class的name來取值
* 記得引入 frontLogin.js

### 6. Index.aspx : 首頁
####
先一般的hello就好，之後寫顯示能用的網站
```
<a class="nav-link" href="#"><%=Session["username"] %> Hello~~</a>
```

### 7. member.aspx : 顯示會員清單
####
* 進來先看看權限是什麼
```
<%---------------------------- 載入 CSS 區域 ----------------------------%>
<%-- pagination.js套件 --%>
<link href="https://pagination.js.org/dist/2.4.2/pagination.css" rel="stylesheet" type="text/css">
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-wEmeIV1mKuiNpC+IOBjI7aAzPcEZeedi5yW5f2yOq55WWLwNGmvvx4Um1vskeMj0" crossorigin="anonymous">

 <% if (Session["permissions"].ToString().Contains("R"))
{%>
    <%--會員文字 + 搜尋框--%>
            ...
    <%--表格--%>
            ...
    <!-- 訊息Modal -->
            ...
    <!-- 編輯Modal -->
            ...
<%}%>
 <% else{%>
    <a href="./frontlogin.aspx" class="nav-link">請先登入</a>
 <%}%>
```
* 會員文字+搜尋框 : 接著再有權限R裏頭顯示會員清單，搜尋功能順便寫
```
<%--會員文字 + 搜尋框--%>
<div class="textTop ">
    <h2>會員管理</h2>
    <div class="searchBox">
        <p>共有 <span></span> 位會員</p>
        <div class="searchFlex">
            <input class="search" type="search" name="txtSearch" id="txtSearch" value="" placeholder="請輸入姓名關鍵字" />
            <span class="searchIcon">
                <img src="./assets/images/searchIcon.svg" alt="" onclick="GetData();" />
            </span>
        </div>
    </div>
</div>
```
* 表格 : 顯示會員清單
```
<%--表格--%>
<div class="wrapper">
    <div class="form table-responsive ">
        <table class="table table-hover">
            <thead class="text-nowrap">
                <tr>
                    <th scope="col">會員編號</th>
                    <th scope="col">姓名</th>
                    <th scope="col">帳號(信箱)</th>
                    <th scope="col">創立時間</th>
                    <th scope="col">會員權限</th>
                    <th scope="col">會員狀態</th>
                    <th class="editTd" scope="col">編輯功能</th>
                </tr>
            </thead>
            <tbody class="memberTbody">
                <%-- 表單內容-表格 --%>
            </tbody>
        </table>
    </div>
    <%--分頁的頁碼--%>
    <div id="pagination-container"></div>
    <%--使用pagination套件--%>
</div>
...
...
...
<%---------------------------- 載入 JS 區域 ----------------------------%>
<%-- pagination.js套件 --%>
<script src="https://pagination.js.org/dist/2.4.2/pagination.min.js"></script>
<%--自己寫的 JS--%>
<script src="./assets/js/member.js"></script>
```
::: info
* [pagination.js套件](https://bootstrap5.hexschool.com/docs/5.0/components/pagination/)
:::
## member.js : 會員清單操作事件功能撰寫
### 1.member.js : ajax取資料
```
/**網頁一開始先做 **/
$(document).ready(function () {
    /*-------------- 導覽列(會員管理)設為 on --------------*/
    $(".member").addClass("on");
    GetData();
});

/** 抓取會員列表*/
...
/** 修改會員狀態*/
...
/** 抓取會員資料*/
...
/** 刪除會員資料*/
```
### 2.member.js : 抓取會員列表
資料格式是照著 pagination.js 的格式，dataSource、pageSize、callback
```
/** 抓取會員列表*/
function GetData() {
    /*分頁功能(使用pagination套件)*/
    const memberTbody = document.querySelector(".memberTbody");
    $("#pagination-container").pagination({
        dataSource: function (done) {
            $.ajax({
                type: "GET",
                url: "/WebService1.asmx/GetMemberList",
                dataType: "json",
                data: {
                    keyword: $('#txtSearch').val()
                },
                success: function (response) {
                    $('#txtSearch').val(''); //重置搜尋欄
                    const thisData = response;

                    /*共有幾位會員*/
                    $('.searchBox > p > span').html(thisData.length);
                    
                    if (thisData.length == 0) {
                        memberTbody.innerHTML = '';
                        $('#msgText').html('查無相關資料');
                        $('#MsgModal').modal("show");
                        return;
                    }
                    done(thisData); // 將資料回傳到下方 callback (預設初始)
                },
            });
        },
        pageSize: 10, // 一頁幾筆資料
        callback: function (data, pagination) {
            const thisData = data;
            let str = "";
            //*組字串並渲染到畫面上*/
            thisData.forEach(function (item, index) {
                var state_html = `<td class="state"><a key=${item.ID} class="stateBtn btn btn-m btn-success ${item.State[0]}" href="javascript:void(0);">${item.State[1]}</a></td>`;
                if (item.State[0] == 'off') state_html = `<td class="state"><a key=${item.ID} class="stateBtn btn btn-m btn-danger ${item.State[0]}" href="javascript:void(0);">${item.State[1]}</a></td>`;
                str += `
                        <tr class="align-middle">
                          <td scope="row">${item.ID}</td>
                          <td>${item.Name}</td>
                          <td class="account">${item.Account}</td>
                          <td>${item.CreateTime}</td>
                          <td class="permissionTd">${item.Permissions}</td>` + state_html +
                        `<td class="editTd">
                            <a class="btn btn-outline-secondary" href="javascript:void(0);" onclick=openEdit('${item.ID}','${item.Permissions}');>編輯</a>
                            <a class="btn btn-danger" href="javascript:void(0);" onclick=openDelete('${item.ID},'${item.Permissions}');>刪除</a>
                          </td>
                        </tr>`;
                memberTbody.innerHTML = str;
            });
        },
    });
}
```
::: success
* [.done()](https://powerkaifu.github.io/2020/10/12/lesson-jq-08.ajax/) : 當資料請求成功時，須執行的程式區段，替代了過去的.success ()
:::
## WebService1.asmx
### 8. GetMemberList 取得會員清單
```
#region 登入會員
#region 取得會員清單
[WebMethod]
public void GetMemberList(string keyword)
{
    MySqlCommand Cmd = new MySqlCommand();
    DataTable dt = new DataTable();
    Cmd.Parameters.AddWithValue("@KeyWord", "%" + keyword+ "%");
    Cmd.CommandText = @"SELECT * FROM tmember ";
    //沒搜尋
    if (!keyword.Equals("")) Cmd.CommandText += @"WHERE Name Like @KeyWord ";
    Cmd.CommandText += @"ORDER BY ID";
    dt = dbCommand.GetTable(Cmd);
    // 轉json傳資料
    List<object> list = new List<object>();
    foreach(DataRow dr in dt.Rows)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("ID", dr["ID"].ToString());
        dict.Add("Name", dr["Name"].ToString());
        dict.Add("Account", dr["Account"].ToString());
        dict.Add("CreateTime", dr["CreateTime"].ToString());
        dict.Add("Permissions",dr["Permissions"].ToString());
        if(dr["State"].Equals(1)) dict.Add("State", new string[] { "on", "正常" }); //on:給js看，正常:給人看
        else                      dict.Add("State", new string[] { "off", "停權" });
        list.Add(dict);
    }
    Context.Response.Write(new JavaScriptSerializer().Serialize(list)); //轉json 
    Context.Response.End();
}
#endregion
```
::: success
* [SQL LIKE](https://www.1keydata.com/tw/sql/sqllike.html) : SQL相似字搜尋
* 記得在web.config加入這段!!!!!
```
    <webServices>
      <protocols>
        <add name="HttpPost"/>
        <add name="HttpGet"/>
      </protocols>
    </webServices>
</system.web>
```
:::
## 結果
![](https://i.imgur.com/B5KVrZh.png)

## 結論
* 今日先這樣，主要先熟悉將通用的功能(SQL執行，CRUD)寫在固定的地方，之後再專注在那個網頁上發生的事情，需要資料或網站發生事件時，用非同步的方式(ajax)傳值給asmx，asmx在執行SQL指令得到結果，Response到ajax，ajax如果取到值success中處裡後續的事情。
* 現在有很多別人開發完的功能，如頁碼功能(pagination.js/.css)、特殊按鍵(bootstrap.js)，但注意如果引入js、css的方式是超連結，要小心那邊超連結失效的問題；那如果是下載的話，之後要記得套件的需要性，不然會引用太多到時候維護起來會超痛苦。
* 可以先畫圖設計網站該有的東西，每個功能需要用到的方式，顯示的東西重要的程度，比起顯示給人用在看權限，不如一開始就不要顯示給沒權限的人用。

## 補充
* [How to connect to MySQL with C# Winforms and XAMPP](https://ourcodeworld.com/articles/read/218/how-to-connect-to-mysql-with-c-sharp-winforms-and-xampp)

## 有遇到的bug
* [預定義的類型“System.Object”未定義或未導入](https://blog.csdn.net/Britripe/article/details/105853723)
* [Bootstrap throws Uncaught Error: Bootstrap's JavaScript requires jQuery](https://stackoverflow.com/questions/22658015/bootstrap-throws-uncaught-error-bootstraps-javascript-requires-jquery)
* ajax 有問題 : 
    1. 有沒有引入 jQuery
    2. 使用2個以上 jQuery，調整成一個