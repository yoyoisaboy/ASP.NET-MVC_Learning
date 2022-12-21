<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="member.aspx.cs" Inherits="Day18.member" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <% if (Session["permissions"]!=null && Session["permissions"].ToString().Contains("R"))
    {%>
        <%--會員文字 + 搜尋框--%>
        <div class="textTop ">
            <h2>會員管理</h2>
            <div class="searchBox">
                <p>共有 <span></span> 位會員</p>
                <div class="searchFlex">
                    <input class="search" type="search" name="txtSearch" id="txtSearch" value="" placeholder="請輸入姓名關鍵字" />
                    <span class="searchIcon">
                        <img src="./assets/images/searchIcon.svg" alt="" onclick="GetData();" />
                    </span>
                </div>
            </div>
        </div>
        <%--表格--%>
        <div class="wrapper">
            <div class="form table-responsive ">
                <table class="table table-hover">
                    <thead class="text-nowrap">
                        <tr>
                            <th scope="col">會員編號</th>
                            <th scope="col">姓名</th>
                            <th scope="col">帳號(信箱)</th>
                            <th scope="col">創立時間</th>
                            <th scope="col">會員權限</th>
                            <th scope="col">會員狀態</th>
                            <th class="editTd" scope="col">編輯功能</th>
                        </tr>
                    </thead>
                    <tbody class="memberTbody">
                        <%-- 表單內容-表格 --%>

                    </tbody>
                </table>
            </div>

            <%--分頁的頁碼--%>
            <div id="pagination-container"></div>
            <%--使用pagination套件--%>
        </div>
        <!-- 訊息Modal -->
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
        <!-- End訊息Modal -->
        <!-- 編輯Modal -->
        <% if (Session["permissions"].ToString().Contains("U"))
        {%>
            <div class="modal fade" id="ItemModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true" style="display: none;">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="exampleModalLabel">修改會員資料</h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">×</span>
                            </button>
                        </div>
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
                                                    <input type="radio" class="form-check-input" name="optionsRadios" id="U" value="CRUD">
                                                    管理者(CRUD)
                                                </label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </form>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-danger btn-pill" data-dismiss="modal">取消</button>
                            <button type="button" class="btn btn-primary btn-pill" onclick="confirmSave();">確認</button>
                        </div>
                    </div>
                </div>
            </div>
        <%}%>

        <!-- End編輯Modal -->
     <%}%>
     <% else{%>
        <a href="./frontlogin.aspx" class="nav-link">請先登入</a>
     <%}%>


    <%---------------------------- 載入 JS 區域 ----------------------------%>
    <%-- pagination.js套件 --%>
    <script src="https://pagination.js.org/dist/2.4.2/pagination.min.js"></script>
    <%--自己寫的 JS--%>
    <script src="./assets/js/member.js"></script>

</asp:Content>
