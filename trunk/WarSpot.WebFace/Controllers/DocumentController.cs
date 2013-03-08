using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using System.IO;
//using ProjectBase.Tools.Wiki;

namespace WarSpot.WebFace.Controllers
{
    public class DocumentController : Controller
    {
        //
        // GET: /Document/

        public ActionResult Index()
        {
            //System.Reflection.Assembly a = System.Reflection.Assembly.GetEntryAssembly();
            //string baseDir = System.IO.Path.GetDirectoryName(a.Location);
            //StreamReader sr = new StreamReader(System.Environment.CurrentDirectory + "/wiki/EULA.wiki");

            //return View(model: (new WikiConverter()).ConvertToHtml(sr.ReadToEnd()));

            return View();
        }

    }
}
