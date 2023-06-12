using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Runtime.Caching;
using Day20.Models;

namespace Day20.Controllers
{
    public class HomeController : Controller
    {
        //step1 : 網頁導入傳值到前端
        public ActionResult Index()
        {
            
            var orderId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);
            //需填入你的網址
            var website = $"https://localhost:44325/";
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
                { "OrderResultURL", $"{website}/Home/PayInfo/{orderId}"},
                { "PaymentInfoURL",  $"{website}/api/Ecpay/AddAccountInfo"},
                { "ClientRedirectURL",  $"{website}/Home/AccountInfo/{orderId}"},
                { "MerchantID",  "2000132"},
                { "IgnorePayment",  "GooglePay#WebATM#CVS#BARCODE"},
                { "PaymentType",  "aio"},
                { "ChoosePayment",  "ALL"},
                { "EncryptType",  "1"},
            };
            //檢查碼
            order["CheckMacValue"] = GetCheckMacValue(order);
            return View(order);
        }
        /// step5 : 取得付款資訊，更新資料庫 OrderResultURL
        [HttpPost]
        public ActionResult PayInfo(FormCollection id)
        {
            var data = new Dictionary<string, string>();
            foreach (string key in id.Keys)
            {
                data.Add(key, id[key]);
            }
            Database1Entities db = new Database1Entities();
            var Orders = db.EcpayOrders.ToList().Where(m => m.MerchantTradeNo == id["MerchantTradeNo"]).FirstOrDefault();
            Orders.RtnCode = int.Parse(id["RtnCode"]);
            if (id["RtnMsg"] == "Succeeded") Orders.RtnMsg = "已付款";
            Orders.PaymentDate = Convert.ToDateTime(id["PaymentDate"]);
            Orders.SimulatePaid = int.Parse(id["SimulatePaid"]);
            db.SaveChanges();
            return View("EcpayView", data);
        }
        /// step5 : 取得虛擬帳號 資訊  ClientRedirectURL
        [HttpPost]
        public ActionResult AccountInfo(FormCollection id)
        {
            var data = new Dictionary<string, string>();
            foreach (string key in id.Keys)
            {
                data.Add(key, id[key]);
            }
            Database1Entities db = new Database1Entities();
            var Orders = db.EcpayOrders.ToList().Where(m => m.MerchantTradeNo == id["MerchantTradeNo"]).FirstOrDefault();
            Orders.RtnCode = int.Parse(id["RtnCode"]);
            if (id["RtnMsg"] == "Succeeded") Orders.RtnMsg = "已付款";
            Orders.PaymentDate = Convert.ToDateTime(id["PaymentDate"]);
            Orders.SimulatePaid = int.Parse(id["SimulatePaid"]);
            db.SaveChanges();
            return View("EcpayView", data);
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
    }
}