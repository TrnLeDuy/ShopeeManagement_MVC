using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Shopee_Management.Models;

namespace Shopee_Management.Controllers
{
    public class DONHANGsController : Controller
    {
        private TMDTdbEntities db = new TMDTdbEntities();

        // GET: DONHANGs
        public ActionResult Index()
        {
            var dONHANGs = db.DONHANGs.Include(d => d.KHACHHANG).Include(d => d.NGUOIBANHANG).Include(d => d.PTTT).Include(d => d.KHUYENMAI);
            return View(dONHANGs.ToList());
        }

        // GET: DONHANGs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DONHANG dONHANG = db.DONHANGs.Find(id);
            if (dONHANG == null)
            {
                return HttpNotFound();
            }
            return View(dONHANG);
        }

        // GET: DONHANGs/Create
        public ActionResult Create()
        {
            ViewBag.id_kh = new SelectList(db.KHACHHANGs, "id_kh", "ho_ten");
            ViewBag.id_nbh = new SelectList(db.NGUOIBANHANGs, "id_nbh", "ten_cua_hang");
            ViewBag.id_pttt = new SelectList(db.PTTTs, "id_pttt", "ten_pttt");
            ViewBag.id_voucher = new SelectList(db.KHUYENMAIs, "id_voucher", "id_nbh");
            return View();
        }

        // POST: DONHANGs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_don,ngay_dat,trang_thai_dh,tt_thanh_toan,tong_cong,thanh_tien,id_pttt,id_kh,id_nbh,id_voucher")] DONHANG dONHANG)
        {
            if (ModelState.IsValid)
            {
                db.DONHANGs.Add(dONHANG);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_kh = new SelectList(db.KHACHHANGs, "id_kh", "ho_ten", dONHANG.id_kh);
            ViewBag.id_nbh = new SelectList(db.NGUOIBANHANGs, "id_nbh", "ten_cua_hang", dONHANG.id_nbh);
            ViewBag.id_pttt = new SelectList(db.PTTTs, "id_pttt", "ten_pttt", dONHANG.id_pttt);
            ViewBag.id_voucher = new SelectList(db.KHUYENMAIs, "id_voucher", "id_nbh", dONHANG.id_voucher);
            return View(dONHANG);
        }

        // GET: DONHANGs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DONHANG dONHANG = db.DONHANGs.Find(id);
            if (dONHANG == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_kh = new SelectList(db.KHACHHANGs, "id_kh", "ho_ten", dONHANG.id_kh);
            ViewBag.id_nbh = new SelectList(db.NGUOIBANHANGs, "id_nbh", "ten_cua_hang", dONHANG.id_nbh);
            ViewBag.id_pttt = new SelectList(db.PTTTs, "id_pttt", "ten_pttt", dONHANG.id_pttt);
            ViewBag.id_voucher = new SelectList(db.KHUYENMAIs, "id_voucher", "id_nbh", dONHANG.id_voucher);
            return View(dONHANG);
        }

        // POST: DONHANGs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_don,ngay_dat,trang_thai_dh,tt_thanh_toan,tong_cong,thanh_tien,id_pttt,id_kh,id_nbh,id_voucher")] DONHANG dONHANG)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dONHANG).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_kh = new SelectList(db.KHACHHANGs, "id_kh", "ho_ten", dONHANG.id_kh);
            ViewBag.id_nbh = new SelectList(db.NGUOIBANHANGs, "id_nbh", "ten_cua_hang", dONHANG.id_nbh);
            ViewBag.id_pttt = new SelectList(db.PTTTs, "id_pttt", "ten_pttt", dONHANG.id_pttt);
            ViewBag.id_voucher = new SelectList(db.KHUYENMAIs, "id_voucher", "id_nbh", dONHANG.id_voucher);
            return View(dONHANG);
        }

        // GET: DONHANGs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DONHANG dONHANG = db.DONHANGs.Find(id);
            if (dONHANG == null)
            {
                return HttpNotFound();
            }
            return View(dONHANG);
        }

        // POST: DONHANGs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DONHANG dONHANG = db.DONHANGs.Find(id);
            db.DONHANGs.Remove(dONHANG);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
