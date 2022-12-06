# ASP .NET MVC 上手 Day06(Model模型應用_LINQ)

###### tags: `asp.netMVC` `C#`

## LINQ技術簡介
* 不同的資料來源如陣列、集合、XML或是資料庫要進行查詢或排序，需要使用不同的查詢技術或資料結構。
* 例如在陣列中搜尋資料，透過循序搜尋法、二分搜尋法、二元樹搜尋法...或其他資料結構。
* 查詢 DataSet 內 DataTable 的特定一筆資料列 (DataRow)，可以使用 DataTable.Rows 集合的 Find 方法找出特定的資料列 (DataRow)。
* 查詢 XML 文件，則可以使用XQuery ; 以SQL Server資料庫做資料來源時，則可透過SQL語法進行查詢。
* 當開發人員對上述不同的資料來源時，需要學習與運用不同的資料查詢技術，才能取得特定所需的資料且加以運用。
* 在 2007 年隨著 .NET Framework 3.5發布嘞LINQ (全名為Language-Integrated Query) 資料查詢技術，可讓開發人員使用一致性的語法來查詢不同的資料來源。
* 例如查詢陣列、集合、DataSet物件、XML、SQL Server資料庫...等，讓開發人員不需要針對不同的資料來源學習不同的資料查詢技術。

### 1. LINQ to Objects
又稱LINQ to Collection，可以查詢實作 IEumerable 或 IEnumerable<T> 介面的集合物件，如查詢陣列、List、集合、檔案...等物件。
### 2. LINQ to XML
使用XML查詢技術的 API，透過 LINQ 查詢運算式可以不需要再額外學習 XPath 或 XQuery，就可以查詢或排序 XML 文件。
### 3. LINQ to DataSet
透過 LINQ 查詢運算式，可以對記憶體內的 DataSet 或 DataTable 進行查詢。
### 4. LINQ to SQL
可以對實作 `IQueryable<T>` 介面的物件做查詢，也可以直接對 SQL Server 和 SQL Server Express 資料庫進行查詢與編輯。由 EntityFramework 與 LINQ to Entity 所取代，且 ASP.NET MVC 最常使用 Entity Framework 來當作 Model 技術，此處 Entity Framework 與 LINQ to Entity 為主來做介紹。

