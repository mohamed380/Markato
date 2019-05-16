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
    public class FeedBacksController : Controller
    {
        private MarkatoEntities7 db = new MarkatoEntities7();

        // GET: FeedBacks
        public ActionResult Index()
        {
            var feedBacks = db.FeedBacks.Include(f => f.Employee).Include(f => f.Project);
            return View(feedBacks.ToList());
        }

        // GET: FeedBacks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeedBack feedBack = db.FeedBacks.Find(id);
            if (feedBack == null)
            {
                return HttpNotFound();
            }
            return View(feedBack);
        }

        [System.Web.Services.WebMethod]
        public int Create(int pid,int eid, int r , string fb)
        {
            var feedback = db.FeedBacks.Where(f => f.MTID == eid && f.ProjectID == pid).FirstOrDefault();
            if (feedback == null)
            {
                var f = new FeedBack() { MTID = eid, ProjectID = pid, Rate = r, FeedBack1 = fb };
                db.FeedBacks.Add(f);
                db.SaveChanges();
                return 0;
            }
            else
            {
                feedback.Rate = r;
                feedback.FeedBack1 = fb;
                db.Entry(feedback).State = EntityState.Modified;
                db.SaveChanges();
                return 1;
            }
          
        
        }

        // GET: FeedBacks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeedBack feedBack = db.FeedBacks.Find(id);
            if (feedBack == null)
            {
                return HttpNotFound();
            }
            ViewBag.MTID = new SelectList(db.Employees, "EmployeeID", "EmployeeFName", feedBack.MTID);
            ViewBag.ProjectID = new SelectList(db.Projects, "projectID", "projectName", feedBack.ProjectID);
            return View(feedBack);
        }

        // POST: FeedBacks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,MTID,Rate,FeedBack1,ProjectID")] FeedBack feedBack)
        {
            if (ModelState.IsValid)
            {
                db.Entry(feedBack).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MTID = new SelectList(db.Employees, "EmployeeID", "EmployeeFName", feedBack.MTID);
            ViewBag.ProjectID = new SelectList(db.Projects, "projectID", "projectName", feedBack.ProjectID);
            return View(feedBack);
        }

        // GET: FeedBacks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeedBack feedBack = db.FeedBacks.Find(id);
            if (feedBack == null)
            {
                return HttpNotFound();
            }
            return View(feedBack);
        }

        // POST: FeedBacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FeedBack feedBack = db.FeedBacks.Find(id);
            db.FeedBacks.Remove(feedBack);
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
