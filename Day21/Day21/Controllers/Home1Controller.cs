using System.Linq;
using System.Web.Mvc;
using Day21.Models;
using System.Web.Security;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Web;
using System;

namespace Day21.Controllers
{
    public class Home1Controller : Controller
    {
        Database1Entities1 db = new Database1Entities1();

        public ActionResult login()
        {
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
                FormsAuthentication.RedirectFromLoginPage(member.account, true); //cookie驗證 : https://blog.miniasp.com/post/2008/02/20/Explain-Forms-Authentication-in-ASPNET-20
                TempData["IsLogin"] = true;
                return RedirectToAction("Buycart", "Home1"); //action, controller
            }
            ViewBag.IsLogin = true;                                          //https://stackoverflow.com/questions/34447310/any-reason-why-my-viewbag-is-not-working
            return View();
        }
        //登入後進
        [Authorize]
        public ActionResult Buycart()
        { 
            ViewBag.IsLogin = TempData["IsLogin"];                               //RedirectToAction : Home收不到Index的ViewBag，TempData 會將值存在seesin 。https://www.codeproject.com/Articles/476967/What-is-ViewData-ViewBag-and-TempData-MVC-Option-2
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
                { "OrderResultURL", $"{website}/Home1/PayInfo/{orderId}"},
                { "PaymentInfoURL",  $"{website}/api/Ecpay/AddAccountInfo"},
                { "ClientRedirectURL",  $"{website}/Home1/AccountInfo/{orderId}"},
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
        //[Authorize] //會出問題QQ
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
        //[Authorize]
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
            FormsAuthentication.SignOut(); //清除cookie
            return RedirectToAction("login");
        }
        
    }
}
