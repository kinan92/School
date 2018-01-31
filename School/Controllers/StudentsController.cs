using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using School.Models;

namespace School.Controllers
{
    public class StudentsController : Controller
    {
        private SchoolDbContext db = new SchoolDbContext();

        // GET: Students
        public ActionResult Index()
        {
            return View(db.Students.ToList());
        }

        // GET: Students/Details/5
        public ActionResult Details(int id = 0)
        {
            if (id < 1)
            {
                return RedirectToAction("Index");
            }
            Student student = db.Students.Include("Courses").SingleOrDefault(x => x.ID == id);
            if (student == null)
            {
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FirstName,LastName,DateOfBirth,City")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

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
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,DateOfBirth,City")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
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
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
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

        [HttpGet]
        public ActionResult AllCourses(int id = 0)
        {
            if (id < 1)
                return RedirectToAction("Index");
            List<Course> course = db.Courses.Include("Students").Where(x => x.Students.FirstOrDefault(a => a.ID == id) == null).ToList();
            ViewBag.sId = id;              //////////// Cours ID//////
            return View(course);
        }
        [HttpGet]
        public ActionResult CoursesToStudents(int id = 0, int cId = 0)
        {
            if (cId < 1 || id < 1)
            {
                return RedirectToAction("Index");
            }
            Course course = db.Courses.FirstOrDefault(s => s.ID == cId);
            if (course == null)
                return RedirectToAction("Details", new { id = id });
            Student student = db.Students.Include("Courses").SingleOrDefault(c => c.ID == id);
            if (course == null)
                return RedirectToAction("Details", new { id = id });
            student.Courses.Add(course);   ///Add course
            db.SaveChanges();       /// Save Changes to the  Data Base

            return RedirectToAction("Details", new { id = id });
        }
    }
}
