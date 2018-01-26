﻿using System;
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
    public class CoursesController : Controller
    {
        private SchoolDbContext db = new SchoolDbContext();

        // GET: Courses
        public ActionResult Index()
        {
            return View(db.Courses.ToList());
        }

        // GET: Courses/Details/5
        public ActionResult Details(int id = 0)
        {
            if (id < 1)
            {
                return RedirectToAction("Index");
            }
            Course course = db.Courses.Include("Assignments").Include("Students").Include("Teachers").FirstOrDefault(x => x.ID == id);
            if (course == null)
            {
                return RedirectToAction("Index");
            }
            return View(course);
        }

        // GET: Courses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Information")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(course);
        }

        // GET: Courses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Information")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(course);
        }

        // GET: Courses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
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
        public ActionResult AllStudents(int id = 0)
        {
            if (id < 1)
                return RedirectToAction("Index");
            List<Student> students = db.Students.Include("Courses").Where(x => x.Courses.FirstOrDefault(a => a.ID == id) == null).ToList();
            ViewBag.cId = id;              //////////// Cours ID//////
            return View(students);
        }

        [HttpGet]
        public ActionResult StudentToCourses(int id = 0, int sId = 0)
        {
            if (sId < 1 || id < 1)
            {
                return RedirectToAction("Index");
            }
            Student student = db.Students.FirstOrDefault(s => s.ID == sId);
            if (student == null)
                return RedirectToAction("Details", new { id = id });
            Course course = db.Courses.Include("Students").SingleOrDefault(c => c.ID == id);
            if (course == null)
                return RedirectToAction("Details", new { id = id });
            course.Students.Add(student);   ///Add student
            db.SaveChanges();       /// Save Changes to the  Data Base

            return RedirectToAction("Details", new { id = id });
        }

        public ActionResult RemoveStudent(int id = 0, int sId = 0)
        {
            if (id < 1 || sId < 1)
                return RedirectToAction("Index");
            Course course = db.Courses.Include("Students").FirstOrDefault(x => x.ID == id);
            if(course != null)
            {
                Student student = course.Students.FirstOrDefault(x => x.ID == sId);
                course.Students.Remove(student);
                db.SaveChanges();
            }
            return RedirectToAction("Details", new { id = id });
        }
    }
}
