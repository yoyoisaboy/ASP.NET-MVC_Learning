# ASP .NET MVC 上手 Day04(View檢視應用、Bootstrap)

###### tags: `asp.netMVC` `C#`

這是之前我們經常使用的步驟之一，在建立完控制器我們就會右鍵-建立檢視器，產出xx.cshtml。

我們來簡單複習一下，重新用專案，Controllers新增控制器
* Sample01Controller.cs
```
public class Sample01Controller : Controller
{
    // GET: Sample01
    public ActionResult Index()
    {
        List<Book> list = new List<Book>();
        list.Add(new Book() { Id = "AWL020600",Name="I love you",Price=520});
        list.Add(new Book() { Id = "AWL020601", Name = "I hate you", Price = 340 });
        list.Add(new Book() { Id = "AWL020602", Name = "boring day boring life", Price = 1000 });
        return View(list);
    }
}
```
* 在Models新增類別，Book.cs
```
public class Book
{

    public string Id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
}
```
* 在Action右鍵，新增檢視到Views中，Index.cshtml
```
@model IEnumerable<Day04.Models.Book>
@{
    Layout = null;
}
<!DOCTYPE html>
<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>Index</title>
</head>
<body>
    <table border="1">
        <tr>

            <td>書號</td>
            <td>書名</td>
            <td>單價</td>
        </tr>
        @{
            foreach (var item in Model)
            {
                <tr>
                    <td>@item.Id</td>
                    <td>@item.Name</td>
                    <td>
                        @if ( @item.Price > 600 )
                        {
                            <span style="background-color:#ff0000;color:white;">@item.Price</span>
                        }
                        else
                        {
                            @item.Price
                        }
                    </td>
                </tr>
            }
        }
        </table>
</body>
</html>
```
結果如下:
![](https://i.imgur.com/bwCHXQf.png)

## Razor 語法實作-影片播放
新增一個Controller
```
public class Sample02Controller : Controller
{
    // GET: Sample02
    public ActionResult Index(int episode = 0)
    {
        List<Movie> list = new List<Movie>();
        list.Add(new Movie() { Id = "EP62gl-sj2I", Name = "JoJo"});
        list.Add(new Movie() { Id = "tfwatFtgWPY", Name = "Yu-Gi-Oh" });
        list.Add(new Movie() { Id = "YsfFKKZOLqw", Name = "YOYOman" });
        ViewBag.MovieId = list[episode].Id;
        return View(list);
    }
}
```
右鍵新增View
```
@{
    Layout = null;
}
<!DOCTYPE html>
<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>Index</title>
</head>
<body>
    <iframe width="560" height="315" 
            src="https://www.youtube.com/embed/@ViewBag.MovieId" 
            frameborder="0" 
            allow="autoplay;encrypted-media" 
            allowfullscreen>
    </iframe>
</body>
</html>
```
如果沒在網址後打 "?epsiode=x" 其他集數x的話，就會是第一部epsiode=0(第一順位)

:::success
[YouTube embed 嵌入介紹](https://support.google.com/youtube/answer/171780?hl=zh-Hant)
:::
進階一點可以新增按鈕來選你要看的影片
```
@model IEnumerable<Day04.Models.Movie>
@{
    Layout = null;
}
<!DOCTYPE html>
<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>Index</title>
    <style>
        /*按鈕框*/
        .link {
            margin: 10px;
            padding: 5px;
            border-style: solid;
            border-color: blue;
            text-decoration: none;
        }
        /*滑鼠停留在按鈕背景*/
        .link:hover {
            background-color: #ebffb9;
        }
        /*影片橢圓框*/
        iframe {
            border-top-left-radius: 90px;
            border-bottom-right-radius: 90px;
            border-top-right-radius: 90px;
            border-bottom-left-radius: 90px; 
        }
    </style>
</head>
<body> 
    <p>
        @{
            int i = 0;
            foreach (var item in Model)
            {
                <!--Url.Action:跑目前的控制器、link套用按鈕框、onmouse事件顯示名稱 -->
                <a href="@Url.Action("Index")?episode=@i" class="link"
                   onmouseover="fnMouseOver('@item.Name');"
                   onmouseout="fnMouseOut();">
                    第 @(i+1) 集
                </a>
                i++;
            }
        }
        <span id="spanTitle"></span>
    </p>
    <iframe width="560" height="315"
            src="https://www.youtube.com/embed/@ViewBag.MovieId"
            frameborder="0"
            allow="autoplay;encrypted-media"
            allowfullscreen>
    </iframe>
    <script>
        /*滑鼠停留在按鈕顯示影片名稱，塞名稱給id=spanTitle*/
        function fnMouseOver(title) {
            document.getElementById("spanTitle").innerHTML = title;
        }
        /*滑鼠離開按鈕，塞""給id=spanTitle*/
        function fnMouseOut() {
            document.getElementById("spanTitle").innerHTML = "";
        }
    </script>
</body>
</html>
```
結果如下:
![](https://i.imgur.com/4icGNcU.png)


---

## Bootstrap

這套件一般在新增檢視會有個選項，"使用版面配置頁" 打勾，就會把 Content 、Scripts 和 Share 資料夾，裏頭會放Bootstrap、jQuery、modernizr...等等，如果沒有再請回看 [Day02](https://hackmd.io/k2VtcDnzRDqrNlOoRK5fUA?view#%E6%B8%AC%E8%A9%A6%E5%82%B3%E5%88%B0Views)安裝一下。
:::info
官方網站[hexschool網站](https://bootstrap5.hexschool.com/docs/5.1/getting-started/introduction/)
:::
最主要是有幾個要引入到cshtml的head中，順序不能顛倒
```
<link href="~/Content/bootstrap.min.css" rel="stylesheet" />
<script src="~/Scripts/jquery-3.3.1.min.js"></script>
<script src="~/Scripts/bootstrap.min.js"></script>
```
![](https://i.imgur.com/Mw1Ecuz.png)
![](https://i.imgur.com/8n3MGjK.png)

要用bootstrap時，就會用class來呼叫你要用的效果。
:::success
bootstrap 範例 : [詳細介紹](https://getbootstrap.com/docs/5.1/examples/) 按F12觀察
w3Shool教學 : [詳細教學](https://www.w3schools.com/bootstrap/bootstrap_get_started.asp)
:::
然而為甚麼要用Bootstrap呢? 
主要是方便使用之外，尤其設計RWD方面也比較輕鬆，要讓使用者體驗好的話，考慮不同的操作平台，版面的設計就很重要。
:::success
Bootstrap grid example : [格線系統](https://bootstrap5.hexschool.com/docs/5.1/layout/grid/) 
| class中如果寫 | 尺寸斷點 |
| -------- | -------- | 
| .col-數字 | 當螢幕寬度小於576px時會進行設定，適用於"手機"等超小型設備使用。此為預設。     |
|.col-sm-數字|當螢幕寬度大於等於576px時會進行設定，適用於"平板"電腦等小型設備使用。|
|.col-md-數字|當螢幕寬度大於768px時會進行設定，適用於"桌上"型電腦等螢幕設備使用。|
|.col-lg-數字|當螢幕寬度大於992px時會進行設定，適用於"大型"型電腦等螢幕設備使用。|
|.col-xl-數字|當螢幕寬度大於1200px時會進行設定，適用於超大型螢幕設備使用。|

一個頁面要看成有 12 個 col 組成

ex : 6+6=12
```
<div class="container">
    <div class="row">   
        <div class="col-6" >佔6欄</div>
        <div class="col-6" >佔6欄</div>
    </div>
</div>
```
![](https://i.imgur.com/rj35FS8.png)


假如同個頁面是在不同尺寸的介面
ex : 4+4+4=12
```
<div class="container">
    <div class="row">   
        <div class="col-12 col-md-4" >區塊1</div>
        <div class="col-12 col-md-4" >區塊2</div>
        <div class="col-12 col-md-4" >區塊3</div>
    </div>
</div>
```
![](https://i.imgur.com/J8pAu6Q.png)

還有很多功能，通常Google搜尋 Bootstrap 關鍵字，都是可以找到相關的文章。
:::

* Sample03Cintroller.cs

```
public class Sample03Controller : Controller
{
    // GET: Sample03
    public ActionResult Index(int episode = 0)
    {
        List<Movie> list = new List<Movie>();
        list.Add(new Movie() { Id = "EP62gl-sj2I", Name = "JoJo" });
        list.Add(new Movie() { Id = "tfwatFtgWPY", Name = "Yu-Gi-Oh" });
        list.Add(new Movie() { Id = "YsfFKKZOLqw", Name = "YOYOman" });
        ViewBag.MovieId = list[episode].Id;
        return View(list);
    }
}
```
可以找youtube上隨便3部片，將?v=xxx給複製貼到Id上，預設episode = 0。
右鍵新增檢視(有套用模型)
* Index.cshtml
```
@model IEnumerable<Day04.Models.Movie>
@{
    ViewBag.Title = "Index";
}

<html>
<head>
    <link href="~/Content/bootstrap.min.css" rel="stylesheet" />
    <script src="~/Scripts/jquery-3.3.1.min.js"></script>
    <script src="~/Scripts/bootstrap.min.js"></script>
    <meta name="viewport" content="width=device-width" />
    <title>Index</title>
</head>
<body>
    <div class="container" style="margin-top:20px;">
        <div class="row">
            <p>
                @{
                    foreach (var item in Model)
                    {
                        <div class="col-12 col-md-4" style="margin-top:20px;">
                            <div class="img-thumbnail">
                                <iframe class="embed-responsive" 
                                        src="https://www.youtube.com/embed/@item.Id"
                                        frameborder="0" allow="autoplay:encrypted-media"
                                        allowfullscreen></iframe>
                                <h4>@item.Name</h4>
                                <a href="https://www.youtube.com/watch?v=@item.Id"
                                    target="_blank" class="btn btn-primary">瀏覽</a>
                                </div>
                        </div>
                    }
                }
            </p>
        </div>
    </div>
</body>
</html>
```
:::success
img-thumbnail : [詳細介紹](https://getbootstrap.com/docs/4.0/content/images/#image-thumbnails)
embed-responsive : [詳細介紹](https://getbootstrap.com/docs/4.0/utilities/embed/)
btn btn-primary : [詳細介紹](https://getbootstrap.com/docs/4.0/components/buttons/)
:::
結果如下:
![](https://i.imgur.com/ESWQVE7.png)
![](https://i.imgur.com/VUkc1TE.png)

---

## Partial View 

需 Model 資料連結時使用，做部分檢視

![](https://i.imgur.com/o76R95g.png)

View/Shared/PartialView檔名.cshtml
```
使用模型
View的排版
```
呼叫
```
@Html.Partial( "PartialView檔名" ,物件模型)
```

我們來實作看看吧，新增新的Sample06Controller，內容跟上一個一樣
```
public class Sample06Controller : Controller
{
    // GET: Sample06
    public ActionResult Index(int episode = 0)
    {
        List<Movie> list = new List<Movie>();
        list.Add(new Movie() { Id = "EP62gl-sj2I", Name = "JoJo" });
        list.Add(new Movie() { Id = "tfwatFtgWPY", Name = "Yu-Gi-Oh" });
        list.Add(new Movie() { Id = "YsfFKKZOLqw", Name = "YOYOman" });
        ViewBag.MovieId = list[episode].Id;
        return View(list);
    }
}
```
新增檢視
```
@model IEnumerable<Day04.Models.Movie>
@{
    Layout = null;
}
<!DOCTYPE html>
<html>
<head>
    <link href="~/Content/bootstrap.min.css" rel="stylesheet" />
    <script src="~/Scripts/jquery-3.3.1.min.js"></script>
    <script src="~/Scripts/bootstrap.min.js"></script>
    <meta name="viewport" content="width=device-width" />
    <title>Index</title>
</head>
<body>
    <div class="container" style="margin-top:20px;">
        <div class="row">
            <p>
                @{
                    foreach (var item in Model)
                    {
                       @Html.Partial("_MyPartialView", item)
                    }
                }
            </p>
        </div>
    </div>
</body>
</html>
```

這裡就有不一樣的地方，會發現我把foreach裡的東西拿掉了，換做是 `@Html.Partial("_MyPartialView", item)`，這就是 Partial View 的呼叫方法，所以我們要在View的資料夾中新增一個資料夾，叫做Shared(右鍵新增資料夾)，並在Shared中在新增_MyPartialView.cshtml的檔案(右鍵加入檢視)。

![](https://i.imgur.com/WI6N4fH.png)

把拿掉的部分貼在_MyPartialView.cshtml中
```
@model Day04.Models.Movie

<div class="col-12 col-md-4" style="margin-top:20px;">
    <div class="img-thumbnail">
        <iframe class="embed-responsive"
                src="https://www.youtube.com/embed/@Model.Id"
                frameborder="0" allow="autoplay:encrypted-media"
                allowfullscreen></iframe>
        <h4>@Model.Name</h4>
        <a href="https://www.youtube.com/watch?v=@Model.Id"
           target="_blank" class="btn btn-primary">瀏覽</a>
    </div>
</div>
```
這裡要注意，原本是用item來代表，這裡則要改成讀Models的傳值方法。
完成後執行看看，結果會跟之前相同。
:::info
* 部分檢視檔案名稱通常以底線 (_) 開頭
* 效能比較 : [詳細介紹](https://kevintsengtw.blogspot.com/2013/07/aspnet-mvc-htmlpartial-htmlrenderpartial.html)
:::

---


## Razor Helper

顯示的內容是可以動態調整，像是用某區塊因為條件不同而有不一樣的顯示。

![](https://i.imgur.com/f9cleaI.png)

這就跟函示很像
```
@helper 輔助方法名稱([引數串列]){
    程式邏輯
}
```
舉個例子，如果沒用Razor Helper的話
```
<p> yoyo 成績:@{
    int avg = (56+34+45)/3;
    if(avg>=60){
        @:及格
    }else{
        @:不及格
    }
}</p>
<p> miku 成績:@{
    int avg = (90+98+100)/3;
    if(avg>=60){
        @:及格
    }else{
        @:不及格
    }
}</p>
<p> dinter 成績:@{
    int avg = (80+90+100)/3;
    if(avg>=60){
        @:及格
    }else{
        @:不及格
    }
}</p>
```
會重複打相同的內容，如果有用Razor Helper
```
@helper ScoreLevel(int math,int eng,int chi){
    int avg = (math+eng+chi)/3;
    if(avg>=60){
        @:及格
    }else{
        @:不及格
    }
}
<p> yoyo 成績:@ScoreLevel(56,34,45)</p>
<p> miku 成績:@ScoreLevel(90,98,100)</p>
<p> dinter 成績:@ScoreLevel(80,90,100)</p>
```
寫法很像函式吧~~ 實作一下

新增一個Sample07Controller，複製之前的貼上，右鍵新增檢視
```
@model IEnumerable<Day04.Models.Movie>
@{
    Layout = null;
}
<!DOCTYPE html>
<html>
<head>
    <link href="~/Content/bootstrap.min.css" rel="stylesheet" />
    <script src="~/Scripts/jquery-3.3.1.min.js"></script>
    <script src="~/Scripts/bootstrap.min.js"></script>
    <meta name="viewport" content="width=device-width" />
    <title>Index</title>
</head>
<body>
    <div class="container" style="margin-top:20px;">
        <div class="row">
            <p>
                @{
                    foreach (var item in Model)
                    {
                        @Display(item);
                    }
                }
            </p>
        </div>
    </div>
</body>
</html>
@helper Display(Day04.Models.Movie item) { 
    <div class="col-12 col-md-4" style="margin-top:20px;">
        <div class="img-thumbnail">
            <iframe class="embed-responsive"
                    src="https://www.youtube.com/embed/@item.Id"
                    frameborder="0" allow="autoplay:encrypted-media"
                    allowfullscreen></iframe>
            <h4>@item.Name</h4>
            <a href="https://www.youtube.com/watch?v=@item.Id"
               target="_blank" class="btn btn-primary">瀏覽</a>
        </div>
    </div>
}
```
不同的是它是寫在同一個cshtml裡面，注意helper的宣告方式

## helper 多載

如何做到全站式應用呢，也就是跟 Partial View 一樣乾淨俐落。
請再新增一個Sample08Controller，內容跟之前一樣，右鍵新增檢視
```
@model IEnumerable<Day04.Models.Movie>
@{
    Layout = null;
}
<!DOCTYPE html>
<html>
<head>
    <link href="~/Content/bootstrap.min.css" rel="stylesheet" />
    <script src="~/Scripts/jquery-3.3.1.min.js"></script>
    <script src="~/Scripts/bootstrap.min.js"></script>
    <meta name="viewport" content="width=device-width" />
    <title>Index</title>
</head>
<body>
    <div class="container" style="margin-top:20px;">
        <div class="row">
            <p>
                @{
                    foreach (var item in Model)
                    {
                        @Myhelper.Display(item);
                    }
                }
            </p>
        </div>
    </div>
</body>
</html>
```
`@Myhelper.Display(item);`是呼叫檔名叫Myhelper的Display，但他跟Partial View 不一樣，是要在專案右鍵->加入->加入ASP.NET資料夾->App_Code(O)，並在App_Code右鍵加入->新增類別->點Web的MVC 5 檢視頁面 (Razor)
![](https://i.imgur.com/ejEY6Yp.png)
![](https://i.imgur.com/qgYTbVh.png)

完成後把程式碼貼到Myhelper.cshtml當中

```
@helper Display(Day04.Models.Movie item)
{
    <div class="col-12 col-md-4" style="margin-top:20px;">
        <div class="img-thumbnail">
            <iframe class="embed-responsive"
                    src="https://www.youtube.com/embed/@item.Id"
                    frameborder="0" allow="autoplay:encrypted-media"
                    allowfullscreen></iframe>
            <h4>@item.Name</h4>
            <a href="https://www.youtube.com/watch?v=@item.Id"
               target="_blank" class="btn btn-primary">瀏覽</a>
        </div>
    </div>
}
```







# [Day05](https://hackmd.io/ib72bytyTo6eIlmmL2HKag)