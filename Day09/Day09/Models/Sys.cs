using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Day09.Models
{
    public class Sys
    {
        public static string StringConverDateTimeString(string str)
        {
            if(str == "" || str == null || str.Length != 14) { return ""; }
            return
                str.Substring(0, 4) + "年" + str.Substring(4, 2) + "月" +
                str.Substring(6, 2) + "日" + str.Substring(8, 2) + "時" +
                str.Substring(10, 2) + "分" + str.Substring(12, 2) + "秒";
        }
    }
}