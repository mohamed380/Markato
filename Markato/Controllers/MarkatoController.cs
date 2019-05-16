using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Markato.Controllers
{
    public class MarkatoController : Controller
    {
        // GET: Markato
        public ActionResult Index()
        {
            Session.Abandon();
            return View();
        }
        public ActionResult SignUp()
        {
            return View();
        }
        public ActionResult Signin(String email, String password)
        {
            return this.RedirectToAction("SignIn", "MainDirectors", new { mail = email, pass = password });
        }
    }
}