using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Day03.Models;
namespace Day03.Controllers
{
    public class Sample04Controller : Controller
    {
        // GET: Sample04
        public ActionResult ShowMember()
        {
            List<MemberList> list = new List<MemberList>();
            list.Add( new MemberList() { Id="1",Name="yoyo",Size=180});
            list.Add(new MemberList() { Id = "2", Name = "miku", Size = 166 });
            list.Add(new MemberList() { Id = "3", Name = "dinter", Size = 175 });
            return View(list);
        }

        // GET: Sample04
        public ActionResult ShowMemberForViewBag()
        {
            List<MemberList> list = new List<MemberList>();
            list.Add(new MemberList() { Id = "1", Name = "yoyo", Size = 180 });
            list.Add(new MemberList() { Id = "2", Name = "miku", Size = 166 });
            list.Add(new MemberList() { Id = "3", Name = "dinter", Size = 175 });
            ViewBag.MemberList = list;
            return View();
        }
    }
}