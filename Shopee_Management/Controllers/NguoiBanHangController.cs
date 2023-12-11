using Shopee_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shopee_Management.Controllers
{
    public class NguoiBanHangController : Controller
    {
        private TMDTdbEntities entities1 = new TMDTdbEntities();

        // GET: NguoiBanHang
        public ActionResult QuanLySanPham()
        {
            
            // Lấy ID người bán hàng từ session
            string idNguoiBanHang = Session["StoreID"] as string;
            var nguoiBanHang = entities1.NGUOIBANHANGs.FirstOrDefault(nbh => nbh.id_nbh == idNguoiBanHang);
            // Lấy danh sách sản phẩm của người bán hàng
            if (nguoiBanHang != null)
            {
                // Get the SANPHAM and CHITIETSP associated with the NGUOIBANHANG
                var sanPhamList = entities1.SANPHAMs
                    .Where(sp => sp.CHITIETSPs.Any(ct => ct.id_nbh == nguoiBanHang.id_nbh))
                    .ToList();

                ViewBag.SanPhamList = sanPhamList;
                ViewBag.NguoiBanHang = nguoiBanHang;

                return View();
            }
            else
            {
                // Handle if NGUOIBANHANG with the given id is not found
                return HttpNotFound();
            }
        }
        public ActionResult XemChiTietSanPham(int id)
        {
            string idNguoiBanHang = Session["StoreID"] as string;

            using (TMDTdbEntities entities1 = new TMDTdbEntities())
            {
                var chiTietSPList = entities1.CHITIETSPs.Where(p => p.id_sp == id).ToList();
                var sanPhamList = entities1.SANPHAMs.ToList();
                var nbhList = entities1.NGUOIBANHANGs.ToList();
                var sizeList = entities1.KICHCOes.ToList();
                var colorList = entities1.BANGMAUs.ToList();
                var brandList = entities1.THUONGHIEUx.ToList();
                var memberList = entities1.XUATXUs.ToList();
                var nganhhangList = entities1.NGANHHANGs.ToList();
                var nhconList = entities1.NGANHHANGCONs.ToList();
                var nhc3List = entities1.NGANHHANGCAP3.ToList();
                var soluongList = entities1.BANGMAUs.ToList();
                ViewBag.SanPhamList = sanPhamList;
                ViewBag.ChiTietSPList = chiTietSPList;
                ViewBag.NBHList = nbhList;
                ViewBag.SizeList = sizeList;
                ViewBag.ColorList = colorList;
                ViewBag.BrandList = brandList;
                ViewBag.MemberList = memberList;
                ViewBag.NganhHangList = nganhhangList;
                ViewBag.NganhHangConList = nhconList;
                ViewBag.NganhHangC3List = nhc3List;
                return View();


            }
        }
        [HttpGet]
        public ActionResult ThemSP()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemSP(SANPHAM sanpham)
        {
            string idNguoiBanHang = Session["StoreID"] as string;

            if (ModelState.IsValid)
            {
                using (TMDTdbEntities entities1 = new TMDTdbEntities())
                {
                    // Thêm sản phẩm vào cơ sở dữ liệu
                    entities1.SANPHAMs.Add(sanpham);
                    entities1.SaveChanges();  // SaveChanges here to get the id_sp

                    // Tạo và thêm chi tiết sản phẩm
                    CHITIETSP chiTietSP = new CHITIETSP
                    {
                        id_sp = sanpham.id_sp,
                        id_nbh = idNguoiBanHang,
                        id_brand = 1,
                        id_member = "US"
                        // Các giá trị khác nếu cần
                    };

                    entities1.CHITIETSPs.Add(chiTietSP);
                    entities1.SaveChanges();
                }

                return RedirectToAction("QuanLySanPham");
            }

            return View(sanpham);
        }
        [HttpGet]
        public ActionResult ThemCTSP()
        {
            using (TMDTdbEntities entities1 = new TMDTdbEntities())
            {
                // Lấy danh sách sản phẩm từ cơ sở dữ liệu và tạo SelectList
                var ctsanPhamList = entities1.SANPHAMs.ToList();
                SelectList ctsanPhamSelectList = new SelectList(ctsanPhamList, "id_sp", "ten_sp");

                var nbhList = entities1.NGUOIBANHANGs.ToList();
                SelectList nbhSelectList = new SelectList(nbhList, "id_nbh", "ten_cua_hang");

                var sizeList = entities1.KICHCOes.ToList();
                SelectList sizeSelectList = new SelectList(sizeList, "id_size", "ten_size");

                var colorList = entities1.BANGMAUs.ToList();
                SelectList colorSelectList = new SelectList(colorList, "id_color", "ten_mau");
                var brandList = entities1.THUONGHIEUx.ToList();
                SelectList brandSelectList = new SelectList(brandList, "id_brand", "ten_brand");
                var memberList = entities1.XUATXUs.ToList();
                SelectList memberSelectList = new SelectList(memberList, "id_member", "ten_member");
                var nganhhangList = entities1.NGANHHANGs.ToList();
                SelectList nganhhangSelectList = new SelectList(nganhhangList, "id_nganhhang", "ten_nganhhang");
                var nhconList = entities1.NGANHHANGCONs.ToList();
                SelectList nhconSelectList = new SelectList(nhconList, "id_nhcon", "ten_nhcon");
                var nhc3List = entities1.NGANHHANGCAP3.ToList();
                SelectList nhc3SelectList = new SelectList(nhc3List, "id_nhc3", "ten_nhc3");
                var soluongList = entities1.BANGMAUs.ToList();


                // Truyền SelectList vào ViewBag để sử dụng trong View
                ViewBag.ChiTietSPSelectList = ctsanPhamSelectList;
                ViewBag.NBHSelectList = nbhSelectList;
                ViewBag.SizeSelectList = sizeSelectList;
                ViewBag.ColorSelectList = colorSelectList;
                ViewBag.BrandSelectList = brandSelectList;
                ViewBag.MemberSelectList = memberSelectList;
                ViewBag.NganhHangSelectList = nganhhangSelectList;
                ViewBag.NganhHangConSelectList = nhconSelectList;
                ViewBag.NganhHangC3SelectList = nhc3SelectList;
                ViewBag.SoLuongList = soluongList;
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemCTSP(CHITIETSP ctsanpham)
        {
            string idNguoiBanHang = Session["StoreID"] as string;
            using (TMDTdbEntities entities1 = new TMDTdbEntities())
            {

                var ctsanPhamList = entities1.SANPHAMs.ToList();
                SelectList ctsanPhamSelectList = new SelectList(ctsanPhamList, "id_sp", "ten_sp");


                var nbhList = entities1.NGUOIBANHANGs.ToList();
                SelectList nbhSelectList = new SelectList(nbhList, "id_nbh", "ten_cua_hang");
                var sizeList = entities1.KICHCOes.ToList();
                SelectList sizeSelectList = new SelectList(sizeList, "id_size", "ten_size");
                var colorList = entities1.BANGMAUs.ToList();
                SelectList colorSelectList = new SelectList(colorList, "id_color", "ten_mau");
                var brandList = entities1.THUONGHIEUx.ToList();
                SelectList brandSelectList = new SelectList(brandList, "id_brand", "ten_brand");
                var memberList = entities1.XUATXUs.ToList();
                SelectList memberSelectList = new SelectList(memberList, "id_member", "ten_member");
                var nganhhangList = entities1.NGANHHANGs.ToList();
                SelectList nganhhangSelectList = new SelectList(nganhhangList, "id_nganhhang", "ten_nganhhang");
                var nhconList = entities1.NGANHHANGCONs.ToList();
                SelectList nhconSelectList = new SelectList(nhconList, "id_nhcon", "ten_nhcon");
                var nhc3List = entities1.NGANHHANGCAP3.ToList();
                SelectList nhc3SelectList = new SelectList(nhc3List, "id_nhc3", "ten_nhc3");
                var soluongList = entities1.BANGMAUs.ToList();

                // Truyền SelectList vào ViewBag để sử dụng trong View
                ViewBag.ChiTietSPSelectList = ctsanPhamSelectList;
                ViewBag.NBHSelectList = nbhSelectList;
                ViewBag.SizeSelectList = sizeSelectList;
                ViewBag.ColorSelectList = colorSelectList;
                ViewBag.BrandSelectList = brandSelectList;
                ViewBag.MemberSelectList = memberSelectList;
                ViewBag.NganhHangSelectList = nganhhangSelectList;
                ViewBag.NganhHangConSelectList = nhconSelectList;
                ViewBag.NganhHangC3SelectList = nhc3SelectList;
                ViewBag.SoLuongList = soluongList;
                ctsanpham.id_nbh = idNguoiBanHang;
                // Tương tự, bạn cần thiết lập SelectList cho các trường khác ở đây

                if (ModelState.IsValid)
                {

                    {
                        // Thêm sản phẩm vào cơ sở dữ liệu
                        entities1.CHITIETSPs.Add(ctsanpham);
                        entities1.SaveChanges();
                    }
                    return RedirectToAction("QuanLySanPham");
                }
                return View(ctsanpham);
            }
        }
        public ActionResult CapNhatSP(int id)
        {
            using (TMDTdbEntities entities1 = new TMDTdbEntities())
            {
                // Get the product by ID
                var product = entities1.SANPHAMs.FirstOrDefault(p => p.id_sp == id);

                if (product != null)
                {
                    return View(product);
                }
            }
            // Handle the case where the product with the given ID was not found.
            return HttpNotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CapNhatProduct(SANPHAM updatedProduct)
        {
            if (ModelState.IsValid)
            {
                using (TMDTdbEntities entities1 = new TMDTdbEntities())
                {
                    // Get the existing product from the database
                    var existingProduct = entities1.SANPHAMs.FirstOrDefault(p => p.id_sp == updatedProduct.id_sp);

                    if (existingProduct != null)
                    {
                        // Update the properties of the existing product with the edited values
                        existingProduct.ten_sp = updatedProduct.ten_sp;
                        existingProduct.gia_sp = updatedProduct.gia_sp;

                        // Save changes to the database
                        entities1.SaveChanges();

                        return RedirectToAction("QuanLySanPham");
                    }
                }
                // Handle the case where the product was not found or there was an issue with saving.
            }
            // Handle the case where the model state is not valid (e.g., validation errors).
            return View(updatedProduct);
        }
        [HttpPost]
        public ActionResult XoaSP(int id)
        {
            using (TMDTdbEntities entities1 = new TMDTdbEntities())
            {
                // Get the product from the database
                var product = entities1.SANPHAMs.FirstOrDefault(p => p.id_sp == id);

                if (product != null)
                {
                    // Remove the product from the database
                    entities1.SANPHAMs.Remove(product);
                    entities1.SaveChanges();

                    return Json(new { success = true });
                }
            }

            return Json(new { success = false });
        }
        public ActionResult CapNhatCTSP(int id)
        {
            using (TMDTdbEntities entities1 = new TMDTdbEntities())
            {
                // Get the product by ID
                var product = entities1.CHITIETSPs.FirstOrDefault(p => p.id_ctsp == id);

                if (product != null)
                {
                    var ctsanPhamList = entities1.SANPHAMs.ToList();
                    SelectList ctsanPhamSelectList = new SelectList(ctsanPhamList, "id_sp", "ten_sp");


                    var nbhList = entities1.NGUOIBANHANGs.ToList();
                    SelectList nbhSelectList = new SelectList(nbhList, "id_nbh", "ten_cua_hang");
                    var sizeList = entities1.KICHCOes.ToList();
                    SelectList sizeSelectList = new SelectList(sizeList, "id_size", "ten_size");
                    var colorList = entities1.BANGMAUs.ToList();
                    SelectList colorSelectList = new SelectList(colorList, "id_color", "ten_mau");
                    var brandList = entities1.THUONGHIEUx.ToList();
                    SelectList brandSelectList = new SelectList(brandList, "id_brand", "ten_brand");
                    var memberList = entities1.XUATXUs.ToList();
                    SelectList memberSelectList = new SelectList(memberList, "id_member", "ten_member");
                    var nganhhangList = entities1.NGANHHANGs.ToList();
                    SelectList nganhhangSelectList = new SelectList(nganhhangList, "id_nganhhang", "ten_nganhhang");
                    var nhconList = entities1.NGANHHANGCONs.ToList();
                    SelectList nhconSelectList = new SelectList(nhconList, "id_nhcon", "ten_nhcon");
                    var nhc3List = entities1.NGANHHANGCAP3.ToList();
                    SelectList nhc3SelectList = new SelectList(nhc3List, "id_nhc3", "ten_nhc3");
                    var soluongList = entities1.BANGMAUs.ToList();

                    // Truyền SelectList vào ViewBag để sử dụng trong View
                    ViewBag.ChiTietSPSelectList = ctsanPhamSelectList;
                    ViewBag.NBHSelectList = nbhSelectList;
                    ViewBag.SizeSelectList = sizeSelectList;
                    ViewBag.ColorSelectList = colorSelectList;
                    ViewBag.BrandSelectList = brandSelectList;
                    ViewBag.MemberSelectList = memberSelectList;
                    ViewBag.NganhHangSelectList = nganhhangSelectList;
                    ViewBag.NganhHangConSelectList = nhconSelectList;
                    ViewBag.NganhHangC3SelectList = nhc3SelectList;
                    ViewBag.SoLuongList = soluongList;
                    return View(product);
                }
            }
            // Handle the case where the product with the given ID was not found.
            return HttpNotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CapNhatCTSP(CHITIETSP updatedProduct)
        {
            if (ModelState.IsValid)
            {
                using (TMDTdbEntities entities1 = new TMDTdbEntities())
                {
                    var existingProduct = entities1.CHITIETSPs.FirstOrDefault(p => p.id_ctsp == updatedProduct.id_ctsp);

                    if (existingProduct != null)
                    {
                        // Update the properties of the existing product with the edited values
                        existingProduct.id_sp = updatedProduct.id_sp;
                        existingProduct.id_nbh = updatedProduct.id_nbh;
                        existingProduct.id_size = updatedProduct.id_size;
                        existingProduct.id_color = updatedProduct.id_color;
                        existingProduct.id_brand = updatedProduct.id_brand;
                        existingProduct.id_member = updatedProduct.id_member;
                        existingProduct.id_nganhhang = updatedProduct.id_nganhhang;
                        existingProduct.id_nhcon = updatedProduct.id_nhcon;
                        existingProduct.id_nhc3 = updatedProduct.id_nhc3;
                        existingProduct.so_luong = updatedProduct.so_luong;
                        // Update other properties in a similar way

                        // Save changes to the database
                        entities1.SaveChanges();

                        return RedirectToAction("QuanLySanPham");
                    }
                }
                // Handle the case where the product was not found or there was an issue with saving.
            }
            // Handle the case where the model state is not valid (e.g., validation errors).
            return View(updatedProduct);
        }
        [HttpPost]
        public ActionResult XoaCTSP(int id)
        {
            using (TMDTdbEntities entities1 = new TMDTdbEntities())
            {
                // Get the product from the database
                var product = entities1.CHITIETSPs.FirstOrDefault(p => p.id_ctsp == id);

                if (product != null)
                {
                    // Remove the product from the database
                    entities1.CHITIETSPs.Remove(product);
                    entities1.SaveChanges();

                    return Json(new { success = true });
                }
            }

            return Json(new { success = false });
        }
        [HttpPost]
        public List<SANPHAM> Search(string key)
        {
            TMDTdbEntities entity = new TMDTdbEntities();
            return entity.SANPHAMs.SqlQuery("Select * FROM SANPHAM JOIN CHITIETSP ON SANPHAM.id_sp = CHITIETSP.id_sp JOIN NGANHHANG ON CHITIETSP.id_nganhhang = NGANHHANG.id_nganhhang WHERE NGANHHANG.ten_nganhhang like N'%" + key + "%'").ToList();
        }

        [HttpPost]
        public ActionResult SearchByCategory(string category)
        {
            using (TMDTdbEntities entities1 = new TMDTdbEntities())
            {

                var products = entities1.SANPHAMs
                    .Where(c => c.ten_sp == category)
                    .ToList();

                return PartialView("_ProductListPartial", products);
            }
        }

        public ActionResult LichSuDonHang()
        {
            string customerID = (string)Session["ID"];
            var cuaHang = entities1.NGUOIBANHANGs.FirstOrDefault(k => k.id_kh == customerID);

            if (cuaHang != null)
            {
                var id_nbh = cuaHang.id_nbh;

                // Retrieve all order details for the specified id_nbh
                var orderDetails = entities1.CHITIETDONHANGs
                    .Where(ct => ct.id_nbh == id_nbh)
                    .ToList();

                // Extract unique id_don values from orderDetails
                var uniqueOrderIds = orderDetails.Select(od => od.id_don).Distinct();

                // Retrieve all orders corresponding to the unique id_don values
                var orders = entities1.DONHANGs
                    .Where(o => uniqueOrderIds.Contains(o.id_don))
                    .ToList();

                return View("LichSuDonHang", orders);
            }
            else
            {
                return RedirectToAction("DangNhap", "KhachHang");
            }
        }
    }
}