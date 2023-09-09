using System;
using System.Web;

namespace Day21.Helper
{
    public static class CookieHelper
    {
        public static void SetCookie(string cookieName, string value, DateTime expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie != null)
            {
                cookie.Value = value;
                cookie.Expires = expires;
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            else
            {
                cookie = new HttpCookie(cookieName);
                cookie.Value = value;
                cookie.Expires = expires;
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }
        public static string GetCookieValue(string cookieName)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie == null || string.IsNullOrEmpty(cookie.Value))
                return "";
            else
                return cookie.Value;
        }
        public static void RemoveCookie(string cookieName)
        {
            SetCookie(cookieName, "", DateTime.Now.AddDays(-1));
        }
    }
}