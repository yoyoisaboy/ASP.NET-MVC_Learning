using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Day13.Models;
namespace Day13.Controllers
{
    public class HomeController : Controller
    {
        NorthwindEntities db = new NorthwindEntities();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string keyword)
        {
            var custs = db.客戶.Where(m => m.地址.Contains(keyword)).
                ToList();
            return View(custs);
        }
    }
}