# ASP .NET MVC 上手 Day10(jQueryMobile套件與常見元件)

###### tags: `asp.netMVC` `C#`

## 介紹
* jQuery Moblie 使用jQuery的語法，不用重新學。
* 相容行動與桌上型平台的瀏覽器，包刮iOS、Android...
* 程式碼輕薄短小，壓縮打約20K。
* 網頁已HTML5標記設計，只需使用少量的script。
* 使用HTML5的data-role屬性來自動初始化
* 以簡單的API處裡使用者的輸入方式，如觸控、滑鼠、鍵盤等。
* 提供強大的佈景主題架構與ThemRoller應用程式讓開發者建立高品質的網頁。


## 設計設定

###  載入檔案
```
<link rel="styleseet" href="http://code.jquery.com/mobile/1.4.5/jquery.mobile-1.4.5.min.css" />
<script src="http://code.jquery.com/jquery-1.11.1.min.js"></script>
<script src="http://code.jquery.com/mobile/1.4.5/jquery.mobile-1.4.5.min.js"></script>
```
###  行動裝置的螢幕寬度，設定viewport
```
<meta name="viewport" content="width=device-width, initial-scale=1">
```
將網頁寬度設為螢幕寬度。

* 頁面由首頁(header)、內容(content)、頁尾(footer)，都是在 `<div>` 標籤中使用data-role屬性定義。

| data-role屬性值 | 說明 | 
| -------- | -------- | 
| data-role = "page"    | 定義區塊為顯示在瀏覽器的頁面     | 
| data-role = "header" | 定義區塊為上方的頁首(標題、按鈕) |
| data-role = "content" | 定義區塊為頁面中的內容，內容可以加入各種不圖元素 |
| data-role = "footer" | 定義區塊為下方的頁尾 | 

