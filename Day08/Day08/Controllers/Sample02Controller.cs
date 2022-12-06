using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Day08.Models;

namespace Day08.Controllers
{
    public class Sample02Controller : Controller
    {
        // GET: Sample02
        public ActionResult Index(int cid = 1)
        {
            NorthwindEntities db = new NorthwindEntities();
            CategoryProductViewModel vm = new CategoryProductViewModel();
            vm.Category = db.產品類別.ToList();
            vm.Product = db.產品資料.Where(m => m.類別編號 == cid).ToList();
            return View(vm);
        }
    }
}