using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Day17.Models;
namespace Day17
{
    public partial class frontlogin : System.Web.UI.Page
    {
        dbmemberEntities1 db = new dbmemberEntities1();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            
            var member = db.Member.Where(m => m.Account == TextBox1.Text && m.Password == Password1.Value).FirstOrDefault(); //查詢
            if (member != null)
            {
                Response.Redirect("Index.aspx");
            }
            else
            {
                Response.Write(@"<script>alert(""登入失敗"");</script>");
            }
            
        }
    
    }
}