:::info
[先玩玩看](https://www.runoob.com/jquerymobile/jquerymobile-tutorial.html)
:::
###  頁面切換
* jQuery Mobile 頁面切換與超連結功能皆可以使用`<a>`標籤設定，只要將href屬性指定到連結的網路資源即可。

| href屬性值 | 功能 | 
| -------- | -------- | 
| href="tel:電話號碼或行動電話"    | 指定播打電話     | 
| href = "sms: 行動電話?body=簡訊內容" | 指定傳送發送簡訊 |
| href = "mailto:信箱?subject=標題&body=內容" | 連結指定網頁 |
| href="#id辨別名稱" | 切換至指定id的`<div>`頁面 | 

* 在與頁面切換的動作中， `<a>` 標籤提供了頁面切換轉場動畫的data-transition屬性，可呈現華麗的動畫特效。

| data-transition屬性值 | 切換頁面效果 | 
| -------- | -------- | 
| data-transition="slide"    | 切換頁面右方滑入(預設)    | 
| data-transition="slideup" | 切換頁面由下方滑入 |
| data-transition="slidedown" | 切換頁面由上方滑入 |
| data-transition="pop" | 使用彈出效果進行切換頁面 |
| data-transition="fade" | 使用淡入淡出方式進行切換頁面 |
| data-transition="flip" | 使用3D翻轉方式進行切換頁面 |
| data-transition="dialog" | 使用對話方塊的方式彈出新的頁面 |
| data-transition="false" | 取消Ajax並連結到指定的新網頁。若傳送資料到另一個網頁時，建議採用此種方式 |
| data-transition="reverse" | 按照data-transition設定，給予相反的效果通常使用在返回上一部或上一部連結中 |

###  ListView清單
* 資料列表示行動網頁或App最常使用的呈現方式之一，在jQueryMobile中提供ListView清單元件可以列表出項目。
* < ul >標籤中指定data-role="listview"< /ul >，< ul > 即變成ListView清單元件。
* < li > 標籤可指定ListView清單的每一個項目。
* < span class="ui-li-count" > ~ < /span > 提示氣泡。

### 巡覽列 
* 一組群組按鈕，在視窗程式、商務網站或App中常見其蹤影。
* 當< div >標籤內指定data-role="navbar"，此時< ul > 和 < li >的項目清單即為巡覽列。
* 巡覽列可置於頁首、內容與頁尾三個區域中。 

## 實作

### 需先安裝
:::success
主要為 < head > 中的內容，先宣告viewport 和 jquery檔案的路徑，這裡可以用Visual Stdio的內建功能，右鍵專案 -> 管理 NuGet 套件 -> 瀏覽，輸入要安裝的套件及選擇對應的版本。安裝完畢可以在 Content 和 Script 中看到安裝檔案
:::
* 以本案例，版本為以下

| 安裝 | 版本 |
| -------- | -------- | 
| jquery.mobile | 1.4.5     | 
| jquery | 1.11.1    |


在 Content 和 Script 中將 `jquery.mobile-1.4.5.min.css` 、 `jquery-1.11.1.min.js` 、 `jquery.mobile-1.4.5.min.js` 用拖移的方式照順序放到< head >中。


### 在 Shared 中新增 _MobileLayout.cshtml 的版面配置頁框(MVC 5檢視頁面)


#### _MobileLayout.cshtml
```
<!DOCTYPE html>
<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>xxx公司</title>
    <link href="~/Content/jquery.mobile-1.4.5.min.css" rel="stylesheet" />
    <script src="~/Scripts/jquery-1.11.1.min.js"></script>
    <script src="~/Scripts/jquery.mobile-1.4.5.min.js"></script>
</head>
<body>
    <div data-role="page" id="home">
        <div data-role="header" data-position="fixed">
            <h1>xxx公司</h1>
        </div>
        @RenderBody()
        <div data-role="footer" data-position="fixed">
            <div data-role="navbar">
                <ul>
                    <li><a href="@Url.Action("index")">首頁</a></li>
                    <li><a href="#about" data-rel="dialog">關於</a></li>
                </ul>
            </div>
            <h1>xxx公司</h1>
        </div>
    </div>  
    <div data-role="page" id="about">
        <div data-role="header"><h1>關於xx</h1></div>
        <div>
            <h3>最優質、最快速、最貼心的服務</h3>
            <p>粉專 : <a href="https://www.facebook.com/" data-ajax="false">FB</a></p>
            <p>信箱 : <a href="123@gmail.com">123@gmail.com</a></p>
            <p>電話 : <a href="tel:041234567">04-1234567</a></p>
        </div>
    </div>
</body>
</html>
```

#### 在 Controller 中新增 MobileController.cs

接著右鍵 Index 新增檢視，注意版面配置頁用剛剛我們設計好_MobileLayout.cshtml頁框

![](https://i.imgur.com/MMtSFAg.png)

執行看看結果

![](https://i.imgur.com/HGgWjtn.png)

---

![](https://i.imgur.com/TiNeQzm.png)


### 產品類別顯示
#### 產品類別統計ViewModel.cs

定義物件內容

```
public class 產品類別統計ViewModel
{
    public int 類別編號 { get; set; }
    public string 類別名稱 { get; set; }
    public int 數量 { get; set; }
}
```

#### MobileController.cs

撈資料
```
dbProductEntities db = new dbProductEntities();
// GET: Mobile
public ActionResult Index()
{
    var category = from c in db.產品類別
                   join p in db.產品資料
                   on c.類別編號 equals p.類別編號
                   into t
                   select new 產品類別統計ViewModel
                   {
                       類別編號 = c.類別編號,
                       類別名稱 = c.類別名稱,
                       數量 = t.Count()
                   };
    return View(category);
}
```

#### Mobile的 Index.cshtml

顯示資料內容，數量用提示氣泡顯示
```
@model IEnumerable<Day10.Models.產品類別統計ViewModel>

@{
    Layout = "~/Views/Shared/_MobileLayout.cshtml";
}

<div data-role="content">
    <div style="text-align:center">
        <img src="~/Images/xxx.png" style="max-width:95%" />
        <p>xxx課程</p>
    </div>
    <ul data-role="listview" data-inset="true">
        <li data-role="list-divider">
            購物類別專區
        </li>
        @foreach(var item in Model)
        {
            <li>
                <a href="@Url.Action("Products")?cid=@item.類別編號" data-ajax="false">
                    @item.類別名稱
                    <span class="ui-li-count">@item.數量</span>
                </a>
            </li>
        }
    </ul>
</div>
```
### 產品資料查詢

#### MobileController.cs
一樣撈資料，拿最新的版本
```
public ActionResult Products(int cid)
{
    ViewBag.CategoryName = db.產品類別
        .Where(m => m.類別編號 == cid)
        .FirstOrDefault()
        .類別名稱;

    var products = db.產品資料
        .Where(m => m.類別編號 == cid)
        .OrderByDescending(m => m.修改日)
        .ToList();
    return View(products);
}
```
#### Mobile的 Products.cshtml
```
@model IEnumerable<Day10.Models.產品資料>
@{
    Layout = "~/Views/Shared/_MobileLayout.cshtml";
}
<h2>Products</h2>
<div data-role="content">
    @{           
        if (Model.Count() != 0)
        {
            <ul data-role="listview" data-inset="true">
                <li data-role="list-divider"><span>@ViewBag.CategoryName</span>類專區</li>
                @foreach (var item in Model)
                {
                    <li>
                        <img src="~/Images/@item.圖示" />
                        <h2>@item.品名</h2>
                        <p>單價:@item.單價</p>
                    </li>
                }
            </ul>
        }
        else
        {
            <h3>目前無<samp>@ViewBag.CategoryName</samp>的產品</h3>
        }
    }
</div>
```


## 小結論
jQueryMobile套件其實還有很多功能，有興趣的朋友可以再研究看看([這裡](https://www.runoob.com/jquerymobile/jquerymobile-tutorial.html))，基本上就是你能把資料呼叫出來前端印出來就差不多了，大同小異。






