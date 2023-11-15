using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using OfficeOpenXml;
using PagedList;
using Shopee_Management.Models;

namespace Shopee_Management.Controllers
{
    public class QuanLyCuaHangController : Controller
    {
        private TMDTdbEntities db = new TMDTdbEntities();

        public FileResult ExportToExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // or LicenseContext.Commercial

            var data = db.NGUOIBANHANGs.ToList(); // Replace with your data retrieval logic

            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Add("QuanLyCuaHang");

                // Add headers
                worksheet.Cells["A1"].Value = "Mã Cửa hàng";
                worksheet.Cells["B1"].Value = "Tên Cửa hàng";
                worksheet.Cells["C1"].Value = "Tên Chủ Cửa hàng";
                worksheet.Cells["D1"].Value = "Số điện thoại";
                worksheet.Cells["E1"].Value = "Địa chỉ";
                worksheet.Cells["F1"].Value = "Email liên lạc";

                // Fill data
                for (int i = 0; i < data.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = data[i].id_nbh;
                    worksheet.Cells[i + 2, 2].Value = data[i].ten_cua_hang;
                    worksheet.Cells[i + 2, 3].Value = data[i].KHACHHANG.ho_ten;
                    worksheet.Cells[i + 2, 4].Value = data[i].KHACHHANG.sdt;
                    worksheet.Cells[i + 2, 5].Value = data[i].KHACHHANG.dia_chi;
                    worksheet.Cells[i + 2, 6].Value = data[i].KHACHHANG.email;
                }

                package.Save();
            }

            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "dscuahang.xlsx");
        }

        // GET: QuanLyCuaHang
        public ActionResult Index(int? page)
        {
            int pageSize = 25;
            int pageNumber = (page ?? 1);

            var cuaHangs = db.NGUOIBANHANGs.OrderBy(ch => ch.id_nbh).ToPagedList(pageNumber, pageSize);
            return View(cuaHangs);
        }

        // GET: QuanLyKhachHang/ChinhSuaTrangThai/id
        public ActionResult ChinhSuaTrangThai(string id)
        {
            var cuaHang = db.NGUOIBANHANGs.Find(id);

            if (cuaHang == null)
            {
                return HttpNotFound();
            }

            return View(cuaHang);
        }

        // POST: QuanLyKhachHang/ChinhSuaTrangThai/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChinhSuaTrangThai(string id, NGUOIBANHANG cuaHang)
        {
            if (ModelState.IsValid)
            {
                var existingCuaHang = db.NGUOIBANHANGs.Find(id);

                if (existingCuaHang != null)
                {
                    existingCuaHang.trang_thai_ch = cuaHang.trang_thai_ch;
                    db.SaveChanges();

                    return RedirectToAction("Index"); // Chuyển hướng sau khi cập nhật
                }
            }

            return View(cuaHang);
        }

        public ActionResult ChiTiet(string id)
        {
            // Lấy thông tin khách hàng từ id
            var cuaHang = db.NGUOIBANHANGs.Find(id);

            if (cuaHang == null)
            {
                // Xử lý khi không tìm thấy khách hàng
                return HttpNotFound();
            }

            return View(cuaHang);
        }
    }
}