語法基本上很像SQL語法。之前Day02就有使用到
```
//LINQ擴充方法
var emp = db.tMember.Where(m => m.Id == Id).FirstOrDefault();
```
:::success
LINQ基本語法 : [介紹連結](https://www.cnblogs.com/dotnet261010/p/8279256.html)
:::
## LINQ簡單帳密登入實作
### ex1 : 簡單的LINQ排序
* 新增一個HomeCotroller.cs
```
public string ShowArray()
{
    int[] score = new int[] { 78, 99, 85, 36, 56 };
    string show = "";
    //LINQ擴充方法
    var result = score.OrderByDescending(m => m);
    //LINQ查詢運算式寫法
    //var result = from m in score
    //              orderby m descending
    //              select m;
    show = "遞減排序: ";
    foreach(var m in result)
    {
        show += m + ",";
    }
    show += "<br />";
    show += "總和: " + result.Sum();
    show += "平均: " + result.Average();
    return show;
}
```
url : https://localhost:44363/Home/ShowArray    

### ex2 : 超不安全url加參數的帳密登入
* Models中加入 Member.cs
```
public class Member
{
    public string UId { get; set; }
    public string Pwd { get; set; }
    public string Name { get; set; }
}
```

* 回 HomeController.cs
記得引用 using Day06.Models
```
public string LoginMember(string uid,string pwd)
{
    Member[] members = new Member[]
    {
        new Member{UId="tom",Pwd="123",Name="湯姆"},
        new Member{UId="jasper",Pwd="456",Name="賈斯柏"},
        new Member{UId="mary",Pwd="789",Name="馬力"}
    };
    //LINQ擴充方法
    var result = members.Where(m => m.UId == uid && m.Pwd == pwd).FirstOrDefault();
    //LINQ查詢方法
    //var result = (from m in members
    //              where m.UId == uid && m.Pwd == pwd
    //              select m).FirstOrDefault();
    string show = "";
    if (result != null)
    {
        show = result.Name + "歡迎";
    }
    else
    {
        show = "登入失敗";
    }
    return show;
}
```
成功的登入url : https://localhost:44363/Home/LoginMember?uid=tom&pwd=123
    
---

## Entity Framework
我們Day02一開始就是先用ADO.NET實體資料模型來建立資料庫
* Entity Framework 是 [ADO.NET](https://docs.microsoft.com/zh-tw/dotnet/framework/data/adonet/ado-net-overview) 中的開發框架，也是在 .NET Framework中的一套程式庫。
* 有了 Entity Framework 之後，開發人員在操作資料時可以比傳統 ADO.NET 應用程式更少的程式碼來建立及維護資料庫資料。
* 下圖所示為 Entity Framework 的架構圖，存取資料來源的底層一樣是使用 ADO.NET Data Provider ( Connection、Command、DataAdapter...)，開發人員只要以物件導向程式設計配合 LINQ 查詢就可以存取資料來源。
  
![](https://i.imgur.com/hQrj15M.png)

* 簡單回顧
新增 : 在 tMember 資料表內新增一筆會員紀錄，有Name、Phone、Age
```
public ActionResult ShowCreat()
{
    return View();
}
[HttpPost]
public ActionResult Create(string Name,string Phone,int Age)
{
    dbMemberEntities db = new dbMemberEntities();
    tMember dbmember = new tMember();
    dbmember.Name = "yoyo";
    dbmember.Phone = "0912345678";
    dbmember.Age = 30;
    db.tMember.Add(dbmember); //新增
    db.SaveChanges(); //執行新增後異動資料庫
    return RedirectToAction("ShowdbMember"); //傳到ShowdbMember動作方法
}
```
修改 : 年齡改成 25 歲
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
    emp.Id = 1;
    emp.Name = "yoyo";
    emp.Phone = "0912345678";
    emp.Age = 25;
    db.SaveChanges();
    return RedirectToAction("ShowdbMember");
}
```
* 刪除
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

---

### 實作  Entity Framework 查詢資料表
在 App_Data 中新增已經準備好的 .mdf檔 ，接著我們來設定實體化的設定。
在 Models -> 右鍵加入 -> 新增項目 -> 資料選 ADO.NET 實體資料模型 -> 下一步*3 -> 資料表選等下會用的table(客戶、員工、產品資料、產品類別)，建完畫面就會出現 .edmx 的介面，你會發現 Models 自動建好每個 table 的 .cs 檔，裏頭都打好了 get，set 存取值。
![](https://i.imgur.com/CVpax8S.png)
:::success
建議每次有新增資料庫後記得右鍵 -> 重建
    
![](https://i.imgur.com/Z9EDzzU.png)
:::

* 寫個印資料的功能，進 HomeController.cs 新增
```
public string ShowEmployee()
{
    TestDataSetEntities db = new TestDataSetEntities();
    //LINQ擴充方法寫法
    var result = db.員工; // 取得 table 的內容
    //var result = from m in db.員工
    //              select m;
    string show = "";
    foreach(var m in result)
    {
        show += "編號 : " + m.員工編號 + "<br/>";
        show += "姓名 : " + m.姓名 + m.稱呼 + "<br/>";
        show += "職稱 : " + m.職稱 + "<hr>";
    }
    return show;
}
```
url : https://localhost:44363/Home/ShowEmployee
    
* 根據地址搜尋
```
public string ShowCustomerByAddress(string keyword)
{
    TestDataSetEntities db = new TestDataSetEntities();
    //LINQ擴充方法寫法
    var result = db.客戶.Where(m=>m.地址.Contains(keyword)); //Contains : 有找到True；反之False
    //LINQ查詢運算式寫法
    //var result = from m in db.客戶
    //              where m.地址.Contains(keyword)
    //              select m;
    string show = "";
    if (result == null) return "查無地址";
    foreach(var m in result)
    { 
        show += "公司名稱 : " + m.公司名稱 + "<br/>";
        show += "連絡人 連絡人職稱  : " + m.連絡人 + m.連絡人職稱 + "<br/>";
        show += "地址 : " + m.地址 + "<hr>";
    }
    return show;
}
```
    
url : https://localhost:44363/Home/ShowCustomerByAddress?keyword=台北市忠孝東路四段32號 

![](https://i.imgur.com/vPGs59c.png)
  
    
url : https://localhost:44363/Home/ShowCustomerByAddress?keyword=忠孝東路   
![](https://i.imgur.com/6WYRVKL.png)



---

* 簡單統計
```
public string ShowProductInfo()
{
    TestDataSetEntities db = new TestDataSetEntities();
    //LINQ 擴充方法寫法
    var reuslt = db.產品資料;
    //LINQ查詢運算式寫法
    //var result = from m in db.產品資料
    //              select m;
    string show = "";
    show += "單價平均: " + reuslt.Average(m => m.單價) + "<br />";
    show += "單價總和: " + reuslt.Sum(m => m.單價) + "<br />";
    show += "紀錄筆數: " + reuslt.Count() + "<br />";
    show += "單價最高: " + reuslt.Max(m => m.單價) + "<br />";
    show += "單價最低: " + reuslt.Min(m => m.單價) + "<br />";
    return show;
}
```
url : https://localhost:44363/Home/ShowProductInfo
![](https://i.imgur.com/sFld7Oq.png)

```
public string ShowProduct()
{
    TestDataSetEntities db = new TestDataSetEntities();
    //LINQ 擴充方法寫法
    var result = db.產品資料
        .Where(m=>m.單價>30)
        .OrderBy(m=>m.單價)
        .ThenByDescending(m=>m.庫存量);
    //LINQ查詢運算式寫法
    //var result = from m in db.產品資料
    //              where m.單價 > 30
    //              orderby m.單價 ascending, m.庫存量 descending
    //              select m;
    string show = "";
    foreach(var m in result)
    {
        show += "產品: " + m.產品 + "<br />";
        show += "單價: " + m.單價 + "<br />";
        show += "庫存: " + m.庫存量 + "<br />";
    }
    return show;
}
```
    
    
url : https://localhost:44363/Home/ShowProduct 
![](https://i.imgur.com/zsE1u7b.png)

* 進階搜尋 : join on into
```
public string ShowProductGroup()
{
    TestDataSetEntities db = new TestDataSetEntities();

    //LINQ查詢運算式寫法
    var result = from category in db.產品類別
                 join product in db.產品類別
                 on category.類別編號 equals product.類別編號
                 into num
                 select new
                 {
                     類別名稱 = category.類別名稱, //定義新名稱
                     產品數量 = num.Count()
                 };
    string show = "";
    foreach (var m in result)
    {
        show += string.Format("{0}類別共{1}個產品<p>", m.類別名稱, m.產品數量);
    }
    return show;
}            
``` 
url : https://localhost:44363/Home/ShowProductGroup
![](https://i.imgur.com/TZ8qdPf.png)

# [Day07](https://hackmd.io/jDTLmhKkT1eyLEWdxurgAQ)