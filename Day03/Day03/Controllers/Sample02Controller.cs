using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
namespace Day03.Controllers
{
    public class Sample02Controller : Controller
    {
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(HttpPostedFileBase[]  myfiles)
        {
            if (myfiles.Count() > 0)
            {
                foreach (var myfile in myfiles)
                {
                    if (myfile.ContentLength > 0)
                    {
                        string savePath = Server.MapPath("~/images/");
                        myfile.SaveAs(savePath + myfile.FileName);
                    }
                }
            }
            return RedirectToAction("ShowPhotos");
        }
        public string ShowPhotos()
        {
            string show = "";
            DirectoryInfo dir = new DirectoryInfo(Server.MapPath("~/images"));
            FileInfo[] fInfo = dir.GetFiles();
            foreach (FileInfo f in fInfo)
            {
                show += string.Format("<img src='../images/{0}' width='500' height='500' > ", f.Name);
            }
            show += "<hr>";
            show += "<a href='Create'>返回</a>";
            return show;
        }
    }
}