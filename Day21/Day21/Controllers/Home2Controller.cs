using System.Linq;
using System.Web.Mvc;
using Day21.Models;
using System.Web.Security;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Web;
using Day21.Helper;
using System;

namespace Day21.Controllers
{
    [token]  // Models -> token.cs
    public class Home2Controller : Controller
    {
        Database1Entities1 db = new Database1Entities1();

        public ActionResult login()
        {
            //Session.Abandon(); 
            //Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", "")); //同下
            CookieHelper.SetCookie("ASP.NET_SessionId", "", DateTime.Now.AddMinutes(1));
            return View();
        }
        //登入
        [HttpPost]
        public ActionResult login(string account, string password)
        {
            Database1Entities1 db = new Database1Entities1();
            var member = db.member.Where(m => m.account == account && m.password == password).FirstOrDefault(); //查詢
            if (member != null)
            {
                CookieHelper.SetCookie("Islogin", "true", DateTime.Now.AddMinutes(1));
                return RedirectToAction("Buycart", "Home2"); //action, controller
            }
            return View();
        }
        //Islogin==true才能進
        [LoginFilter]
        public ActionResult Buycart()
        {
            //進入這裡MVC會自動生成 ASP.NET_SessionId
            var orderId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);
            var website = $"https://localhost:44354/"; //記得確認一下數字有沒有一樣
            var order = new Dictionary<string, string>
            {
                { "MerchantTradeNo",  orderId},
                { "MerchantTradeDate",  DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")},
                { "TotalAmount",  "100"},
                { "TradeDesc",  "無"},
                { "ItemName",  "測試商品"},
                { "ExpireDate",  "3"},
                { "CustomField1",  ""},
                { "CustomField2",  ""},
                { "CustomField3",  ""},
                { "CustomField4",  ""},
                { "ReturnURL",  $"{website}/api/Ecpay/AddPayInfo"},
                { "OrderResultURL", $"{website}/Home2/PayInfo/{orderId}"},
                { "PaymentInfoURL",  $"{website}/api/Ecpay/AddAccountInfo"},
                { "ClientRedirectURL",  $"{website}/Home2/AccountInfo/{orderId}"},
                { "MerchantID",  "2000132"},
                { "IgnorePayment",  "GooglePay#WebATM#CVS#BARCODE"},
                { "PaymentType",  "aio"},
                { "ChoosePayment",  "ALL"},
                { "EncryptType",  "1"},
            };
            order["CheckMacValue"] = GetCheckMacValue(order);
            return View(order);
        }
        private string GetCheckMacValue(Dictionary<string, string> order)
        {
            var param = order.Keys.OrderBy(x => x).Select(key => key + "=" + order[key]).ToList();
            var checkValue = string.Join("&", param);
            //測試用的 HashKey
            var hashKey = "5294y06JbISpM5x9";
            //測試用的 HashIV
            var HashIV = "v77hoKGq4kWxNNIS";
            checkValue = $"HashKey={hashKey}" + "&" + checkValue + $"&HashIV={HashIV}";
            checkValue = HttpUtility.UrlEncode(checkValue).ToLower();
            checkValue = GetSHA256(checkValue);
            return checkValue.ToUpper();
        }
        private string GetSHA256(string value)
        {
            var result = new StringBuilder();
            var sha256 = SHA256Managed.Create();
            var bts = Encoding.UTF8.GetBytes(value);
            var hash = sha256.ComputeHash(bts);
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }

        [HttpPost]
        [LoginFilter]
        public ActionResult PayInfo(FormCollection id)
        {
            var data = new Dictionary<string, string>();
            foreach (string key in id.Keys)
            {
                data.Add(key, id[key]);
            }
            return View("EcpayView", data);
        }
        [HttpPost]
        [LoginFilter]
        public ActionResult AccountInfo(FormCollection id)
        {
            var data = new Dictionary<string, string>();
            foreach (string key in id.Keys)
            {
                data.Add(key, id[key]);
            }
            return View("EcpayView", data);
        }
        // GET: Logout
        public ActionResult Index_logout()
        {
            Session.Clear();
            CookieHelper.SetCookie("Islogin", "", DateTime.Now.AddDays(-1));
            return RedirectToAction("login");
        }

    }
}