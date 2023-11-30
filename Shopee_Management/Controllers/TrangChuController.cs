using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Shopee_Management.Models;

namespace Shopee_Management.Controllers
{
    public class TrangChuController : Controller
    {
        private TMDTdbEntities db = new TMDTdbEntities();

        // GET: TrangChu
        public ActionResult Index()
        {
            var model = new ModelTrangChu
            {
                ctsp = db.CHITIETSPs
                    .Include(c => c.BANGMAU)
                    .Include(c => c.THUONGHIEU)
                    .Include(c => c.XUATXU)
                    .Include(c => c.NGUOIBANHANG)
                    .Include(c => c.NGANHHANG)
                    .Include(c => c.NGANHHANGCON)
                    .Include(c => c.NGANHHANGCAP3)
                    .Include(c => c.KICHCO)
                    .Include(c => c.SANPHAM)
                    .ToList(),
                nganhhang = db.NGANHHANGs.ToList()
            };

            return View(model);
        }

        // POST: TrangChu
        [HttpPost]
        public ActionResult FilterProducts(int? nganhHangId, decimal? minPrice, decimal? maxPrice)
        {
            var filteredProducts = db.CHITIETSPs
                .Include(c => c.BANGMAU)
                .Include(c => c.THUONGHIEU)
                .Include(c => c.XUATXU)
                .Include(c => c.NGUOIBANHANG)
                .Include(c => c.NGANHHANG)
                .Include(c => c.NGANHHANGCON)
                .Include(c => c.NGANHHANGCAP3)
                .Include(c => c.KICHCO)
                .Include(c => c.SANPHAM)
                .ToList();

            // Lọc theo ngành hàng nếu có ngành hàng được chọn
            if (nganhHangId != null)
            {
                filteredProducts = filteredProducts.Where(c => c.NGANHHANG?.id_nganhhang == nganhHangId).ToList();
            }


            // Lọc theo giá nếu có giá được chỉ định
            if (minPrice != null && maxPrice != null)
            {
                filteredProducts = filteredProducts.Where(c => c.SANPHAM.gia_sp >= minPrice && c.SANPHAM.gia_sp <= maxPrice).ToList();
            }

            var filteredModel = new ModelTrangChu
            {
                ctsp = filteredProducts,
                nganhhang = db.NGANHHANGs.ToList()
            };

            return View("Index", filteredModel);
        }

        public ActionResult Detail(int productId)
        {
            // Lấy thông tin chi tiết sản phẩm dựa trên ID
            var productDetail = db.CHITIETSPs
                .Include(c => c.BANGMAU)
                .Include(c => c.THUONGHIEU)
                .Include(c => c.XUATXU)
                .Include(c => c.NGUOIBANHANG)
                .Include(c => c.NGANHHANG)
                .Include(c => c.NGANHHANGCON)
                .Include(c => c.NGANHHANGCAP3)
                .Include(c => c.KICHCO)
                .Include(c => c.SANPHAM)
                .FirstOrDefault(c => c.id_ctsp == productId);

            if (productDetail == null)
            {
                // Xử lý khi không tìm thấy sản phẩm
                return RedirectToAction("Index", "TrangChu");
            }

            return View(productDetail);
        }
    }
}
