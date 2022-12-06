using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Day09.Models;
namespace Day09.Controllers
{
    public class CategoryController : Controller
    {
        dbProductEntities db = new dbProductEntities();
        // GET: Category
        [Authorize]
        public ActionResult Index()
        {
            string uid = User.Identity.Name;
            string Permission = db.會員.Where(m => m.帳號 == uid).FirstOrDefault().權限;
            ViewBag.Permission = Permission;

            List<產品類別> category = new List<產品類別>();
            foreach(var item in db.產品類別.OrderByDescending(m => m.修改日))
            {
                category.Add(new 產品類別()
                {
                    類別編號 = item.類別編號,
                    類別名稱 = item.類別名稱,
                    編輯者 = item.編輯者,
                    修改日 = Sys.StringConverDateTimeString(item.修改日), 
                    建立日 = Sys.StringConverDateTimeString(item.建立日)
                });
            }
            return View(category);
        }

        [Authorize]
        public ActionResult Create()
        {
            string uid = User.Identity.Name;
            string Permission = db.會員.Where(m => m.帳號 == uid).FirstOrDefault().權限;
            if (!Permission.Contains("C"))
            {
                return RedirectToAction("Index", "PermissionErrorMsg", new { msg = "權限不足" });

            }
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult Create(string 類別名稱)
        {
            var tempProduct = db.產品類別.Where(m => m.類別名稱 == 類別名稱).FirstOrDefault();
            if (tempProduct != null)
            {
                ViewBag.IsProduct = true;
                return View();
            }

            string editdate = DateTime.Now.ToString("yyyyMMddHHmmss");

            產品類別 category = new 產品類別();
            category.類別名稱 = 類別名稱;
            category.編輯者 = User.Identity.Name;
            category.建立日 = editdate;
            category.修改日 = editdate;
            db.產品類別.Add(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Delete(int cid)
        {
            string uid = User.Identity.Name;
            string Permission = db.會員.Where(m => m.帳號 == uid).FirstOrDefault().權限;
            if (!Permission.Contains("D"))
            {
                return RedirectToAction("Index", "PermissionErrorMsg", new { msg = "權限不足" });
            }
            var products = db.產品資料.Where(m => m.類別編號 == cid).ToList();
            var category = db.產品類別.Where(m => m.類別編號 == cid).FirstOrDefault();
            db.產品資料.RemoveRange(products);
            db.產品類別.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Edit(int cid)
        {
            string uid = User.Identity.Name;
            string Permission = db.會員.Where(m => m.帳號 == uid).FirstOrDefault().權限;
            if (!Permission.Contains("D"))
            {
                return RedirectToAction("Index", "PermissionErrorMsg", new { msg = "權限不足" });
            }
            var category = db.產品類別.Where(m => m.類別編號 == cid).FirstOrDefault();
            return View(category);
        }
        [Authorize]
        [HttpPost]
        public ActionResult Edit(int 類別編號,string 類別名稱)
        {
            string editdate = DateTime.Now.ToString("yyyyMMddHHmmss");
            var category = db.產品類別.Where(m => m.類別編號 == 類別編號).FirstOrDefault();
            category.類別名稱 = 類別名稱;
            category.編輯者 = User.Identity.Name;
            category.修改日 = editdate;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}