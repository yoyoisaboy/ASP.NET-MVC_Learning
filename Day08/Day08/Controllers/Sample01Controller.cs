using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Day08.Models;

namespace Day08.Controllers
{
    public class Sample01Controller : Controller
    {
        // GET: Sample01
        public ActionResult Index()
        {
            NorthwindEntities db = new NorthwindEntities();
            var product = from c in db.產品類別
                          join p in db.產品資料
                          on c.類別編號 equals p.類別編號
                          select new ProductViewModel
                          {
                              產品編號 = p.產品編號,
                              類別名稱 = c.類別名稱,
                              產品 = p.產品,
                              單位數量 = p.單位數量,
                              單價 = p.單價,
                              特價 = (int)((double)p.單價 * 0.9)
                          };
            return View(product);
        }
        public ActionResult test()
        {
            return View();
        }
    }
}