using System;
using Shopee_Management.Helpers;
using Shopee_Management.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;

namespace Shopee_Management.Controllers
{
    public class KhachHangController : Controller
    {
        private TMDTdbEntities db = new TMDTdbEntities();

        public ActionResult Default()
        {
            // Kiểm tra xem có thông điệp lỗi không
            if (TempData["Error"] != null)
            {
                ViewBag.ErrorMessage = TempData["Error"].ToString();
            }
            return View();
        }


        public class NguoiBanHangInfo
        {
            public string id_nbh { get; set; }
            public string ten_cua_hang { get; set; }

        }


        [HttpPost]
        public ActionResult DangNhap([Bind(Include = "username, password")] KHACHHANG khachhang)
        {
            if(string.IsNullOrEmpty(khachhang.username))
            {
                return View(khachhang);
            }
            if (string.IsNullOrEmpty(khachhang.password))
            {
                return View(khachhang);
            }
            if(ModelState.IsValid)
            {
                string hashPassword = Hash.ComputeSha256(khachhang.password);
                //Tìm người dùng có tên đăng nhập và password
                var user = db.KHACHHANGs.FirstOrDefault(k => k.username == khachhang.username && k.password == hashPassword);

                if (user != null)
                {
                    if (user.tinh_trang_kh == 0)
                    {
                        TempData["Error"] = "Tài khoản này hiện tại đang không hiệu lực!";
                        return RedirectToAction("Default");
                    }
                    else
                    {
                        
                        Session["KhachHang"] = user;
                        Session["ID"] = user.id_kh;
                        Session["Username"] = user.username;
                        Session["HoTen"] = user.ho_ten;
                        Session["Avatar"] = user.avatar;
                        //Lấy id và tên cửa hàng 
                        string query = "SELECT id_nbh, ten_cua_hang FROM KHACHHANG, NGUOIBANHANG WHERE NGUOIBANHANG.id_kh = @p1";
                        string id_kh = user.id_kh; // Assuming user.id_kh is a string, adjust the type accordingly
                        var result = db.Database.SqlQuery<NguoiBanHangInfo>(query, new SqlParameter("@p1", id_kh)).FirstOrDefault();
                        if (result != null)
                        {
                            // Access the values of id_nbh and ten_cua_hang
                            string id_nbh = result.id_nbh;
                            string ten_cua_hang = result.ten_cua_hang;
                            // Store the values in the session
                            Session["StoreID"] = id_nbh;
                            Session["StoreName"] = ten_cua_hang;
                        }
                        TempData["Success"] = "Đăng nhập thành công !";
                        return Redirect("/TrangChu/Index");
                    }
                }
                else
                    TempData["Error"] = "Tên đăng nhập hoặc mật khẩu không đúng!";
            }
            return RedirectToAction("Default");
        }

