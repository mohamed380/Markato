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
    public class RequestsController : Controller
    {
        private MarkatoEntities7 db = new MarkatoEntities7();

        // GET: Requests
        public ActionResult Index()
        {
            var requests = db.Requests.Include(r => r.Employee).Include(r => r.Project);
            return View(requests.ToList());
        }
        [System.Web.Services.WebMethod]
        public int send(int pid, string EmpMail)
        {
            var c =new EmployeesController();
            var emp = c.find(EmpMail);
            if(emp != null)
            {
                var r = new Request();
                r.projectID = pid;
                r.employeeID=emp.EmployeeID;
                db.Requests.Add(r);
                db.SaveChanges();
                return 1;
            }
            return 0;
        }
        [System.Web.Services.WebMethod]
        public int SendRequest(int pid,int mdid)
        {
            var req = db.Requests.Where(r => r.employeeID == mdid && r.projectID == pid).FirstOrDefault();
            req.sr = 0;
            DelMD(pid ,mdid);
            db.Entry(req).State = EntityState.Modified;
            db.SaveChanges();
            return 1;
        }
        [System.Web.Services.WebMethod]
        public int MDSendRequest(int pid, int mdid)
        {
            var id = db.Requests.ToList().Count() + 1;
            var req = new Request() { id = id, employeeID = mdid, sr = 1, projectID = pid };
            db.Requests.Add(req);
            db.SaveChanges();
            return 1;
        }
        public int DelMD(int Pid ,int mdid)
        {
            var req = db.Requests.Where(r=> r.projectID == Pid && r.employeeID!= mdid);
            foreach (var r in req)
            {
                db.Requests.Remove(r);
            }
            db.SaveChanges();
            return 1;
        }
        public int Del(int Pid, int Eid)
        {
            var req = db.Requests.Where(r => r.projectID == Pid && r.employeeID == Eid);
            foreach (var r in req)
            {
                db.Requests.Remove(r);
            }
            db.SaveChanges();
            return 1;
        }
        public List<Markato.Models.Project> getRequests(int Eid)
        {
            var Requests = db.Requests.Where(r => r.employeeID == Eid).Include(r => (r.Project).Customer).Include(r=> (r.Project).Teams).ToList();
            List<Project> projects = new List<Project> { };
            foreach (var r in Requests)
            {
                projects.Add(r.Project);
            }
            return projects;
        }
        // GET: Requests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // GET: Requests/Create
        public ActionResult Create()
        {
            ViewBag.employeeID = new SelectList(db.Employees, "EmployeeID", "EmployeeFName");
            ViewBag.MDID = new SelectList(db.Employees, "EmployeeID", "EmployeeFName");
            ViewBag.projectID = new SelectList(db.Projects, "projectID", "projectName");
            return View();
        }

        // POST: Requests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,projectID,MDID,employeeID")] Request request)
        {
            if (ModelState.IsValid)
            {
                db.Requests.Add(request);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.employeeID = new SelectList(db.Employees, "EmployeeID", "EmployeeFName", request.employeeID);
            ViewBag.MDID = new SelectList(db.Employees, "EmployeeID");
            ViewBag.projectID = new SelectList(db.Projects, "projectID", "projectName", request.projectID);
            return View(request);
        }

        // GET: Requests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            ViewBag.employeeID = new SelectList(db.Employees, "EmployeeID", "EmployeeFName", request.employeeID);
            ViewBag.MDID = new SelectList(db.Employees, "EmployeeID");
            ViewBag.projectID = new SelectList(db.Projects, "projectID", "projectName", request.projectID);
            return View(request);
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,projectID,MDID,employeeID")] Request request)
        {
            if (ModelState.IsValid)
            {
                db.Entry(request).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.employeeID = new SelectList(db.Employees, "EmployeeID", "EmployeeFName", request.employeeID);
            ViewBag.MDID = new SelectList(db.Employees, "EmployeeID");
            ViewBag.projectID = new SelectList(db.Projects, "projectID", "projectName", request.projectID);
            return View(request);
        }

        // GET: Requests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Request request = db.Requests.Find(id);
            db.Requests.Remove(request);
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
