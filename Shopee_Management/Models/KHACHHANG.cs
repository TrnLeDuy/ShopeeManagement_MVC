//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Shopee_Management.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class KHACHHANG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KHACHHANG()
        {
            this.DONHANGs = new HashSet<DONHANG>();
            this.NGUOIBANHANGs = new HashSet<NGUOIBANHANG>();
        }
    
        public string id_kh { get; set; }
        public string ho_ten { get; set; }
        public string sdt { get; set; }
        public string email { get; set; }
        public Nullable<System.DateTime> ngay_sinh { get; set; }
        public string dia_chi { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string avatar { get; set; }
        public Nullable<int> tinh_trang_kh { get; set; }
        public Nullable<int> diem_tich_luy { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DONHANG> DONHANGs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NGUOIBANHANG> NGUOIBANHANGs { get; set; }
    }
}
