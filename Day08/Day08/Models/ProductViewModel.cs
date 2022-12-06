using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Day08.Models;
namespace Day08.Models
{
    public class ProductViewModel
    {
        public int 產品編號 { get; set; }
        public int 特價 { get; set; }
        public string 產品 { get; set; }
        public string 類別名稱 { get; set; }
        public string 單位數量 { get; set; }
        public Nullable<decimal> 單價 { get; set; }

    }
}