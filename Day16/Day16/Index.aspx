<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Day16.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/> 
    <%-- chart.js套件 --%>
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script> 
    <script src="https://cdn.jsdelivr.net/npm/chart.js@3.5.1/dist/chart.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/chartjs-adapter-date-fns/dist/chartjs-adapter-date-fns.bundle.min.js"></script>
    <script src="/Scripts/jquery-3.6.1.min.js"></script>
    
</head>
<body>
    <%--下拉式選單--%>
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
    <%--新台幣長條圖--%>
    <div class="chart" id="GA4_1_show" style="display: block;">
        <h3 style="text-align: center;margin-bottom: 20px;">新台幣長條圖</h3>
        <canvas id="myChart_GA4_1" style="width:80%; max-height:90%"></canvas>   
   </div>
    <%--人民幣長條圖--%>
    <div class="chart" id="GA4_2_show" style="display:none;">
        <h3 style="text-align: center;margin-bottom: 20px;">人民幣分布圖</h3>
        <canvas id="myChart_GA4_2" style="width:80%; max-height:100%"></canvas>   
    </div>
     <%--日圓長條圖--%>
    <div class="chart" id="GA4_3_show" style="display:none;">
        <h3 style="text-align: center;margin-bottom: 20px;">日圓分布圖</h3>
        <canvas id="myChart_GA4_3" style="width:80%; max-height:100%"></canvas>   
    </div>
    <%--韓元長條圖--%>
    <div class="chart" id="GA4_4_show" style="display:none;">
        <h3 style="text-align: center;margin-bottom: 20px;">韓元分布圖</h3>
        <canvas id="myChart_GA4_4" style="width:80%; max-height:100%"></canvas>   
    </div>
    <%--新加坡元長條圖--%>
    <div class="chart" id="GA4_5_show" style="display:none;">
        <h3 style="text-align: center;margin-bottom: 20px;">新加坡元分布圖</h3>
        <canvas id="myChart_GA4_5" style="width:80%; max-height:100%"></canvas>   
    </div>
    <%--歐元長條圖--%>
    <div class="chart" id="GA4_6_show" style="display:none;">
        <h3 style="text-align: center;margin-bottom: 20px;">歐元分布圖</h3>
        <canvas id="myChart_GA4_6" style="width:80%; max-height:100%"></canvas>   
    </div>
    <%--英鎊長條圖--%>
    <div class="chart" id="GA4_7_show" style="display:none;">
        <h3 style="text-align: center;margin-bottom: 20px;">英鎊分布圖</h3>
        <canvas id="myChart_GA4_7" style="width:80%; max-height:100%"></canvas>   
    </div>
    <%--澳幣長條圖--%>
    <div class="chart" id="GA4_8_show" style="display:none;">
        <h3 style="text-align: center;margin-bottom: 20px;">澳幣分布圖</h3>
        <canvas id="myChart_GA4_8" style="width:80%; max-height:100%"></canvas>   
    </div>
    
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
        </div>      
   </form>

    <!-- GA4_show_bar  -->  
    <script src="/assets/js/GA_json.js"></script>
     <!--<script>
        $(document).ready(function () {
            var data_JSON = JSON.parse('<%=catch_json_url_with_binding()%>');
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
        });
          
    //0~255產生隨機數字
    function generateRandomInt() {
        return Math.floor((Math.random() * (255 - 0)) + 0);
    }
     </script>
     --> 
    <script>
        $("#GA4_Select").change(function () {
            $('.chart').each(function (item, value) {
                $("#" + value.id).css("display", "none");
            });
            $("#" + this.value).css("display", "block");
        });
        
    </script>
        
</body>  
</html>
