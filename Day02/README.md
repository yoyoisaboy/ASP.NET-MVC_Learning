# ASP .NET MVC 上手 Day02_資料庫(新增&修改&刪除)
###### tags: `asp.netMVC` `C#`

## 建立資料庫

* 在專案底下找App_Data，右鍵加入->新增項目->資料(左邊)->SQL Server資料庫->取名(範例取dbMember.mdf)，可以看到左邊出現伺服器總管。
* 在伺服器總管底下的資料連結裡，找我們的dbMember.mdf，右鍵資料表->加入新的資料表。接著輸入table名稱(範例為tMember)和欄位型態(nvarchar(50)、int)。
* 接著輸入欄位的名稱和資料型別，點擊Id找右下角的屬性，找最後一個識別規格，展開它並將(為識別)改成True，這樣之後塞資料會自動遞增Id。
* 在按更新(在表格的上面)->更新資料庫->左邊的伺服器總管按重新整理->就可以看到資料表新增了剛剛定義好的table。
![](https://i.imgur.com/3nJV7A9.png)
* 右鍵資料表中新增的table->顯示資料表資料，輸入Name、Phone、Age資料，Id會自己遞增。
![](https://i.imgur.com/aICkUwA.png)
* 輸入完後，要將這資料更新到Models中，剛我們點右下角的屬性改回原本的方案總管，右鍵Models->加入->新增項目->資料(左邊)->選ADO.NET實體資料模型->取名(範例為dbMemberModel)->新增->下一步->下一步(記住下面的勾勾，這名稱之後會用到)->下一步->勾資料表(展開確認剛新增的table)->完成。完成如下圖
:::success
ADO.NET實體資料模型 : [超詳細介紹](https://docs.microsoft.com/zh-tw/dotnet/framework/data/adonet/entity-data-model)
:::
![](https://i.imgur.com/NwHcUkN.png)
* 最後很重要的記得右鍵你的專案->建置，這樣他才會讀到local端DB的table
![](https://i.imgur.com/vT0GwlE.png)


---


## 測試傳到Views
* 在 Controllers 的 HomeController.cs中新增以下
```
dbMemberEntities db = new dbMemberEntities();
public ActionResult ShowdbMember()
{
    //Id由大排到小, t 是隨便取
    var emps = db.tMember.OrderByDescending(t => t.Id).ToList();
    return View(emps);
}
``` 
* 右鍵ShowdbMember()->新增檢視->MVC x 檢視->加入(這次記得勾使用版面配置頁，也就是bootstrap引入框架)
:::danger
到這裡我要補充一下，有些人可能都有裝好，但也有可能跟我一樣沒裝好的，只好就自己裝。沒裝好像這樣~
![](https://i.imgur.com/eJn7fWs.png)
除了Content裡沒看到bootstrap之外，也少了Scripts。
應該要長這樣~
![](https://i.imgur.com/RE3MvBE.png)

如果沒有出現到這以下步驟安裝，到你的專案右鍵->管理NuGet套件
![](https://i.imgur.com/bF1LpXe.png)
接著點瀏覽搜尋處輸入 
1. bootstrap 安裝 3.3.4 
2. jQuery 安裝 3.4.1 
3. modernizr 安裝 2.8.3
安裝好後就出現在你專案底下了，安裝的版本有相容性關係，最好確認完再安裝，我所提供的版本是根據 Views->Shared 的 _Layout.cshtml 如下
```
第7、8、9行
<link href="~/Content/Site.css" rel="stylesheet" type="text/css" />
<link href="~/Content/bootstrap.min.css" rel="stylesheet" type="text/css" />
<script src="~/Scripts/modernizr-2.8.3.js"></script>
第37、78行
<script src="~/Scripts/jquery-3.4.1.min.js"></script>
<script src="~/Scripts/bootstrap.min.js"></script>
```
順便說明_Layout.cshtml，它是網頁的外框的感覺，將Home中的xxx.cshtml合併起來。
其中有三個地方有@
1. @ViewBag.Title : 就是ShowdbMember.cshtml裡的 ```<title>```
2. @Html.ActionLink : 一樣Ctrl點Html、ActionLink展開看註解，有分常完整的解釋，懶得看的就看前三個，分別是(網頁呈現文字,xxxController中的哪一個控制器，通常是Index,哪一個Controller)
3. @RenderBody :  xxx.cshtml 的內容擺放區
:::
* 接著改剛創好的 ShowdbMember.cshtml 如下
```
@model IList<hackMD_MVCdemo.Models.tMember>
@{
    ViewBag.Title = "會員清單";
}
<h2>ShowdbMember</h2>
<table class="table">
    <tr>
        <td>編號</td>
        <td>姓名</td>
        <td>手機</td>
        <td>年齡</td>
    </tr>
    @foreach (var item in Model)
    {
        <tr>
            <td>@item.Id</td>
            <td>@item.Name</td>
            <td>@item.Phone</td>
            <td>@item.Age</td>
        </tr>
    }
</table>
```
可以發現第一行跟[Day01](https://hackmd.io/o4fD84RHTTqfzI9eyngQ_w?both)用差不多，就差呼叫的是資料庫的table，其他都一樣。

執行看看結果，會如下

![](https://i.imgur.com/aNyJLAo.png)

打勾的地方研究改看看

忘了說圖中的會員清單、新增會員是在 _Layout.cshtml的這新增
```
<div class="navbar-collapse collapse">
    <ul class="nav navbar-nav">
        <li><a href="@Url.Action("ShowdbMember")">會員清單</a></li>
        <li><a href="@Url.Action("ShowCreat")">新增會員</a></li>
    </ul>
</div>
```
直覺 "" 內就是你的控制器，因為ShowCreate還沒創所以會沒網頁。


---

## Create 資料

* 一樣先到 HomeController.cs 多一個控制器，如下
```
public ActionResult ShowCreat()
{
    return View();
}
```        
跟之前一樣右鍵 -> 新增檢視 -> MVC x 檢視 -> 加入
修改ShowCreat.cshtml如下
```
@{
    ViewBag.Title = "員工新增";
}
<h2>員工新增</h2>
<form method="post" action="@Url.Action("Create")">
    <p>姓名<input type="text" name="Name" required class="form-control" /></p>
    <p>電話<input type="text" name="Phone" required class="form-control" /></p>
    <p>年齡<input type="number" name="Age" required class="form-control" max="80" min="20" /></p>
    <p><input type="submit" value="新增" class="btn btn-success" /></p>
</form>
```
:::success
Http Method(Post、Get、Delete、Put): [詳細介紹](https://note.artchiu.org/2017/09/30/常見的http-method的不同性質分析：getpost和其他4種method的差別/)

可以發現form 中的 action多一個Create，當submit被點時就會把資料傳到控制器那邊，所以假如控制器那邊沒有定義好收資料的控制器會沒反應。
:::
* 取得前端給的資料，我們回到 HomeController.cs 再新增如下
```
[HttpPost]
public ActionResult Create(string Name,string Phone,int Age)
{
    tMember dbmember = new tMember();
    dbmember.Name = Name;
    dbmember.Phone = Phone;
    dbmember.Age = Age;
    db.tMember.Add(dbmember);
    db.SaveChanges();
    return RedirectToAction("ShowdbMember");
}
```
:::success
* [HttpPost] : 建立與前端的關係
* 會有三個參數進來，定義好型態
* 建立table的物件，tMember這個你可以Ctrl進去看，你會發現Models已經把他的get、set定義好了，所以可以直接建立物件來用。
* 接下來就把資料輸入進dbmember這個空殼裡面，裝完再把資料Add進tMember，並SaveChanges起來。
* 處裡完直接進入ShowMember這個控制器的頁面
* RedirectToAction : 導向其他外的 Controller
:::
到ShowCreat.cshtml執行，輸入資料進去

![](https://i.imgur.com/7MiMlTf.png)
![](https://i.imgur.com/gqvZR0W.png)

回到左邊的資料庫，看資料表的tMember -> 右鍵 -> 顯示資料表資料，可以看到資料確實餵進去了。
![](https://i.imgur.com/607QPtG.png)


---

## Edit 資料

我們在 ShowMember.cshtml 中加入一些修改按，如下
```
@model IList<hackMD_MVCdemo.Models.tMember>
@{
    ViewBag.Title = "會員清單";
}
<h2>ShowdbMember</h2>
<table class="table">
    <tr>
        <td>編號</td>
        <td>姓名</td>
        <td>手機</td>
        <td>年齡</td>
        <td></td>
    </tr>
    @foreach (var item in Model)
    {
        <tr>
            <td>@item.Id</td>
            <td>@item.Name</td>
            <td>@item.Phone</td>
            <td>@item.Age</td>
            <td><a href="@Url.Action("ShowEdit")?Id=@item.Id" class="btn btn-info">修改</a></td>
        </tr>
    }
</table>
```
![](https://i.imgur.com/7QmLjLv.png)
可以發現修改的按鈕，Url.Action的方法是用Edit的控制器，?後面是要傳的參數，這裡是根據Id去知道哪一位要被改。
:::success
?號傳送參數?Querystring : [詳細連結](https://marcus116.blogspot.com/2018/09/net-querystring.html)，熟悉之後你要對網址有些感覺
:::
* 回到 HomeController.cs ， 新增以下 -> 對ShowEdit右鍵 -> 新增檢視...
```
public ActionResult ShowEdit(int Id)
{
    var emp = db.tMember.Where(m => m.Id == Id).FirstOrDefault(); //讀資料庫特定Id的欄位資料
    return View(emp);
}
[HttpPost]
public ActionResult Edit(int Id,string Name, string Phone, int Age)
{
    var emp = db.tMember.Where(m => m.Id == Id).FirstOrDefault(); //讀資料庫特定Id的欄位資料
    emp.Id = Id;
    emp.Name = Name;
    emp.Phone = Phone;
    emp.Age = Age;
    db.SaveChanges();
    return RedirectToAction("ShowdbMember");
}
```
* ShowEdit.cshtml 如下
```
@model hackMD_MVCdemo.Models.tMember
@{
    ViewBag.Title = "修改";
}
<h2>修改</h2>
<form method="post" action="@Url.Action("Edit")">
    <input type="hidden" value="@Model.Id" name="Id" />
    <p>姓名<input type="text" name="Name" value="@Model.Name" required class="form-control" /></p>
    <p>電話<input type="text" name="Phone" value="@Model.Phone" required class="form-control" /></p>
    <p>年齡<input type="number" name="Age" value="@Model.Age" required class="form-control" max="80" min="20" /></p>
    <p><input type="submit" value="儲存" class="btn btn-danger" /></p>
</form>
```
當按下修改按鈕，會進入修改的頁面(ShowEdit.cshtml)，輸入完資料按下按鈕後會傳(value的Id、Name、Phone、Age)進入 Edit 的 Controller 。
:::danger
如果你直接執行ShowEdit.cshtml會出現錯誤，你可以觀察錯誤訊息，發現說這個頁面是需要傳參數(這範例是Id)才能運作的。 這原因請觀察HomeController.cs的ShowEdit函式，都是要傳值得，所以跑到沒值得當然會出錯。
:::

原本:
![](https://i.imgur.com/bqPtpN8.png)
修改後:
![](https://i.imgur.com/CPJNipw.png)


---

## Delete 資料

會以上的話這個就簡單了吧，我就直接講啦~
HomeController.cs 新增以下
```
//Delete
public ActionResult Delete(int Id)
{
    var emp = db.tMember.Where(m => m.Id == Id).FirstOrDefault();
    db.tMember.Remove(emp); //就是已經刪掉資料的table
    db.SaveChanges(); //記得存
    return RedirectToAction("ShowdbMember");
}
```
在 HomeMember.cshtml 新增按鈕在"新增"按鈕旁邊
```
<a href="@Url.Action("Delete")?Id=@item.Id" onclick="return confirm('確定刪除?');" class="btn btn-danger">刪除</a>
```
:::success
onclick="return confirm('確定刪除?');"這是按下去跳出提示視窗，confirm()函式中的引數是確認框的提示語。js用法
:::
![](https://i.imgur.com/IwTh3qJ.png)



END