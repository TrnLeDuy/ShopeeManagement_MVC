using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Shopee_Management.Models;

namespace Shopee_Management.Controllers
{
    public class RevenueController : Controller
    {
        private TMDTdbEntities db = new TMDTdbEntities();

        public ActionResult Daily(DateTime? chosenDate)
        {
            if (!chosenDate.HasValue)
            {
                chosenDate = DateTime.Today;
            }

            var dailyOrdersCount = GetDailyOrdersCount(chosenDate.Value);
            return View(dailyOrdersCount);
        }

        public ActionResult Weekly(DateTime? chosenWeek)
        {
            if (!chosenWeek.HasValue)
            {
                chosenWeek = DateTime.Today;
            }

            var weeklyOrdersCount = GetWeeklyOrdersCount(chosenWeek.Value);
            var weeklyLabels = GetWeeklyLabels(chosenWeek.Value);

            ViewBag.WeeklyLabels = weeklyLabels;
            return View(weeklyOrdersCount);
        }

        public ActionResult Monthly()
        {
            var monthlyRevenueData = GetMonthlyOrders();
            var monthlyLabels = GetMonthlyLabels();

            ViewBag.MonthlyLabels = monthlyLabels;
            return View(monthlyRevenueData);
        }

        public ActionResult Yearly()
        {
            var yearlyRevenueData = GetYearlyOrders();
            var yearlyLabels = GetYearlyLabels();

            ViewBag.YearlyLabels = yearlyLabels;
            return View(yearlyRevenueData);
        }


        private int GetDailyOrdersCount(DateTime chosenDate)
        {
            var startDate = chosenDate.Date;
            var endDate = startDate.AddDays(1);

            var dailyOrdersCount = db.DONHANGs
                .Where(d =>
                    d.ngay_dat >= startDate &&
                    d.ngay_dat < endDate)
                .Count();

            return dailyOrdersCount;
        }

        private List<int> GetWeeklyOrdersCount(DateTime chosenWeek)
        {
            DateTime startDateOfWeek = chosenWeek.AddDays(-(int)chosenWeek.DayOfWeek);
            DateTime endDateOfWeek = startDateOfWeek.AddDays(6);

            var weeklyOrdersCount = new List<int>();

            for (DateTime date = startDateOfWeek; date <= endDateOfWeek; date = date.AddDays(1))
            {
                var dailyOrdersCount = db.DONHANGs
                    .Where(d =>
                        d.ngay_dat.HasValue &&
                        d.ngay_dat.Value.Year == date.Year &&
                        d.ngay_dat.Value.Month == date.Month &&
                        d.ngay_dat.Value.Day == date.Day)
                    .Count();

                weeklyOrdersCount.Add(dailyOrdersCount);
            }

            return weeklyOrdersCount;
        }

        private List<string> GetWeeklyLabels(DateTime chosenWeek)
        {
            var startDateOfWeek = chosenWeek.Date.AddDays(-(int)chosenWeek.DayOfWeek);
            var endDateOfWeek = startDateOfWeek.AddDays(6);
            var labels = new List<string>();

            for (DateTime date = startDateOfWeek; date <= endDateOfWeek; date = date.AddDays(1))
            {
                labels.Add(date.ToShortDateString());
            }

            return labels;
        }

        private List<int> GetMonthlyOrders()
        {
            var today = DateTime.Today;
            var startDateOfMonth = new DateTime(today.Year, today.Month, 1);
            var endDateOfMonth = startDateOfMonth.AddMonths(1).AddDays(-1);

            var monthlyOrders = new List<int>();

            for (DateTime date = startDateOfMonth; date <= endDateOfMonth; date = date.AddDays(1))
            {
                var ordersCount = db.DONHANGs
                    .Count(d =>
                        d.ngay_dat.HasValue &&
                        d.ngay_dat.Value.Year == date.Year &&
                        d.ngay_dat.Value.Month == date.Month &&
                        d.ngay_dat.Value.Day == date.Day);

                monthlyOrders.Add(ordersCount);
            }

            return monthlyOrders;
        }


        private List<string> GetMonthlyLabels()
        {
            var today = DateTime.Today;
            var startDateOfMonth = new DateTime(today.Year, today.Month, 1);
            var endDateOfMonth = startDateOfMonth.AddMonths(1).AddDays(-1);

            var monthlyLabels = new List<string>();

            for (DateTime date = startDateOfMonth; date <= endDateOfMonth; date = date.AddDays(1))
            {
                monthlyLabels.Add(date.ToString("yyyy-MM-dd")); // Format the date
            }

            return monthlyLabels;
        }

        private List<int> GetYearlyOrders()
        {
            var today = DateTime.Today;
            var startDateOfYear = new DateTime(today.Year, 1, 1);
            var endDateOfYear = startDateOfYear.AddYears(1).AddDays(-1);

            var yearlyOrders = new List<int>();

            for (int month = 1; month <= 12; month++)
            {
                var ordersCount = db.DONHANGs
                    .Where(d =>
                        d.ngay_dat.HasValue &&
                        d.ngay_dat.Value.Year == today.Year &&
                        d.ngay_dat.Value.Month == month)
                    .Count();

                yearlyOrders.Add(ordersCount);
            }

            return yearlyOrders;
        }

        private List<string> GetYearlyLabels()
        {
            var today = DateTime.Today;
            var startDateOfYear = new DateTime(today.Year, 1, 1);
            var endDateOfYear = startDateOfYear.AddYears(1).AddDays(-1);

            var yearlyLabels = new List<string>();

            for (DateTime date = startDateOfYear; date <= endDateOfYear; date = date.AddDays(1))
            {
                yearlyLabels.Add(date.ToString("yyyy-MM-dd")); // Format the date
            }

            return yearlyLabels;
        }
    }
}
