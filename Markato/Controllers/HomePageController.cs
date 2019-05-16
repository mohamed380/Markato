using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Markato.Controllers
{
    public class HomePageController : Controller
    {
        // GET: HomePage
        public ActionResult HomePage()
        {
            var p = new ProjectsController();


            return View(p.Pending());
        }
    }
}