using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Day05.Models
{
    public class Employee
    {
        public string EmpId { get; set; }
        public int Department { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Salary { get; set; }
    }
}