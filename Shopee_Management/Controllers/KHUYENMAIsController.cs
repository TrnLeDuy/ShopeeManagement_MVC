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
    public class KHUYENMAIsController : Controller
    {
        private TMDTdbEntities db = new TMDTdbEntities();

        // GET: KHUYENMAIs
        public ActionResult Index()
        {
            var kHUYENMAIs = db.KHUYENMAIs.Include(k => k.NGUOIBANHANG).Include(k => k.NHANVIEN);
            return View(kHUYENMAIs.ToList());
        }

        // GET: KHUYENMAIs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHUYENMAI kHUYENMAI = db.KHUYENMAIs.Find(id);
            if (kHUYENMAI == null)
            {
                return HttpNotFound();
            }
            return View(kHUYENMAI);
        }

        // GET: KHUYENMAIs/Create
        public ActionResult Create()
        {
            ViewBag.id_nbh = new SelectList(db.NGUOIBANHANGs, "id_nbh", "ten_cua_hang");
            ViewBag.id_nv = new SelectList(db.NHANVIENs, "id_nv", "ho_ten");
            return View();
        }

        // POST: KHUYENMAIs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_voucher,ty_le_giam,ngay_tao,ngay_bat_dau,ngay_ket_thuc,id_nbh,id_nv,soluong")] KHUYENMAI kHUYENMAI)
        {
            if (ModelState.IsValid)
            {
                db.KHUYENMAIs.Add(kHUYENMAI);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_nbh = new SelectList(db.NGUOIBANHANGs, "id_nbh", "ten_cua_hang", kHUYENMAI.id_nbh);
            ViewBag.id_nv = new SelectList(db.NHANVIENs, "id_nv", "ho_ten", kHUYENMAI.id_nv);
            return View(kHUYENMAI);
        }

        // GET: KHUYENMAIs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHUYENMAI kHUYENMAI = db.KHUYENMAIs.Find(id);
            if (kHUYENMAI == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_nbh = new SelectList(db.NGUOIBANHANGs, "id_nbh", "ten_cua_hang", kHUYENMAI.id_nbh);
            ViewBag.id_nv = new SelectList(db.NHANVIENs, "id_nv", "ho_ten", kHUYENMAI.id_nv);
            return View(kHUYENMAI);
        }

        // POST: KHUYENMAIs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_voucher,ty_le_giam,ngay_tao,ngay_bat_dau,ngay_ket_thuc,id_nbh,id_nv,soluong")] KHUYENMAI kHUYENMAI)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kHUYENMAI).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_nbh = new SelectList(db.NGUOIBANHANGs, "id_nbh", "ten_cua_hang", kHUYENMAI.id_nbh);
            ViewBag.id_nv = new SelectList(db.NHANVIENs, "id_nv", "ho_ten", kHUYENMAI.id_nv);
            return View(kHUYENMAI);
        }

        // GET: KHUYENMAIs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHUYENMAI kHUYENMAI = db.KHUYENMAIs.Find(id);
            if (kHUYENMAI == null)
            {
                return HttpNotFound();
            }
            return View(kHUYENMAI);
        }

        // POST: KHUYENMAIs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KHUYENMAI kHUYENMAI = db.KHUYENMAIs.Find(id);
            db.KHUYENMAIs.Remove(kHUYENMAI);
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
