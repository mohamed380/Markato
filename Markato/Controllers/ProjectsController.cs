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
    public class ProjectsController : Controller
    {
        private MarkatoEntities7 db = new MarkatoEntities7();

        // GET: Projects
        public ActionResult Index()
        {
            var projects = db.Projects.Include(p => p.Customer).Include(p => p.Employee);
            return View(projects.ToList());
        }
        //======================================================================================
        public List<Project> Pending()
        {
            return db.Projects.Where(p => p.projectStatues == "pending").ToList();
        }
        public ProjectsInfo GetProjects(int? id)
        {
            List<List<Request>> reqs = new List<List<Request>> { };
            List<Project> projects;
            if (id ==null)
            {
                 projects = db.Projects.Include(p => p.Customer).Include(p => p.Employee).Include(p=> p.Teams).ToList();
           
            foreach (var p in projects)
            {
                reqs.Add(p.Requests.ToList());  
            }
            }
            else
            {
                projects = db.Projects.Where(p=> p.projectMDID == id).Include(p => p.Customer).Include(p => p.Employee).ToList();
            }

            List<Team> teams = new List<Team> { };
            foreach (var p in projects)
            {
                var team = db.Teams.Where(t => t.ProjectID == p.projectID).Include(t => t.ProjectTeamHistories).FirstOrDefault();
                if (team != null)
                {

                    foreach (var e in team.ProjectTeamHistories.ToList())
                    {   if (e.Employee.FeedBacks.ToList().Count() > 0)
                        {
                            var x = e.Employee.FeedBacks.FirstOrDefault().Rate;
                        }
                    }
                }
                teams.Add(team);
            }

            ProjectsInfo pi = new ProjectsInfo { projects = projects, Teams = teams,Prequests = reqs };
            return pi;
        }
        public Project Find(int id)
        {
            return db.Projects.Where(p => p.projectID == id).FirstOrDefault();
        }
        [System.Web.Services.WebMethod]
        public int MDAccProject(int Eid,int Pid)
        {
            var project = db.Projects.Where(p => p.projectID == Pid).FirstOrDefault();
            project.projectMDID = Eid;
            var d = DateTime.Today;
            var x = d.Date.ToString("dd/MM/yyyy");
            project.ProjectStart = x;
            project.projectStatues = "inprogress";
            db.Entry(project).State = EntityState.Modified;
           var req =  db.Requests.Where(r => r.employeeID == Eid && r.projectID == Pid).FirstOrDefault();
            db.Requests.Remove(req);
            db.SaveChanges();
            return 1;
        } [System.Web.Services.WebMethod]
        public int EditProject(int Pid,String Desc,String Name)
        {
            var project = db.Projects.Where(p => p.projectID == Pid).FirstOrDefault();
            project.projectDesc = Desc;
            project.projectName = Name;
            db.Entry(project).State = EntityState.Modified;
            db.SaveChanges();
            return 1;
        }
        [System.Web.Services.WebMethod]
        public int RemoveML(int ProjectId)
        {
            var pr = db.Projects.Where(p => p.projectID == ProjectId).FirstOrDefault();
            var team = db.Teams.Where(t => t.ProjectID == pr.projectID).FirstOrDefault();
            team.TeamLeaderID = null;
            db.Entry(team).State = EntityState.Modified;
            db.SaveChanges();
            return 1;
        }
        [System.Web.Services.WebMethod]
        public int RemoveMT(int TeamID, int id)
        {
            var teamMembers = db.ProjectTeamHistories.Where(t => t.TeamID == TeamID).ToList();
            foreach (var m in teamMembers)
            {
                if (m.Employee.EmployeeID == id)
                {
                    db.ProjectTeamHistories.Remove(m);
                    db.SaveChanges();
                    return 1;
                }
            }
            return 0;
            
        }
        [System.Web.Services.WebMethod]
        public int LeaveProject(int pid)
        {
            var p = db.Projects.Find(pid);
            p.projectMDID = null;
            p.projectStatues = "pending";
            db.Entry(p).State = EntityState.Modified;
            db.SaveChanges();
            return 1;
        }
        [System.Web.Services.WebMethod]
        public int SetSchedual(int projectid, string pe)
        {
            
            var p = db.Projects.Where(pr => pr.projectID == projectid).FirstOrDefault();
            if(p != null)
            {
               //var a =  pe.ToString().Split('-');
               // pe = a[0] + '/' + a[1] + '/' + a[2];
                p.ProjectEnd = pe;
                db.Entry(p).State = EntityState.Modified;
                db.SaveChanges();
                return 1;
            }
            return 0;
        }
        public int DeleteProject(int pid)
        {
            var project = db.Projects.Where(p=> p.projectID == pid).FirstOrDefault();
            var team = project.Teams.FirstOrDefault();
            if (team != null)
            {
                var pt = project.Teams.FirstOrDefault().TeamID;

                var tx = db.Teams.Where(y => y.TeamID == pt).FirstOrDefault();
                var pth = db.ProjectTeamHistories.Where(th => th.TeamID == tx.TeamID).ToList();
                foreach (var x in pth)
                {
                    db.ProjectTeamHistories.Remove(x);
                }
                db.Teams.Remove(tx);
            }
          
            db.Projects.Remove(project);
            db.SaveChanges();
            return 1;
        }
        [System.Web.Services.WebMethod]
        public int CreateProject(string desc, string name,int cid)
        {
            var id = db.Projects.ToList().Count() +5;
            var d = DateTime.Today;
            var x = d.Date.ToString("dd/MM/yyyy");
            var project = new Project()
            { projectID = id, projectName = name, projectDesc = desc,
             projectOwnerID = cid, projectMDID = null, projectPrice = null,
             projectStatues = "pending",
                ProjectPic=null,ProjectStart=null,
             ProjectEnd= null,uploadDate=x};
            db.Projects.Add(project);
          
            db.SaveChanges();
            return 1;
        }
        //=========================================================================================
        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }
        [System.Web.Services.WebMethod]
        public int SetPrice(int proid, int pp)
        {
            var p = db.Projects.Where(pr => pr.projectID == proid).FirstOrDefault();
            
            p.projectPrice = pp;
            db.Entry(p).State = EntityState.Modified;
            db.SaveChanges();
            return 1;
            
        }
        [System.Web.Services.WebMethod]
        public int CloseProject(int pid)
        {
           var pro =db.Projects.Where(p => p.projectID == pid).FirstOrDefault();
            pro.projectStatues = "end";
            var d = DateTime.Today;
            pro.ProjectEnd = d.Date.ToString("dd/MM/yyyy");
            db.Entry(pro).State = EntityState.Modified;
            db.SaveChanges();
            return 1;
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            ViewBag.projectOwnerID = new SelectList(db.Customers, "CustomerID", "CustomerFName");
            ViewBag.projectMDID = new SelectList(db.Employees, "EmployeeID", "EmployeeFName");
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "projectID,projectName,projectDesc,projectOwnerID,projectMDID,projectPrice,projectStart,projectEnd,projectStatues")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.projectOwnerID = new SelectList(db.Customers, "CustomerID", "CustomerFName", project.projectOwnerID);
            ViewBag.projectMDID = new SelectList(db.Employees, "EmployeeID", "EmployeeFName", project.projectMDID);
            return View(project);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            ViewBag.projectOwnerID = new SelectList(db.Customers, "CustomerID", "CustomerFName", project.projectOwnerID);
            ViewBag.projectMDID = new SelectList(db.Employees, "EmployeeID", "EmployeeFName", project.projectMDID);
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "projectID,projectName,projectDesc,projectOwnerID,projectMDID,projectPrice,projectStart,projectEnd,projectStatues")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.projectOwnerID = new SelectList(db.Customers, "CustomerID", "CustomerFName", project.projectOwnerID);
            ViewBag.projectMDID = new SelectList(db.Employees, "EmployeeID", "EmployeeFName", project.projectMDID);
            return View(project);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
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
