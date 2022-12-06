# ASP .NET MVC 上手 Day09(會員登入機制)

###### tags: `asp.netMVC` `C#`

因為功能之前都有講解過，所以此篇的程式碼不會做太多的解說。
# 會員登入流程

1. Authorize 驗證
2. web.config 設定未登入時想導入的網頁
3. FormsAuthentication.RedirectFormLoginPage(member.帳號,false); 視窗關閉後，Cookie自動失效
4. 判斷登入狀態
---
## 會員登入 HomeController.cs
```
public class HomeController : Controller
{
    // GET: Home
    public ActionResult Index()
    {

        return View();
    }

    [HttpPost]
    public ActionResult Index(string 帳號, string 密碼) //看cshtml中name的名稱
    {
        dbProductEntities db = new dbProductEntities();
            var member = db.會員.Where(m => m.帳號 == 帳號 && m.密碼 == 密碼).FirstOrDefault(); //查詢
            if (member != null)
            {
                FormsAuthentication.RedirectFromLoginPage(member.帳號, true); //驗證
                return RedirectToAction("Index", "Category");
            }
            ViewBag.IsLogin = true;
            return View();
        }
    }
```
## Index.cshtml 登入介面
```
@{
    Layout = null;
}
<!DOCTYPE html>
<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>會員登入</title>
    <link href="~/Content/bootstrap.min.css" rel="stylesheet" />
    <script src="~/Scripts/jquery-3.4.1.min.js"></script>
    <script src="~/Scripts/bootstrap.min.js"></script>
</head>
<body>
    <form action="@Url.Action("Index")" method="post">
        <div class="container" style="margin-top:20px;">
            <div class="row">
                <div class="panel panel-primary">
                    <div class="panel-heading">會員登入</div>
                    <div class="form-group">
                        <label for="帳號 ">帳號 : </label>
                        <input type="text" class="form-control" id="帳號" name="account" />
                    </div>
                    <div class="form-group">
                        <label for="password">密碼 : </label>
                        <input type="password" class="form-control" id="密碼" name="密碼" />
                    </div>
                    <input type="submit" value="登入" class="btn btn-primary" />
                    @if(ViewBag.IsLogin != null)
                    {
                        <div class="alert alert-danger">
                            <strong>密碼錯誤!</strong> 請重新登入
                        </div>
                    }
                </div>
            </div>
        </div>
    </form>
</body>
</html>

```
## Web.config 的設定
當沒有通過驗證，就是執行`loginUrl="~/Home/Index"`的動作方法
```
<system.web>
    <compilation debug="true" targetFramework="4.7.2" />
    <httpRuntime targetFramework="4.7.2" />
    <authentication mode="Forms">
          <forms loginUrl="~/Home/Index"></forms>
    </authentication>
</system.web>
```


# 共用錯誤訊息View設計
## Controllers 中的 PermissionErrorMsgController.cs撰寫錯誤顯示
```
public class PermissionErrorMsgController : Controller
{
    [Authorize]
    // GET: PermissionErrorMsg
    public ActionResult Index(string msg)
    {
        ViewBag.ErrorMsg = msg;
        return View();
    }
}
```
## 新增檢視 Index.cshtml
```

@{
    ViewBag.Title = "權限錯誤";
}

<h2>權限錯誤</h2>

<div class="alert alert-danger" >
    <strong>權限錯誤</strong> @ViewBag.ErrorMsg


</div>
```
之後如果權限不足，再傳直給PermissionErrorMsgController.cs的 Index中
```
return RedirectToAction("Index", "PermissionErrorMsg", new { msg = "你算哪根蔥?敢點這功能?!" });
```

# 會員管理系統
## Controller 中的MemberController.cs 撰寫登入的機制

