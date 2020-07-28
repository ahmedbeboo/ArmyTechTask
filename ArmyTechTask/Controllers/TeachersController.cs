using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ArmyTechTask.Models;
using PagedList;

namespace ArmyTechTask.Controllers
{
    public class TeachersController : Controller
    {
        private ArmyTechTaskEntities db = new ArmyTechTaskEntities();

        // GET: Teachers
        public ActionResult Index(int? page)
        {

            int pageSize = 20;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;



            var teachers = db.Teachers.OrderBy(t => t.ID);
            var tch = teachers.ToPagedList(pageIndex, pageSize);

            return View(tch);

            //return View(db.Teachers.ToList());
        }


        // GET: Teachers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,BirthDate")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                var count = db.Teachers.Count();
                teacher.ID = count + 1;
                db.Teachers.Add(teacher);
                db.SaveChanges();
                //return RedirectToAction("Index");

                return RedirectToAction("Edit", new { id = teacher.ID });

            }

            return View(teacher);
        }

        // GET: Teachers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,BirthDate")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                db.Entry(teacher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(teacher);
        }


        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Teacher teacher = db.Teachers.Find(id);
            db.Teachers.Remove(teacher);
            db.SaveChanges();
            //return RedirectToAction("Index");

            return Json(new
            {
                msg = "Successfully Deleted :" + teacher.Name
            });
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
