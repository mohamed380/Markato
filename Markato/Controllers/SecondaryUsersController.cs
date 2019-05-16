using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Markato.Models;

namespace Markato.Controllers
{
    public class SecondaryUsersController : CustomersController
    {
        // GET: SecondaryUsers
        public ActionResult SecondaryUsersProfile()
        {
            if (Session["Customer"] == null && Session["emp"] == null)
            {
                return RedirectToAction("SUlogin", "Markato");
            }
            else if (Session["Customer"] != null)
            {
                var CurrenCustomer = (Customer)(Session["Customer"]);
                var c = new CustomersController();
                var pi = c.GetProjects(CurrenCustomer.CustomerID);
                return View(pi);
            }
            
            var CurrentEmp = (Employee)(Session["emp"]);
            if(CurrentEmp.JID == 4)
            {
                var pth = new ProjectTeamHistoriesController();
                var pimt = pth.GetProjects(CurrentEmp.EmployeeID);
                var r = new RequestsController();
                List<Project> Rprojects;
                Rprojects = r.getRequests(CurrentEmp.EmployeeID);
                pimt.Rprojects = Rprojects;
                return View(pimt);
            }

            return RedirectToAction("Index", "Markato");
        }
    }
}