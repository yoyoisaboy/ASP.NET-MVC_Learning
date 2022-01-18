using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using hackMD_MVCdemo.Models;
namespace hackMD_MVCdemo.Controllers
{
    
    public class HomeController : Controller
    {
        dbMemberEntities db = new dbMemberEntities();
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.Isay = "hello".ToString();
            return RedirectToAction("ShowdbMember");
        }

        public ActionResult ShowMember()
        {
            //陣列物件
            CMember[] emps = new CMember[]
            {
                //塞資料
                new CMember(){Id=1,Name="yoyo"},
                new CMember(){Id=2,Name="miku"}

            };
            return View(emps.ToList());
        }

        public ActionResult ShowdbMember()
        {
            //Id由大排到小, t 是隨便取
            var emps = db.tMember.OrderByDescending(t => t.Id).ToList();

            return View(emps);
        }
        //Create
        public ActionResult ShowCreat()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(string Name,string Phone,int Age)
        {
            tMember dbmember = new tMember();
            dbmember.Name = Name;
            dbmember.Phone = Phone;
            dbmember.Age = Age;
            db.tMember.Add(dbmember);
            db.SaveChanges();
            return RedirectToAction("ShowdbMember");
        }
        //Edit
        public ActionResult ShowEdit(int Id)
        {
            var emp = db.tMember.Where(m => m.Id == Id).FirstOrDefault(); //讀資料庫特定Id的欄位資料
            return View(emp);
        }
        [HttpPost]
        public ActionResult Edit(int Id,string Name, string Phone, int Age)
        {
            var emp = db.tMember.Where(m => m.Id == Id).FirstOrDefault(); //讀資料庫特定Id的欄位資料
            emp.Id = Id;
            emp.Name = Name;
            emp.Phone = Phone;
            emp.Age = Age;
            db.SaveChanges();
            return RedirectToAction("ShowdbMember");
        }
        //Delete
        public ActionResult Delete(int Id)
        {
            var emp = db.tMember.Where(m => m.Id == Id).FirstOrDefault();
            db.tMember.Remove(emp);
            db.SaveChanges();
            return RedirectToAction("ShowdbMember");
        }
    }
}