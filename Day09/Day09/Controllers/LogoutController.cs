using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Day09.Controllers
{
    public class LogoutController : Controller
    {
        // GET: Logout
        public ActionResult Index()
        {
            FormsAuthentication.SignOut(); //清除cookie

            return RedirectToAction("Index","Home");
        }
    }
}