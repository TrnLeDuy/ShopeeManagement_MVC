using Shopee_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shopee_Management.Controllers
{
    public class DashboardController : Controller
    {
        private TMDTdbEntities db = new TMDTdbEntities();
        private double total = 0;
        private double loiNhuan = 0;
        // GET: Dashboard
        public ActionResult Dashboard()
        {
            foreach (var entity in db.DONHANGs)
            {
                total += (double)entity.thanh_tien;
            }
            loiNhuan = total * 0.4;
            ViewBag.loiNhuan = loiNhuan;
            ViewBag.countCuaHang = db.NGUOIBANHANGs.Count();
            ViewBag.countSanPham = db.SANPHAMs.Count();
            ViewBag.countDonHang = db.DONHANGs.Count();
            ViewBag.countKhachHang = db.KHACHHANGs.Count();
            ViewBag.countDoanhThu = total;
            return View();
        }
    }
}
