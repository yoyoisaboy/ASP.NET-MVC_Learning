$(document).ready(function () {
    $(".loginBtn").click(function () {
        var temp = true;
        if (!$('.account').val()) {
            alert('信箱欄位不能為空');
            temp = false;
            return;
        }
        if (!checkEmail($('.account').val())) {
            alert('信箱格式錯誤');
            temp = false;
            return;
        }
        if (!$('.password').val()) {
            alert('密碼欄位不能為空');
            temp = false;
            return;
        }
        if (temp) {
            $.ajax({
                type: "POST",
                url: "/WebService1.asmx/LoginMember",
                data: {
                    Account: $('.account').val(),
                    Password: $('.password').val(),
                },
                contentType: 'application/x-www-form-urlencoded',
                dataType: 'text',
                success: function (response) {
                    response = response.replace(/^\"|\"$/g, '');       
                    if (response == "success") {
                        location.href = "/index.aspx";
                    } else {
                        alert("登入失敗");
                    }
                },
                error: function (err) {
                    console.log(err);
                }
            });
        }
    });
});

function checkEmail(email) {
    var emailRegxp = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/; //簡單的驗證
    if (emailRegxp.test(email) != true)
        return false;
    else 
        return true;
}