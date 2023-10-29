﻿using Shopee_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Shopee_Management.Controllers
{
    public class QuanLySanPhamController : Controller
    {

        // GET: QuanLySanPham
        public ActionResult Index()
        {
            using (TMDTdbEntities entities1 = new TMDTdbEntities())
            {
                // Truy vấn danh sách sản phẩm từ cơ sở dữ liệu
                var sanPhamList = entities1.SANPHAMs.ToList();

                // Truy vấn danh sách chi tiết sản phẩm từ cơ sở dữ liệu
                var chiTietSPList = entities1.CHITIETSPs.ToList();

                // Truyền danh sách sản phẩm và danh sách chi tiết sản phẩm vào View
                ViewBag.SanPhamList = sanPhamList;
                ViewBag.ChiTietSPList = chiTietSPList;

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
            if (ModelState.IsValid)
            {
                using (TMDTdbEntities entities1 = new TMDTdbEntities())
                {
                    // Thêm sản phẩm vào cơ sở dữ liệu
                    entities1.SANPHAMs.Add(sanpham);
                    entities1.SaveChanges();
                }
                return RedirectToAction("Index");
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

                // Tương tự, bạn cần thiết lập SelectList cho các trường khác ở đây

                if (ModelState.IsValid)
                {
                    
                    {
                        // Thêm sản phẩm vào cơ sở dữ liệu
                        entities1.CHITIETSPs.Add(ctsanpham);
                        entities1.SaveChanges();
                    }
                    return RedirectToAction("Index");
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

                        return RedirectToAction("Index");
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

                        return RedirectToAction("Index");
                    }
                }
                // Handle the case where the product was not found or there was an issue with saving.
            }
            // Handle the case where the model state is not valid (e.g., validation errors).
            return View(updatedProduct);
        }
    }
}
