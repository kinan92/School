using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace School.Models
{
    public class Assignment
    {
        public int ID { get; set; }

        public string Name { get; set; }
        public string Information { get; set; }

        public Course ConnectedToCourse { get; set; }

    }
}