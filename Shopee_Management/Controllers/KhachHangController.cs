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
                var user = db.KHACHHANGs.FirstOrDefault(k => k.username == khachhang.username && k.password == khachhang.password);

                if (user != null)
                {
                    if (user.tinh_trang_kh == 0)
                    {
                        ViewBag.ThongBao = "Tài khoản này hiện tại đang không hiệu lực!";
                    }
                    else
                    {
                        Session["KhachHang"] = user;
                        Session["ID"] = user.id_kh;
                        Session["Username"] = user.username;
                        Session["HoTen"] = user.ho_ten;
                        Session["Avatar"] = user.avatar;
                        return Redirect("/TrangChu/Index");
                    }
                }
                else
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng!";
            }
            return View();
        }

        [HttpPost]
        public ActionResult DangKy([Bind(Include = "email, username, password")] KHACHHANG khachhang)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(khachhang.email)) { return View(khachhang); }
                if (string.IsNullOrEmpty(khachhang.username)) { return View(khachhang); }
                if (string.IsNullOrEmpty(khachhang.password)) { return View(khachhang); }
                // Kiểm tra tài khoản hoặc email đã được đăng ký chưa
                var existingUser = db.KHACHHANGs.Where(k => k.username == khachhang.username || k.email == khachhang.email).FirstOrDefault(); ;
                if (existingUser == null)
                {
                    db.Database.ExecuteSqlCommand(
                        "INSERT INTO KHACHHANG (email, username, password) VALUES (@p0, @p1, @p2)",
                        khachhang.email, khachhang.username, khachhang.password);
                    TempData["Success"] = "Tạo tài khoản thành công !";
                    return RedirectToAction("Default");
                }
            }
            TempData["Error"] = "Tạo tài khoản thất bại !";
            return View();
        }


        public ActionResult DangXuat()
        {
            Session["KhachHang"] = null;
            Session["Username"] = null;
            Session["HoTen"] = null;
            Session["Avatar"] = null;
            Session.Abandon();
            return RedirectToAction("/TrangChu/Index");
        }
    }
}