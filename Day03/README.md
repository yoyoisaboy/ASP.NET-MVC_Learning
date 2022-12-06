# ASP .NET MVC 上手 Day03(Controller控制器應用)

###### tags: `asp.netMVC` `C#`


我們再來建一個新的專案吧，並在Controller新增一個控制器，取名為HomeController.cs
## Action
定義好一些資料，HomeController.cs 如下
```
public class HomeController : Controller
{
    string[] name = new string[] { "yoyo", "miku", "dinter" };
    int[] score = new int[] { 100, 90, 80 };
    // GET: Home
    public String Index()
    {
        string show = "";
        for(int i = 0; i < name.Length; i++)
        {
            show +=
                "<ul>" +
                  "<li>name : " + name[i] +
                  "<li>score : " + score[i] +
                "</ul>";
        }
        return show;
    }
}
```
![](https://i.imgur.com/t3zX8as.png)
這是Day01一開始做的事情，這裡比較不同的是將html的指令寫好傳到前端去。
如果有參數的話就改用Action的方法，新增以下
```
public String ShowPage01(int Index1,int Index2)
{
    string show = "";
    show +=
        "<ul>" +
            "<li>name : " + name[Index1] +
            "<li>score : " + score[Index1] +
        "</ul>"+
        "<ul>" +
            "<li>name : " + name[Index2] +
            "<li>score : " + score[Index2] +
        "</ul>";
    return show;
}
```
如果只打 `https://localhost:44326/Home/ShowPage01` 會顯示錯誤，錯誤會說需要加入參數，後面多加 ?index=1&index2=1。
像這樣 `https://localhost:44326/Home/ShowPage01?index1=0&index2=1`
如果多個參數用 '&' 來隔開。
![](https://i.imgur.com/k4EpTkI.png)

---

## ViewData 、 ViewBag 與 TempData

### ViewData 
繼承 Dictionary類別，ViewData 是個字典物件，以鍵/值(Key/Value)的方式存取資料，ex: ViewData["鍵"] = 值，範例如下:

* Controller
```
public class HomeController: Controller
{
    public ActionResult Index()
    {
        ViewData["Name"] = "yoyo";
        return View();
    }
}
```
* View
```
<div>
    <h1> @ViewData["Name"] </h1>
</div>
```
### ViewBag

1. 這功能是在 ASP. NET MVC 3 後才新增的
2. 使用方式與 ViewData 相同，差別在於這個有動態(dynamic)型別。[型別介紹](https://ithelp.ithome.com.tw/articles/10201839)
3. 執行速度會比較慢。 ex : ViewBag.屬性 = 值;
* Controller
```
public class HomeController: Controller
{
    public ActionReult Index()
    {
        ViewBag.Name = "yoyo";
        return View();
    }
}
```
* View
```
<div>
    <h1> @ViewBag.Name</h1>
</div>
```
:::success 
### Session物件
1. 每個使用者都有自己的Session物件，使用這在不同的Request中，用Session來保存資訊，存的方法是透過Cookies實現。 [Session/Cookies?](https://progressbar.tw/posts/91)
2. 使用 Session 物件時，用戶端的Cookies功能需要開啟
3. ex : Session["變數"] = 值;
![](https://i.imgur.com/BaD1e7r.png)
:::
### TempData
1. 也是字典物件
2. 可跨不同的Action傳遞資料(同一個或不同的Controller)，但是只能傳一次到 Action，導向頁面之後TempData的資料就會被清除。
3. TempData會將資料儲存在Session中，他的生命週期為一個請求(Request)，一旦請求結束就會被刪除。
4. TempData存放於Session中，當View使用完成TempData就會消失，若祥存起來，可以用 TempData.Keep() 方法。
5. ex : TempData["鍵"] = 值;
* Controller
```
public class HomeController:Controller
{
    public ActionResult Index(){
        TempData["Name"] = "yoyo";
        return RedirectToAction("About");
    }
    public ActionResult About(){
        return View();
    }
}
```
* View
```
<div>
    <h1> @TempData["Name"]</h1>
</div>
```
---

## 模型聯繫
回顧一下Controller中Action路由參數名稱的執行方法。
(因之前有介紹過，所以我不會再一一說明，給大家複習一下)

### ex1 : http://IP/Home/Index
```
public class HomeController:Controller
{
    public ActionResult Index()
    {
        ...
    }
}
```
* 使用 GET 方式傳送資料是以 QueryString(使用Key/Value)將參數家在URL後面再向伺服器請求。除隱密性低傳送的資料也較少。

### ex2 : http://IP/Home/Index?Id=1&Name=yoyo
```
public class HomeController:Controller
{
    [HttpGet] //這是預設，可省略
    public ActionResult Index(int Id,String Name)
    {
        ...
    }
}
```
* 使用POST方式傳送資料是將資料放在訊息主體內進行傳送，大多用表單使用。
* 請求的內容長度沒有限制，也不會在網址上看到，所以比GET安全，可以傳隱密性高且大量的資料。

### ex3 : 簡單型
* View
```
<form method="post" action="Home/Login">
    <p>帳號<input type="text" name="Id"></p>
    <p>密碼<input type="text" name="Pw"></p>
    <p><input type="submit" value="登入"></p>
</form>
```
* Controller
```
public class HomeController:Controller
{
    [HttpPost] 
    public ActionResult Login(String Id, String Pw)
    {
        ...
    }
}
```
#### 實作ex3簡單型
```
//HomeController.cs

public class HomeController : Controller
{
    public ActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public ActionResult Create(string Id,string Name,int Size)
    {
        ViewBag.Id = Id;
        ViewBag.Name = Name;
        ViewBag.Size = Size;
        return View();
    }
}

```
右鍵Create->新增檢視->MVC x -> 加入，產生Create.cshtml
```
@{
    ViewBag.Title = "Create";
}
<!DOCTYPE html>
<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>Create</title>
</head>
<body>
    <form method="post" action="@Url.Action("Create")" )>
        <p>編號:<input type="text" name="Id" value="@ViewBag.Id"/></p>
        <p>姓名:<input type="text" name="Name"  value="@ViewBag.Name"/></p>
        <p>長度:<input type="text" name="Size"  value="@ViewBag.Size"/></p>
        <p><input type="submit" value="確定"/></p>
    </form>
    <hr/>
    @{           
        if (ViewBag.Id != null && ViewBag.Name != null && ViewBag.Size != null){
            <p>完成新增產品資料</p>  
            <p>
                編號:@ViewBag.Id | 姓名: @ViewBag.Name | 
                長度:@ViewBag.Size
            </p>
           }
    }
</body>
</html>
```
執行看看結果，https://localhost:xxxxx/Home/Create
### ex4 : 複雜型
會多一個在 Models 裡的類別，定義傳/收的參數型別
* Models
```
public class Member{
    public string Id {get;set;}
    public string Pw {get;set;}
}
```
* Controller
```
public class HomeController:Controller
{
    [HttpPost] 
    public ActionResult Login(Member member)
    {
        ViewBag.Id = member.Id;
        ViewBag.Pw = member.Pw;
    }
}
```
#### 實作ex4複雜型
在新增一個Contorller，取名Sample01Controller.cs
```
using 專案名稱.Models;
...
public class Sample01Controller : Controller
{
    public ActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public ActionResult Create(MemberList m)
    {
        ViewBag.Id = m.Id;
        ViewBag.Name = m.Name;
        ViewBag.Size = m.Size;
        return View();
    }
}
```
右鍵Create->新增檢視->MVC x -> 加入，產生Create.cshtml，將ex3的Create.cshtml複製貼上，並執行看看。 https://localhost:xxxxx/Sample01/Create

### ex5 : FormCollection
```
public class HomeController:Controller
{
    [HttpPost] 
    public ActionResult Login(FormCollection forms)
    {
        ViewBag.Id = forms["Id"];
        ViewBag.Pw = forms["Pw"];
        //ViewBag.Id = forms[0];
        //ViewBag.Pw = forms[1];
    }
}
```
#### 實作 ex5 FormCollection
```
public class Sample02Controller : Controller
{
    public ActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public ActionResult Create(FormCollection forms)
    {

        ViewBag.Id = forms["Id"];
        ViewBag.Name = forms["Name"];
        ViewBag.Size = forms["Size"];
        return View();
    }
}
```
右鍵Create->新增檢視->MVC x -> 加入，產生Create.cshtml，將ex3的Create.cshtml複製貼上，並執行看看。 https://localhost:xxxxx/Sample02/Create

ex5結果跟ex3、ex4一樣，但你心裡要知道他們傳送的方式不一樣。

我比較推薦ex4的方式，比較不會打錯字，也相對比較安全。

---

## 直接用物件傳，不用(ViewData、ViewBag 與 TempData)

### 實作 ex6 List<> 傳送
通常這裡用呼叫資料庫，或是json檔案，適合跨語言的寫法(C#、python串接資料，兩邊都透過讀資料庫/檔案的方式操作)
* Sample04Controller.cs
```
public class Sample04Controller : Controller
{
    // GET: Sample04
    public ActionResult ShowMember()
    {
        List<MemberList> list = new List<MemberList>();
        list.Add( new MemberList() { Id="1",Name="yoyo",Size=180});
        list.Add(new MemberList() { Id = "2", Name = "miku", Size = 166 });
        list.Add(new MemberList() { Id = "3", Name = "dinter", Size = 175 });
        return View(list);
    }
}
```
* ShowMember.cshtml ， 最上面有多一個呼叫Models的方法
```
@model List<Day03.Models.MemberList>
@{
    ViewBag.Title = "ShowMember";
}
<!DOCTYPE html>
<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>Showmember</title>
</head>
<body>
    <div>
        @foreach (var item in Model)
        {
            <ul>
                <li>編號:@item.Id</li>
                <li>姓名:@item.Name</li>
                <li>長度:@item.Size</li>
            </ul>
        }
    </div>
</body>
</html>
```
另一個不推薦的方法，轉成 ViewBag 的方法
* Sample04Controller.cs
```
public ActionResult ShowMemberForViewBag()
{
    List<MemberList> list = new List<MemberList>();
    list.Add(new MemberList() { Id = "1", Name = "yoyo", Size = 180 });
    list.Add(new MemberList() { Id = "2", Name = "miku", Size = 166 });
    list.Add(new MemberList() { Id = "3", Name = "dinter", Size = 175 });
    ViewBag.MemberList = list; //會發現打完 "." 後不會出現提示
    return View();
}
```
* ShowMemberForViewBag.cshtml
```
@{
    ViewBag.Title = "ShowMemberForViewBag";
}
<!DOCTYPE html>
<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>Showmember</title>
</head>
<body>
    <div>
        @foreach (var item in ViewBag.MemberList)
        {
            <ul>
                <li>編號:@item.Id</li>
                <li>姓名:@item.Name</li>
                <li>長度:@item.Size</li>
            </ul>
        }
    </div>
</body>
</html>
```
這裡也會發現在打 item. 之後也不會出現提示字元，這都會有可能打錯字，導致花時間Debug錯誤。

結論是熟悉:
1. 變數的型別類型，[強(string、int)、弱(var)、泛(List<>)??](https://www.itread01.com/content/1546810744.html)
2. Action傳/收方法可根據資料的多寡選擇適合的寫法，用簡單或複雜。
3. 一定要考量安全、不吃太多記憶的寫法 (未來會提到，這攸關使用者體驗)


---

## 檔案上傳實作

* Sample01Controller.cs : 程式邏輯順序就是前端設計讀檔案的按鍵->後端要有接收和傳送的控制器
```
public class Sample01Controller : Controller
{
    public ActionResult Create() //網站進入點
    {
        return View();
    }
    [HttpPost]
    public ActionResult Create(HttpPostedFileBase myfile)//接收
    { 
        string filename = "";
        if(myfile != null)
        {
            if (myfile.ContentLength > 0) //存檔在images
            {
                filename = Path.GetFileName(myfile.FileName);
                string path = string.Format("{0}/{1}", Server.MapPath("~/images"), filename);
                Console.WriteLine(path);
                myfile.SaveAs(path);
            }

        }
        return RedirectToAction("ShowPhotos");
    }
    public string ShowPhotos()//傳送ShowPhotos的html，你會發現這端程式碼會塞在body中，其他是自動打好的
    {
        string show = "";
        DirectoryInfo dir = new DirectoryInfo(Server.MapPath("~/images"));
        FileInfo[] fInfo = dir.GetFiles();
        foreach(FileInfo f in fInfo)
        {
            show += string.Format("<img src='../images/{0}' width='500' height='500' > ", f.Name);
        }
        show += "<hr>";
        show += "<a href='Create'>返回</a>";
        return show;
    }
}
```
* Create.cshtml
```
@{
    Layout = null;
}
<!DOCTYPE html>
<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>Create</title>
</head>
<body>
    <form enctype="multipart/form-data" method="post" action="@Url.Action("Create")">
        <p>照片<input type="file" name="myfile" /></p>
        <p><input type="submit" value="upload" /></p>
    </form>
</body>
</html>
```
:::danger
這邊要注意的是，如需要使用HttpPostedFileBase上傳檔案的話，則必須添加 [enctype = "multipart/form-data"](https://stackoverflow.com/questions/4526273/what-does-enctype-multipart-form-data-mean)，不然怎麼樣都沒辦法正常收到資料！
:::
你上傳的檔案會再這個專案路徑底下的images資料夾出現。

### 多按鍵上傳多個檔案 : myfiles視為整體(所以用Length)
有種把檔案放在array中
```
[HttpPost]
public ActionResult Create(HttpPostedFileBase[] myfiles)
{
    string filename = "";
    for(int i = 0; i < myfiles.Length; i++)
    {
        HttpPostedFileBase myfile = (HttpPostedFileBase)myfiles[i];
        if (myfile != null)
        {
            if (myfile.ContentLength > 0)
            {
                filename = Path.GetFileName(myfile.FileName);
                string path = string.Format("{0}/{1}", Server.MapPath("~/images"), filename);
                Console.WriteLine(path);
                myfile.SaveAs(path);
            }
        }
    } 
    return RedirectToAction("ShowPhotos");
}
```
改Create.cshtml
```
@{
    Layout = null;
}
<!DOCTYPE html>
<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>Create</title>
</head>
<body>
    <form enctype="multipart/form-data" method="post" action="@Url.Action("Create")">
        <p>照片1<input type="file" name="myfiles" /></p>
        <p>照片2<input type="file" name="myfiles" /></p>
        <p>照片3<input type="file" name="myfiles" /></p>
        <p><input type="submit" value="upload" /></p>
    </form>
</body>
</html>
```
### 一個按鍵上傳多個 : myfiles視為多個組成( Count() )

```
[HttpPost]
public ActionResult Create(HttpPostedFileBase[]  myfiles)
{
    if (myfiles.Count() > 0) 
    {
        for (int i =0;i<myfiles.Count();i++)
        {
            HttpPostedFileBase myfile = (HttpPostedFileBase)myfiles[i];
            if (myfile.ContentLength > 0)
            {
                Console.WriteLine(myfile);
                string savePath = Server.MapPath("~/images/");
                myfile.SaveAs(savePath + myfile.FileName);
            }
        }
    }
    return RedirectToAction("ShowPhotos");
}
```
融會貫通之前的型別的話，也可以寫成
```
[HttpPost]
public ActionResult Create(HttpPostedFileBase[]  myfiles)
{
    if (myfiles.Count() > 0)
    {
        foreach (var myfile in myfiles)
        {
            if (myfile.ContentLength > 0)
            {
                string savePath = Server.MapPath("~/images/");
                myfile.SaveAs(savePath + myfile.FileName);
            }
        }
    }
    return RedirectToAction("ShowPhotos");
}
```
:::success
速度我覺得差蠻多的!!
[Array.Count() VS Array.Length](https://dotblogs.com.tw/sf71316/2013/06/18/105604)
:::
* Create.cshtml
```
@{
    Layout = null;
}
<!DOCTYPE html>
<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>Create</title>
</head>
<body>
    <div>
        @using (Html.BeginForm("Create", "Sample02", FormMethod.Post, new { enctype = "multipart/form-data" }))
        {
            <input type="file" name="myfiles" multiple />
            <input type="submit" value="上傳" />
        }
    </div>
    <form enctype="multipart/form-data" method="post" action="@Url.Action("Create")">
        <p>照片<input type="file" name="myfiles" multiple /></p>
        <p><input type="submit" value="upload" /></p>
    </form>
</body>
</html>
```
這裡我多個寫法，其實功能都一樣，form 上面是用 Helper 的寫法 [認識View - Helper](https://ithelp.ithome.com.tw/articles/10160299)
:::danger
這裡執行後，假如你上傳大張或很多張圖片的話，有可能出現錯誤，"超出最大要求長度"。這是因為根據MSDN記載，ASP.NET為防止駭客利用大檔案進行DOS(Denial Of Service)攻擊，所以把上傳檔案大小限制在4096KB(4MB)，因此當上傳檔案超過4MB時，就會收到System.Web.HttpException 超出最大的要求長度的錯誤訊息。
因此你可以到Web.config修改成以下。 [詳細介紹](https://dotblogs.com.tw/terrychuang/2011/04/07/22303)
```
<system.web>
 <httpRuntime targetFramework="x.x" maxRequestLength="102400" executionTimeout="600"/>
</system.web>
```
maxRequestLength  為 上傳檔案大小 單位(KB)
executionTimeout 為 上傳時間 單位(S)
:::

---
