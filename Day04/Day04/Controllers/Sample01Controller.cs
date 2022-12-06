using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Day04.Models;
namespace Day04.Controllers
{
    public class Sample01Controller : Controller
    {
        // GET: Sample01
        public ActionResult Index()
        {
            List<Book> list = new List<Book>();
            list.Add(new Book() { Id = "AWL020600",Name="I love you",Price=520});
            list.Add(new Book() { Id = "AWL020601", Name = "I hate you", Price = 340 });
            list.Add(new Book() { Id = "AWL020602", Name = "boring day boring life", Price = 1000 });
            return View(list);
        }
    }
}