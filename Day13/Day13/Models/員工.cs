//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Day13.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class 員工
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public 員工()
        {
            this.訂貨主檔 = new HashSet<訂貨主檔>();
        }
    
        public int 員工編號 { get; set; }
        public string 姓名 { get; set; }
        public string 名 { get; set; }
        public string 職稱 { get; set; }
        public string 稱呼 { get; set; }
        public Nullable<System.DateTime> 出生日期 { get; set; }
        public Nullable<System.DateTime> 雇用日期 { get; set; }
        public string 地址 { get; set; }
        public string 城市 { get; set; }
        public string 行政區 { get; set; }
        public string 區域號碼 { get; set; }
        public string 國家地區 { get; set; }
        public string 電話號碼 { get; set; }
        public string 內部分機號碼 { get; set; }
        public string 相片 { get; set; }
        public string 附註 { get; set; }
        public Nullable<int> 主管 { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<訂貨主檔> 訂貨主檔 { get; set; }
    }
}
