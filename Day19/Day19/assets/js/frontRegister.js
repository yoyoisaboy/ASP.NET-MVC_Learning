$(document).ready(function () {
    var temp = true;
    $(".registerBtn").click(function () {
        if (!$('.account').val()) {
            alert("信箱欄位不能為空");
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
        if (!$('.Confirmpassword').val()) {
            swal('確認密碼欄位不能為空');
            temp = false;
            return;
        }
        if ($('.password').val() !== $('.Confirmpassword').val()) {
            alert('確認密碼不相符');
            temp = false;
            return;
        }
        if (!$('.name').val()) {
            alert('姓名欄位不能為空');
            temp = false;
            return;
        }
        if (temp) {
            $.ajax({
                type: "POST",
                url: "/WebService1.asmx/CreateMember",
                data: {
                    account: $('.account').val(),
                    password: $('.password').val(),
                    name: $('.name').val(),
                },
                contentType: 'application/x-www-form-urlencoded',
                dataType: 'text',
                success: function (response) {
                    response = response.replace(/^\"|\"$/g, '');
                    alert(response);
                    if (response == "註冊成功") {
                        location.href = "/frontlogin.aspx";
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
    var emailRegxp = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/; //2009-2-12更正為比較簡單的驗證
    if (emailRegxp.test(email) != true)
        return false;
    else 
        return true;
}