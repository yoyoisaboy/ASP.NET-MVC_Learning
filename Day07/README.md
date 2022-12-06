# ASP .NET MVC 上手 Day07(Model模型應用_CRUD)

###### tags: `asp.netMVC` `C#`



# 模型驗證
* 好處是幫助使用者在輸入資料時，只能輸入符合準則的資料內容，以確保資料的正確性。
* ASP.NET MVC 提供了 Model Validate(模型驗證) 的方式讓開發人員進行資料驗證，Model Validate 是透過 .NET Framework 的DataAnnotation 進行驗證。
* 使用時必須引用【using System.ComponentModel;】 和 【using System.ComponentModel.DataAnnotations;】 命名空間。
* Model Validate 讓開發人員以附加屬性的方式，幫助模型加入驗證的規則，並提供用戶端和伺服器驗證檢查，可進行驗證的有**必填資料**、**輸入字串長度**、**驗證資料型別**、**電子郵件驗證**...等等

## 類別的模型中加入驗證屬性
```
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

public partial class tStudent {
    [DisplayName("學號")]
    [Required(ErrorMessage = "學號不可空白")]
    [StringLength(7,ErrorMessage = "學號必須是2-7個字元",MinimumLength = 2)]
    public string fStuId {get;set;}
    
    [DisplayName("姓名")]
    public string fName{get;set;}
}
```
## 動作方法加入判斷驗證的程式碼
```
dbStudentEntities db = new dbStudentEntities();
[HttpPost]
public ActionResult Create(tStudent stu){
    if(ModelState.IsValid){ //通過驗證ModelState.IsValid會傳回true，否則傳回false 
    db.tStudent.Add(stu);
    db.SaveChanges();
    return RedirectToAction("Index");
    }
    return View(stu);
}
```
## View使用 Html Helper 設計表單程式碼
```
@model Day07.Models.tStudent
...
    @using (Html.BeginForm() ){
        ...
        <div>
            @Html.LabelFor(model => model.fStuId)
            <div>
                @Html.EditorFor(model => model.fStuId)
                @Html.ValidationMessageFor(model => model.fStuId)
            </div>
        </div>
    }
```
:::success
常用的驗證屬性 : [詳細連結](https://docs.microsoft.com/zh-tw/aspnet/core/mvc/models/validation?view=aspnetcore-6.0)
:::

---
# 模型驗證簡例
## DisplayName
1. 可設定欄位的顯示名稱。
2. 使用此屬性必須引用 System.ComponentModel 命名空間。
3. 如下寫法，設定 fStuId 屬性在View頁面上顯示為 "學號"。
```
[DisplayName("學號")]
public string fStuId {get;set;}
```
## Required
1. 可設定必填欄位。
2. 使用此屬性必須引用 System.ComponentModel.DataAnnotations 命名空間。
3. 如下寫法，設定 fName 屬性在View頁面中為必填欄位，當驗證失敗時會顯示"學號不可空白"的訊息。
```
[Required("學號不可空白")]
public string fName {get;set;}
```
## Range
1. 可設定欄位內容的數值範圍。
2. 使用此屬性必須引用 System.ComponentModel.DataAnnotations 命名空間。
3. 如下寫法，驗證fSalary屬性在View頁面欄位的內容必須22000~50000之間，當驗證失敗時會顯示 "薪資必須介於22000~50000"的訊息。
```
[Range(22000,50000,ErrorMessage="薪資必須介於22000~50000")]
public string fSalary {get;set;}
```
## Compare
1. 可驗證另一個欄位是否與目前的欄位內容相同。
2. 使用此屬性必須引用 System.Web.Mvc 命名空間。
3. 如下寫法，驗證fPwd 和 fRePwd 兩個欄位是否相同，當驗證失敗會顯示 "兩組密碼必須相同" 的訊息。
```
[Compare("fPwd","fRePwd"),ErrorMessage="兩組密碼必須相同"]
public string fPwd {get;set;}
public string fRePwd{get;set;}
```
## EmailAddress
1. 可驗證欄位內容是否為電子信箱格式。
2. 使用此屬性必須引用 System.ComponentModel.DataAnnotations 命名空間。
3. 如下寫法，驗證fEmail 屬性在View頁面欄位的內容必須是電子信箱格式，當驗證失敗時會顯示 "Email格式有誤" 的訊息。
```
[EmailAddress(ErrorMessage="Email格式有誤")]
public string fEmail {get;set;}
```

## StringLength
1. 可設定欄位所允許輸入字串的最大長度。
2. 使用此屬性必須引用 System.ComponentModel.DataAnnotations 命名空間。
3. 如下寫法，驗證fStuId屬性在View頁面欄位的內容必須是2-7個字元，當驗證失敗時會顯示 "學號必須是2-7個字元" 的訊息。
```
[StringLength(7,ErrorMessage="學號必須是2-7個字元",MinimumLength=2)]
public string fStuId {get;set;}
```

## Url
1. 可驗證欄位內容是否為網址格式。
3. 使用此屬性必須引用 System.ComponentModel.DataAnnotations 命名空間。
4. 如下寫法，驗證fUrl屬性在View頁面欄位的內容必須是網址格式，當驗證失敗時會顯示 "資料內容必須為網址格式"的訊息。
```
[Url(ErrorMessage="資料內容必須為網址格式")]
public string fUrl {get;set;}
```
---

# 實作
大概就是把Day02的東西再做一遍

## 新增資料庫
* 首先在APP_Data右鍵加入 -> 新增項目 -> 選擇資料的 SQL Server 資料庫，取完名稱後新增。

* 接著來加入新的資料表
![](https://i.imgur.com/sdKyLFL.png)

* 新增以下資料，記得案更新
![](https://i.imgur.com/H1zHNqd.png)

更新完後再重新整理，就可以看到資料表中有多東西了。

* 建立ADO.NET實體資料模型
換回方案總管 -> 右鍵 Models -> 加入 -> 新增項目 -> ADO.NET實體資料模型，並取好名稱 -> 新增 -> 基本上都下一步，其中一頁資料表要打勾

![](https://i.imgur.com/gOu6Dgq.png)

* 習慣設定完資料庫，在專案上右鍵 -> 重建，完成後如下
![](https://i.imgur.com/ZAVgRpz.png)
:::success
如果想換欄位順序，可以在 T-SQL中做變更順序
:::
:::danger
* 如果你想要刪掉模型，除了刪掉Models的資料庫檔案之外，Web.config裡的
```
<connectionStrings>
    <add name="dbEmpEntities" connectionString="metadata=res://*/Models.dbEmpModel.csdl|res://*/Models.dbEmpModel.ssdl|res://*/Models.dbEmpModel.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=(LocalDB)\MSSQLLocalDB;attachdbfilename=|DataDirectory|\dbEmp.mdf;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;" providerName="System.Data.EntityClient" />
  </connectionStrings>
```
也記得刪掉
* 之後關掉重開Visual Studio就可以了
![](https://i.imgur.com/06tERDd.png)

:::

##  新增資料庫 Table 欄位
通常先把資料庫設計完再建立會比較好

* 先打開資料表定義
![](https://i.imgur.com/EH9uHfu.png)
* 直接做變動，變動完記得案更新和重建
![](https://i.imgur.com/tVbTH4K.png)

:::warning
* 發現實體資料庫模型的內容沒更新，請右鍵找"從資料庫更新模型"就可以咯
![](https://i.imgur.com/ApXtOdh.png)
* 發現 tEmployee.cs 的內容沒更新到，1. 手動改tEmployee.cs 2.ADO.NET介面點欄位，改屬性。
![](https://i.imgur.com/oCApA87.png)
* 改完一定要重建專案喔!!! 並檢查tEmployee.cs 和 ADO.NET的內容一致。
:::

## 檢視頁面功能

* 在Controller右鍵 -> 加入 -> 控制器 
改成以下
```
public class HomeController : Controller
{
    // GET: Home
    public ActionResult Index()
    {
        dbEmpEntities db = new dbEmpEntities();

        return View(db.tEmployee.ToList());
    }
}
```



* 右鍵Index新增檢視，這次用範本 List
![](https://i.imgur.com/taY7jWL.png)

完成後你可以看到，很多東西都打好了~可以先執行看看
![](https://i.imgur.com/QFFVuBn.png)



---

## Create 新增資料

* 在 Index 新增Create方法，跟之前的寫法一樣。
```
public ActionResult Create()
{

    return View();
}

[HttpPost]
public ActionResult Create(tEmployee emp)
{
    if (ModelState.IsValid)
    {
        db.tEmployee.Add(emp);
        db.SaveChanges();
        return RedirectToAction("Index");
    }
    return View();
}
```
:::success
ModelState.IsValid : [運用範例](https://blog.miniasp.com/post/2016/03/14/ASPNET-MVC-Developer-Note-Part-28-Understanding-ModelState)
:::

* 加入檢視，使用範本加入，接著執行看看，輸入資料按下新增。
![](https://i.imgur.com/mL5ZNe7.png)

![](https://i.imgur.com/OC34jsZ.png)

新增完後，接著就會跳轉到 Index 檢視頁面，顯示新增的內容，你可以執行第二次，你會發現第一次的新增的內容會顯保留，表示資料已經被輸入到資料庫裡面了。

:::info
你會發現我名稱取錯了，如果要改回正確名稱tDate，請看最後的內容。
:::

* Create 模型驗證 ComponentModel

Create.cshtml的tDate中新增，type="date"的格式
```
@Html.EditorFor(model => model.fDate, new { htmlAttributes = new { @class = "form-control", type="date" } })
```
模型驗證其實一般都是在前端去做判斷，後端讀到的資料應該都是要正常的資料。不過這裡還是實作一次後端如何做驗證。

首先先到 Models 中接收資料的 tEmployee.cs，在這裡定義資料的類型。

```
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
public partial class tEmployee
{
    [DisplayName("員工名稱")]
    [Required]
    public string fName { get; set; }

    [DisplayName("員工編號")]
    [Range(22000,50000, ErrorMessage = "22000~50000")]
    [Required]
    public Nullable<int> fSalary { get; set; }
    //正則表達式
    [DisplayName("員工手機")]
    [RegularExpression("^09[0-9]{8}$",ErrorMessage ="手機輸入有誤")]
    [Required]
    public string fPhone { get; set; }

    [DisplayName("員工信箱")]
    [EmailAddress]
    [Required]
    public string fEmail { get; set; }

    [DisplayName("員工編號")]
    [StringLength(5, ErrorMessage = "至少5個字元", MinimumLength = 5)]
    [Required]
    public string fEmpId { get; set; }
    public int fId { get; set; }

    [DisplayName("員工日期")]
    [Required]
    public string fDate { get; set; }
}
```
注意要寫在 public 上面。
空值已經在建立Create.cshtml就寫好了，`@Html.ValidationMessageFor`

![](https://i.imgur.com/aM1pHZV.png)

:::success
手機防呆 : [正則表達式用法](https://ithelp.ithome.com.tw/articles/10196283)
:::


---

## Delete 刪除資料

* Index.html 多提示視窗
```
@Html.ActionLink("刪除", "Delete", new { fId=item.fId },new { @class="btn btn-danger",onclick="return confirm('確定要刪除嗎?');"})
```
* HomeController.cs
```
public ActionResult Delete(int fId)
{
    var emp = db.tEmployee.Where(m => m.fId == fId).FirstOrDefault();
    db.tEmployee.Remove(emp);
    db.SaveChanges();
    return RedirectToAction("Index");
}
```



---

## Edit 編輯資料
* Index.html 多提示視窗
```
@Html.ActionLink("編輯", "Edit", new { fId=item.fId },new { @class="btn btn-success"})
```
* HomeController.cs
```
public ActionResult Edit(int fId)
{
    var emp = db.tEmployee.Where(m => m.fId == fId).FirstOrDefault();
    return View(emp);
}
[HttpPost]
public ActionResult Edit(tEmployee emp)
{
    if (ModelState.IsValid)
    {
        db.Entry(emp).State = System.Data.Entity.EntityState.Modified;
        db.SaveChanges();
        return RedirectToAction("Index");
    }
    return View(emp);
}
```
* Edit範本
![](https://i.imgur.com/JtEXRHv.png)
按鍵文字改一下，日期多 `type="date"`



---

## 防止新增重複
* CreateController.cs
```
[HttpPost]
public ActionResult Create(tEmployee emp)
{
    string empId = emp.fEmpId;
    var tempEmp = db.tEmployee.Where(m => m.fEmpId == empId).FirstOrDefault();

    if (tempEmp != null) //若為null代表無重複員工編號
    {
        ViewBag.Show = true;
        return View(emp);
    }

    if (ModelState.IsValid)
    {
        db.tEmployee.Add(emp);
        db.SaveChanges();
        return RedirectToAction("Index");
    }
    return View(emp);
}
```
* Create.cshtml
用bootstrap新增警告視窗，假如ViewBag.Show收到true
```
@if (ViewBag.Show != null)
{
    <div class="alert alert-danger">
        <strong>員工編號重複!</strong>請重新指定員工編號
    </div>
}
```



:::danger
# 題外話
## bug1 : 更改資料庫中的table欄位名稱
因為你是直接手動改的，你改完，有些檔會有之前舊的名稱，全更新新的之外。
資料庫T-SQL要更新，注意 APO.NET 實體資料庫也要更新，下圖的資料表對應要左右都有，不然會在SaveChanges那行出現 `'System.Data.Entity.Infrastructure.DbUpdateException' 類型的例外狀況發生於 EntityFramework.dll` 的錯誤訊息，導致實體模型位不進去資料庫中。
![](https://i.imgur.com/iLRCFYS.png)
:::


[Day08](https://hackmd.io/@JpF6T4ZnQ4CVWYEPJpeFYw/HJnSyW8Mq)