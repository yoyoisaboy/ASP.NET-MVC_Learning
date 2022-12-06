using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Day10.Controllers
{
    public class PermissionErrorMsgController : Controller
    {
        [Authorize]
        // GET: PermissionErrorMsg
        public ActionResult Index(string msg)
        {
            ViewBag.ErrorMsg = msg;
            return View();
        }
    }
}