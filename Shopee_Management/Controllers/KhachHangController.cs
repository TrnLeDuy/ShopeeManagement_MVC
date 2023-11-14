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

        public ActionResult Index()
        {
            var khachHang = db.KHACHHANGs.Include(t => t.NGUOIBANHANGs);
            return View(khachHang.ToList());
        }
        public ActionResult Default()
        {
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
            return View();
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
            return View();
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


    }
}