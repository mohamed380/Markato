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
    public class TeamsController : Controller
    {
        private MarkatoEntities7 db = new MarkatoEntities7();

        // GET: Teams
        public ActionResult Index()
        {
            var teams = db.Teams.Include(t => t.Employee).Include(t => t.Employee1).Include(t => t.Project);
            return View(teams.ToList());
        }
        public ProjectsInfo GetLeaderProjects(int id)

        {
            var p = new ProjectsController();
            var teams = db.Teams.Where(t => t.TeamLeaderID == id).Include(t=> t.ProjectTeamHistories).ToList();
            List<Project> pro = new List<Project> { };
            foreach (var t in teams)
            { 
            pro.Add(p.Find((int)t.ProjectID));
            }
            
            ProjectsInfo pi = new ProjectsInfo { projects = pro, Teams = teams };
            return pi;
        }
        public int LeaderAccProject(int Eid, int Pid)
        {
            var x = new RequestsController();
            var Team = db.Teams.Where(t => t.ProjectID == Pid).FirstOrDefault();
            if(Team == null)
            {
                var p = db.Projects.Where(pro => pro.projectID == Pid).FirstOrDefault();
                int id = db.Teams.ToList().Count()+1;

                var team = new Team() { ProjectID = Pid, TeamMDID = (int)p.projectMDID, TeamLeaderID = Eid,TeamID=id };
                db.Teams.Add(team);
                x.Del(Pid, Eid);
                db.SaveChanges();
                return 1;
            }
            Team.TeamLeaderID = Eid;
            db.Entry(Team).State = EntityState.Modified;
            x.Del(Pid, Eid);
            db.SaveChanges();
            return 1;
        }
        public int LeaveProject(int pid)
        {
            var p = db.Teams.Where(t => t.ProjectID == pid).FirstOrDefault();
            p.TeamLeaderID = null;
            db.Entry(p).State = EntityState.Modified;
            db.SaveChanges();
            return 1;
        }
//======================================================================================================
        // GET: Teams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }
       /* public void RemoveML(int ProjectId)
        {
            var team = db.Teams.Where(t=> t.ProjectID == ProjectId).FirstOrDefault();
            team.TeamLeaderID = null;
            db.Entry(pr).State = EntityState.Modified;
            db.SaveChanges();
        }*/

        // GET: Teams/Create
        public ActionResult Create()
        {
            ViewBag.TeamLeaderID = new SelectList(db.Employees, "EmployeeID", "EmployeeFName");
            ViewBag.TeamMDID = new SelectList(db.Employees, "EmployeeID", "EmployeeFName");
            ViewBag.ProjectID = new SelectList(db.Projects, "projectID", "projectName");
            return View();
        }

        // POST: Teams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TeamID,TeamMDID,TeamLeaderID,ProjectID")] Team team)
        {
            if (ModelState.IsValid)
            {
                db.Teams.Add(team);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TeamLeaderID = new SelectList(db.Employees, "EmployeeID", "EmployeeFName", team.TeamLeaderID);
            ViewBag.TeamMDID = new SelectList(db.Employees, "EmployeeID", "EmployeeFName", team.TeamMDID);
            ViewBag.ProjectID = new SelectList(db.Projects, "projectID", "projectName", team.ProjectID);
            return View(team);
        }

        // GET: Teams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            ViewBag.TeamLeaderID = new SelectList(db.Employees, "EmployeeID", "EmployeeFName", team.TeamLeaderID);
            ViewBag.TeamMDID = new SelectList(db.Employees, "EmployeeID", "EmployeeFName", team.TeamMDID);
            ViewBag.ProjectID = new SelectList(db.Projects, "projectID", "projectName", team.ProjectID);
            return View(team);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TeamID,TeamMDID,TeamLeaderID,ProjectID")] Team team)
        {
            if (ModelState.IsValid)
            {
                db.Entry(team).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TeamLeaderID = new SelectList(db.Employees, "EmployeeID", "EmployeeFName", team.TeamLeaderID);
            ViewBag.TeamMDID = new SelectList(db.Employees, "EmployeeID", "EmployeeFName", team.TeamMDID);
            ViewBag.ProjectID = new SelectList(db.Projects, "projectID", "projectName", team.ProjectID);
            return View(team);
        }

        // GET: Teams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Team team = db.Teams.Find(id);
            db.Teams.Remove(team);
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
