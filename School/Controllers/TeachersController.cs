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
    public class TeachersController : Controller
    {
        private SchoolDbContext db = new SchoolDbContext();

        // GET: Teachers
        public ActionResult Index()
        {
            return View(db.Teachers.ToList());
        }

        // GET: Teachers/Details/5
        public ActionResult Details(int id = 0)
        {
            if (id < 0)
            {
                return RedirectToAction("Index");
            }
            Teacher teacher =  db.Teachers.Include("Courses").SingleOrDefault(x => x.ID == id);
            if (teacher == null)
            {
                return RedirectToAction("Index");
            }
            return View(teacher);
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
        public ActionResult Create([Bind(Include = "ID,FirstName,LastName,DateOfBirth,City")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                db.Teachers.Add(teacher);
                db.SaveChanges();
                return RedirectToAction("Index");
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
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,DateOfBirth,City")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                db.Entry(teacher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(teacher);
        }

        // GET: Teachers/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Teacher teacher = db.Teachers.Find(id);
            db.Teachers.Remove(teacher);
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
            List<Course> course = db.Courses.Include("Teachers").Where(x => x.Students.FirstOrDefault(a => a.ID == id) == null).ToList();
            ViewBag.sId = id;              //////////// Cours ID//////
            return View(course);
        }

        [HttpGet]
        public ActionResult CoursesToTeachers(int id = 0, int cId = 0)
        {
            if (cId < 1 || id < 1)
            {
                return RedirectToAction("Index");
            }
            Course course = db.Courses.FirstOrDefault(s => s.ID == cId);
            if (course == null)
                return RedirectToAction("Details", new { id = id });
            Teacher teacher = db.Teachers.Include("Courses").SingleOrDefault(c => c.ID == id);
            if (course == null)
                return RedirectToAction("Details", new { id = id });

            teacher.Courses.Add(course);   ///Add course

            db.SaveChanges();       /// Save Changes to the  Data Base

            return RedirectToAction("Details", new { id = id });
        }
    }
}
