using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Day10.Models;


namespace Day10.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string account,string password)
        {
            dbProductEntities db = new dbProductEntities();
            var member = db.會員.Where(m => m.帳號 == account && m.密碼 == password ).FirstOrDefault();
            
            if (member !=null)
            {
                FormsAuthentication.RedirectFromLoginPage(member.帳號, true);

                return RedirectToAction("Index", "Category");
                
            }
            ViewBag.IsLogin = true;
            return View();
        }
    }
}