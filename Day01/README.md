# ASP .NET MVC 上手 Day01_簡單專案
###### tags: `asp.netMVC` `C#`
### 新增專案
* 找 ASP.NET Web 應用程式(.NET Framework)
![](https://i.imgur.com/AGKedSx.png)
* 去專案名稱，以及路徑，建立
* 選空白，右邊把 MVC的選項打勾，建立
### 簡單的傳值應用
:::    success
MVC有三個主要的資料夾，分別是:
1. Controllers : 控制器，負責使用者的介面互動，運行程式邏輯
2. Models : 存取應用程式中的來源，像是資料庫
3. Views : 呈現使用者介面資訊
[詳細介紹](https://zh.wikipedia.org/wiki/MVC)
:::
* 在 Controllers 右鍵 -> 加入 -> 控制器 -> MVC x 控制器-空白 ->輸入控制器名稱 (範例取 HomeController) -> 加入
:::warning
假如沒出現控制器的選項，到工具->取得工具與功能，工作負載中安裝ASP.NET與網頁程式開發，在個別元件中打MVC安裝ASP.NET MVC x。
裝完重開VB就會出現控制器了。
:::
* HomeController.cs 改成以下，執行看看
```
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hackMD_MVCdemo.Controllers
{
    
    public class HomeController : Controller
    {
        // GET: Home
        public String Index()
        {
            return "hello";
        }
    }
}
```
:::success
http://ip/控制器(Home)/動作方法(Index)
HomeController後面的Controller是固定寫法，前面Home當網址的控制器，Index當網址的動作方法。

這規則定義在專案裡的App_Start資料夾的RouteConfig.cs中，[詳細介紹](https://ithelp.ithome.com.tw/articles/10203560)
:::

* 用View檢視，HomeController.cs 改成以下
```
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hackMD_MVCdemo.Controllers
{
    
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.Isay = "hello".ToString();
            return View();
        }
    }
}
```
* 點 Index，右鍵->新增檢視->MVC x -> 把使用版面配置的勾取消 (之後會再用)->加入，可以看到Views/Home有新增。Index.cshtml改成以下，執行看看，觀察網址。
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
    <div> 
        I say : @ViewBag.Isay
    </div>
</body>
</html>
```
:::info
* @ 是在html中下C#的指令 ， [詳細介紹](https://ithelp.ithome.com.tw/articles/10240365)
* Layout這東西你可以按住Ctrl點他進去看他背後的宣告方法，它在System.Web.WebPages底下，把它拉開你會發現有註解說明。
* ViewBag同理，它在System.Web.Mvc底下，看它註解寫甚麼，看看其他的功能與差別(ex:Model、Url、ViewBag、ViewContext、ViewData)。
* View同理，它在System.Web.Mvc底下...
:::
### 建立Model
* 在Models的資料夾右鍵 -> 加入 -> 新增項目 -> 程式碼(左邊) -> 選類別，取名(範例取 CMember.cs)->新增，CMember.cs改以下
```
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace hackMD_MVCdemo.Models
{
    public class CMember
    {
        public int Id { get; set; }
        public String Name { get; set; }
    }
}
```
:::success
物件導向類別封裝 : [詳細介紹](https://dotblogs.azurewebsites.net/lucy_NoteForCoding/2020/06/17/Object_Orientation_Encapsulation)
:::
* 回 Controllers 的 HomeController.cs，新增以下
```
public ActionResult ShowMember()
{
    //陣列物件
    CMember[] emps = new CMember[]
    {
        //塞資料
        new CMember(){Id=1,Name="yoyo"},
        new CMember(){Id=2,Name="miku"}

    };
    return View(emps.ToList());
}
```
* 跟之前一樣，在ShowMember右鍵->新增檢視...->加入，到Views->Home將ShowMember.cshtml改以下，執行看看，在網頁按F12看html的變化。
:::danger
請別複製貼上，用手打，因為你在打的過程中，可以知道有沒有引入到，假如你在打的過程都沒顯示快速拼好的參數，代表沒讀到，要再回頭看原因
:::
```
@model IList<hackMD_MVCdemo.Models.CMember>
@{Layout = null;}
<!DOCTYPE html>
<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>ShowMember</title>
</head>
<body>
    @foreach (var item in Model)
    {
        <div>
            編號:@item.Id <br />
            姓名:@item.Name
        </div>
    }
</body>
</html>
```
:::success
@model : 強型別，[詳細介紹](https://docs.microsoft.com/zh-tw/aspnet/core/tutorials/first-mvc-app/adding-model?view=aspnetcore-6.0&tabs=visual-studio#strongly-typed-models-and-the-model-directive)
@foreach : 重複寫{}內的東西
Model : 等於IList<hackMD_MVCdemo.Models.CMember>拿Controller 傳來的資料
:::

最後這個你腦袋要有個流程圖，之後加入資料庫或在前端傳資料到後端會比較不會亂掉。

END
---

## [Day02](https://hackmd.io/k2VtcDnzRDqrNlOoRK5fUA)















