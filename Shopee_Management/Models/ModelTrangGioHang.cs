using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopee_Management.Models
{
    public class ModelTrangGioHang
    {
        public Cart Cart { get; set; }
        public IEnumerable<KHUYENMAI> khuyenmai { get; set; }
    }
}