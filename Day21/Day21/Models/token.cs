using Day21.Helper;
using System;
using System.Web;
using System.Web.Mvc;

namespace Day21.Models
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class token : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //可以在中繼點觀察一下cookie的變化
            string temp = CookieHelper.GetCookieValue("IsLogin"); //可用中繼點看變化
            string NET_SessionId = CookieHelper.GetCookieValue("ASP.NET_SessionId");
            //跳轉回來登入保持
            if (NET_SessionId != null) HttpContext.Current.Session["IsLogin"] = "true";

        }
    }
}