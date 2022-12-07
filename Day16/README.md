# ASP .NET Web應用程式上手(.NET Framework) Day 16 (Chart.js圖形視覺化)

###### tags: `ASP .NET Web` `C#` `.NET Framework`

github:[Day16](https://github.com/yoyoisaboy/ASP.NET-MVC_Learning/tree/main/Day16)

## 前言
撈到的資料屬於量化的資料，很多數字比較不易觀察，於是我們可以用圖形視覺化的方式呈現結果。目前網路資源很多，可以使用別人刻好的框架撰寫結果。本專案使用Chart.js當作例子實現。
會用到以下技巧:
* json資料取直重新整理數值
* 下拉是選單選擇幣別
* 觸發事件變換圖形
* json資料重新整理成表格

## 安裝套件
1. jQuery安裝
![](https://i.imgur.com/cAEQqMm.png)
2. jQuery 與 chart.js 引入
3. 放<head>中
```
<%-- chart.js套件 --%>
<script src="https://cdn.jsdelivr.net/npm/chart.js"></script> 
<script src="https://cdn.jsdelivr.net/npm/chart.js@3.5.1/dist/chart.js"></script>
<script src="https://cdn.jsdelivr.net/npm/chartjs-adapter-date-fns/dist/chartjs-adapter-date-fns.bundle.min.js"></script>
<script src="/Scripts/jquery-3.6.1.min.js"></script>
```

## index.aspx
### html定義好圖
* 一開始先把我們要的圖形先定義好，當選擇某一幣別就顯示(style="display: block;")，沒有選到就不顯示(style="display: none;")。
    * 下拉是選單
    ```
    <div>
        <select id="GA4_Select">
            <option value="GA4_1_show" selected>新台幣</option>
            <option value="GA4_2_show">人民幣</option>
            <option value="GA4_3_show">日圓</option>
            <option value="GA4_4_show">韓元</option>
            <option value="GA4_5_show">新加坡元</option>
            <option value="GA4_6_show">歐元</option>
            <option value="GA4_7_show">英鎊</option>
            <option value="GA4_8_show">澳幣</option>
        </select>
    </div>
    ```
    * chart.js圖形:記住id名稱，style為控制顯示
    ```
    <%--新台幣長條圖--%>
    <div class="chart" id="GA4_1_show" style="display: block;">
        <h3 style="text-align: center;margin-bottom: 20px;">新台幣長條圖</h3>
        <canvas id="myChart_GA4_1" style="width:100%; max-height:100%"></canvas>   
   </div>
    <%--人民幣長條圖--%>
    <div class="chart" id="GA4_2_show" style="display:none;">
        <h3 style="text-align: center;margin-bottom: 20px;">人民幣分布圖</h3>
        <canvas id="myChart_GA4_2" style="width:100%; max-height:100%"></canvas>
    </div>
     <%--日圓長條圖--%>
    <div class="chart" id="GA4_3_show" style="display:none;">
        <h3 style="text-align: center;margin-bottom: 20px;">日圓分布圖</h3>
        <canvas id="myChart_GA4_3" style="width:100%; max-height:100%"></canvas>
    </div>
    <%--韓元長條圖--%>
    <div class="chart" id="GA4_4_show" style="display:none;">
        <h3 style="text-align: center;margin-bottom: 20px;">韓元分布圖</h3>
        <canvas id="myChart_GA4_4" style="width:100%; max-height:100%"></canvas>
    </div>
    <%--新加坡元長條圖--%>
    <div class="chart" id="GA4_5_show" style="display:none;">
        <h3 style="text-align: center;margin-bottom: 20px;">新加坡元分布圖</h3>
        <canvas id="myChart_GA4_5" style="width:100%; max-height:100%"></canvas>
    </div>
    <%--歐元長條圖--%>
    <div class="chart" id="GA4_6_show" style="display:none;">
        <h3 style="text-align: center;margin-bottom: 20px;">歐元分布圖</h3>
        <canvas id="myChart_GA4_6" style="width:100%; max-height:100%"></canvas>
    </div>
    <%--英鎊長條圖--%>
    <div class="chart" id="GA4_7_show" style="display:none;">
        <h3 style="text-align: center;margin-bottom: 20px;">英鎊分布圖</h3>
        <canvas id="myChart_GA4_7" style="width:100%; max-height:100%"></canvas>
    </div>
    <%--澳幣長條圖--%>
    <div class="chart" id="GA4_8_show" style="display:none;">
        <h3 style="text-align: center;margin-bottom: 20px;">澳幣分布圖</h3>
        <canvas id="myChart_GA4_8" style="width:100%; max-height:100%"></canvas>
    </div>
    ```
* 因此網頁讀入時會取得撈到的資料，然後將資料重新整理成chart.js能吃的格式，以[長條圖](https://www.chartjs.org/docs/latest/charts/bar.html)為例子，共有三大部分:
1. data
2. config
3. new Chart
每張圖的格式不一樣，以官網說得為主。根據格式定義重新整理json，所以先把後端傳過來的資料作接收吧。
        
## 取得後端資料
介紹兩種方式:
1. <%函式名稱%>
2. Ajax
### 1. 用JS獲取本頁面中的服務器端控件值 <%函式名稱%>，印出來看看
```
<!-- GA4_show_bar  -->  
<script>
    $(document).ready(function () {
        var data_JSON = JSON.parse('<%=catch_json_url_with_binding()%>');
        console.log(data_JSON);
    });
</script>
```
![](https://i.imgur.com/rmaWKic.png)

:::success
注意幾點:
* 這個用法式C# ASP. NET FrameWork的特殊用法，'<%=xxx%>' 一定要寫在Index.aspx中，你用引入<script>/.xxx.js檔的方式會有問題，這點要注意。
* 這寫法容易把功能都些在同一個檔案，有些共用的功能就只能在寫一次，所以用這方法最好是只有在這才會使用的方法。
:::
### index.aspx完整版
知道資料格式後，根據格式把資料放到chart.js的格式中
```
<script>
    $(document).ready(function () {
        var data_JSON = JSON.parse('<%=catch_json_url_with_binding()%>');
        // x軸式年分
        var year_labels_arr = [];
        // 每年每個幣別，當y軸
        var GA_dict = { "新台幣": [], "人民幣": [], "日圓": [], "韓元": [], "新加坡元": [], "歐元": [], "英鎊": [], "澳幣": [] };
        // 幣別
        var GA_name = ["新台幣", "人民幣", "日圓", "韓元", "新加坡元", "歐元", "英鎊", "澳幣"];
        // 創chart流水號
        let count = 1;
        // 塞資料到字典
        data_JSON.forEach(function (member) {
            year_labels_arr.push(member["月別"]);
            GA_dict["新台幣"].push(member["新台幣"]);
            GA_dict["人民幣"].push(member["人民幣"]);
            GA_dict["日圓"].push(member["日圓"]);
            GA_dict["韓元"].push(member["韓元"]);
            GA_dict["新加坡元"].push(member["新加坡元"]);
            GA_dict["歐元"].push(member["歐元"]);
            GA_dict["英鎊"].push(member["英鎊"]);
            GA_dict["澳幣"].push(member["澳幣"]);
        });
        // 拿每個字典的資料，塞到data中，data是chart.js的bar資料格式，共創建8個Chart
        GA_name.forEach(function (member) {
            const data = {
                labels: year_labels_arr,
                datasets: [{
                    label: member,
                    data: GA_dict[member],
                    borderColor: 'rgba(' + generateRandomInt() + ',' + generateRandomInt() + ',' + generateRandomInt() + ',1)',
                    backgroundColor: 'rgba(' + generateRandomInt() + ',' + generateRandomInt() + ',' + generateRandomInt() + ',1)',
                    hoverBorderWidth: 5,
                    hoverBorderColor: 'green',
                }],
            };
            // 塞data資料，圖形為bar，長條圖
            const config = {
                type: 'bar',
                data: data,
                options: {
                },
            };
            // 根據canvas的id，創建Chart到html中
            const myChart_GA4 = new Chart(
                document.getElementById('myChart_GA4_' + count.toString()),
                config
            );
            count += 1;
        });
    });

    //0~255產生隨機數字
    function generateRandomInt() {
        return Math.floor((Math.random() * (255 - 0)) + 0);
    }

 </script>
```

### 2. Ajax方法
注意:
* 與前端溝通需要用[WebMethod]傳值
* 函式必須是 static
#### Index.aspx.cs
```
[WebMethod]
public static String catch_json_url_with_ajax()
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
    //Label1.Text = json_url;
    return reply;
}
```
新增資料夾，創建GA_json.js
![](https://i.imgur.com/at95d4C.png)
#### GA_json.js
```
$(document).ready(function () {
    $.ajax({
        type: "post",
        url: "Index.aspx/catch_json_url_with_ajax",
        contentType: " application/json; charset=utf-8 ",
        dataType: "json",
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (response) {
            var data_JSON = JSON.parse(response.d);
            var year_labels_arr = [];
            var GA_dict = { "新台幣": [], "人民幣": [], "日圓": [], "韓元": [], "新加坡元": [], "歐元": [], "英鎊": [], "澳幣": [] };
            var GA_name = ["新台幣", "人民幣", "日圓", "韓元", "新加坡元", "歐元", "英鎊", "澳幣"];
            let count = 1;
            data_JSON.forEach(function (member) {
                year_labels_arr.push(member["月別"]);
                GA_dict["新台幣"].push(member["新台幣"]);
                GA_dict["人民幣"].push(member["人民幣"]);
                GA_dict["日圓"].push(member["日圓"]);
                GA_dict["韓元"].push(member["韓元"]);
                GA_dict["新加坡元"].push(member["新加坡元"]);
                GA_dict["歐元"].push(member["歐元"]);
                GA_dict["英鎊"].push(member["英鎊"]);
                GA_dict["澳幣"].push(member["澳幣"]);
            });
            GA_name.forEach(function (member) {
                const data = {
                    labels: year_labels_arr,
                    datasets: [{
                        label: member,
                        data: GA_dict[member],
                        borderColor: 'rgba(' + generateRandomInt() + ',' + generateRandomInt() + ',' + generateRandomInt() + ',1)',
                        backgroundColor: 'rgba(' + generateRandomInt() + ',' + generateRandomInt() + ',' + generateRandomInt() + ',1)',
                        hoverBorderWidth: 5,
                        hoverBorderColor: 'green',
                    }],
                };
                const config = {
                    type: 'bar',
                    data: data,
                    options: {
                    },
                };

                const myChart_GA4 = new Chart(
                    document.getElementById('myChart_GA4_' + count.toString()),
                    config
                );
                count += 1;
            });
        },
    });
    
});

//0~255產生隨機數字
function generateRandomInt() {
    return Math.floor((Math.random() * (255 - 0)) + 0);
}
```
    
#### Index.aspx
引用js
```
<script src="/assets/js/GA_json.js">
```

這樣分開寫的好處是比較好整理，也比較簡潔。
:::info
注意幾點:
1. 寫成API串前端 [WebMethod]，並將方法宣告為靜態static([相關介紹](https://dotblogs.com.tw/justforgood/2016/10/21/172354))
2. 引用js到.aspx
3. url: "Index.aspx/catch_json_url" : 別打成Index.aspx.cs，後面是函式名稱         
4. 留意type([RESTful API？](https://aws.amazon.com/tw/what-is/restful-api/))、url(後端.aspx\有[WebMethod]的函式名稱)、contentType(UTF8轉繁體)、dataType(json格式)、error(API呼叫失敗)、success(API呼叫成功)用法
5. response.d : response是個物件，response.d是取得值
:::  

本專案之後都用Ajax的方法。
## 下拉式選單
之前定義好了選單，會需要有個觸發事件，當我改變下拉式選單的值，則先把全部都隱藏，之後再把選到的值顯示出來。
#### Index.aspx
* 選單的id是GA4_Select，前面記得加# ([selector](https://www.w3schools.com/cssref/css_selectors.php)?)
* .change(function (){...}); 當值改變時要做什麼.... ([jQuery觸發事件](https://www.w3schools.com/jquery/default.asp))
*  $('.chart').each(function (item, value) : 取得每一個class="chart"的item(位置),value(元素值)
* 顯示適用css的效果，所以注意最裡面的(.each)是用#value.id找所有id，找到改標籤中的style="display: block;"，this.id是選單選中的id。

```
<script>
    $("#GA4_Select").change(function () {
        $('.chart').each(function (item, value) {
            $("#" + value.id).css("display", "none");
        });
        $("#" + this.value).css("display", "block");
    });
</script>
```

## 顯示結果
![](https://i.imgur.com/lwheNLw.png)

---
 
## 結論
一開始蠻不習慣的，各種程式語言都碰一點，但其實每一個用法都是做固定的事情，開發者則是要思考哪一種方法最快，時時刻刻想著如果很多人操作，會不會出現問題。本日主要想透過視覺化的案例簡單走一遍前後端的整合。  


## 補充區
* [jQuery Ajax in ASP.NET](https://www.c-sharpcorner.com/UploadFile/337082/jquery-ajax-in-Asp-Net/)
