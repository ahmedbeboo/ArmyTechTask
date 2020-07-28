using ArmyTechTask.BL;
using ArmyTechTask.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ArmyTechTask.Controllers
{
    public class StudentsController : Controller
    {
        private ArmyTechTaskEntities db = new ArmyTechTaskEntities();

        // GET: Students
        public ActionResult Index(int? page)
        {
            int pageSize = 20;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;



            var query = from S in db.Students
                        join ST in db.StudentTeachers on S.ID equals ST.ID into yG
                        from y1 in yG.DefaultIfEmpty()
                        select S;

            var q = query.ToList();

            var st = q.ToPagedList(pageIndex, pageSize);

            //var students = db.Students.Include(s => s.Field).Include(s => s.Governorate).Include(s => s.Neighborhood).OrderBy(s => s.ID);
            //var st = students.ToPagedList(pageIndex, pageSize);

            var teachersList = db.Teachers.Select(t => new { t.ID, t.Name }).ToList();


            List<SelectListItem> teacherList = new List<SelectListItem>();
            foreach (var temp in teachersList)
            {
                teacherList.Add(new SelectListItem() { Text = temp.Name, Value = temp.ID.ToString() });
            }
            ViewBag.teachers = teacherList;


            return View(st);
        }



        public ActionResult Create()
        {
            ViewBag.FieldId = new SelectList(db.Fields, "ID", "Name");
            ViewBag.GovernorateId = new SelectList(db.Governorates, "ID", "Name");
            ViewBag.NeighborhoodId = new SelectList(db.Neighborhoods, "ID", "Name");


            return View();
        }

        [HttpPost]
        public ActionResult GetNeighborhoodId(GovernmentInfo governmateInfo)
        {

            var Result = db.Neighborhoods.Where(n => n.GovernorateId == governmateInfo.govermentId).Select(x => new { Id = x.GovernorateId, Name = x.Name }).ToList();
            return Json(new
            {
                res = Result
            });
        }

        [HttpPost]
        public ActionResult AddTeacher(StudentTeacherInfo studentTeacherInfo)
        {
            try
            {
                var teacherExist = db.StudentTeachers.Where(st => st.StudentId == studentTeacherInfo.studentId && st.TeacherId == studentTeacherInfo.teacherId).FirstOrDefault();
                if (teacherExist == null)
                {
                    StudentTeacher studentTeacher = new StudentTeacher();
                    studentTeacher.TeacherId = studentTeacherInfo.teacherId;
                    studentTeacher.StudentId = studentTeacherInfo.studentId;
                    if (ModelState.IsValid)
                    {
                        db.StudentTeachers.Add(studentTeacher);
                        db.SaveChanges();
                    }

                    return Json(new
                    {
                        msg = "Successfully Added This Teacher To The Student"
                    });
                }
                else
                {
                    return Json(new
                    {
                        msg = "This Teacher Is Already Applied To The Student"
                    });
                }
            }
            catch
            {
                return Json(new
                {
                    msg = "Can't Add This Teacher To The Student"
                });
            }

        }

        [HttpPost]
        public ActionResult DeleteStudentTeacher(StudentTeacherInfo studentTeacherInfo)
        {
            try
            {
                StudentTeacher studentTeacher = db.StudentTeachers.Where(st=>st.StudentId==studentTeacherInfo.studentId&&st.TeacherId== studentTeacherInfo.teacherId).FirstOrDefault();

                if (studentTeacher != null)
                {
                    db.StudentTeachers.Remove(studentTeacher);
                    db.SaveChanges();

                    return Json(new
                    {
                        msg = "Successfully Deleted This Teacher From The Student"
                    });
                }


                else
                {
                    return Json(new
                    {
                        msg = "Can't Delete This Teacher From The Student"
                    });
                }
                
            }
            catch
            {
                return Json(new
                {
                    msg = "Can't Delete This Teacher From The Student"
                });
            }

        }


        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,BirthDate,GovernorateId,NeighborhoodId,FieldId")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Edit", new { id = student.ID });
            }

            ViewBag.FieldId = new SelectList(db.Fields, "ID", "Name", student.FieldId);
            ViewBag.GovernorateId = new SelectList(db.Governorates, "ID", "Name", student.GovernorateId);
            ViewBag.NeighborhoodId = new SelectList(db.Neighborhoods, "ID", "Name", student.NeighborhoodId);
            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.FieldId = new SelectList(db.Fields, "ID", "Name", student.FieldId);
            ViewBag.GovernorateId = new SelectList(db.Governorates, "ID", "Name", student.GovernorateId);


            ViewBag.NeighborhoodId = new SelectList(db.Neighborhoods, "ID", "Name", student.NeighborhoodId);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,BirthDate,GovernorateId,NeighborhoodId,FieldId")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FieldId = new SelectList(db.Fields, "ID", "Name", student.FieldId);
            ViewBag.GovernorateId = new SelectList(db.Governorates, "ID", "Name", student.GovernorateId);
            ViewBag.NeighborhoodId = new SelectList(db.Neighborhoods, "ID", "Name", student.NeighborhoodId);
            return View(student);
        }


        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var studentTeacherList = db.StudentTeachers.Where(st => st.StudentId == id).ToList();
            db.StudentTeachers.RemoveRange(studentTeacherList);

            Student student = db.Students.Find(id);
            db.Students.Remove(student);

            db.SaveChanges();

            return Json(new
            {
                msg = "Successfully Deleted :" + student.Name
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
