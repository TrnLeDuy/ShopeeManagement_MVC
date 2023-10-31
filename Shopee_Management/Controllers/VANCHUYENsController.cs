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
    public class VANCHUYENsController : Controller
    {
        private TMDTdbEntities db = new TMDTdbEntities();

        // GET: VANCHUYENs
        public ActionResult Index()
        {
            var vANCHUYENs = db.VANCHUYENs.Include(v => v.DONHANG).Include(v => v.SHIPPER);
            return View(vANCHUYENs.ToList());
        }

        // GET: VANCHUYENs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VANCHUYEN vANCHUYEN = db.VANCHUYENs.Find(id);
            if (vANCHUYEN == null)
            {
                return HttpNotFound();
            }
            return View(vANCHUYEN);
        }

        // GET: VANCHUYENs/Create
        public ActionResult Create()
        {
            ViewBag.id_don = new SelectList(db.DONHANGs, "id_don", "id_kh");
            ViewBag.id_shipper = new SelectList(db.SHIPPERs, "id_shipper", "ho_ten");
            return View();
        }

        // POST: VANCHUYENs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_vc,phi_vc,id_don,id_shipper,ngay_giao_hang,trang_thai_giao_hang")] VANCHUYEN vANCHUYEN)
        {
            if (ModelState.IsValid)
            {
                db.VANCHUYENs.Add(vANCHUYEN);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_don = new SelectList(db.DONHANGs, "id_don", "id_kh", vANCHUYEN.id_don);
            ViewBag.id_shipper = new SelectList(db.SHIPPERs, "id_shipper", "ho_ten", vANCHUYEN.id_shipper);
            return View(vANCHUYEN);
        }

        // GET: VANCHUYENs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VANCHUYEN vANCHUYEN = db.VANCHUYENs.Find(id);
            if (vANCHUYEN == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_don = new SelectList(db.DONHANGs, "id_don", "id_kh", vANCHUYEN.id_don);
            ViewBag.id_shipper = new SelectList(db.SHIPPERs, "id_shipper", "ho_ten", vANCHUYEN.id_shipper);
            return View(vANCHUYEN);
        }

        // POST: VANCHUYENs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_vc,phi_vc,id_don,id_shipper,ngay_giao_hang,trang_thai_giao_hang")] VANCHUYEN vANCHUYEN)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vANCHUYEN).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_don = new SelectList(db.DONHANGs, "id_don", "id_kh", vANCHUYEN.id_don);
            ViewBag.id_shipper = new SelectList(db.SHIPPERs, "id_shipper", "ho_ten", vANCHUYEN.id_shipper);
            return View(vANCHUYEN);
        }

        // GET: VANCHUYENs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VANCHUYEN vANCHUYEN = db.VANCHUYENs.Find(id);
            if (vANCHUYEN == null)
            {
                return HttpNotFound();
            }
            return View(vANCHUYEN);
        }

        // POST: VANCHUYENs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VANCHUYEN vANCHUYEN = db.VANCHUYENs.Find(id);
            db.VANCHUYENs.Remove(vANCHUYEN);
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
