using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Day08.Models;

namespace Day08.Models
{
    public class CategoryProductViewModel
    {
        public List<產品類別> Category { get; set; }
        public List<產品資料> Product { get; set; }
    }
}