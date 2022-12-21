﻿/**網頁一開始先做 **/
$(document).ready(function () {
    /*-------------- 導覽列(會員管理)設為 on --------------*/
    $(".member").addClass("on");
    GetData();

    $(".btn-pill").on("click", function (e) {
        e.preventDefault();
        $("#MsgModal").modal("hide");
        $("#ItemModal").modal("hide");
    });
    $(".close").on("click", function (e) {
        e.preventDefault();
        $("#MsgModal").modal("hide");
        $("#ItemModal").modal("hide");
    });
    $("input[type=radio][name=optionsRadios]").change(function () {
        $("#hidGender").val(this.value);
    });
});
/** 抓取會員列表*/
function GetData() {
    /*分頁功能(使用pagination套件)*/
    const memberTbody = document.querySelector(".memberTbody");
    $("#pagination-container").pagination({
        dataSource: function (done) {
            $.ajax({
                type: "GET",
                url: "/WebService1.asmx/GetMemberList",
                dataType: "json",
                data: {
                    keyword: $('#txtSearch').val()
                },
                success: function (response) {
                    $('#txtSearch').val('');
                    const thisData = response;
                    /*共有幾位會員*/
                    $('.searchBox > p > span').html(thisData.length);
                    if (thisData.length == 0) {
                        memberTbody.innerHTML = '';
                        $('#msgText').html('查無相關資料');
                        $('#MsgModal').modal("show");
                        return;
                    }
                    done(thisData); // 將資料回傳到下方 callback (預設初始)
                },
            });
        },
        pageSize: 10, // 一頁幾筆資料
        callback: function (data, pagination) {
            const thisData = data;
            let str = "";
            //*組字串並渲染到畫面上*/
            thisData.forEach(function (item, index) {
                var state_html = `<td class="state"><a key=${item.ID} class="stateBtn btn btn-m btn-success ${item.State[0]}" href="javascript:void(0);">${item.State[1]}</a></td>`;
                if (item.State[0] == 'off') state_html = `<td class="state"><a key=${item.ID} class="stateBtn btn btn-m btn-danger ${item.State[0]}" href="javascript:void(0);">${item.State[1]}</a></td>`;
                str += `<tr class="align-middle">
                          <td scope="row">${item.ID}</td>
                          <td>${item.Name}</td>
                          <td class="account">${item.Account}</td>
                          <td>${item.CreateTime}</td>
                          <td class="permissionTd">${item.Permissions}</td>` + state_html +
                        `<td class="editTd">
                            <a class="btn btn-outline-secondary" href="javascript:void(0);" onclick=openEdit('${item.ID}');> 編輯</a>
                            <a class="btn btn-danger" href="javascript:void(0);" onclick=openDelete('${item.ID}');>刪除</a>
                         </td> 
                        </tr>`;
                memberTbody.innerHTML = str;
            });
            /*-------------- 更換會員狀態 --------------*/
            $('.stateBtn').click(function (e) {
                e.preventDefault();
                var id = $(this).attr('key');
                var state;
                if ($(this).hasClass('off')) {
                    $(this).removeClass('btn-danger off');
                    $(this).addClass('btn-success on');
                    $(this).text("正常");
                    state = '1';
                } else {
                    $(this).removeClass('btn-success on');
                    $(this).addClass('btn-danger off');
                    $(this).text("停權");
                    state = '0';
                }
                //更新資料庫
                ChangeState(id, state);
            })
        },
    });
}
/** 修改會員狀態*/

/** 抓取會員資料*/

/** 刪除會員資料*/
