# ASP .NET Web應用程式上手(.NET Framework) Day 15 (爬蟲串個API)

###### tags: `ASP .NET Web` `C#` `.NET Framework`


## 前言
目前我們撰寫的地方是Page_Load，也就是網站讀入時就會去執行，如果這網站是線上版，很多人去使用這網站的話會頻繁的做出請求，提供API的網站就可能會覺得是DDOS而黑單請求方的IP，導致無法爬蟲撈不到資料，因此會根據資料的類型去適當地向API方做出請求，例如本例子是貨幣的每月匯率，是不需要非常頻繁的去向API網站請求資料，所以可以寫一個每一個月請求一次的功能出來，把資料存到資料庫，之後顯示的數據都從資料庫拿就好。
> 本篇先直接請求顯示結果，未來在介紹如何存進資料庫。

## 準備工作

* 範例API資料來源 : [國際主要國家貨幣每月匯率概況](https://data.gov.tw/dataset/31897)
* Chrome擴充功能 : [JSONView](https://chrome.google.com/webstore/detail/jsonview/gmegofmjomhknnokphhckolhcffdaihd?hl=zh-TW) ，方便網頁看json
* 安裝NuGet套件 : HTML Agility Pack

各位可以先看範例的資料，他是需要載下來的檔案，其實可以用爬蟲的方式將資料show出來。

![](https://i.imgur.com/JbcUtQl.png)

## 程式碼
### C#
```
public String catch_json_url_with_binding()
        {
            string Product_Name = "";
            string reply = "null";
            HtmlWeb webClient = new HtmlWeb(); // 建立呼叫網站Client端，模擬請求
            HtmlDocument doc = webClient.Load("https://data.gov.tw/dataset/31897"); // 載入網站
            HtmlNodeCollection item = doc.DocumentNode.SelectNodes($"/ html / head / script[1] / text()"); // 根據要的那段html右鍵複製XPath
            if (item != null)
            {
                Product_Name = item[0].InnerText.ToString();
                string json_url_left = "\"encodingFormat\":\"JSON\",\"contentUrl\":\"";
                string json_url_right = "},{\"@type\":\"DataDownload\",\"encodingFormat\":\"WEBSERVICES\",";
                int loc_left = Product_Name.IndexOf(json_url_left);
                int loc_right = Product_Name.IndexOf(json_url_right);
                string cut_substr = "\"},{\"@type\":\"DataDownload\",\"encodingFor";
                string json_url = Product_Name.Substring(loc_left + json_url_left.Length, loc_right - loc_left - cut_substr.Length); //切出json的URL

                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                reply = client.DownloadString(json_url);
            }
            Label1.Text = reply;
            return reply;
        }
```
:::info
應該還有更好的寫法，因為他的格式是json，可以用Newtonsoft.Json將字串轉成Json，下面程式碼是無腦的字串切割，切出我們要的Json網址。
:::

* 執行結果
![](https://i.imgur.com/TmSFNrK.png)

#### 說明
1. HtmlWeb、HtmlDocument、HtmlNodeCollection : [官方介紹](https://html-agility-pack.net/parser)
2. loc_left / loc_right  : 找 json_url_left / json_url_right的字串位置
3. json_url : Substring (位置，切多長)，取網址

### Python
#### .cs : python執行緒
```
public String catch_json_url_with_python()
        {
            var path = Server.MapPath("~/assets/python/");
            ProcessStartInfo start = new ProcessStartInfo();
            start.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory; //獲取和設置當前目錄的完全路徑
            start.FileName = $"C:/Users/{Environment.UserName}/AppData/Local/Programs/Python/Python37/python.exe";
            start.Arguments = path + "catch_json_url.py";
            start.UseShellExecute = false; //Shell 處理序
            start.RedirectStandardOutput = true; //將Console上的訊息輸出成文字檔
            start.RedirectStandardError = true; //重定向標準錯誤輸出
            start.CreateNoWindow = true; //不顯示程序窗口
            start.ErrorDialog = true; //獲取或設置一個值，該值指示在進程無法啟動時是否向用戶顯示錯誤對話框。

            string text = "";
            using (Process process = Process.Start(start))
            {
                process.BeginErrorReadLine();
                while (!process.StandardOutput.EndOfStream) //讀最後一行
                {
                    text = process.StandardOutput.ReadLine();

                }
                process.WaitForExit();
            }
            Label1.Text = text;
            return text;
        }
```
#### .\assets\python\catch_json_url.py

```
#使用requests 、lxml 套件
import requests
from lxml import  html
url = "https://data.gov.tw/dataset/31897"
res=requests.get(url)
#用lxml的html函式來處理內容格式
byte_data = res.content
source_code = html.fromstring(byte_data)
result = source_code.xpath("/html/head/script[1]/text()")[0] #F12->右鍵那段html->Copy->Copy Xpath

#找URL
out = result.find("JSON")
l_len = out+len('JSON","contentUrl":"')
r_len  = result[l_len:].find('"')+l_len
json_url = result[l_len:r_len]

htmls = requests.get(json_url,verify=False)
print(htmls.text)
```
![](https://i.imgur.com/dWdFdnw.png)
#### 說明
1. C#執行python.exe，python要安裝requests套件
2. 新增資料夾

## 結論

可以看到印出來的結果是一個json格式，之後帶大家用javaScript的方式整理資料，用圖形視覺化的方式顯示結果。

