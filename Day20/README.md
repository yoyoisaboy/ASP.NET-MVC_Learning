# ASP .NET MVC 上手Day20(串綠界金流)

###### tags: `asp.netMVC` `C#` `30日挑戰`

本篇記錄用 ASP .NET MVC 串綠界金流的過程。比較容易遇到問題的地方有兩個:
1. 綠界的respone資料型態(信用卡、超商條碼...)，本篇會舉兩種接收方式，用Controller的[HttpPost]和用ApiController方法(WebAPI)。
> 其實兩者都可以用，不一定都要用哪一種，此篇主要是記錄我開發過程的紀錄
3. 模擬付款這測試方式須注意你所測試的環境IP是要對外的，127.0.0.1/localhost是收不到的喔!! 我有跑去問綠界那邊才恍然大悟。

## 何謂綠界
* [綠界科技公司簡介](https://www.ecpay.com.tw/About/Introduction)
* [綠界科技全方位金流API技術文件](https://developers.ecpay.com.tw/?p=2509)
## 測試的後台
1. 測試後台
    * 網址：https://vendor-stage.ecpay.com.tw
    * 特店編號：2000132
    * 廠商管理後台登入帳號：stagetest1234
    * 廠商管理後台登入密碼：test1234
    * 身分證件末四碼/統一編號：53538851
    * HashKey：5294y06JbISpM5x9
    * HashIV：v77hoKGq4kWxNNIS
2. 測試信用卡
    * 卡號：4311-9522-2222-2222
    * 安全碼：222
    * 信用卡測試有效月/年：MM/YYYY 值請大於現在當下時間的月/年

## 方法一 : 用 Controller 接收 Client post

### 1. HomeController.cs : Index
* 將資料準備好傳到前端顯示
```
//step1 : 網頁導入傳值到前端
public ActionResult Index()
{
    var orderId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);
    //需填入你的網址
    var website = $"https://localhost:44325/";
    var order = new Dictionary<string, string>
    {
        //綠界需要的參數
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
    //檢查碼
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
### 2. Index.cshtml
* 後端傳值到前端把form的值補上，按submit出去。
```
@model System.Collections.Generic.Dictionary<string, string>
@{ Layout = null; }
<!DOCTYPE html>
<html>
<body>
    <form id="form" name="form" method="POST" action="https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut/V5">
        <!--step2 : 收到後端的值印出來-->
        @foreach (var key in @Model.Keys.ToList())
        {
            @(key) <input type="text" name="@key" value="@Model[key]" /><br />
        }
        <button type="submit" id="checkoutBtn">送出</button>
    </form>

    <!-- js套件 -->
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/5.1.3/js/bootstrap.bundle.min.js"></script>
    <script src="https://code.jquery.com/jquery-3.5.1.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/jquery-twzipcode@1.7.14/jquery.twzipcode.min.js"></script>
    <!-- 自己的 -->
    <script src="../../assets/js/ecpay.js"></script>
</body>
</html>
```
![](https://i.imgur.com/qH6ltdn.png)

* 自動跳轉到綠界介面照步驟走，此範例是走信用卡支付，因此最後post到 "OrderResultURL"

![](https://i.imgur.com/BnzGbHU.png)

### 3. 按下按鈕，上傳到資料庫，先來新增資料庫的table
* 在App_Data建一個資料庫，Models建立實體資料庫模型，欄位就看你要哪些。
```
CREATE TABLE [dbo].[EcpayOrders] (
    [MerchantTradeNo]      NVARCHAR (50) NOT NULL,
    [MemberID]             NVARCHAR (50) NULL,
    [RtnCode]              INT           NULL,
    [RtnMsg]               NVARCHAR (50) NULL,
    [TradeNo]              NVARCHAR (50) NULL,
    [TradeAmt]             INT           NULL,
    [PaymentDate]          DATETIME      NULL,
    [PaymentType]          NVARCHAR (50) NULL,
    [PaymentTypeChargeFee] NVARCHAR (50) NULL,
    [TradeDate]            NVARCHAR (50) NULL,
    [SimulatePaid]         INT           NULL,
    CONSTRAINT [PK_EcpayOrders] PRIMARY KEY CLUSTERED ([MerchantTradeNo] ASC)
);
```
### 4. ecpay.js : 送出checkoutBtn

```
$("#checkoutBtn").on('click', (e) => {
    //e.preventDefault(); //因為送出就跳轉到綠界，這個可以停住確認自己的console.log的內容

    let formData = $("#form").serializeArray();
    var json = {};
    $.each(formData, function () {
        json[this.name] = this.value || "";
    });

    console.log(json);

    //step3 : 新增訂單到資料庫
    $.ajax({
        type: 'POST',
        url: 'https://localhost:44325/api/Ecpay/AddOrders',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(json),
        success: function (res) {
            console.log(res);
        },
        error: function (err) { console.log(err); },
    });

});
```
* 注意前端Index.cshtml要引入js喔!!

![](https://i.imgur.com/D4qfoJD.png)

### 5. 後端 EcpayController.cs : AddOrders
預設訂單產生的一開始是未付款的狀態，之後透過濾界回傳的資訊更新資料庫內容。
```
Database1Entities db = new Database1Entities();
//step4 : 新增訂單
[System.Web.Http.HttpPost]
[System.Web.Http.Route("api/Ecpay/AddOrders")]
public string AddOrders(get_localStorage json)
{
    string num = "0";
    try
    {
        EcpayOrders Orders = new EcpayOrders();
        Orders.MemberID = json.MerchantID;
        Orders.MerchantTradeNo = json.MerchantTradeNo;
        Orders.RtnCode = 0; //未付款
        Orders.RtnMsg = "訂單成功尚未付款";
        Orders.TradeNo = json.MerchantID.ToString();
        Orders.TradeAmt = json.TotalAmount;
        Orders.PaymentDate = Convert.ToDateTime(json.MerchantTradeDate);
        Orders.PaymentType = json.PaymentType;
        Orders.PaymentTypeChargeFee = "0";
        Orders.TradeDate = json.MerchantTradeDate;
        Orders.SimulatePaid = 0;
        db.EcpayOrders.Add(Orders);
        db.SaveChanges();
        num = "OK";
    }
    catch (Exception ex)
    {
        num = ex.ToString();
    }
    return num;
}
```
* 到這裡可以看一下table，確認有新增進來了，可以注意目前RtnMsg是"訂單成功尚未付款"，RtnCode是0。
![](https://i.imgur.com/6uCltsL.png)

### 後端 HomeController.cs : PayInfo
:::warning
這裡注意LINQ中(此為Where)不要出現toString()之類的函示，會出現LINQ無法辨識型態的問題。看MVC 版本，有些給過。
:::
```
/// step5 : 取得付款資訊，更新資料庫
[HttpPost]
public ActionResult PayInfo(FormCollection id)
{
    var data = new Dictionary<string, string>();
    foreach (string key in id.Keys)
    {
        data.Add(key, id[key]);
    }
    Database1Entities db = new Database1Entities();
    string temp = id["MerchantTradeNo"]; //寫在LINQ(下一行)會出錯，
    var ecpayOrder = db.EcpayOrders.Where(m => m.MerchantTradeNo == temp).FirstOrDefault();
    if (ecpayOrder != null)
    {
        ecpayOrder.RtnCode = int.Parse(id["RtnCode"]);
        if (id["RtnMsg"] == "Succeeded") ecpayOrder.RtnMsg = "已付款";
        ecpayOrder.PaymentDate = Convert.ToDateTime(id["PaymentDate"]);
        ecpayOrder.SimulatePaid = int.Parse(id["SimulatePaid"]);
        db.SaveChanges();
    }

    return View("EcpayView", data);
}
/// step5 : 取得虛擬帳號 資訊
[HttpPost]
public ActionResult AccountInfo(FormCollection id)
{
    var data = new Dictionary<string, string>();
    foreach (string key in id.Keys)
    {
        data.Add(key, id[key]);
    }
    Database1Entities db = new Database1Entities();
    string temp = id["MerchantTradeNo"]; //寫在LINQ會出錯
    var ecpayOrder = db.EcpayOrders.Where(m => m.MerchantTradeNo == temp).FirstOrDefault();
    if (ecpayOrder != null)
    {
        ecpayOrder.RtnCode = int.Parse(id["RtnCode"]);
        if (id["RtnMsg"] == "Succeeded") ecpayOrder.RtnMsg = "已付款";
        ecpayOrder.PaymentDate = Convert.ToDateTime(id["PaymentDate"]);
        ecpayOrder.SimulatePaid = int.Parse(id["SimulatePaid"]);
        db.SaveChanges();
    }

    return View("EcpayView", data);
}
```
### 前端 EcpayView.cshtml
顯示回傳結果，可以到綠界的後台看看有沒有多那筆。
```
@model System.Collections.Generic.Dictionary<string, string>
@{ Layout = null; }
<!DOCTYPE html>
<html>
<body>
    <!--step6 : 顯示回傳結果-->
    @foreach (var key in @Model.Keys.ToList())
    {
        @(key) <input type="text" name="@key" value="@Model[key]" disabled /><br />
    }
</body>
</html>
```
* 回傳頁面
![](https://i.imgur.com/ZHEqgB1.png)

* 再確認一下資料庫內容，成功的話RtnCode為"1"，失敗為"0"
![](https://i.imgur.com/ZT6iQW4.png)



## 模擬付款
### EcpayController:AddPayInfo、AddAccountInfo
:::danger
信用卡測沒問題，但有用模擬付款的方式就可能有問題，因此IP記得用對外的喔~~ 
:::
```
[System.Web.Http.HttpPost]
[System.Web.Http.Route("api/Ecpay/AddPayInfo")]
public HttpResponseMessage AddPayInfo(JObject info)
{
    try
    {
        var cache = MemoryCache.Default;
        cache.Set(info.Value<string>("MerchantTradeNo"), info, DateTime.Now.AddMinutes(60));
        return ResponseOK();
    }
    catch (Exception e)
    {
        return ResponseError();
    }
}
[System.Web.Http.HttpPost]
[System.Web.Http.Route("api/Ecpay/AddAccountInfo")]
public HttpResponseMessage AddAccountInfo(JObject info)
{
    try
    {
        var cache = MemoryCache.Default;
        cache.Set(info.Value<string>("MerchantTradeNo"), info, DateTime.Now.AddMinutes(60));
        return ResponseOK();
    }
    catch (Exception e)
    {
        return ResponseError();
    }
}
private HttpResponseMessage ResponseError()
{
    var response = new HttpResponseMessage();
    response.Content = new StringContent("0|Error");
    response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
    return response;
}
private HttpResponseMessage ResponseOK()
{
    var response = new HttpResponseMessage();
    response.Content = new StringContent("1|OK");
    response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
    return response;
}
```
* 此方法是回傳到"ReturnURL"所設的網址
* 注意Response回傳的值 "1|OK"

### 可能問題
* 如果你執行時發現，回傳到PayInfo的資料是空的，那有可能是綠界回傳給你的資料被擋了，原因是你的網址要是對外的IP(不是用localhost的)。
* webapi無反應，確認有沒有沒設定到的~~
    * Global.asax
    ```
    GlobalConfiguration.Configure(WebApiConfig.Register);
    ```
    * 在App_Start中有WebApiConfig.cs
    ```
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 設定和服務

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
    ```


#### 參考來源
綠界操作畫面可以看看這篇
* Wei 技術分享: [「綠界(Ecpay)」金流介接教學](https://weitechshare.blogspot.com/2020/11/ecpay.html)

---

## 方法二
### 前端 Index.cshtml
先塞值進去~~
```
<form id="form" name="form" action="https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut/V5" method="POST">
    MerchantTradeNo <input type="text" name="MerchantTradeNo" value="b6d48d8c344741adbebb"><br>
    MerchantTradeDate <input type="text" name="MerchantTradeDate" value="2023/04/20 10:58:54"><br>
    TotalAmount <input type="text" name="TotalAmount" value="100"><br>
    TradeDesc <input type="text" name="TradeDesc" value="無"><br>
    ItemName <input type="text" name="ItemName" value="測試商品"><br>
    ExpireDate <input type="text" name="ExpireDate" value="3"><br>
    CustomField1 <input type="text" name="CustomField1" value=""><br>
    CustomField2 <input type="text" name="CustomField2" value=""><br>
    CustomField3 <input type="text" name="CustomField3" value=""><br>
    CustomField4 <input type="text" name="CustomField4" value=""><br>
    ReturnURL <input type="text" name="ReturnURL" value="https://localhost:44325//api/Ecpay/AddPayInfo"><br>
    OrderResultURL <input type="text" name="OrderResultURL" value="https://localhost:44325//Home/PayInfo/b6d48d8c344741adbebb"><br>
    PaymentInfoURL <input type="text" name="PaymentInfoURL" value="https://localhost:44325//api/Ecpay/AddAccountInfo"><br>
    ClientRedirectURL <input type="text" name="ClientRedirectURL" value="https://localhost:44325//Home/AccountInfo/b6d48d8c344741adbebb"><br>
    MerchantID <input type="text" name="MerchantID" value="2000132"><br>
    IgnorePayment <input type="text" name="IgnorePayment" value="GooglePay#WebATM#CVS#BARCODE"><br>
    PaymentType <input type="text" name="PaymentType" value="aio"><br>
    ChoosePayment <input type="text" name="ChoosePayment" value="ALL"><br>
    EncryptType <input type="text" name="EncryptType" value="1"><br>
    CheckMacValue <input type="text" name="CheckMacValue" value="7A0384B727EC16E654F0686FF441D4B9195D6918268198049AC458DA39FFD33C"><br>
    <button class="col-4 btn btn-danger payBtn" type="submit" id="checkoutBtn">送出</button>
</form>

<!-- 自己的ecpay.js路徑 -->
<script src="../../assets/js/ecpay.js"></script>
```


# 字架對外臨時網址

* [IIS 設定](https://reurl.cc/EGAVpa)
* [如何在IIS上發佈ASP.NET MVC專案](https://medium.com/eleanor-hsu/如何在iis上發佈asp-net-mvc專案-4f1c3c3cb929)









