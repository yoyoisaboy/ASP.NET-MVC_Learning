using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Day10.Models
{
    public class Sys
    {
        public static string StringConverDateTimeString(string str)
        {
            if (str == "" || str == null || str.Length != 14) { return ""; }
            // 20180721013539 -> 2018年07月21日01時35分39秒
            return
                str.Substring(0, 4) + "年" + str.Substring(4, 2) + "月" +
                str.Substring(6, 2) + "日" + str.Substring(8, 2) + "時" +
                str.Substring(10, 2) + "分" + str.Substring(12, 2) + "秒";
        }
    }
}