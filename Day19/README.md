ASP .NET Web應用程式上手(.NET Framework) Day 19 (網路服務Web API asmx_Part2)
===
###### tags: `ASP .NET Web` `C#` `.NET Framework`

github : [Day19](https://github.com/yoyoisaboy/ASP.NET-MVC_Learning/tree/main/Day19)

## 前言
Part1 把介面刻好了，也把資料撈到印在上面，接下來做編輯與刪除的功能，用彈跳視窗(Modal)顯示訊息和編輯介面。

目前有:
1. Index.aspx : 首頁
2. member.aspx : 顯示會員清單
3. frontlogin.aspx : 登入介面
4. frontRegister.aspx : 註冊會員(純介面)
5. Site1.Master : aspx母框
6. dbCommand.cs : SQL指令執行
7. frontLogin.js : ajax登入
8. member.js : ajax抓取會員列表

## 會員狀態
### member.js : ChangeState修改會員狀態
狀態按鈕改變時，根據那一行的id，改當下的State。
```
function ChangeState(id, state) {
    $.ajax({
        type: "POST",
        url: "/WebService1.asmx/SetMemberState",
        contentType: 'application/x-www-form-urlencoded',
        dataType: 'text',
        data: {
            id: id,
            state: state
        },
        success: function (str) {
            str = str.replace(/^\"|\"$/g, '');//去除string前后的双引号
            $('#msgText').html(str);
            $('#MsgModal').modal("show");
        }, error: function (data) {
            console.log('無法送出');
        }
    });
}
```
### WebService1.asmx.cs : 修改會員狀態
UPDAT更新資料
```
#region 修改會員狀態
[WebMethod]
public void SetMemberState(string id, int state)
{
    string mMsg = "";
    MySqlCommand Cmd = new MySqlCommand();
    Cmd.Parameters.AddWithValue("@Date", DateTime.Now.Date);
    Cmd.Parameters.AddWithValue("@ID", id);
    Cmd.Parameters.AddWithValue("@State", state);
    Cmd.CommandText = @"UPDATE tmember SET State=@State WHERE ID=@ID";
    try
    {
        if (dbCommand.ExecuteSQL(Cmd)) mMsg = "資料修改成功";
        else mMsg = "fail";
    }
    catch { mMsg = "資料修改失敗"; }
    Context.Response.Write(new JavaScriptSerializer().Serialize(mMsg));
    Context.Response.End();
}
#endregion
```
### 結果
可以F12看html變化
![](https://i.imgur.com/w5LjDom.png)



## 會員編輯
### member.aspx : 彈跳視窗訊息Modal
當按下"編輯"按鈕，顯示Modal出來，沒按就隱藏起來。
```
<% if (Session["permissions"]!=null && Session["permissions"].ToString().Contains("R"))
{%>
    <%--會員文字 + 搜尋框--%>
    ...
    <%--表格--%>
    ...
    <%-- 訊息Modal --%>
    <div class="modal fade" id="MsgModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">訊息通知</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div id="msgText"></div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger btn-pill" data-dismiss="modal" id="btnModeClose">OK</button>
                </div>
            </div>
        </div>
    </div>
    <%-- 編輯Modal --%>
    ...
<% else{%>
    <a href="./frontlogin.aspx" class="nav-link">請先登入</a>
<%}%>
```
* msgText : 主要顯示的文字訊息。ex:編輯成功、刪除成功、修改成功...等等的訊息。
* MsgModal : 控制顯示或隱藏的名稱
* ItemModal : 控制顯示或隱藏的名稱

### member.js : 當按鈕或勾勾改變時...
```
/**網頁一開始先做 **/
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
    //名字是 optionsRadios 的 radio 改變時
    $("input[type=radio][name=optionsRadios]").change(function () {
        $("#hidPermissions").val(this.value);
    });
});
```
* e.preventDefault(); : [沒有跳轉事件](https://ithelp.ithome.com.tw/articles/10198999)
### member.aspx : 彈跳視窗編輯Modal
```
<% if (Session["permissions"].ToString().Contains("U"))
{%>
    <div class="modal fade" id="ItemModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true" style="display: none;">
      <div class="modal-dialog" role="document">
         <div class="modal-content">
            <!--modal-header-->
            <!--modal-body-->
            <!--modal-footer-->
          </div>
       </div>
    </div>
<%}%>
```
* class="modal fade" : 顯示加show
* 內容分成頭、中、尾
### member.aspx : modal-header 頭
```
<div class="modal-header">
    <h5 class="modal-title" id="exampleModalLabel">修改會員資料</h5>
    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
        <span aria-hidden="true">×</span>
    </button>
</div>
```
### member.aspx : modal-body 中
```
<div class="modal-body">
    <form role="form" id="myform" enctype="multipart/form-data">
        <input type="hidden" name="hidID" id="hidID" />
        <div class="row">
            <div class="col-sm-12">
                <div class="form-group">
                    <label for="txtID">會員編號</label>
                    <input type="text" class="form-control" name="txtID" id="txtID" disabled required>
                </div>
                <div class="form-group">
                    <label for="txtName">姓名(必填欄位)</label>
                    <input type="text" class="form-control" name="txtName" id="txtName" placeholder="姓名" required>
                </div>
                <div class="form-group">
                    <label for="txtAccount">Account(必填欄位)</label>
                    <input type="email" class="form-control" name="txtAccount" id="txtAccount" placeholder="Email" required>
                </div>
                <div class="form-group">
                    <label for="txtSort">權限</label>
                    <br />
                    <input type="hidden" name="hidPermissions" id="hidPermissions" />
                    <div class="form-check" style="float: left">
                        <label class="form-check-label">
                            <input type="radio" class="form-check-input" name="optionsRadios" id="optStatus1" value="R" checked>
                            一般會員(R)
                        </label>
                        <label class="form-check-label" style="margin-left: 40px;">
                            <input type="radio" class="form-check-input" name="optionsRadios" id="optStatus2" value="CR">
                            廠商(CR)
                        </label>
                        <label class="form-check-label" style="margin-left: 40px;">
                            <input type="radio" class="form-check-input" name="optionsRadios" id="optStatus3" value="CRUD">
                            管理者(CRUD)
                        </label>
                    </div>
                </div>
            </div>
        </div>
    </form>
</div>
```
* 編號一般都不能改，所以顯示就好。
* enctype="multipart/form-data" : [編碼方式](https://stackoverflow.com/questions/4526273/what-does-enctype-multipart-form-data-mean)
* form 裡放顯示的資料，js端從這整個form中拿資料，資料都是根據id找到標籤，拿標籤外的字。

### member.aspx : modal-footer 尾
```
<div class="modal-footer">
    <button type="button" class="btn btn-danger btn-pill" data-dismiss="modal">取消</button>
    <button type="button" class="btn btn-primary btn-pill" onclick="confirmSave();">確認</button>
</div>
```
* confirmSave() : 修改會員資料存檔觸發事件，傳過去是整個form裡的東西，取值是根據id的名稱取value值。

### member.js : openEdit 抓取會員資料
按下編輯按鈕會根據按鈕的id抓資料
```
/** 抓取會員資料*/
function openEdit(id) {
    $.ajax({
        type: 'POST',
        url: "/WebService1.asmx/GetMemberData",
        dataType: 'json',
        data: {
            id: id,
        },
        success: function (response) {
            if (response.length != 0) {
                response.forEach(function (item, index) {
                    $('#hidID').val(`${item.ID}`);
                    $('#txtID').val(`${item.ID}`);
                    $('#txtName').val(`${item.Name}`);
                    $('#txtAccount').val(`${item.Account}`);
                    $("input[name=optionsRadios][value=" + `${item.Permissions}` + "]").prop('checked', 1);
                    $('#hidPermissions').val(`${item.Permissions}`);
                });
                $('#ItemModal').modal("show");
            }
            else {
                alert("無權限");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            if (navigator.notification)
                navigator.notification.alert('GetData error,' + textStatus + ': ' + errorThrown, null, 'error');
            else
                alert('GetData error,' + textStatus + ': ' + errorThrown);
        }
    });
}
```
* [jQuery prop()方法](https://www.runoob.com/jquery/html-prop.html)

### WebService1.asmx.cs : 取得會員各欄位資料
```
#region 取得會員各欄位資料
[WebMethod(EnableSession = true)]
public void GetMemberData(string id)
{
    MySqlCommand Cmd = new MySqlCommand();
    DataTable dt = new DataTable();
    List<object> list = new List<object>();
    var check_premissions = HttpContext.Current.Session["permissions"];
    if (check_premissions != null && check_premissions.ToString().Contains("U"))
    {
        Cmd.Parameters.AddWithValue("@ID", id);
        Cmd.CommandText = @"select * From tmember WHERE ID=@ID";
        dt = dbCommand.GetTable(Cmd);
        foreach (DataRow dr in dt.Rows)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("ID", dr["ID"].ToString());
            dict.Add("Account", dr["Account"].ToString());
            dict.Add("Name", dr["Name"].ToString());
            dict.Add("Permissions", dr["Permissions"].ToString());
            list.Add(dict);
        }
    }
    Context.Response.Write(new JavaScriptSerializer().Serialize(list));
    Context.Response.End();
}
```
## member.aspx -> 編輯Modal -> confirmSave事件
這裡可以想一下，修改完資料，把欄位中的值再傳到js
### member.js : confirmSave事件
```
/** 修改會員資料存檔*/
function confirmSave() {
    var form = $('form')[0];
    var formData = new FormData(form);
    console.log(formData.get('txtName'));
    $.ajax({
        type: "POST",
        url: "/WebService1.asmx/MemberEdit",
        enctype: 'multipart/form-data',
        data: formData,
        contentType: false,
        cache: false,
        processData: false,
        success: function () {
            location.reload();
        }, error: function (data) {
            console.log('無法送出');
        }
    });
}
```
* 拿html中第一個form，轉FormData
* enctype 記得寫編碼方式，在< form>標籤有定義
:::info
* [[WebAPIs] Form, FormData, 表單取值, AJAX 檔案與資料上傳](https://pjchender.dev/webapis/webapis-form-formdata/)
* [關於jQuery AJAX cache參數](https://blog.darkthread.net/blog/about-jquery-ajax-cache-option/)
* [processData:false](https://blog.xuite.net/kb8.gyes/free/25002288) : data轉成字串(String)的功能關掉
:::
### WebService1.asmx.cs : 修改會員資料
```
#region 修改會員資料
[WebMethod(EnableSession = true)]
public void MemberEdit()
{
    string id = HttpContext.Current.Request.Params["hidID"];
    string name = HttpContext.Current.Request.Params["txtName"];
    string account = HttpContext.Current.Request.Params["txtAccount"];
    string permissions = HttpContext.Current.Request.Params["hidPermissions"];
    MySqlCommand Cmd = new MySqlCommand();
    Cmd.Parameters.AddWithValue("@ID", id);
    Cmd.Parameters.AddWithValue("@Name", name);
    Cmd.Parameters.AddWithValue("@Account", account);
    Cmd.Parameters.AddWithValue("@Permissions", permissions);
    Cmd.CommandText = @"UPDATE tmember SET 
                        Name=@Name,Account=@Account,Permissions=@Permissions 
                        WHERE ID=@ID";
    var check_premissions = HttpContext.Current.Session["permissions"];
    if (check_premissions != null && check_premissions.ToString().Contains("U"))
    {
        try
        {
            dbCommand.ExecuteSQL(Cmd);
        }
        catch { }
    }
}
#endregion
```
* [網路基礎 - HTTP、Request、Response](https://yakimhsu.com/project/project_w4_Network_http.html)
### 結果
可以F12觀察 html 變化
![](https://i.imgur.com/A8w3aAU.png)

## 刪除會員
### member.js :  刪除會員資料 openDelete()
```
/** 刪除會員資料*/
function openDelete(id) {
    var r = confirm("確定要刪除嗎?");
    if (r == true) {
        $.ajax({
            type: 'POST',
            url: "/WebService1.asmx/DeleteMember",
            dataType: 'json',
            data: {
                id: id
            },
            success: function (response) {
                //console.log(response);
                location.reload();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                if (navigator.notification)
                    navigator.notification.alert('GetData error,' + textStatus + ': ' + errorThrown, null, 'error');
                else
                    alert('Delete error,' + textStatus + ': ' + errorThrown);
            }
        });
    }
}
```
:::warning
* [jquery ajax error函數和及其參數詳細說明](https://shiyousan.com/post/635433082130309661)
:::
### WebService1.asmx.cs : 刪除會員資料
```
[WebMethod(EnableSession = true)]
public void DeleteMember(string id)
{
    string mMsg = "";
    MySqlCommand Cmd = new MySqlCommand();
    Cmd.Parameters.AddWithValue("@ID", id);
    Cmd.CommandText = @"DELETE FROM tmember  WHERE ID=@ID";
    var check_premissions = HttpContext.Current.Session["permissions"];
    if (check_premissions != null && check_premissions.ToString().Contains("D"))
    {
        try
        {
            if (dbCommand.ExecuteSQL(Cmd)) mMsg = "資料刪除成功";
            else mMsg = "Delete fail";
        }
        catch { mMsg = "資料刪除失敗"; }

    }
    Context.Response.Write(new JavaScriptSerializer().Serialize(mMsg));
    Context.Response.End();
}
```
### 結果
![](https://i.imgur.com/KApx6WR.png)

## 註冊 : frontRegister.aspx
### frontRegister.aspx : 頁面
```
<div class="contentFrontLogin container">
    <div class="loginBox">
        <p class="h1 text-center">會員註冊</p>
        <div class="loginTest">
            <!-- Email input -->
            <div class="form-outline mb-4">
                <asp:TextBox type="email" ID="account" runat="server" class="account form-control input-lg"  TabIndex="1" placeholder="電子郵件"></asp:TextBox>
            </div> 
            <!-- Name input -->
            <div class="form-outline mb-4">
                <asp:TextBox type="text" ID="name" runat="server" class="name form-control input-lg"  TabIndex="1" placeholder="名字"></asp:TextBox>
            </div> 
            <!-- Password input -->
            <div class="form-outline mb-4">
                <input id="password" runat="server" type="password" class="password form-control input-lg"  placeholder="密碼" />
            </div>
            <!-- Confirmpassword input -->
             <div class="form-outline mb-4">
                <input id="Confirmpassword" runat="server" type="password" class="Confirmpassword form-control input-lg"  placeholder="確認密碼" />
            </div>
            <div class="form-outline mb-4">
                <asp:Button ID="registerBtn" class="registerBtn btn btn-dark" runat="server" Text="註冊" OnClientClick="return false;" />
                <a class="nav-link login" href="./frontlogin.aspx">已經是會員?</a>
                <br />
            </div>
       </div>
    </div>
</div>
<script src="assets/js/frontRegister.js"></script>
```
:::info
記得Account是不可以重複的。
![](https://i.imgur.com/59YSvKk.png)
:::

### frontRegister.js
* 防呆記得做
* 主要傳account、password、name，其他資料在後台新增
* 完成後跳轉到登入畫面
```
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
```
### WebService1.asmx.cs : CreateMember
* 第一個 MySqlCommand : 找最大編號 +1，本例子是用流水編號，你也可以用 SQL 內建新增資料時自動+1 (AUTO_INCREMENT)。
* 第二個 MySqlCommand : 新增資料，因為Account是唯一，所以如果以重複的話ExecuteSQL執行完會是false。
```
#region frontRegister.js會員後台
#region 註冊會員
[WebMethod]
public void CreateMember(string account, string password, string name)
{
    string mMsg = "";
    MySqlCommand Cmd = new MySqlCommand();
    DataTable dt = new DataTable();
    Cmd.CommandText = @"SELECT * FROM tmember ";
    Cmd.CommandText += @"ORDER BY ID DESC ";
    dt = dbCommand.GetTable(Cmd);
    var memberId = int.Parse(dt.Rows[0]["ID"].ToString());
    mMsg = String.Format("{0:000000}", Convert.ToInt16(memberId + 1)); //最大ID+1

    MySqlCommand Cmd2 = new MySqlCommand();
    Cmd2.CommandText = @"INSERT INTO tmember(ID,Account,Password,Name,CreateTime,State) VALUES(@ID,@Account,@Password,@Name,@CreateTime,@State)";
    Cmd2.Parameters.AddWithValue("@CreateTime", DateTime.Now.Date);
    Cmd2.Parameters.AddWithValue("@ID", mMsg);
    Cmd2.Parameters.AddWithValue("@Account", account);
    Cmd2.Parameters.AddWithValue("@Password", password);
    Cmd2.Parameters.AddWithValue("@Name", name);
    Cmd2.Parameters.AddWithValue("@State", '1');
    try
    {
        if(dbCommand.ExecuteSQL(Cmd2)) mMsg = "註冊成功";
        else mMsg = "信箱已被註冊";
    }
    catch (Exception e) 
    {
        mMsg = "註冊失敗";
    }
    Context.Response.Write(new JavaScriptSerializer().Serialize(mMsg));
    Context.Response.End();
}
#endregion
#endregion
```
### 結果
![](https://i.imgur.com/77HPFeh.png)

## 結論 
OK~~ 到這裡就是把CRUD刻完了，多練幾次後，大部分開發的功能都是圍繞在這觀念，各位把這個摸熟，就有好的基礎，並好好地摸大神的套件，打造好看的網站吧~~

