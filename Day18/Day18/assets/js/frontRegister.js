/*---------------------------- 下拉式選單 ----------------------------*/
$(document).ready(function () {
    // 選擇的時候，變換文字顏色
    $(".gender").change(function () {
        $(".gender").css("color", "#212529");
    });
    $(".registerBtn").click(function () {
        var temp = false;
        if (!$('.account').val()) {
            swal("信箱欄位不能為空");
            temp = false;
            return;
        }
        if (!checkEmail($('.account').val())) {
            swal('信箱格式錯誤');
            temp = false;
            return;
        }
        if (!$('.password').val()) {
            swal('密碼欄位不能為空');
            temp = false;
            return;
        }
        if (!$('.Confirmpassword').val()) {
            swal('確認密碼欄位不能為空');
            temp = false;
            return;
        }
        if ($('.password').val() !== $('.Confirmpassword').val()) {
            swal('確認密碼不相符');
            temp = false;
            return;
        }
        if (!$('.name').val()) {
            swal('姓名欄位不能為空');
            temp = false;
            return;
        }
        if ($('.gender').val() == 0 || $('.gender').val() ==null) {
            swal('請選擇性別');
            temp = false;
            return;
        }
        temp = true;
        if (temp) {
            $.ajax({
                        type: "POST",
                        url: "/WebService.asmx/CreateMember",
                        data: {
                            email: $('.account').val(),
                            password: $('.password').val(),
                            name: $('.name').val(),
                            gender: $('.gender').val(),
                        },
                        contentType: 'application/x-www-form-urlencoded',
                        dataType: 'text',
                        success: function (response) {
                            response = response.replace(/^\"|\"$/g, '');
                            swal(response);
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