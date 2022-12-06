# ASP .NET MVC 上手 Day05(版面配置頁)

###### tags: `asp.netMVC` `C#`


這篇會實際帶一遍使用版面配置頁的流程，也就是之前Day02有用到的，會Views中多了Shared的資料夾，裏頭有寫好的_Layout.cshtml。

這功能主要是可以設定權限給不同的權限的用戶做使用。

## 版面配置使用的檔案
* _ViewStart.cshtml : 預設要套用哪個版面配置頁。預設是長這樣
```
@{
    Layout = "~/Views/Shared/_Layout.cshtml";
}
```
Layout.cshtml指定@RanderBody()是內容頁面存放區。
```
<html>
...
...
@RanderBody()
...
```
* Shared/版面配置頁.cshtml : 建立的多個版面配置頁做切換或套用。
* 資料夾/內容頁面.cshtml : 內容頁面，通常在Build資料夾下。


---

## Action動作方法套用主板頁面
* 未使用Model
```
public ActionResult Index(){
    return View("內容頁面View", "版面配置頁View");
}
```
* 使用Model
```
public ActionResult Index(){
    return View("內容頁面View","版面配置頁View",Model);
}
```

新增一個專案，新增控制器和檢視，記得勾選使用版面配置。

![](https://i.imgur.com/F1qVl8w.png)

它會自動生成 Content 和 Scripts。

:::warning
假如 Content 和 Scripts 沒出現或沒bootstrap、jQuery，請回Day02把套件安裝起來
:::

* _Layout.cshtml
```
<div class="container">
    <div class="navbar-header">
        <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
        </button>
        @Html.ActionLink("Application name", "Index", "Home", new { area = "" }, new { @class = "navbar-brand" })
    </div>
    <div class="navbar-collapse collapse">
        <ul class="nav navbar-nav">
        </ul>
    </div>
</div>
```

這段是bootstrap的導覽頁。 根據 @Html.ActionLink 知道用哪一個控制器(Home)的哪一個動作方法(Index)

```
<div class="container body-content">
    @RenderBody()
    <hr />
    <footer>
        <p>&copy; @DateTime.Now.Year - My ASP.NET Application</p>
    </footer>
</div>
```

@RenderBody() 則是我們每頁要放的內容。

* 套件引用的地方
```
<head>
...
    <link href="~/Content/Site.css" rel="stylesheet" type="text/css" />
    <link href="~/Content/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <script src="~/Scripts/modernizr-2.8.3.js"></script>
...
...
...
    <script src="~/Scripts/jquery-1.9.1.min.js"></script>
    <script src="~/Scripts/bootstrap.min.js"></script>
</body>
</html>

```
你勾選完使用版面配置後，可以看到他自動生成的_Layout.cshtml，可以看到套件的版本，這範例已經是非常舊的版本了，你可以依照你喜歡的版本去NuGet套件管理員那升級，之後再修改版本。

可以看一下Index.cshtml
```
@{
    ViewBag.Title = "Index";
}
```
就是對應到_Layout.cshtml
```
<title>@ViewBag.Title - My ASP.NET Application</title>
```

可以執行看看結果

---

來實作一下多一頁，複製 _Layout.cshtml 的內容，在 Shared 中新增檢視，這次別勾選版面配置，取名為 _Layout02.cshtml，把複製內容貼上。

然後來修改 _Layout.cshtml，修改這幾行
```
...
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>@ViewBag.Title - 花朵風格作品集</title>
    ...
    ...
    @Html.ActionLink("花朵風格作品集", "Index", "Home", new { area = "" }, new { @class = "navbar-brand" })
    ...
    ...
    <div class="container body-content">
        <div class="jumbotron">
            <h1>花朵風格作品集</h1>
            <p>
                各種風格花朵，任你選擇
            </p>
        </div>
        @RenderBody()
        <hr />
        <footer>
            <p>&copy; @DateTime.Now.Year - XXX花朵設計店</p>
        </footer>
    </div>
    ...
    
```

之後再到 HomeController.cs 中新增動作

```
public ActionResult Index()
{
    return View("Index","_Layout");
}

public ActionResult Index02()
{
    return View("Index", "_Layout02");
}
```

最後在頁面新增按鈕來跳轉頁面

* Index.cshtml
```
@{
    ViewBag.Title = "Index";
}

<h2>Index</h2>

<a href="@Url.Action("Index")" class="btn btn-primary">版面一</a>
<a href="@Url.Action("Index02")" class="btn btn-success">版面二</a>
<p></p>
```

完成後執行看看~~ 觀察網址 和 頁面的變化 ， 這應用是可以用在權限的不同，顯示對應的版面給用戶。



---

## 資料分頁 PageList

分頁劉使用者瀏覽網頁時更加方便，讓數百千筆的紀錄不用一次載入，可以讓使用者以分頁的方式逐頁的瀏覽資訊。 

1. 解決資料數量龐大的問題。
2. 較佳的使用者體驗。

那可以用方便的 PageList 套件來做設計，可以到 NuGet 封裝管理員中安裝(參考右鍵->瀏覽->搜尋打 PageListMVC)
![](https://i.imgur.com/QsV9ljT.png)


之後準備好7張圖片，放在images的資料夾裡面，接著將 images 資料夾左鍵按住，拖移到專案當中(Day05)
![](https://i.imgur.com/EErkBm5.png)

* 在 Models 中新增一個類別 flower.cs，建立屬性

```
namespace Day05.Models
{
    public class flower
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
    }
}
```
* 修改HomeController.cs 的 Index()
```
public ActionResult Index(int page=1)
{
    int pagesize = 3;
    int pagecurrent = page < 1 ? 1 : page;
    string[] id = new string[] { "A01", "A02", "A03", "A04", "A05", "A06", "A07" };
    string[] name = new string[]
    {
        "花色","玻璃","變形蟲","曼陀羅","夜晚","蛇皮","彩色幾何"
    };
    int[] price = new int[] { 100,200,300,400,500,600,700 };
    List<Flower> list = new List<Flower>();
    for(var i = 0;i < id.Length; i++)
    {
        Flower flow = new Flower();
        flow.Id = id[i];
        flow.Name = name[i];
        flow.Price = price[i];
        list.Add(flow);
    }
    var pagedlist = list.ToPagedList(pagecurrent, pagesize); //(顯示第幾頁,顯示幾筆)
    return View("Index", "_Layout", pagedlist); //("內容頁面View","版面配置頁View",Model)
}
```

* 修改 Index.cshtml
```
@using PagedList.Mvc <!--引入PagedList模板-->
@using PagedList
@model IPagedList<Day05.Models.Flower> //用 PageList 來套用在 Models 的 Flower物件
@{
    ViewBag.Title = "Index";
}
<link href="~/Content/PagedList.css" rel="stylesheet" /> <!--從Content資料夾中將 PagedList.cs 拖移到這裡-->
<a href="@Url.Action("Index")" class="btn btn-primary">版面一</a>
<a href="@Url.Action("Index02")" class="btn btn-success">版面二</a>
<p></p>
<table class="table"> <!--用迴圈將圖顯示出來-->
    <tr>
        <th>編號</th>
        <th>名稱</th>
        <th>金額</th>
        <th>圖示</th>
    </tr>
    @foreach(var item in Model) 
    {
        string imgsrc = @item.Id + ".jpeg"; 
        <tr>
            <td>@item.Id</td>
            <td>@item.Name</td>
            <td>@item.Price</td>
            <td>
                <img src="~/images/@imgsrc" width="200" />
            </td>
        </tr>
    }
</table>
@Html.PagedListPager(Model, page=>Url.Action("Index",new {page})) <!--跳轉頁碼 -->
```

執行看看結果，如果你有點擊版面二按鈕，發現會有問題，原因是 Index.cshtml 是有套用 PagedList 的模型，所以 `var item in Model` 的Model會沒收到來自 Controller 的 Model`("內容頁面View","版面配置頁View",Model)`，所以解決方式就是把Index()的程式碼複製到 Index02() 就好了。
```
public ActionResult Index02(int page = 1)
{
    int pagesize = 3; //顯示3張圖
    int pagecurrent = page < 1 ? 1 : page; // ? True : False 如果page<1則1，不是則是page
    string[] id = new string[] { "A01", "A02", "A03", "A04", "A05", "A06", "A07" };
    string[] name = new string[]
    {
        "花色","玻璃","變形蟲","曼陀羅","夜晚","蛇皮","彩色幾何"
    };
    int[] price = new int[] { 100, 200, 300, 400, 500, 600, 700 };
    List<Flower> list = new List<Flower>(); //Models的Flower
    for (var i = 0; i < id.Length; i++)
    {
        Flower flow = new Flower();
        flow.Id = id[i];
        flow.Name = name[i];
        flow.Price = price[i];
        list.Add(flow);
    }
    var pagedlist = list.ToPagedList(pagecurrent, pagesize); //(顯示第幾頁,顯示幾筆)
    return View("Index", "_Layout02", pagedlist); //("內容頁面View","版面配置頁View",Model)
}
```
![](https://i.imgur.com/YNly8yI.png)

---

## Html Helper 
是 View 用來產生 HTML 的方法，可產生表單、文字欄、核取方塊、下拉式選單、超連結或驗證訊息...等。優點如下 : 

1. 繫結 Model 模型，傳遞表單資料。
2. Model 模型可套用 Data Annotations 自動產生前端驗證。
3. 依資料型別產生適當的HTML元素，簡化使用上的複雜度。
4. HTML Helper 支援多載方法，可依傳入的參數建構不同的屬性。
5. 有 intellisense 輔助，自動出現程式碼參考，快速完成撰寫。

### 以 @ 開頭，後面接 Html. 再加上方法，例如: @Html.xxx([引數串列])
```
@Html.ActionLink
(
    "首頁", "Index", "Home",
    new {num = 1},
    new {@style="background:blue"}
)
```
```
<a href="Home/Index?num=1" style="background:blue">
首頁
</a>
```

### HTML Helper 依傳入的參數是否為 Model 模型，可分為強型別和非強型別

強行別類型會傳入 Model，且方法字尾會加上For，同時使用Lamdba表示式。

非強型別 :
@Html.TextBox(名稱，預設值，屬性)
| 定義 | 產生 |
| -------- | -------- |
| @Html.TextBox("fName")     | <input type="text" id = "fName" name = "fName" value="" >  | 
* 舉例
```
@Html.TextBox("txtName", null, new{placeholder="請輸入姓名"})
```
產生下面
```
<input id="txtName" name="txtName" type="text" placeholder="請輸入姓名" value="" />
```
強型別 :

| 定義 | 產生 |
| -------- | -------- |
| @Html.TextBoxFor(m=>m.fName)     | <input type="text" id = "fName" name = "fName" value="" >  | 
* 舉例
```
@model 專案名稱.Models.Flower
@Html.TextBoxFor( model => model.txtName, new{ @class="form-control", placeholder="請輸入姓名"} )
```
產生下面
```
<input class="form-control" placeholder="請輸入姓名"  id="txtName" name="txtName" type="text" value="" />
```
:::success
HTML Helper : [詳細介紹](https://dotblogs.com.tw/dog0416/2016/04/06/110004#HtmlHelper)、[其他功能介紹](https://www.tutorialsteacher.com/mvc/html-helpers)
:::

## Url Helper 

1. @Url.Action("動作方法") : 使用當初創建 cshtml 的 Controller
2. @Url.Action("動作方法","控制器") : 跳到其他的控制器
3. @Url.Action("動作方法","控制器",new{參數="值"}) : 帶參數傳過去


## 實作 Html Helper

在 Models 中新增類別，員工資料來當作例子
* Employee.cs
```
public class Employee
{
    public string EmpId { get; set; }
    public int Department { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public int Salary { get; set; }
}
```
* HomeController.cs
```
List<SelectListItem> GetDepartment() //GetDepartment方法，回傳一個 List 下拉式清單
{
    List<SelectListItem> list = new List<SelectListItem>();
    list.Add(new SelectListItem()
    {
        Text = "資訊",
        Value = "1",
        Selected = true
    });
    list.Add(new SelectListItem()
    {
        Text = "設計",
        Value = "2",
        Selected = true
    });
    list.Add(new SelectListItem()
    {
        Text = "會計",
        Value = "3",
        Selected = true
    });
    return list;
}
public ActionResult Create()
{
    ViewBag.Department = GetDepartment();
    return View();
}
[HttpPost] //按下submit時，接收資料
public ActionResult Create(Employee emp)
{
    ViewBag.Department = GetDepartment();
    SelectListItem empdep = GetDepartment().Where(m => m.Value == emp.Department.ToString()).FirstOrDefault();
    ViewBag.Show = string.Format("部門 : {0}<br>姓名 : {1}<br>信箱 : {2}<br>薪資 : {3}<br>", empdep.Text, emp.Name, emp.Email, emp.Salary);  //注意型態是字串還是整數，下一行empdep.Text or emp.Department 比較看看
    return View(emp);
}
```
* 這裡的新增檢視使用Create範本，模型類別是Employee，勾選"參考指令碼程式庫"和使用版面配置頁
![](https://i.imgur.com/bELVkAe.png)

創完先執行看看，你會發現他幫你先打好基本的頁面了~~

因為 Department 是想用下拉式的方法呈現，所以改一下那段的html
```
<div class="form-group">
    @Html.LabelFor(model => model.Department, htmlAttributes: new { @class = "control-label col-md-2" })
    <div class="col-md-10">
        @Html.DropDownListFor(model => model.Department, dep, new { @class = "form-control"})
        @Html.ValidationMessageFor(model => model.Department, "", new { @class = "text-danger" })
    </div>
</div>
```
![](https://i.imgur.com/G4wtUqY.png)


接著在下面寫呈現的html
```
<div>
    @Html.ActionLink("Back to List", "Index")
</div>
<hr />
@Html.Raw(@ViewBag.Show)

```
![](https://i.imgur.com/264YHjB.png)

小技巧如果你要Debug，可以ViewBag.Show來把值印到前端喔 !

# [Day06](https://hackmd.io/@JpF6T4ZnQ4CVWYEPJpeFYw/HJgGh9mJc)