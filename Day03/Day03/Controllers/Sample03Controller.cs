using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Day03.Controllers
{
    public class Sample03Controller : Controller
    {
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection forms)
        {

            ViewBag.Id = forms["Id"];
            ViewBag.Name = forms["Name"];
            ViewBag.Size = forms["Size"];
            return View();
        }
    }
}