using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Day03.Models;
using System.IO;

namespace Day03.Controllers
{
    public class Sample01Controller : Controller
    {
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(HttpPostedFileBase myfile)
        {
            string filename = "";
            if(myfile != null)
            {
                if (myfile.ContentLength > 0) 
                {
                    filename = Path.GetFileName(myfile.FileName);
                    string path = string.Format("{0}/{1}", Server.MapPath("~/images"), filename);
                    Console.WriteLine(path);
                    myfile.SaveAs(path);
                }
                
            }
            return RedirectToAction("ShowPhotos");
        }
        public string ShowPhotos()
        {
            string show = "";
            DirectoryInfo dir = new DirectoryInfo(Server.MapPath("~/images"));
            FileInfo[] fInfo = dir.GetFiles();
            foreach(FileInfo f in fInfo)
            {
                show += string.Format("<img src='../images/{0}' width='500' height='500' > ", f.Name);
            }
            show += "<hr>";
            show += "<a href='Create'>返回</a>";
            Console.WriteLine(show);
            return show;
        }
    }
}