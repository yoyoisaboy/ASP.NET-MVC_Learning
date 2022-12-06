using Day04.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Day04.Controllers
{
    public class Sample07Controller : Controller
    {
        // GET: Sample07
        public ActionResult Index(int episode = 0)
        {
            List<Movie> list = new List<Movie>();
            list.Add(new Movie() { Id = "EP62gl-sj2I", Name = "JoJo" });
            list.Add(new Movie() { Id = "tfwatFtgWPY", Name = "Yu-Gi-Oh" });
            list.Add(new Movie() { Id = "YsfFKKZOLqw", Name = "YOYOman" });
            ViewBag.MovieId = list[episode].Id;
            return View(list);
        }
    }
}