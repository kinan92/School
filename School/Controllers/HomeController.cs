using School.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace School.Controllers
{
    public class HomeController : Controller
    {
        SchoolDbContext db = new SchoolDbContext();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
    }
}