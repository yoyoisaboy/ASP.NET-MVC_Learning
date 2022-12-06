using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Day10.Models;

namespace Day10.Controllers
{
    public class ProductController : Controller
    {
        dbProductEntities db = new dbProductEntities();
        [Authorize]
        public ActionResult Index(int cid = 1)
        {
            string uid = User.Identity.Name;
            string Permission = db.會員.Where(m => m.帳號 == uid).FirstOrDefault().權限;
            ViewBag.Permission = Permission;

            產品類別產品資料ViewModel vm = new 產品類別產品資料ViewModel();
            vm.Category = db.產品類別
            .OrderByDescending(m => m.修改日).ToList();
            var tempProduct = db.產品資料
            .Where(m => m.類別編號 == cid)
            .OrderByDescending(m => m.修改日).ToList();

            List<產品資料> product = new List<產品資料>();
            foreach (var item in tempProduct)
            {
                product.Add(new 產品資料()
                {
                    產品編號 = item.產品編號,
                    品名 = item.品名,
                    單價 = item.單價,
                    圖示 = item.圖示,
                    類別編號 = item.類別編號,
                    編輯者 = item.編輯者,
                    修改日 = Sys.StringConverDateTimeString(item.修改日),
                    建立日 = Sys.StringConverDateTimeString(item.建立日)
                });
            }
            vm.Product = product;
            return View(vm);
        }

        [Authorize]
        public ActionResult Create()
        {
            string uid = User.Identity.Name;
            string Permission = db.會員.Where(m => m.帳號 == uid).FirstOrDefault().權限;
            if (!Permission.Contains("C"))
            {
                return RedirectToAction("Index", "PermissionErrorMsg", new { msg = "您的身份無新增的權限" });
            }
            ViewBag.Category = db.產品類別.ToList();
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(string 產品編號, string 品名, int 單價, HttpPostedFileBase fImg, int 類別編號)
        {
            var tempProduct = db.產品資料.Where(m => m.產品編號 == 產品編號).FirstOrDefault();
            if (tempProduct != null)
            {
                ViewBag.IsProduct = true;
                ViewBag.Category = db.產品類別.ToList();
                return View();
            }

            string fileName = "question.png";
            if (fImg != null)
            {
                if (fImg.ContentLength > 0)
                {
                    fileName = Guid.NewGuid().ToString() + ".jpg";
                    var path = string.Format("{0}/{1}", Server.MapPath("~/Images"), fileName);
                    fImg.SaveAs(path);
                }
            }

            string editdate = DateTime.Now.ToString("yyyyMMddHHmmss");
            產品資料 product = new 產品資料();
            product.產品編號 = 產品編號;
            product.品名 = 品名;
            product.單價 = 單價;
            product.圖示 = fileName;
            product.類別編號 = 類別編號;
            product.編輯者 = User.Identity.Name;
            product.建立日 = editdate;
            product.修改日 = editdate;
            db.產品資料.Add(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Delete(string pid)
        {
            string uid = User.Identity.Name;
            string Permission = db.會員.Where(m => m.帳號 == uid).FirstOrDefault().權限;
            if (!Permission.Contains("D"))
            {
                return RedirectToAction("Index", "PermissionErrorMsg", new { msg = "您的身份無刪除的權限" });
            }
            var product = db.產品資料.Where(m => m.產品編號 == pid).FirstOrDefault();
            var filename = product.圖示;
            if (filename != "question.png")
            {
                System.IO.File.Delete
                (string.Format("{0}/{1}", Server.MapPath("~/Images"), filename));
            }
            db.產品資料.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [Authorize]
        public ActionResult Edit(string pid)
        {
            string uid = User.Identity.Name;
            string Permission = db.會員.Where(m => m.帳號 == uid).FirstOrDefault().權限;
            if (!Permission.Contains("U"))
            {
                return RedirectToAction("Index", "PermissionErrorMsg", new { msg = "您的身份無編輯的權限" });
            }
            var product = db.產品資料.Where(m => m.產品編號 == pid).FirstOrDefault();
            ViewBag.Category = db.產品類別.ToList();
            return View(product);
        }


        [Authorize]
        [HttpPost]
        public ActionResult Edit(string 產品編號, string 品名, int 單價, HttpPostedFileBase fImg, string 圖示, int 類別編號)
        {

            string fileName = "";
            if (fImg != null)
            {
                if (fImg.ContentLength > 0)
                {
                    fileName = Guid.NewGuid().ToString() + ".jpg";
                    var path = string.Format("{0}/{1}", Server.MapPath("~/Images"), fileName);
                    fImg.SaveAs(path);
                }
            }
            else
            {
                fileName = 圖示; //若無上傳圖，則使用hidden隱藏欄位的舊檔名
            }

            string editdate = DateTime.Now.ToString("yyyyMMddHHmmss");
            var product = db.產品資料.Where(m => m.產品編號 == 產品編號)
            .FirstOrDefault();
            product.品名 = 品名;
            product.單價 = 單價;
            product.圖示 = fileName;
            product.類別編號 = 類別編號;
            product.編輯者 = User.Identity.Name;
            product.修改日 = editdate;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}