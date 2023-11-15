using OfficeOpenXml;
using PagedList;
using Shopee_Management.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shopee_Management.Controllers
{
    public class QuanLyKhachHangController : Controller
    {
        private TMDTdbEntities db = new TMDTdbEntities();

        public FileResult ExportToExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // or LicenseContext.Commercial

            var data = db.KHACHHANGs.ToList(); // Replace with your data retrieval logic

            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Add("QuanLyKhachHang");

                // Add headers
                worksheet.Cells["A1"].Value = "Mã Khách Hàng";
                worksheet.Cells["B1"].Value = "Họ và tên Khách";
                worksheet.Cells["C1"].Value = "Số điện thoại";
                worksheet.Cells["D1"].Value = "Sinh nhật";
                worksheet.Cells["E1"].Value = "Địa chỉ";
                worksheet.Cells["F1"].Value = "Điểm tích lũy";

                // Fill data
                for (int i = 0; i < data.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = data[i].id_kh;
                    worksheet.Cells[i + 2, 2].Value = data[i].ho_ten;
                    worksheet.Cells[i + 2, 3].Value = data[i].sdt;
                    worksheet.Cells[i + 2, 4].Value = data[i].ngay_sinh?.ToString("dd/MM/yyyy");
                    worksheet.Cells[i + 2, 5].Value = data[i].dia_chi;
                    worksheet.Cells[i + 2, 6].Value = data[i].diem_tich_luy;
                }

                package.Save();
            }

            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "khachhang.xlsx");
        }

        //Quản lý Danh sách Khách hàng
        public ActionResult Index(int? page)
        {
            int pageSize = 25;
            int pageNumber = (page ?? 1);

            var khachHangs = db.KHACHHANGs.OrderBy(kh => kh.id_kh).ToPagedList(pageNumber, pageSize);
            return View(khachHangs);
        }

        // GET: QuanLyKhachHang/ChinhSuaTrangThai/id
        public ActionResult ChinhSuaTrangThai(string id)
        {
            var khachHang = db.KHACHHANGs.Find(id);

            if (khachHang == null)
            {
                return HttpNotFound();
            }

            return View(khachHang);
        }

        // POST: QuanLyKhachHang/ChinhSuaTrangThai/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChinhSuaTrangThai(string id, KHACHHANG khachHang)
        {
            if (ModelState.IsValid)
            {
                var existingKhachHang = db.KHACHHANGs.Find(id);

                if (existingKhachHang != null)
                {
                    existingKhachHang.tinh_trang_kh = khachHang.tinh_trang_kh;
                    db.SaveChanges();

                    return RedirectToAction("Index"); // Chuyển hướng sau khi cập nhật
                }
            }

            return View(khachHang);
        }

        public ActionResult ChiTiet(string id)
        {
            // Lấy thông tin khách hàng từ id
            var khachHang = db.KHACHHANGs.Find(id);

            if (khachHang == null)
            {
                // Xử lý khi không tìm thấy khách hàng
                return HttpNotFound();
            }

            return View(khachHang);
        }

    }
}