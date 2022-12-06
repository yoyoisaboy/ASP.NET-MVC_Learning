using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Day05.Models;
using PagedList;

namespace Day05.Controllers
{
    public class HomeController : Controller
    {
        List<SelectListItem> GetDepartment() //GetDepartment方法，回傳一個 List 下拉式清單
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Text = "資訊",
                Value = "1",
                Selected = true
            });
            list.Add(new SelectListItem()
            {
                Text = "設計",
                Value = "2",
                Selected = true
            });
            list.Add(new SelectListItem()
            {
                Text = "會計",
                Value = "3",
                Selected = true
            });
            return list;
        }
        public ActionResult Create()
        {
            ViewBag.Department = GetDepartment();
            return View();
        }
        [HttpPost] //按下submit時，接收資料
        public ActionResult Create(Employee emp)
        {
            ViewBag.Department = GetDepartment();
            SelectListItem empdep = GetDepartment().Where(m => m.Value == emp.Department.ToString()).FirstOrDefault(); //注意型態是字串還是整數，下一行empdep.Text or emp.Department 比較看看
            ViewBag.Show = string.Format("部門 : {0}<br>姓名 : {1}<br>信箱 : {2}<br>薪資 : {3}<br>", empdep.Text, emp.Name, emp.Email, emp.Salary);
            return View(emp);
        }
        // GET: Home
        public ActionResult Index(int page=1)
        {
            int pagesize = 3; //顯示3張圖
            int pagecurrent = page < 1 ? 1 : page; // ? True : False 如果page<1則1，不是則是page
            string[] id = new string[] { "A01", "A02", "A03", "A04", "A05", "A06", "A07" };
            string[] name = new string[]
            {
                "花色","玻璃","變形蟲","曼陀羅","夜晚","蛇皮","彩色幾何"
            };
            int[] price = new int[] { 100,200,300,400,500,600,700 };
            List<Flower> list = new List<Flower>(); //Models的Flower
            for(var i = 0;i < id.Length; i++)
            {
                Flower flow = new Flower();
                flow.Id = id[i];
                flow.Name = name[i];
                flow.Price = price[i];
                list.Add(flow);
            }
            var pagedlist = list.ToPagedList(pagecurrent, pagesize); //(顯示第幾頁,顯示幾筆)
            return View("Index", "_Layout", pagedlist); //("內容頁面View","版面配置頁View",Model)
        }

        public ActionResult Index02(int page = 1)
        {
            int pagesize = 3; //顯示3張圖
            int pagecurrent = page < 1 ? 1 : page; // ? True : False 如果page<1則1，不是則是page
            string[] id = new string[] { "A01", "A02", "A03", "A04", "A05", "A06", "A07" };
            string[] name = new string[]
            {
                "花色","玻璃","變形蟲","曼陀羅","夜晚","蛇皮","彩色幾何"
            };
            int[] price = new int[] { 100, 200, 300, 400, 500, 600, 700 };
            List<Flower> list = new List<Flower>(); //Models的Flower
            for (var i = 0; i < id.Length; i++)
            {
                Flower flow = new Flower();
                flow.Id = id[i];
                flow.Name = name[i];
                flow.Price = price[i];
                list.Add(flow);
            }
            var pagedlist = list.ToPagedList(pagecurrent, pagesize); //(顯示第幾頁,顯示幾筆)
            return View("Index", "_Layout02", pagedlist); //("內容頁面View","版面配置頁View",Model)
        }
    }
}