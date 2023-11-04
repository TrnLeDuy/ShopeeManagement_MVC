using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopee_Management.Models
{
    public class ModelTrangChu
    {
        public IEnumerable<CHITIETSP> ctsp { get; set; }
        public IEnumerable<NGANHHANG> nganhhang { get; set; }
    }
}