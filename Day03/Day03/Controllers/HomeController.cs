using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Day03.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(string Id,string Name,int Size)
        {
            ViewBag.Id = Id;
            ViewBag.Name = Name;
            ViewBag.Size = Size;
            return View();
        }

    }
}