```
using Day09.Models;
...
public class MemberController : Controller
{
    dbProductEntities db = new dbProductEntities();
    // GET: Member
    [Authorize]
    public ActionResult Index()
    {
        //HttpContext.User 屬性
        string uid = User.Identity.Name;
        string role = db.會員.Where(m => m.帳號 == uid).FirstOrDefault().角色;
        if (role != "管理者")
        {
            return RedirectToAction("Index", "PermissionErrorMsg", new { msg = "你算哪根蔥?敢點這功能?!" });
        }
        List<會員> members = new List<會員>();
        foreach(var item in db.會員)
        {
            var member = new 會員();
            member.帳號 = item.帳號;
            member.密碼 = item.密碼; 
            //判斷字元，改放中文
            member.權限 = (item.權限.Contains("R") ? "讀取 " : "") +
                          (item.權限.Contains("C") ? "新增 " : "") +
                          (item.權限.Contains("U") ? "修改 " : "") +
                          (item.權限.Contains("D") ? "刪除 " : "");
            member.角色 = item.角色;
            members.Add(member);

        }
        return View(members);
    }
}
```

## 檢視頁面 Index.cshtml

List 範本、使用版面配置頁
```
<table class="table">
    <tr>
        <th>
            @Html.DisplayNameFor(model => model.帳號)
        </th>
        <th>
            @Html.DisplayNameFor(model => model.密碼)
        </th>
        <th>
            @Html.DisplayNameFor(model => model.角色)
        </th>
        <th>
            @Html.DisplayNameFor(model => model.權限)
        </th>
        <th></th>
    </tr>

@foreach (var item in Model) {
    <tr>
        <td>
            @Html.DisplayFor(modelItem => item.帳號)
        </td>
        <td>
            @Html.DisplayFor(modelItem => item.密碼)
        </td>
        <td>
            @Html.DisplayFor(modelItem => item.角色)
        </td>
        <td>
            @Html.DisplayFor(modelItem => item.權限)
        </td>
        <td>
            @Html.ActionLink("編輯", "Edit", new { userid = item.帳號 }, new { @class = "btn btn-success"}) 
            @Html.ActionLink("刪除", "Delete", new { userid = item.帳號 }, new { @class = "btn btn-danger", onclick = "return confirm('確定刪除?');" }) 
        </td>
    </tr>
}

</table>
```
:::success
HttpContext.User 屬性 : [詳細內容](https://docs.microsoft.com/zh-tw/dotnet/api/system.web.httpcontext.user?view=netframework-4.8)
:::


## 登入非管理者
![](https://i.imgur.com/JXT4Rpm.png)
## 登入管理者
![](https://i.imgur.com/lWMKQcp.png)

---

## Create
一樣會需要有 Authorize 來判斷是否登入、role 判斷權限
```
[Authorize]
public ActionResult Create()
{
    string uid = User.Identity.Name;
    string role = db.會員.Where(m => m.帳號 == uid).FirstOrDefault().角色;
    if (role != "管理者")
    {
        return RedirectToAction("Index", "PermissionErrorMsg", new { msg = "你算哪根蔥?敢點這功能?!" });
    }

    return View();
}
[Authorize]
[HttpPost]
public ActionResult Create(string 帳號,string 密碼,string 角色,string[] 權限)
{
    string userid = 帳號;
    var tempMember = db.會員.Where(m => m.帳號 == userid).FirstOrDefault();
    if(tempMember != null)
    {
        ViewBag.IsMember = true;
        return View();
    }
    string Permission = "R";
    for (int i = 0; i < 權限.Length; i++)
    {
        Permission += 權限[i];
    }
    會員 member = new 會員();
    member.帳號 = 帳號;
    member.密碼 = 密碼;
    member.角色 = 角色;
    member.權限 = Permission;
    db.會員.Add(member);
    db.SaveChanges();
    return RedirectToAction("Index");
}
```
## 新增檢視Create.cshtml
![](https://i.imgur.com/9cU9iLu.png)

```
@model Day09.Models.會員

@{
    ViewBag.Title = "會員新增";
}

<h2>會員新增</h2>

@using (Html.BeginForm()) 
{
    @Html.AntiForgeryToken()
    
    <div class="form-horizontal">
       
        <hr />
        @Html.ValidationSummary(true, "", new { @class = "text-danger" })
        <div class="form-group">
            @Html.LabelFor(model => model.帳號, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(model => model.帳號, new { htmlAttributes = new { @class = "form-control" } })
                @Html.ValidationMessageFor(model => model.帳號, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(model => model.密碼, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(model => model.密碼, new { htmlAttributes = new { @class = "form-control" } })
                @Html.ValidationMessageFor(model => model.密碼, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(model => model.角色, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(model => model.角色, new { htmlAttributes = new { @class = "form-control" } })
                @Html.ValidationMessageFor(model => model.角色, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(model => model.權限, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                <label class="checkbox-inline">基本功能讀取</label>
                <label class="checkbox-inline"><input type="checkbox" value="C" name="權限" />新增</label>
                <label class="checkbox-inline"><input type="checkbox" value="U" name="權限" />修改</label>
                <label class="checkbox-inline"><input type="checkbox" value="D" name="權限" />刪除</label>
            </div>
        </div>

        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <input type="submit" value="新增" class="btn btn-default" />
            </div>
        </div>
    </div>
}

@if(ViewBag.IsMember != null)
{
    <div class="alert alert-danger">
        <strong>此帳號有人使用</strong> 重新建立!!
    </div> 
}

```
---

## Delete

```
[Authorize]
public ActionResult Delete(string userid)
{
    string uid = User.Identity.Name;
    string role = db.會員.Where(m => m.帳號 == uid).FirstOrDefault().角色;
    if (role != "管理者")
    {
        return RedirectToAction("Index", "PermissionErrorMsg", new { msg = "你算哪根蔥?敢點這功能?!" });
    }
    var member = db.會員.Where(m => m.帳號 == userid).FirstOrDefault();
    db.會員.Remove(member);
    db.SaveChanges();
    return RedirectToAction("Index");
}
```

---

## Edit

```
[Authorize]
public ActionResult Edit(string userid)
{
    string uid = User.Identity.Name;
    string role = db.會員.Where(m => m.帳號 == uid).FirstOrDefault().角色;
    if (role != "管理者")
    {
        return RedirectToAction("Index", "PermissionErrorMsg", new { msg = "你算哪根蔥?敢點這功能?!" });
    }
    var member = db.會員.Where(m => m.帳號 == userid).FirstOrDefault();
    return View(member);
}

[Authorize]
[HttpPost]
public ActionResult Edit(string 帳號,string 密碼,string 角色,string[] 權限)
{
    string Permission = "R";
    if (權限 != null)
    {
        for(int i =0; i < 權限.Length; i++)
        {
            Permission += 權限[i];
        }
    }
    var member = db.會員.Where(m => m.帳號 == 帳號).FirstOrDefault();
    member.密碼 = 密碼;
    member.角色 = 角色;
    member.權限 = Permission;
    db.SaveChanges();
    return RedirectToAction("Index");
}
```
## 新增檢視 Edit.cshtml

* 一樣把權限的地方改成勾選的
* @readonly = "readonly" -> 防止帳號更改
```
@model Day09.Models.會員

@{
    ViewBag.Title = "會員編輯";
}

<h2>會員編輯</h2>

@using (Html.BeginForm())
{
    @Html.AntiForgeryToken()
    
    <div class="form-horizontal">
        <hr />
        @Html.ValidationSummary(true, "", new { @class = "text-danger" })
        <div class="form-group">
            @Html.LabelFor(model => model.帳號, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(model => model.帳號, new { htmlAttributes = new { @class = "form-control" ,@readonly = "readonly"} })
                @Html.ValidationMessageFor(model => model.帳號, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(model => model.密碼, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(model => model.密碼, new { htmlAttributes = new { @class = "form-control" } })
                @Html.ValidationMessageFor(model => model.密碼, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(model => model.角色, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(model => model.角色, new { htmlAttributes = new { @class = "form-control" } })
                @Html.ValidationMessageFor(model => model.角色, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(model => model.權限, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                <div class="col-md-10">
                    <label class="checkbox-inline">基本功能讀取</label>
                    <label class="checkbox-inline"><input type="checkbox" value="C" name="權限" @(Model.權限.Contains("C") ? "checked" : "")>新增</label>
                    <label class="checkbox-inline"><input type="checkbox" value="U" name="權限" @(Model.權限.Contains("U") ? "checked" : "")>修改</label>
                    <label class="checkbox-inline"><input type="checkbox" value="D" name="權限" @(Model.權限.Contains("D") ? "checked" : "")>刪除</label>
                </div>
            </div>
         </div>

        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <input type="submit" value="編輯" class="btn btn-default" />
            </div>
        </div>
    </div>
}

<div>
    @Html.ActionLink("Back to List", "Index")
</div>

```

# 產品類別管理

一樣有新增、修改、刪除、編輯，把所有的tabel改成產品類別 ，其他都差不多

## 變動時間 在Models中定義 Sys.cs ， 標準化時間格式
```
public static string StringConverDateTimeString(string str)
{
    if(str == "" || str == null || str.Length != 14) { return ""; }
    return
        str.Substring(0, 4) + "年" + str.Substring(4, 2) + "月" +
        str.Substring(6, 2) + "日" + str.Substring(8, 2) + "時" +
        str.Substring(10, 2) + "分" + str.Substring(12, 2) + "秒";
}
```


## 產品類別新增、修改、刪除、編輯
```
public class CategoryController : Controller
{
    dbProductEntities db = new dbProductEntities();
    // GET: Category
    [Authorize]
    public ActionResult Index()
    {
        string uid = User.Identity.Name;
        string Permission = db.會員.Where(m => m.帳號 == uid).FirstOrDefault().權限;
        ViewBag.Permission = Permission;

        List<產品類別> category = new List<產品類別>();
        foreach(var item in db.產品類別.OrderByDescending(m => m.修改日))
        {
            category.Add(new 產品類別()
            {
                類別編號 = item.類別編號,
                類別名稱 = item.類別名稱,
                編輯者 = item.編輯者,
                修改日 = Sys.StringConverDateTimeString(item.修改日),
                建立日 = Sys.StringConverDateTimeString(item.建立日)
            });
        }
        return View(category);
    }

    [Authorize]
    public ActionResult Create()
    {
        string uid = User.Identity.Name;
        string Permission = db.會員.Where(m => m.帳號 == uid).FirstOrDefault().權限;
        if (!Permission.Contains("C"))
        {
            return RedirectToAction("Index", "PermissionErrorMsg", new { msg = "權限不足" });

        }
        return View();
    }
    [Authorize]
    [HttpPost]
    public ActionResult Create(string 類別名稱)
    {
        var tempProduct = db.產品類別.Where(m => m.類別名稱 == 類別名稱).FirstOrDefault();
        if (tempProduct != null)
        {
            ViewBag.IsProduct = true;
            return View();
        }

        string editdate = DateTime.Now.ToString("yyyyMMddHHmmss");

        產品類別 category = new 產品類別();
        category.類別名稱 = 類別名稱;
        category.編輯者 = User.Identity.Name;
        category.建立日 = editdate;
        category.修改日 = editdate;
        db.產品類別.Add(category);
        db.SaveChanges();
        return RedirectToAction("Index");
    }

    [Authorize]
    public ActionResult Delete(int cid)
    {
        string uid = User.Identity.Name;
        string Permission = db.會員.Where(m => m.帳號 == uid).FirstOrDefault().權限;
        if (!Permission.Contains("D"))
        {
            return RedirectToAction("Index", "PermissionErrorMsg", new { msg = "權限不足" });
        }
        var products = db.產品資料.Where(m => m.類別編號 == cid).ToList();
        var category = db.產品類別.Where(m => m.類別編號 == cid).FirstOrDefault();
        db.產品資料.RemoveRange(products);
        db.產品類別.Remove(category);
        db.SaveChanges();
        return RedirectToAction("Index");
    }

    [Authorize]
    public ActionResult Edit(int cid)
    {
        string uid = User.Identity.Name;
        string Permission = db.會員.Where(m => m.帳號 == uid).FirstOrDefault().權限;
        if (!Permission.Contains("D"))
        {
            return RedirectToAction("Index", "PermissionErrorMsg", new { msg = "權限不足" });
        }
        var category = db.產品類別.Where(m => m.類別編號 == cid).FirstOrDefault();
        return View(category);
    }
    [Authorize]
    [HttpPost]
    public ActionResult Edit(int 類別編號,string 類別名稱)
    {
        string editdate = DateTime.Now.ToString("yyyyMMddHHmmss");
        var category = db.產品類別.Where(m => m.類別編號 == 類別編號).FirstOrDefault();
        category.類別名稱 = 類別名稱;
        category.編輯者 = User.Identity.Name;
        category.修改日 = editdate;
        db.SaveChanges();
        return RedirectToAction("Index");
    }

}
```

## Create.cshtml
```
@model Day09.Models.產品類別

@{
    ViewBag.Title = "產品類別新增";
}

<h2>產品類別新增</h2>

@using (Html.BeginForm())
{
    @Html.AntiForgeryToken()

    <div class="form-horizontal">

        <hr />

        <div class="form-group">
            @Html.LabelFor(model => model.類別名稱, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(model => model.類別名稱, new { htmlAttributes = new { @class = "form-control" } })
                @Html.ValidationMessageFor(model => model.類別名稱, "", new { @class = "text-danger" })
            </div>
        </div>


        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <input type="submit" value="新增" class="btn btn-default" />
            </div>
        </div>
    </div>
}

@if (ViewBag.IsProduct != null)
{
    <div class="alert alert-danger">
        <strong>已有此類別!</strong> 重新建立!!
    </div>
}
```
## Edit.cshtml
```
@model Day09.Models.產品類別

@{
    ViewBag.Title = "類別編輯";
}

<h2>類別編輯</h2>

@using (Html.BeginForm())
{
    @Html.AntiForgeryToken()
    
    <div class="form-horizontal">
        <hr />
        @Html.ValidationSummary(true, "", new { @class = "text-danger" })
        <div class="form-group">
            @Html.LabelFor(model => model.類別編號, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(model => model.類別編號, new { htmlAttributes = new { @class = "form-control" ,@readonly = "readonly"} })
                @Html.ValidationMessageFor(model => model.類別編號, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(model => model.類別名稱, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(model => model.類別名稱, new { htmlAttributes = new { @class = "form-control" } })
                @Html.ValidationMessageFor(model => model.類別名稱, "", new { @class = "text-danger" })
            </div>
        </div>



        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <input type="submit" value="Save" class="btn btn-default" />
            </div>
        </div>
    </div>
}

<div>
    @Html.ActionLink("Back to List", "Index")
</div>

```
---

# 產品資料管理
* 這比較不一樣的是會用到"產品資料"和"產品類別"的table，所以會先定義兩邊的資料封裝
## Models 中 產品類別產品資料ViewModel.cs
```
public class 產品類別產品資料ViewModel
{
    public List<產品類別> Category { get; set; }
    public List<產品資料> Product { get; set; }
}
```
## ProductController.cs 的Index顯示頁
```
public class ProductController : Controller
{
    dbProductEntities db = new dbProductEntities();
    // GET: Product
    [Authorize]
    public ActionResult Index(int cid = 1)
    {
        string uid = User.Identity.Name;
        string Permission = db.會員.Where(m => m.帳號 == uid).FirstOrDefault().權限;
        ViewBag.Permission = Permission;

        產品類別產品資料ViewModel vm = new 產品類別產品資料ViewModel();
        vm.Category = db.產品類別.OrderByDescending(m => m.修改日).ToList();
        var tempProduct = db.產品資料.Where(m => m.類別編號 == cid).OrderByDescending(m => m.修改日).ToList();

        List<產品資料> product = new List<產品資料>();
        foreach(var item in tempProduct)
        {
            product.Add(new 產品資料()
            {
                產品編號 = item.產品編號,
                品名 = item.品名,
                單價 = item.單價,
                圖示 = item.圖示,
                類別編號 = item.類別編號,
                編輯者 = item.編輯者,
                修改日 = Sys.StringConverDateTimeString(item.修改日),
                建立日 = Sys.StringConverDateTimeString(item.建立日)
            });
        }
        vm.Product = product;
        return View(vm);
    }
}
```

## Index.cshtml
顯示清單，可不可以編輯&刪除是根據登入者的權限來判斷` ViewBag.Permission = Permission;`用這段來傳到前端判斷。

```
@model Day09.Models.產品類別產品資料ViewModel
@{
    ViewBag.Title = "產品列表";
    string Permission = ViewBag.Permission.ToString();
}

<div>產品列表</div>
<div class="row">
    <div class="col-sm-2">
        @foreach(var item in Model.Category)
            {
                <p>@Html.ActionLink(item.類別名稱,"Index",new { cid = item.類別編號})</p>
            }
    </div>
    <div class="col-sm-10">

        <table class="table">
            <tr>
                <th>
                    @Html.DisplayNameFor(model => model.Product.FirstOrDefault().產品編號)
                </th>
                <th>
                    @Html.DisplayNameFor(model => model.Product.FirstOrDefault().品名)
                </th>
                <th>
                    @Html.DisplayNameFor(model => model.Product.FirstOrDefault().單價)
                </th>
                <th>
                    @Html.DisplayNameFor(model => model.Product.FirstOrDefault().圖示)
                </th>
                <th>
                    @Html.DisplayNameFor(model => model.Product.FirstOrDefault().編輯者)
                </th>
                <th>
                    @Html.DisplayNameFor(model => model.Product.FirstOrDefault().建立日)
                </th>
                <th>
                    @Html.DisplayNameFor(model => model.Product.FirstOrDefault().修改日)
                </th>
                <th></th>
            </tr>

            @foreach (var item in Model.Product)
            {
                <tr>
                    <td>
                        @Html.DisplayFor(modelItem => item.產品編號)
                    </td>
                    <td>
                        @Html.DisplayFor(modelItem => item.品名)
                    </td>
                    <td>
                        @Html.DisplayFor(modelItem => item.單價)
                    </td>
                    <td>
                        <img src="~/Images/@item.圖示" style="width:120px;" />
                    </td>
                    <td>
                        @Html.DisplayFor(modelItem => item.編輯者)
                    </td>
                    <td>
                        @Html.DisplayFor(modelItem => item.建立日)
                    </td>
                    <td>
                        @Html.DisplayFor(modelItem => item.修改日)
                    </td>
                    <td>
                       @if (Permission.Contains("U"))
                        {
                           @Html.ActionLink("編輯", "Edit", new {pid = item.產品編號 }, new {@class = "btn btn-success"})
                        }
                       <p></p>
                        @if (Permission.Contains("D"))
                        {
                            @Html.ActionLink("刪除", "Delete", new { pid = item.產品編號 }, new {@class="btn btn-danger",onclick="return confirm('確定刪除?');"})

                        }
                    </td>
                </tr>
            }

        </table>

    </div>
</div>

```
## ProductController.cs 的 Create新增頁面
有多做防呆的機制，多`ViewBag.IsProduct = true;`來判斷有沒有重複新增。
```
[Authorize]
public ActionResult Create()
{
    string uid = User.Identity.Name;
    string Permission = db.會員.Where(m => m.帳號 == uid).FirstOrDefault().權限;
    if (!Permission.Contains("C"))
    {
        return RedirectToAction("Index", "PermissionErrorMsg", new { msg = "身分不足" });

    }
    ViewBag.Category = db.產品類別.ToList();
    return View();
}
[Authorize]
[HttpPost]
public ActionResult Create(string 產品編號, string 品名, int 單價, HttpPostedFileBase fImg,int 類別編號)
{
    var tempProduct = db.產品資料.Where(m => m.產品編號 == 產品編號).FirstOrDefault();
    if (tempProduct != null)
    {
        ViewBag.IsProduct = true;
        ViewBag.Category = db.產品類別.ToList();
        return View();
    }


    string fileName = "question.png";
    if (fImg != null)
    {
        if (fImg.ContentLength > 0)
        {
            fileName = Guid.NewGuid().ToString() + ".jpg";
            var path = string.Format("{0}/{1}", Server.MapPath("~/Images"), fileName);
            fImg.SaveAs(path);
        }
    }

    string editdate = DateTime.Now.ToString("yyyyMMddHHmmss");
    產品資料 product = new 產品資料();
    product.產品編號 = 產品編號;
    product.品名 = 品名;
    product.單價 = 單價;
    product.圖示 = fileName;
    product.類別編號 = 類別編號;
    product.編輯者 = User.Identity.Name;
    product.建立日 = editdate;
    product.修改日 = editdate;
    db.產品資料.Add(product);
    db.SaveChanges();
    return RedirectToAction("Index");
}
```
## Create.cshtml
```
@using Day09.Models

@model Day09.Models.產品資料

@{
    ViewBag.Title = "產品新增";
    IEnumerable<產品類別> category = ViewBag.Category;
}

<h2>產品新增</h2>

<form action="@Url.Action("Create")" method="post" enctype="multipart/form-data">

    <div class="form-horizontal">
        <hr />
        @Html.ValidationSummary(true, "", new { @class = "text-danger" })
        <div class="form-group">
            @Html.LabelFor(model => model.產品編號, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(model => model.產品編號, new { htmlAttributes = new { @class = "form-control" } })
                @Html.ValidationMessageFor(model => model.產品編號, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(model => model.品名, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(model => model.品名, new { htmlAttributes = new { @class = "form-control" } })
                @Html.ValidationMessageFor(model => model.品名, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(model => model.單價, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(model => model.單價, new { htmlAttributes = new { @class = "form-control" } })
                @Html.ValidationMessageFor(model => model.單價, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(model => model.圖示, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                <input type="file" id="fImg" name="fImg" class="form-control" />
            </div>
        </div>

        <div class="form-group">
            <label class="control-label col-md-2">
                類別名稱
            </label>
            <div class="col-md-10">
                <select id="類別編號" name="類別編號" class="form-control">
                    @foreach (var item in category)
                    {
                        <option value="@item.類別編號">@item.類別名稱</option>
                    }
                </select>
            </div>
        </div>


        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <input type="submit" value="新增" class="btn btn-default" />
            </div>
        </div>
    </div>
</form>

@if (ViewBag.IsProduct != null)
{
    <div class="alert alert-danger">
        <strong>產品編輯不能重複！</strong> 誰重新建立！！
    </div>
}

```
## ProductController.cs 的 Delete 刪除功能
這邊使用`System.IO.File.Delete` 來刪除圖片檔案。

```
[Authorize]
public ActionResult Delete(string pid)
{
    string uid = User.Identity.Name;
    string Permission = db.會員.Where(m => m.帳號 == uid).FirstOrDefault().權限;
    if (!Permission.Contains("C"))
    {
        return RedirectToAction("Index", "PermissionErrorMsg", new { msg = "身分不足" });

    }
    var product = db.產品資料.Where(m => m.產品編號 == pid).FirstOrDefault();
    var filename = product.圖示;
    if(filename != "question.png")
    {
        System.IO.File.Delete(string.Format("{0}/{1}", Server.MapPath("~/Images"), filename) );
    }

    db.產品資料.Remove(product);
    db.SaveChanges();
    return RedirectToAction("Index");
}
```

## ProductController.cs 中的 Edit編輯頁
* `Guid.NewGuid()` 產生亂數字串
```
[Authorize]
public ActionResult Edit(string pid)
{
    string uid = User.Identity.Name;
    string Permission = db.會員.Where(m => m.帳號 == uid).FirstOrDefault().權限;
    if (!Permission.Contains("U"))
    {
        return RedirectToAction("Index", "PermissionErrorMsg", new { msg = "身分不足" });

    }
    var product = db.產品資料.Where(m => m.產品編號 == pid).FirstOrDefault();
    ViewBag.Category = db.產品類別.ToList();
    return View(product);
}
[Authorize]
[HttpPost]
public ActionResult Edit(string 產品編號, string 品名, int 單價, HttpPostedFileBase fImg,string 圖示, int 類別編號)
{
    string fileName = "";
    var product = db.產品資料.Where(m => m.產品編號 == 產品編號).FirstOrDefault();

    if (fImg != null)
    {
        if (fImg.ContentLength > 0)
        {
            fileName = Guid.NewGuid().ToString() + ".jpg";
            var path = string.Format("{0}/{1}", Server.MapPath("~/Images"), fileName);
            fImg.SaveAs(path);
            System.IO.File.Delete(string.Format("{0}/{1}", Server.MapPath("~/Images"), product.圖示));
        }
    }
    else
    {
        fileName = 圖示;
    }
    string editdate = DateTime.Now.ToString("yyyyMMddHHmmss");
    product.品名 = 品名;
    product.單價 = 單價;
    product.圖示 = fileName;
    product.類別編號 = 類別編號;
    product.編輯者 = User.Identity.Name;
    product.修改日 = editdate;
    db.SaveChanges();
    return RedirectToAction("Index");
}
```
## Edit.cshtml
```
@using Day09.Models

@model Day09.Models.產品資料

@{
    ViewBag.Title = "產品編輯";
    IEnumerable<產品類別> category = ViewBag.Category;
}

<h2>產品編輯</h2>

<form action="@Url.Action("Edit")" method="post" enctype="multipart/form-data">

    <div class="form-horizontal">
        <hr />
        @Html.ValidationSummary(true, "", new { @class = "text-danger" })
        <div class="form-group">
            @Html.LabelFor(model => model.產品編號, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(model => model.產品編號, new { htmlAttributes = new { @class = "form-control" , @readonly="readonly"} })
                @Html.ValidationMessageFor(model => model.產品編號, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(model => model.品名, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(model => model.品名, new { htmlAttributes = new { @class = "form-control" } })
                @Html.ValidationMessageFor(model => model.品名, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(model => model.單價, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(model => model.單價, new { htmlAttributes = new { @class = "form-control" } })
                @Html.ValidationMessageFor(model => model.單價, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(model => model.圖示, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                <input type="file" id="fImg" name="fImg" class="form-control" />
                @Html.HiddenFor(model=>model.圖示)
                @if (Model.圖示 == "question.png")
                {
                    @:無產品圖
                }
                else
                { 
                    <img src="~/Images/@Model.圖示" style="width:150px;" />
                }
            </div>
        </div>

        <div class="form-group">
            <label class="control-label col-md-2">
                類別名稱
            </label>
            <div class="col-md-10">
                <select id="類別編號" name="類別編號" class="form-control">
                    @foreach (var item in category)
                    {
                        <option value="@item.類別編號"  @(item.類別編號==Model.類別編號 ? "selected" : "")   >
                               @item.類別名稱
                        </option>
                    }
                </select>
            </div>
        </div>


        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <input type="submit" value="編輯" class="btn btn-default" />
            </div>
        </div>
    </div>
</form>



```

[Day10](https://hackmd.io/@JpF6T4ZnQ4CVWYEPJpeFYw/HkD2ZESN5)
