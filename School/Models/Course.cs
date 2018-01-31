using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace School.Models
{
    public class Course
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Information { get; set; }

        public virtual List<Student> Students { get; set; }
        public List<Teacher> Teachers { get; set; }
        public virtual List<Assignment> Assignments { get; set; }


    }
}