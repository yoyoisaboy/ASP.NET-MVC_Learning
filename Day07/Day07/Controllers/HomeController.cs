using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Day07.Models;

namespace Day07.Controllers
{
    public class HomeController : Controller
    {
        dbEmpEntities db = new dbEmpEntities();
        // GET: Home
        public ActionResult Index()
        {

            return View(db.tEmployee.ToList());
        }
        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Create(tEmployee emp)
        {
            string empId = emp.fEmpId;
            var tempEmp = db.tEmployee.Where(m => m.fEmpId == empId).FirstOrDefault();
            
            if (tempEmp != null) //若為null代表無重複員工編號
            {
                ViewBag.Show = "same_fEmpId";
                return View(emp);
            }

            if (ModelState.IsValid)
            {
                db.tEmployee.Add(emp);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(emp);
        }

        public ActionResult Delete(int fId)
        {
            var emp = db.tEmployee.Where(m => m.fId == fId).FirstOrDefault();
            db.tEmployee.Remove(emp);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int fId)
        {
            var emp = db.tEmployee.Where(m => m.fId == fId).FirstOrDefault();
            return View(emp);
        }
        [HttpPost]
        public ActionResult Edit(tEmployee emp)
        {
            if (ModelState.IsValid)
            {
                db.Entry(emp).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(emp);
        }
    }
}