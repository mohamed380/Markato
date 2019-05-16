using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Markato.Models;

namespace Markato.Controllers
{
    public class EmployeesController : Controller
    {
        public MarkatoEntities7 db = new MarkatoEntities7();
        public ActionResult SignIn(string mail,string pass)
        {
            var emp = db.Employees.Where(y => y.EmployeeEmail.Equals(mail) && y.pass.Equals(pass) && y.ban ==1).FirstOrDefault();
            if (emp == null)
            {
                return this.RedirectToAction("SignIn", "Customers", new { mail = mail, pass = pass });
            }
            Session["emp"] = null;
            Session["emp"] = emp;
            var ce = (Employee)(Session["emp"]);
            if (ce.JID == 4)
            {
                Session["JID"] = ce.JID;
                return this.RedirectToAction("SecondaryUsersProfile", "SecondaryUsers");
            }
            return this.RedirectToAction("MainDirectorsPage", "MainDirectors");
        }
        public Employee find(string mail)
        {
           return db.Employees.Where(e => e.EmployeeEmail == mail).FirstOrDefault();
        }
       
        public Employee Details(int? id)
        {
            if (id == null)
            {
                return null;
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return null;
            }
            return employee;
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            ViewBag.EmployeeManagerID = new SelectList(db.Employees, "EmployeeID", "EmployeeFName");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "JID,EmployeeManagerID,EmployeeFName,EmployeeLName,EmployeeMobile,EmployeeEmail,EmployeePhoto,pass,EmployeeBio")] Employee employee)
        {
            employee.EmployeeID = db.Employees.ToList().Count() + 1;
            employee.ban= 1;
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeManagerID = new SelectList(db.Employees, "EmployeeID", "EmployeeFName", employee.EmployeeManagerID);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeManagerID = new SelectList(db.Employees, "EmployeeID", "EmployeeFName", employee.EmployeeManagerID);
            return View(employee);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeID,EmployeeManagerID,EmployeeFName,EmployeeLName,EmployeeMobile,EmployeeEmail,EmployeePhoto,pass,EmployeeBio,JobTitle,ban")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeManagerID = new SelectList(db.Employees, "EmployeeID", "EmployeeFName", employee.EmployeeManagerID);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
