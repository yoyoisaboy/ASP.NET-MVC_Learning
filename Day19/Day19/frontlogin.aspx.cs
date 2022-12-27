using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Day19
{
    public partial class frontlogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Session.Remove("userid");
            Session.Remove("username");
            Session.Remove("permissions");
            Response.Redirect("frontlogin.aspx");
        }
    }
}