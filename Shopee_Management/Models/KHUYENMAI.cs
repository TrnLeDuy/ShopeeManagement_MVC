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
    
    public partial class KHUYENMAI
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KHUYENMAI()
        {
            this.DONHANGs = new HashSet<DONHANG>();
        }
    
        public int id_voucher { get; set; }
        public Nullable<double> ty_le_giam { get; set; }
        public Nullable<System.DateTime> ngay_tao { get; set; }
        public Nullable<System.DateTime> ngay_bat_dau { get; set; }
        public Nullable<System.DateTime> ngay_ket_thuc { get; set; }
        public string id_nbh { get; set; }
        public Nullable<int> id_nv { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DONHANG> DONHANGs { get; set; }
        public virtual NGUOIBANHANG NGUOIBANHANG { get; set; }
        public virtual NHANVIEN NHANVIEN { get; set; }
    }
}
