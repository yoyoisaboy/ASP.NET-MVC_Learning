using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Day17.Models;
namespace Day17
{
    public partial class frontRegister : System.Web.UI.Page
    {
        dbmemberEntities1 db = new dbmemberEntities1();
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            if ( TextBox1 != null && !string.IsNullOrWhiteSpace(TextBox1.Text) && Password1 != null && !string.IsNullOrWhiteSpace(Password1.Value) && Password2 != null && !string.IsNullOrWhiteSpace(Password2.Value) )
            {
                String password1 = Password1.Value;
                String password2 = Password2.Value;
                if (password1== password2)
                {
                    String account = TextBox1.Text;
                    var member_db = db.Member.Where(m => m.Account == account).FirstOrDefault(); //查詢
                    if (member_db == null)
                    {
                        Member member = new Member();
                        member.Account = account;
                        member.Password = password1;
                        db.Member.Add(member);
                        db.SaveChanges();
                        Response.Redirect("frontlogin.aspx");
                    }
                    else Response.Write(@"<script>alert(""帳號已存在"");</script>");
                }
                else Response.Write(@"<script>alert(""密碼不一致"");</script>");
            }
            else Response.Write(@"<script>alert(""註冊失敗"");</script>");
        }
    }
}