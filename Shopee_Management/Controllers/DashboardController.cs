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

        public ActionResult DailyChart(DateTime chosenDate)
        {
            var dailyRevenue = CalculateDailyRevenue(chosenDate);
            return Json(new { Revenue = dailyRevenue }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult WeeklyChart(DateTime chosenWeek)
        {
            var weeklyRevenue = CalculateWeeklyRevenue(chosenWeek);
            return Json(new { Revenue = weeklyRevenue }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MonthlyChart(int year, int month)
        {
            var monthlyRevenue = CalculateMonthlyRevenue(year, month);
            return Json(new { Revenue = monthlyRevenue }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult YearlyChart(int year)
        {
            var yearlyRevenue = CalculateYearlyRevenue(year);
            return Json(new { Revenue = yearlyRevenue }, JsonRequestBehavior.AllowGet);
        }

        private double CalculateDailyRevenue(DateTime chosenDate)
        {
            DateTime startDate = chosenDate.Date;
            DateTime endDate = startDate.AddDays(1).AddTicks(-1);

            double dailyRevenue = db.DONHANGs
                .Where(d => d.ngay_dat >= startDate && d.ngay_dat <= endDate)
                .Sum(d => (double?)d.thanh_tien) ?? 0;

            return dailyRevenue;
        }

        private double CalculateWeeklyRevenue(DateTime chosenWeek)
        {
            DateTime startDateOfWeek = chosenWeek.AddDays(-(int)chosenWeek.DayOfWeek);
            DateTime endDateOfWeek = startDateOfWeek.AddDays(6);

            double weeklyRevenue = db.DONHANGs
                .Where(d => d.ngay_dat >= startDateOfWeek && d.ngay_dat <= endDateOfWeek)
                .Sum(d => (double?)d.thanh_tien) ?? 0;

            return weeklyRevenue;
        }

        private double CalculateMonthlyRevenue(int year, int month)
        {
            DateTime startDateOfMonth = new DateTime(year, month, 1);
            DateTime endDateOfMonth = startDateOfMonth.AddMonths(1).AddDays(-1);

            double monthlyRevenue = db.DONHANGs
                .Where(d => d.ngay_dat >= startDateOfMonth && d.ngay_dat <= endDateOfMonth)
                .Sum(d => (double?)d.thanh_tien) ?? 0;

            return monthlyRevenue;
        }

        private double CalculateYearlyRevenue(int year)
        {
            DateTime startDateOfYear = new DateTime(year, 1, 1);
            DateTime endDateOfYear = startDateOfYear.AddYears(1).AddDays(-1);

            double yearlyRevenue = db.DONHANGs
                .Where(d => d.ngay_dat >= startDateOfYear && d.ngay_dat <= endDateOfYear)
                .Sum(d => (double?)d.thanh_tien) ?? 0;

            return yearlyRevenue;
        }
    }
}
