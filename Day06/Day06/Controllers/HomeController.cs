using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Day06.Models;

namespace Day06.Controllers
{
    public class HomeController : Controller
    {
        public string ShowProductGroup()
        {
            TestDataSetEntities db = new TestDataSetEntities();

            //LINQ查詢運算式寫法
            var result = from category in db.產品類別
                         join product in db.產品類別
                         on category.類別編號 equals product.類別編號 
                         into num
                         select new
                         {
                             類別名稱 = category.類別名稱, //定義新名稱
                             產品數量 = num.Count()
                         };
            string show = "";
            foreach (var m in result)
            {
                show += string.Format("{0}類別共{1}個產品<p>", m.類別名稱, m.產品數量);
            }
            return show;
        }
        public string ShowProduct()
        {
            TestDataSetEntities db = new TestDataSetEntities();
            //LINQ 擴充方法寫法
            var result = db.產品資料
                .Where(m=>m.單價>30)
                .OrderBy(m=>m.單價)
                .ThenByDescending(m=>m.庫存量);
            //LINQ查詢運算式寫法
            //var result = from m in db.產品資料
            //              where m.單價 > 30
            //              orderby m.單價 ascending, m.庫存量 descending
            //              select m;
            string show = "";
            foreach(var m in result)
            {
                show += "產品: " + m.產品 + "<br />";
                show += "單價: " + m.單價 + "<br />";
                show += "庫存: " + m.庫存量 + "<br />";
            }
            return show;
        }
        public string ShowProductInfo()
        {
            TestDataSetEntities db = new TestDataSetEntities();
            //LINQ 擴充方法寫法
            var reuslt = db.產品資料;
            //LINQ查詢運算式寫法
            //var result = from m in db.產品資料
            //              select m;
            string show = "";
            show += "單價平均: " + reuslt.Average(m => m.單價) + "<br />";
            show += "單價總和: " + reuslt.Sum(m => m.單價) + "<br />";
            show += "紀錄筆數: " + reuslt.Count() + "<br />";
            show += "單價最高: " + reuslt.Max(m => m.單價) + "<br />";
            show += "單價最低: " + reuslt.Min(m => m.單價) + "<br />";
            return show;
        }
        public string ShowCustomerByAddress(string keyword)
        {
            TestDataSetEntities db = new TestDataSetEntities();
            //LINQ擴充方法寫法
            var result = db.客戶.Where(m=>m.地址.Contains(keyword)); //Contains : 有找到True；反之False
            //LINQ查詢運算式寫法
            //var result = from m in db.客戶
            //              where m.地址.Contains(keyword)
            //              select m;
            string show = "";
            if (result == null) return "查無地址";
            foreach(var m in result)
            { 
                show += "公司名稱 : " + m.公司名稱 + "<br/>";
                show += "連絡人 連絡人職稱  : " + m.連絡人 + m.連絡人職稱 + "<br/>";
                show += "地址 : " + m.地址 + "<hr>";
            }
            return show;
        }
        public string ShowEmployee()
        {
            TestDataSetEntities db = new TestDataSetEntities();
            //LINQ擴充方法寫法
            var result = db.員工; // 取得 table 的內容
            //var result = from m in db.員工
            //              select m;
            string show = "";
            foreach(var m in result)
            {
                show += "編號 : " + m.員工編號 + "<br/>";
                show += "姓名 : " + m.姓名 + m.稱呼 + "<br/>";
                show += "職稱 : " + m.職稱 + "<hr>";
            }
            return show;
        }


        public string LoginMember(string uid,string pwd)
        {
            Member[] members = new Member[]
            {
                new Member{UId="tom",Pwd="123",Name="湯姆"},
                new Member{UId="jasper",Pwd="456",Name="賈斯柏"},
                new Member{UId="mary",Pwd="789",Name="馬力"}
            };
            //LINQ擴充方法
            var result = members.Where(m => m.UId == uid && m.Pwd == pwd).FirstOrDefault();
            //LINQ查詢方法
            //var result = (from m in members
            //              where m.UId == uid && m.Pwd == pwd
            //              select m).FirstOrDefault();
            string show = "";
            if (result != null)
            {
                show = result.Name + "歡迎";
            }
            else
            {
                show = "登入失敗";
            }
            return show;
        }
        public string ShowArray()
        {
            int[] score = new int[] { 78, 99, 85, 36, 56 };
            string show = "";
            //LINQ擴充方法
            var result = score.OrderByDescending(m => m);
            //LINQ查詢運算式寫法
            //var result = from m in score
            //              orderby m descending
            //              select m;
            show = "遞減排序: ";
            foreach(var m in result)
            {
                show += m + ",";
            }
            show += "<br />";
            show += "總和: " + result.Sum();
            show += " 平均: " + result.Average();
            return show;
        }
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
    }
}