# ASP .NET MVC 上手Day21(Session、Cookie?)第三方支付完後，會自動登出??

###### tags: `asp.netMVC` `C#` `30日挑戰`

會使用到Session或Cookie的狀況，可以記得已登入的資訊、購物車內容、最近瀏覽...等等，會需要先將值暫時放在Session或Cookie的狀況。

那我們就拿Day20一樣的例子(Ecpay)，特別的是我們送出後會進入Ecpay的網頁，操作完後才在跳回到我們的網站，看看用Session、Cookie來確定登入的狀況，以及第三方支付的方式跳轉回來後自動登出的問題~

那一開始我們先來了解Session、Cookie

## Session

英文的解釋大致就知道意思，"具有狀態的一段期間"，記憶狀態發生了什麼事情

***一種讓Request變成stateful的機制***


* [stateful機制](https://medium.com/andy-blog/kubernetes-那些事-stateless-與stateful-2c68cebdd635)
![](https://hackmd.io/_uploads/rJVw8BQ32.png)
簡單來說就是每一次Request都會被記錄起來，以後還可以做存取，想像成類似資料庫


在 Cookie 還沒出現以前，可以建立 Session，可以把狀態資訊放在網址上面或藏在 form 表單中

## Cookie

![](https://hackmd.io/_uploads/Bkqn3SXh2.png)

把Request的狀態，用Cookie存起來(Set-Cookie)，有Cookie之後實作Session非常方便

可看看這篇 : [白話 Session 與 Cookie：從經營雜貨店開始](https://hulitw.medium.com/session-and-cookie-15e47ed838bc)、[Cookie 和 Session 究竟是什麼？有什麼差別？](https://tw.alphacamp.co/blog/cookie-session-difference)

本篇就來實作一下Session跟Cookie的用法

### Cookie限制!!

| Cookie | 限制 | 
| -------- | -------- | 
| 大小   | 單個4K     | 
| 個數   | 一個domain下最多20個     |


## 實作1 -> System.Web.Security.FormsAuthentication

### 資料庫
* 先來建立簡單的會員資料庫，本篇主要講Session、Cookie所以直接輸入帳號密碼
![](https://hackmd.io/_uploads/B1_A_DQ32.png)

### 設定有登入才看的到
MVC CRUD 可以參考我[Day09](https://kroy1002.medium.com/asp-net-mvc-上手-day09-會員登入機制-940fb915ba0f)、[Day12](https://kroy1002.medium.com/asp-net-mvc-上手-day12-網路服務web-api-非同步會員管理系統-b2fbd3781f2d)、[Day17](https://kroy1002.medium.com/asp-net-web應用程式上手-net-framework-day-17-asp-net-framework內建的crud-bb418cccb2f1)的文章

### Home1Controller.cs : login(string account, string password) 
```
 //登入
[HttpPost]
public ActionResult login(string account, string password) 
{
    Database1Entities1 db = new Database1Entities1();
    var member = db.member.Where(m => m.account == account && m.password == password).FirstOrDefault(); //查詢
    if (member != null)
    {
        FormsAuthentication.RedirectFromLoginPage(member.account, true); //cookie驗證 : https://blog.miniasp.com/post/2008/02/20/Explain-Forms-Authentication-in-ASPNET-20
        TempData["IsLogin"] = true;
        return RedirectToAction("Buycart", "Home1"); //action, controller
    }
    ViewBag.IsLogin = true;                                          //https://stackoverflow.com/questions/34447310/any-reason-why-my-viewbag-is-not-working
    return View();
}
```
* 接收 account 和 password
* 用 LINQ 到資料庫查有無資料

> 補充
> * FormsAuthentication.RedirectFromLoginPage 簡單來說就是在cookie塞值[[更多介紹!!](https://blog.miniasp.com/post/2008/02/20/Explain-Forms-Authentication-in-ASPNET-20)]
> ```
> <system.web>
>     <authentication mode="Forms">
>       <forms loginUrl="~/Home1/login"></forms>
>     </authentication>
> </system.web>
> ```
> ![](https://hackmd.io/_uploads/HJ3S59_n2.png)
> * ViewBag 重新整理會不見
> * [ASP.NET Form 驗證 .ASPXAUTH Cookie 行為深入觀察](https://blog.darkthread.net/blog/aspxauth-cookie-timeout/)

### Views -> login.cshtml的 form
```
<form action="@Url.Action("login")" method="post">
    <div class="container" style="margin-top:20px;">
        <div class="row">
            <div class="panel panel-primary">
                <div class="panel-heading">會員登入</div>
                <div class="form-group">
                    <label for="account ">帳號 : </label>
                    <input type="text" class="form-control" id="account" name="account" />
                </div>
                <div class="form-group">
                    <label for="password">密碼 : </label>
                    <input type="password" class="form-control" id="password" name="password" />
                </div>
                <input type="submit" value="登入" class="btn btn-primary" />
                @if (ViewBag.IsLogin != null)
                {
                    <div class="alert alert-danger">
                        <strong>密碼錯誤!</strong> 請重新登入
                    </div>
                }
            </div>
        </div>
    </div>
</form>
```
* @Url.Action("login") post 到後端
* ViewBag.IsLogin == true 才可登入

### Home1Controller.cs : Buycart
```
//登入後進
[Authorize]
public ActionResult Buycart()
{ 
    ViewBag.IsLogin = TempData["IsLogin"];                               //RedirectToAction : Home收不到Index的ViewBag，TempData 會將值存在seesin 。https://www.codeproject.com/Articles/476967/What-is-ViewData-ViewBag-and-TempData-MVC-Option-2
    var orderId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);
    var website = $"https://localhost:44354/"; //記得確認一下數字有沒有一樣
    var order = new Dictionary<string, string>
    {
        { "MerchantTradeNo",  orderId},
        { "MerchantTradeDate",  DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")},
        { "TotalAmount",  "100"},
        { "TradeDesc",  "無"},
        { "ItemName",  "測試商品"},
        { "ExpireDate",  "3"},
        { "CustomField1",  ""},
        { "CustomField2",  ""},
        { "CustomField3",  ""},
        { "CustomField4",  ""},
        { "ReturnURL",  $"{website}/api/Ecpay/AddPayInfo"},
        { "OrderResultURL", $"{website}/Home/PayInfo/{orderId}"},
        { "PaymentInfoURL",  $"{website}/api/Ecpay/AddAccountInfo"},
        { "ClientRedirectURL",  $"{website}/Home/AccountInfo/{orderId}"},
        { "MerchantID",  "2000132"},
        { "IgnorePayment",  "GooglePay#WebATM#CVS#BARCODE"},
        { "PaymentType",  "aio"},
        { "ChoosePayment",  "ALL"},
        { "EncryptType",  "1"},
    };
    order["CheckMacValue"] = GetCheckMacValue(order);
    return View(order);
}
private string GetCheckMacValue(Dictionary<string, string> order)
{
    var param = order.Keys.OrderBy(x => x).Select(key => key + "=" + order[key]).ToList();
    var checkValue = string.Join("&", param);
    //測試用的 HashKey
    var hashKey = "5294y06JbISpM5x9";
    //測試用的 HashIV
    var HashIV = "v77hoKGq4kWxNNIS";
    checkValue = $"HashKey={hashKey}" + "&" + checkValue + $"&HashIV={HashIV}";
    checkValue = HttpUtility.UrlEncode(checkValue).ToLower();
    checkValue = GetSHA256(checkValue);
    return checkValue.ToUpper();
}
private string GetSHA256(string value)
{
    var result = new StringBuilder();
    var sha256 = SHA256Managed.Create();
    var bts = Encoding.UTF8.GetBytes(value);
    var hash = sha256.ComputeHash(bts);
    for (int i = 0; i < hash.Length; i++)
    {
        result.Append(hash[i].ToString("X2"));
    }
    return result.ToString();
}
```
這裡就是登入才能進入的頁面，除非登出。

### Views -> Shared -> _Layout.cshtml
母框加登出
```
<div class="navbar-collapse collapse">
    <ul class="nav navbar-nav">
        <li>@Html.ActionLink("登出", "Index_logout", "Home")</li>
    </ul>
</div>
```

### Home1Controller -> Index_logout 
登出
```
public ActionResult Index_logout()
{
    FormsAuthentication.SignOut(); //清除cookie
    return RedirectToAction("Index");
}
```

### Home1Controller -> PayInfo(FormCollection id)
綠界回傳
```
[HttpPost]
//[Authorize] //會出問題QQ
public ActionResult PayInfo(FormCollection id)
{
    var data = new Dictionary<string, string>();
    foreach (string key in id.Keys)
    {
        data.Add(key, id[key]);
    }
    return View("EcpayView", data);
}
```

### 發現綠界回傳後會沒登入
 
如果在 PayInfo 上面多加 [Authorize] ，綠界跳回來 ViewBag 就會被洗掉，然後變成自動登出了。

從網址可以知道，這是沒登入想進到有 [Authorize] 會出現的樣子，其實綠界是有傳給我們東西的，但進不去我們的網頁，因此把 [Authorize] 拿掉就沒這問題。

![](https://hackmd.io/_uploads/BkX4iH7Th.png)

但是，我還是想要加入，多一層保護~

---

## 實作2 -> ASP.NET_SessionId 保持登入

### 多token保持登入狀態(Session)，自己刻一個登入確認
有時候用內建的還要研究(ex:Authorize、FormsAuthentication)，倒不如自己刻一個，也比較好知道邏輯，到時有問題也比較知道怎麼D霸個。

### 順序
![](https://hackmd.io/_uploads/H1Fio9F0n.png)

### CookieHelper.cs : SetCookie、GetCookie、RemoveCookie

自己寫一個cookie的CRUD，可以設定有效時間
```
public static void SetCookie(string cookieName, string value, DateTime expires)
{
    HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
    if (cookie != null)
    {
        cookie.Value = value;
        cookie.Expires = expires;
        HttpContext.Current.Response.Cookies.Add(cookie);
    }
    else
    {
        cookie = new HttpCookie(cookieName);
        cookie.Value = value;
        cookie.Expires = expires;
        HttpContext.Current.Response.Cookies.Add(cookie);
    }
}
public static string GetCookieValue(string cookieName)
{
    HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
    if (cookie == null || string.IsNullOrEmpty(cookie.Value))
        return "";
    else
        return cookie.Value;
}
public static void RemoveCookie(string cookieName)
{
    SetCookie(cookieName, "", DateTime.Now.AddDays(-1));
}                                                              
```

### Home2Controller.cs : login()
進入login先清掉Session
```
public ActionResult login()
{
    //Session.Abandon(); 
    CookieHelper.SetCookie("ASP.NET_SessionId", "", DateTime.Now.AddMinutes(1));
    return View();
}
//登入
[HttpPost]
public ActionResult login(string account, string password)
{
    Database1Entities1 db = new Database1Entities1();
    var member = db.member.Where(m => m.account == account && m.password == password).FirstOrDefault(); //查詢
    if (member != null)
    {
        CookieHelper.SetCookie("Islogin", "true", DateTime.Now.AddMinutes(1));
        return RedirectToAction("Buycart", "Home2"); //action, controller
    }
    return View();
}
```
* 這裡清理是因為ASP MVC再啟動時會自動產生ASP.NET_SessionId[[介紹](https://blog.csdn.net/weixin_46879188/article/details/122133353)]
* 發現放在Cookie的ASP.NET_SessionId在綠界跳轉回來後依然存在，所以為了能透夠過這東西來確認保持登入，先重置成""，在登入完後ASP.NET_SessionId會再自動產生新的值。

> 補充 : 
> * [Samesite Cookie綠界技術問題](https://www.ecpay.com.tw/CascadeFAQ/CascadeFAQ_Qa?nID=3914)
> * [購物網站串接第三方API後，購物網站的SESSION被清空](https://ithelp.ithome.com.tw/questions/10201666)
> * [Session.RemoveAll() 及Session.Abandon() 的差別](https://alen1985.pixnet.net/blog/post/26299563)


### token.cs : 登入時紀錄 IsLogin、ASP.NET_SessionId
![](https://hackmd.io/_uploads/BkgzGsKCn.png)

再登入後，ASP.NET_SessionId自動產生值，設定Islogin為true

```
[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
public class token : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        //可以在中繼點觀察一下cookie的變化
        string temp = CookieHelper.GetCookieValue("IsLogin"); //可用中繼點看變化
        string NET_SessionId = CookieHelper.GetCookieValue("ASP.NET_SessionId");
        //跳轉回來登入保持
        if (NET_SessionId != null) HttpContext.Current.Session["IsLogin"] = "true";
    }
}
```

> 補充:
> * [客製 ActionFilterAttribute](https://jeffprogrammer.wordpress.com/2015/10/23/asp-net-mvc-%E5%AE%A2%E8%A3%BD-actionfilterattribute/)
### Home2Controller.cs : Buycart()
```
//Islogin==true才能進
[LoginFilter]
public ActionResult Buycart()
{
    ...跟實作1一樣
}
private string GetCheckMacValue(Dictionary<string, string> order)...
private string GetSHA256(string value)...
```
LoginFilter 判斷是否有登入

### LoginFilter.cs
```
[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
public class LoginFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        string check_login = CookieHelper.GetCookieValue("Islogin");
        // 存Session中，跳轉回來會被洗掉，因此順序是先進token再到這裡，在token中重新刻 Islogin
        if (check_login != "true") HttpContext.Current.Session["Islogin"] = check_login;
        var sessionName = HttpContext.Current.Session["Islogin"]; //可用中繼點看變化，如果跳轉回來不會有Islogin存在
        if (sessionName == null || sessionName.ToString() == "") filterContext.Result = new RedirectResult("/Home2/login");
    }
}
```
跳轉回來後，Session的Islogin會被洗掉，因此會自動登出，所以在token那會把洗掉的Islogin寫回來，這裡就不會登出了。
### Home2Contrller.cs : PayInfo()
```
[LoginFilter]
public ActionResult PayInfo(FormCollection id)
{
    ...跟實作1一樣
}
```

### Home2Contrller.cs : Index_logout()
```
public ActionResult Index_logout()
{
    Session.Clear();
    CookieHelper.SetCookie("Islogin", "", DateTime.Now.AddDays(-1));
    return RedirectToAction("login");
}
```
### 綠界回傳
![](https://hackmd.io/_uploads/Hk-0xs4an.png)

挖賽，真的沒登出了，太好了>0<~


END