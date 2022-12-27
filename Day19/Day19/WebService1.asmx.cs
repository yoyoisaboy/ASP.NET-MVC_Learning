using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using Day19.App_Code;
using MySql.Data.MySqlClient;

namespace Day19
{
    /// <summary>
    ///WebService1 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
    [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        #region member.js會員後台
        #region 登入會員
        [WebMethod(EnableSession = true)]
        public void LoginMember(string Account, string Password)
        {
            string mMsg = "";
            MySqlCommand Cmd = new MySqlCommand();
            Cmd.CommandText = @"SELECT ID,Account,Name,Permissions FROM tmember WHERE Account=@Account AND Password=@Password AND State=@State";
            Cmd.Parameters.AddWithValue("@Account", Account);
            Cmd.Parameters.AddWithValue("@Password", Password);
            Cmd.Parameters.AddWithValue("@State", 1);
            DataTable dt = dbCommand.GetTable(Cmd);
            if (dt.Rows.Count > 0)
            {
                HttpContext.Current.Session.Add("userid", dt.Rows[0][0].ToString()); //User.Identity.Name
                HttpContext.Current.Session.Add("username", dt.Rows[0][2].ToString());
                HttpContext.Current.Session.Add("permissions", dt.Rows[0][3].ToString());
                mMsg = "success";
            }
            else mMsg = "Login fail";

            Context.Response.Write(new JavaScriptSerializer().Serialize(mMsg));
            Context.Response.End();
        }
        #endregion
        #region 取得會員清單
        [WebMethod]
        public void GetMemberList(string keyword)
        {
            MySqlCommand Cmd = new MySqlCommand();
            DataTable dt = new DataTable();
            Cmd.Parameters.AddWithValue("@KeyWord", "%" + keyword + "%");
            Cmd.CommandText = @"SELECT * FROM tmember ";
            //沒搜尋
            if (!keyword.Equals("")) Cmd.CommandText += @"WHERE Name Like @KeyWord ";
            Cmd.CommandText += @"ORDER BY ID";

            dt = dbCommand.GetTable(Cmd);
            // 轉json傳資料
            List<object> list = new List<object>();
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("ID", dr["ID"].ToString());
                dict.Add("Name", dr["Name"].ToString());
                dict.Add("Account", dr["Account"].ToString());
                dict.Add("CreateTime", dr["CreateTime"].ToString());
                dict.Add("Permissions", dr["Permissions"].ToString());
                if (dr["State"].ToString().Equals("1")) dict.Add("State", new string[] { "on", "正常" }); //on:給js看，正常:給人看
                else dict.Add("State", new string[] { "off", "停權" });
                list.Add(dict);
            }
            Context.Response.Write(new JavaScriptSerializer().Serialize(list)); //轉json 
            Context.Response.End();
        }
        #endregion
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
        #endregion
        #region 修改會員狀態
        [WebMethod(EnableSession = true)]
        public void SetMemberState(string id, int state)
        {
            string mMsg = "";
            MySqlCommand Cmd = new MySqlCommand();
            Cmd.Parameters.AddWithValue("@Date", DateTime.Now.Date);
            Cmd.Parameters.AddWithValue("@ID", id);
            Cmd.Parameters.AddWithValue("@State", state);
            Cmd.CommandText = @"UPDATE tmember SET State=@State WHERE ID=@ID";
            var check_premissions = HttpContext.Current.Session["permissions"];
            if (check_premissions != null && check_premissions.ToString().Contains("U"))
            {
                try
                {
                    if (dbCommand.ExecuteSQL(Cmd)) mMsg = "資料修改成功";
                    else mMsg = "Set fail";
                }
                catch { mMsg = "資料修改失敗"; }
            }
            else { mMsg = "資料修改失敗"; }
            Context.Response.Write(new JavaScriptSerializer().Serialize(mMsg));
            Context.Response.End();
        }
        #endregion
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
        #region 刪除會員
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
            else { mMsg = "刪除失敗"; }
            Context.Response.Write(new JavaScriptSerializer().Serialize(mMsg));
            Context.Response.End();
        }
        #endregion
        #endregion

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
    }

}
