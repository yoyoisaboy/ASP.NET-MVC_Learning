# ASP .NET Web應用程式上手(.NET Framework) Day 14(Hello word~)

###### tags: `ASP .NET Web` `C#` `.NET Framework`


## 前言

之前寫過 ASP .NET MVC框架，有三大元素 Home 、 View 、 Controller，這三個組成網站的前/後端，本篇則會跳脫這框架，想像成把這三樣東西整合在一起變成一個(.aspx)的檔案，這裡面包含前端(.aspx)與後端(.cs)。

## 新增專案
* 選擇ASP .NET Web 應用程式(.NET Framework)
![](https://i.imgur.com/7DUs0C5.jpg)
* 取完專案名稱後，建立空白專案
![](https://i.imgur.com/IrsRXq6.png)
* 加入，新增項目
![](https://i.imgur.com/PkUO150.png)
* Web表單(.aspx) : 通常主頁會取名 Index
![](https://i.imgur.com/nnPKuck.png)
![](https://i.imgur.com/bX1LFv3.png)

主要可以看到第一行，這段將.cs檔串起來
```
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="DataAnalysis_ChartJS.Index" %>
```
## Hello word
* 用label顯示Hello word，在檢視->工具箱->找Label->拖移到 < div > 中
![](https://i.imgur.com/74I9HPc.png)

* 接著在.cs打
```
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DataAnalysis_ChartJS
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write("yoyo, ");
            Label1.Text = "hello word";
        }
    }
}
```
* 執行結果
![](https://i.imgur.com/GOWQjvq.png)


## 結論
1. 可以發現有些工具箱的html標籤會用< asp:... >< /asp:... >包起來，可以直接接收或穿送到後端(.cs)。

2. 建議未來開發專案時，css、js、不同程式語言...等等檔案用普遍開發者會取的資料夾名稱整理
![](https://i.imgur.com/gAUI0Xh.png)

3. 大概有個感覺，.aspx 跟之前ASP .NET MVC不一樣的地方，一個網站都可以用 .aspx 完成，之後會提到用 MasterPageFile 製作母框。


## 重要補充區(務必看一下，對於ASP .NET 的執行有些觀念):
1. [ASP.NET 開發人員不可不知的 IIS (IIS for ASP.NET Developers)](https://www.slideshare.net/regionbbs/aspnet-iis-iis-for-aspnet-developers)
2. [Understanding IIS 7.0 Architecture : Request Processing in Application Pool](https://www.wmlcloud.com/servers/server-applications/microsoft-iis/understanding-iis-7-0-architecture-request-processing-in-application-pool/)
3. 執行流程

![](https://i.imgur.com/TuDaYhX.png)

