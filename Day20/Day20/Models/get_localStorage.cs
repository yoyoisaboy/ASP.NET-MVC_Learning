using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Day20.Models
{
    public class get_localStorage
    {
        public string MerchantID { get; set; }
        public string MerchantTradeNo { get; set; }
        public string MerchantTradeDate { get; set; }
        public string PaymentType { get; set; }
        public int TotalAmount { get; set; }
        public string TradeDesc { get; set; }
        public string ItemName { get; set; }
        public string ChoosePayment { get; set; }
        public string ReturnURL { get; set; }
        public string ClientBackURL { get; set; }
        public string OrderResultURL { get; set; }
        public string IgnorePayment { get; set; }
        public string EncryptType { get; set; }
        public string CheckMacValue { get; set; }
    }

}
/*
//特店交易編號
{ "MerchantTradeNo",  orderId},
//特店交易時間 yyyy/MM/dd HH:mm:ss
{ "MerchantTradeDate",  DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")},
//交易金額
{ "TotalAmount",  "100"},
//交易描述
{ "TradeDesc",  "無"},
//商品名稱
{ "ItemName",  "測試商品"},
//允許繳費有效天數(付款方式為 ATM 時，需設定此值)
{ "ExpireDate",  "3"},
//自訂名稱欄位1
{ "CustomField1",  ""},
//自訂名稱欄位2
{ "CustomField2",  ""},
//自訂名稱欄位3
{ "CustomField3",  ""},
//自訂名稱欄位4
{ "CustomField4",  ""},
//綠界回傳付款資訊至此URL
{ "ReturnURL",  $"{website}/api/Ecpay/AddPayInfo"},
//使用者於綠界付款完成後，綠界將會轉址至此URL
{ "OrderResultURL", $"{website}/Home/PayInfo/{orderId}"},
//付款方式為 ATM 時，當使用者於綠界操作結束時，綠界回傳 虛擬帳號資訊至 此URL
{ "PaymentInfoURL",  $"{website}/api/Ecpay/AddAccountInfo"},
//付款方式為 ATM 時，當使用者於綠界操作結束時，綠界會轉址至 此URL。
{ "ClientRedirectURL",  $"{website}/Home/AccountInfo/{orderId}"},
//特店編號， 2000132 測試綠界編號
{ "MerchantID",  "2000132"},
//忽略付款方式
{ "IgnorePayment",  "GooglePay#WebATM#CVS#BARCODE"},
//交易類型 固定填入 aio
{ "PaymentType",  "aio"},
//選擇預設付款方式 固定填入 ALL
{ "ChoosePayment",  "ALL"},
//CheckMacValue 加密類型 固定填入 1 (SHA256)
{ "EncryptType",  "1"},
 */