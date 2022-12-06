$(document).ready(function () {
    $.ajax({
        type: "post",
        url: "Index.aspx/catch_json_url_with_ajax",
        contentType: " application/json; charset=utf-8 ",
        dataType: " json ",
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (response) {
            var data_JSON = JSON.parse(response.d);
            var year_labels_arr = [];
            var GA_dict = { "新台幣": [], "人民幣": [], "日圓": [], "韓元": [], "新加坡元": [], "歐元": [], "英鎊": [], "澳幣": [] };
            var GA_name = ["新台幣", "人民幣", "日圓", "韓元", "新加坡元", "歐元", "英鎊", "澳幣"];
            let count = 1;

            document.getElementById("Label1").innerHTML = response.d;
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


