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
    public class CustomersController : Controller
    {
        private MarkatoEntities7 db = new MarkatoEntities7();

        public ActionResult SignIn(string mail, string pass)
        {
            var Customer = db.Customers.Where(y => y.CustomerEmail.Equals(mail) && y.pass.Equals(pass) && y.ban == 1).FirstOrDefault();
            if (Customer == null)
            {
                return this.RedirectToAction("Index", "Markato");
            }
            Session["Customer"] = null;
            Session["Customer"] = Customer;
            return this.RedirectToAction("SecondaryUsersProfile", "SecondaryUsers");
        }
        public ProjectsInfo GetProjects(int CID)
        {
            var projects = db.Projects.Where(p => p.projectOwnerID == CID).ToList();
            
            List<Team> teams = new List<Team> { };
            List<List<Request>> reqs = new List<List<Request>> { };
            foreach (var p in projects)
            {
                var team = db.Teams.Where(t => t.ProjectID == p.projectID).Include(t => t.ProjectTeamHistories).FirstOrDefault();
                reqs.Add(p.Requests.ToList());
                teams.Add(team);
            }
            ProjectsInfo pi = new ProjectsInfo { projects = projects, Teams = teams,Rprojects = null,Prequests =reqs };
            return pi;

        }
        public int SendRequest(int pid,int mdid)
        {
            var r = new RequestsController();
            return r.SendRequest(pid, mdid);
        }
        [System.Web.Services.WebMethod]
        public ActionResult Register([Bind(Include = "CustomerFName,CustomerLName,CustomerEmail,CustomerPhone,pass")] Customer customer)
        {
            customer.ban = 1;
            var id = db.Customers.ToList().Count() + 7;
            customer.CustomerID = id;

            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("SUlogin","Markato");
            }

            return RedirectToAction("Signin", "Markato");
        }
        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomerID,CustomerFName,CustomerLName,CustomerEmail,CustomerPhone,CustomerPhoto,pass,ban")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerID,CustomerFName,CustomerLName,CustomerEmail,CustomerPhone,CustomerPhoto,pass,ban")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
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
