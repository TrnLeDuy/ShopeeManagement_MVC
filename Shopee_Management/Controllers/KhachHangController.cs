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


        public ActionResult DangXuat()
        {
            Session["KhachHang"] = null;
            Session["ID"] = null;
            Session["Username"] = null;
            Session["HoTen"] = null;
            Session["Avatar"] = null;
            Session.Abandon();
            return Redirect("/TrangChu/Index");
        }

        [HttpGet]
        public ActionResult ChiTietKhachHang()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChiTietKhachHang([Bind(Include = "ho_ten, ngay_sinh, dia_chi, avatar")] KHACHHANG kh)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(kh.ho_ten)) { return View(kh); }
                if (string.IsNullOrEmpty(kh.dia_chi)) { return View(kh); }
                string id_kh = (string)Session["ID"];
                db.Database.ExecuteSqlCommand(
                    "UPDATE KHACHHANG SET ho_ten = @p0, ngay_sinh = @p1, dia_chi = @p2, avatar = @p3 WHERE id_kh = @p4",
                    kh.ho_ten, kh.ngay_sinh, kh.dia_chi, kh.avatar, id_kh);
                TempData["Success"] = "Cập nhật thông tin hoàn tất!";
                Session["ID"] = null;
                return RedirectToAction("Default");
            }
            return View(kh);
        }

    }
}