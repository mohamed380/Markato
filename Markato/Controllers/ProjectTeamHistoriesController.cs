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
    public class ProjectTeamHistoriesController : Controller
    {
        private MarkatoEntities7 db = new MarkatoEntities7();

        // GET: ProjectTeamHistories
        public ActionResult Index()
        {
            var projectTeamHistories = db.ProjectTeamHistories.Include(p => p.Employee).Include(p => p.Team);
            return View(projectTeamHistories.ToList());
        }
        [System.Web.Services.WebMethod]
        public int Request(int tid , int eid)
        {
            var r = db.ProjectTeamHistories.Where(t => t.MemberID == eid && t.TeamID == tid).FirstOrDefault();
            r.Req = 0;
            db.Entry(r).State = EntityState.Modified;
            db.SaveChanges();
            return 1;
        }
        public ProjectsInfo GetProjects(int MTID)
        {
            var t = db.ProjectTeamHistories.Where(m => m.MemberID == MTID).Include(m => m.Team).ToList();
            List<Project> pro = new List<Project>();
            List<Team> team = new List<Team>();
           foreach( var v in t)
            {
                pro.Add(v.Team.Project);
                team.Add(v.Team);
            }
            var pi = new ProjectsInfo() { projects = pro, Teams = team };
            return pi;
        }
        [System.Web.Services.WebMethod]
        public int TAccProject(int Eid,int Pid,int Tid)
        {
            var id  = db.ProjectTeamHistories.ToList().Count() + 1;
            var m = new ProjectTeamHistory() {ID =id,TeamID =Tid,MemberID = Eid,Req = 1};
            db.ProjectTeamHistories.Add(m);
            db.SaveChanges();
            var x = new RequestsController();
            x.Del(Pid,Eid);
            return 1;
        }
        [System.Web.Services.WebMethod]
        public int RemoveReq(int TeamID, int id)
        {
            var m = db.ProjectTeamHistories.Where(t => t.MemberID == id && t.TeamID == TeamID).FirstOrDefault();
            m.Req = 1;
            db.Entry(m).State = EntityState.Modified;
            db.SaveChanges();
            return 1;
        }















        // GET: ProjectTeamHistories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTeamHistory projectTeamHistory = db.ProjectTeamHistories.Find(id);
            if (projectTeamHistory == null)
            {
                return HttpNotFound();
            }
            return View(projectTeamHistory);
        }

        // GET: ProjectTeamHistories/Create
        public ActionResult Create()
        {
            ViewBag.MemberID = new SelectList(db.Employees, "EmployeeID", "EmployeeFName");
            ViewBag.TeamID = new SelectList(db.Teams, "TeamID", "TeamID");
            return View();
        }

        // POST: ProjectTeamHistories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MemberID,TeamID,ID")] ProjectTeamHistory projectTeamHistory)
        {
            if (ModelState.IsValid)
            {
                db.ProjectTeamHistories.Add(projectTeamHistory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MemberID = new SelectList(db.Employees, "EmployeeID", "EmployeeFName", projectTeamHistory.MemberID);
            ViewBag.TeamID = new SelectList(db.Teams, "TeamID", "TeamID", projectTeamHistory.TeamID);
            return View(projectTeamHistory);
        }

        // GET: ProjectTeamHistories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTeamHistory projectTeamHistory = db.ProjectTeamHistories.Find(id);
            if (projectTeamHistory == null)
            {
                return HttpNotFound();
            }
            ViewBag.MemberID = new SelectList(db.Employees, "EmployeeID", "EmployeeFName", projectTeamHistory.MemberID);
            ViewBag.TeamID = new SelectList(db.Teams, "TeamID", "TeamID", projectTeamHistory.TeamID);
            return View(projectTeamHistory);
        }

        // POST: ProjectTeamHistories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MemberID,TeamID,ID")] ProjectTeamHistory projectTeamHistory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectTeamHistory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MemberID = new SelectList(db.Employees, "EmployeeID", "EmployeeFName", projectTeamHistory.MemberID);
            ViewBag.TeamID = new SelectList(db.Teams, "TeamID", "TeamID", projectTeamHistory.TeamID);
            return View(projectTeamHistory);
        }

        // GET: ProjectTeamHistories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTeamHistory projectTeamHistory = db.ProjectTeamHistories.Find(id);
            if (projectTeamHistory == null)
            {
                return HttpNotFound();
            }
            return View(projectTeamHistory);
        }

        // POST: ProjectTeamHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProjectTeamHistory projectTeamHistory = db.ProjectTeamHistories.Find(id);
            db.ProjectTeamHistories.Remove(projectTeamHistory);
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