        [HttpPost]
        public ActionResult DangKy([Bind(Include = "sdt, email, username, password")] KHACHHANG khachhang)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(khachhang.email)) { return View(khachhang); }
                if (string.IsNullOrEmpty(khachhang.sdt)) { return View(khachhang); }
                if (string.IsNullOrEmpty(khachhang.username)) { return View(khachhang); }
                if (string.IsNullOrEmpty(khachhang.password)) { return View(khachhang); }
                // Kiểm tra tài khoản hoặc email đã được đăng ký chưa
                var existingUser = db.KHACHHANGs.Where(k => k.username == khachhang.username || k.email == khachhang.email).FirstOrDefault(); ;
                if (existingUser == null)
                {
                    string hashPassword = Hash.ComputeSha256(khachhang.password);
                    db.Database.ExecuteSqlCommand(
                        "INSERT INTO KHACHHANG (sdt, email, username, password) VALUES (@p0, @p1, @p2, @p3)",
                        khachhang.sdt, khachhang.email, khachhang.username, hashPassword); 
                    TempData["Success"] = "Tạo tài khoản thành công !";
                    var newuser = db.KHACHHANGs.Where(k => k.username == khachhang.username).FirstOrDefault();
                    if (newuser != null)
                    {
                        Session["ID"] = newuser.id_kh;
                    }
                    return RedirectToAction("ChiTietKhachHang");
                }
            }
            TempData["Error"] = "Tạo tài khoản thất bại !";
            return RedirectToAction("Default");
        }

        public ActionResult DoiMatKhau()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DoiMatKhau(string oldPassword, string newPassword, string repeatPassword)
        {
            KHACHHANG khachHang = db.KHACHHANGs.Find(Session["ID"].ToString());

            if(khachHang == null)
            {
                return RedirectToAction("DangNhap");
            }
            if(newPassword != repeatPassword)
            {
                TempData["Error"] = "Mật khẩu mới không khớp với nhau!";
                return View();
            }
            if(khachHang.password != Hash.ComputeSha256(oldPassword))
            {
                TempData["Error"] = "Mật khẩu cũ không khớp!";
                return View();
            }

            khachHang.password = Hash.ComputeSha256(newPassword);
            db.Entry(khachHang).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Success"] = "Đổi mật khẩu thành công!";
            return RedirectToAction("DangXuat");
        }


        public ActionResult DangXuat()
        {
            Session["KhachHang"] = null;
            Session["ID"] = null;
            Session["Username"] = null;
            Session["HoTen"] = null;
            Session["Avatar"] = null;
            Session["StoreID"] = null;
            Session["StoreName"] = null;
            Session.Abandon();
            return Redirect("/TrangChu/Index");
        }

        [HttpGet]
        public ActionResult ChiTietKhachHang()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChiTietKhachHang([Bind(Include = "ho_ten, ngay_sinh, dia_chi, avatar")] KHACHHANG kh, HttpPostedFileBase avatarFile)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(kh.ho_ten)) { return View(kh); }
                if (string.IsNullOrEmpty(kh.dia_chi)) { return View(kh); }

                string id_kh = (string)Session["ID"];

                var existingKhachHang = db.KHACHHANGs.Find(id_kh);

                if (existingKhachHang != null)
                {
                    existingKhachHang.ho_ten = kh.ho_ten;
                    existingKhachHang.ngay_sinh = kh.ngay_sinh;
                    existingKhachHang.dia_chi = kh.dia_chi;

                    if (avatarFile != null && avatarFile.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(avatarFile.FileName);
                        var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                        avatarFile.SaveAs(path);
                        existingKhachHang.avatar = fileName;
                    }

                    db.SaveChanges();

                    TempData["Success"] = "Cập nhật thông tin hoàn tất!";
                    Session["ID"] = null;
                    return RedirectToAction("Default");
                }
            }

            return View(kh);
        }

        // Action để hiển thị form đăng ký cửa hàng
        public ActionResult DangKyCuaHang()
        {
            // Kiểm tra đăng nhập sử dụng Session
            if (Session["KhachHang"] == null)
            {
                // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
                return RedirectToAction("Default", "KhachHang");
            }
            return View();
        }

        // Action để xử lý đăng ký cửa hàng
        [HttpPost]
        public ActionResult DangKyCuaHang([Bind(Exclude = "id_nbh")] NGUOIBANHANG model, HttpPostedFileBase hinhCuaHang)
        {
            string idNguoiBanHang_New = Session["ID"] as string;

            if (ModelState.IsValid)
            {
                // Kiểm tra xem đã tồn tại tài khoản có cùng ID_chưa
                if (db.NGUOIBANHANGs.Any(nbh => nbh.id_nbh == model.id_nbh))
                {
                    ModelState.AddModelError("", "Tài khoản đã tồn tại.");
                    return View(model);
                }

                // Lưu hình cửa hàng vào thư mục và cập nhật tên hình trong model
                if (hinhCuaHang != null && hinhCuaHang.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(hinhCuaHang.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    hinhCuaHang.SaveAs(path);
                    model.hinh_cua_hang = fileName;
                }

                //Gán ID Khách hàng cho Chủ sở hữu của việc tạo Cửa hàng
                model.id_kh = idNguoiBanHang_New;

                // Thực hiện câu lệnh SQL để chèn dữ liệu vào bảng NGUOIBANHANG
                string sqlInsert = "INSERT INTO NGUOIBANHANG (ten_cua_hang, id_kh, sdt_ch, dia_chi_ch, email_ch) VALUES (@ten_cua_hang, @id_kh, @sdt_ch, @dia_chi_ch, @email_ch)";

                db.Database.ExecuteSqlCommand(sqlInsert,
                    new SqlParameter("@ten_cua_hang", model.ten_cua_hang),
                    new SqlParameter("@id_kh", model.id_kh),
                    new SqlParameter("@sdt_ch", model.sdt_ch),
                    new SqlParameter("@dia_chi_ch", model.dia_chi_ch),
                    new SqlParameter("@email_ch", model.email_ch));


                TempData["Success"] = "Đăng ký cửa hàng thành công!";
                return RedirectToAction("Index", "TrangChu"); // Chuyển hướng đến trang chủ hoặc trang khác tùy vào yêu cầu
            }

            // Nếu ModelState không hợp lệ, hiển thị lại form đăng ký với thông báo lỗi
            return View(model);
        }
    }
}