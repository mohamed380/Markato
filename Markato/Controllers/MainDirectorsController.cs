using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Markato.Models;
namespace Markato.Controllers 
{
    public class MainDirectorsController : EmployeesController
    {
        public ActionResult MainDirectorsPage()
        {
            if (Session["emp"] == null)
            {
                return RedirectToAction("Index", "Markato");
            }
            var CurrentEmp = (Employee)Session["emp"];
            
            
            ViewData["MainID"] = CurrentEmp.JID;
            Session["JID"] = CurrentEmp.JID;
            var c = new ProjectsController();
            var r = new RequestsController();
            List<Project> Rprojects;
            Rprojects = r.getRequests(CurrentEmp.EmployeeID);
            ProjectsInfo pi;
            
            if (CurrentEmp.JID== 1)
            {
                 pi = c.GetProjects(null);
                return View(pi);
            }else if (CurrentEmp.JID == 2)
            {
                pi = c.GetProjects(CurrentEmp.EmployeeID);
                pi.Rprojects = Rprojects;
                return View(pi);
            }
            else if (CurrentEmp.JID == 3)
            {
                var t = new TeamsController();
                pi = t.GetLeaderProjects(CurrentEmp.EmployeeID);
                pi.Rprojects = Rprojects;
                return View(pi);
            }
            

            return HttpNotFound();
        }
        public ActionResult EmployeeProfile(int EmpID)
        {
            var Emp = Details(EmpID);
            
            var c = new ProjectsController();
            var pi = new ProjectsInfo();
            
             if (Emp.JID == 2)
            {
                pi = c.GetProjects(Emp.EmployeeID);
                pi.Emp = Emp;
                return View(pi);
            }
            else if (Emp.JID == 3)
            {
                var t = new TeamsController();
                pi = t.GetLeaderProjects(EmpID);
                pi.Emp = Emp;
                return View(pi);
            }
            else if (Emp.JID == 4)
            {
                var pth = new ProjectTeamHistoriesController();
                pi = pth.GetProjects(Emp.EmployeeID);
                pi.Emp = Emp;
                return View(pi);
            }


            return HttpNotFound();
        }

        public ActionResult GetAAP()
        {
            return RedirectToAction("GetAP", "Projects");
        }
       


    }
}