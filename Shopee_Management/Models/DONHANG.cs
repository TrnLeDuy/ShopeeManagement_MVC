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
    
    public partial class DONHANG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DONHANG()
        {
            this.CHITIETDONHANGs = new HashSet<CHITIETDONHANG>();
            this.DONHANG_QUATANG = new HashSet<DONHANG_QUATANG>();
            this.VANCHUYENs = new HashSet<VANCHUYEN>();
        }
    
        public int id_don { get; set; }
        public Nullable<System.DateTime> ngay_dat { get; set; }
        public Nullable<int> trang_thai_dh { get; set; }
        public Nullable<int> tt_thanh_toan { get; set; }
        public Nullable<decimal> tong_cong { get; set; }
        public Nullable<decimal> thanh_tien { get; set; }
        public Nullable<int> id_pttt { get; set; }
        public string id_kh { get; set; }
        public string id_nbh { get; set; }
        public Nullable<int> id_voucher { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHITIETDONHANG> CHITIETDONHANGs { get; set; }
        public virtual KHACHHANG KHACHHANG { get; set; }
        public virtual NGUOIBANHANG NGUOIBANHANG { get; set; }
        public virtual PTTT PTTT { get; set; }
        public virtual KHUYENMAI KHUYENMAI { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DONHANG_QUATANG> DONHANG_QUATANG { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VANCHUYEN> VANCHUYENs { get; set; }
    }
}
