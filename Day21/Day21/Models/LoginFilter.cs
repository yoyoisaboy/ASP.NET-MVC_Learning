using Day21.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Day21.Models
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class LoginFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string check_login = CookieHelper.GetCookieValue("Islogin");
            // 存Session中，跳轉回來會被洗掉，因此順序是先進token再到這裡，在token中重新刻 Islogin
            if (check_login != "true") HttpContext.Current.Session["Islogin"] = check_login;
            var sessionName = HttpContext.Current.Session["Islogin"]; //可用中繼點看變化，如果跳轉回來不會有Islogin存在
            if (sessionName == null || sessionName.ToString() == "") filterContext.Result = new RedirectResult("/Home2/login");
        }
    }
}