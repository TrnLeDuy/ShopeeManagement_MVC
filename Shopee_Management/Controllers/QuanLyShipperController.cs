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
    public class QuanLyShipperController : Controller
    {
        private TMDTdbEntities db = new TMDTdbEntities();

        // GET: QuanLyShipper
        public ActionResult Index()
        {
            var shipper = db.SHIPPERs.Include(s => s.CTYVANCHUYEN);
            return View(shipper.ToList());
        }

        // GET: QuanLyShipper/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SHIPPER shipper = db.SHIPPERs.Find(id);
            if (shipper == null)
            {
                return HttpNotFound();
            }
            return View(shipper);
        }

        // GET: QuanLyShipper/Create
        public ActionResult ThemShipper()
        {
            ViewBag.id_cty = new SelectList(db.CTYVANCHUYENs, "id_cty", "ten_cty");
            return View();
        }

        // POST: QuanLyShipper/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemShipper([Bind(Include = "id_shipper,ho_ten,sdt,email,dia_chi,trang_thai_shipper,id_cty")] SHIPPER sHIPPER)
        {
            if (ModelState.IsValid)
            {
                db.SHIPPERs.Add(sHIPPER);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_cty = new SelectList(db.CTYVANCHUYENs, "id_cty", "ten_cty", sHIPPER.id_cty);
            return View(sHIPPER);
        }

        // GET: QuanLyShipper/Edit/5
        public ActionResult CapNhatShipper(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SHIPPER shipper = db.SHIPPERs.Find(id);
            if (shipper == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_cty = new SelectList(db.CTYVANCHUYENs, "id_cty", "ten_cty", shipper.id_cty);
            return View(shipper);
        }

        // POST: QuanLyShipper/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CapNhatShipper([Bind(Include = "id_shipper,ho_ten,sdt,email,dia_chi,trang_thai_shipper,id_cty")] SHIPPER shipper)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shipper).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_cty = new SelectList(db.CTYVANCHUYENs, "id_cty", "ten_cty", shipper.id_cty);
            return View(shipper);
        }

        // GET: QuanLyShipper/Delete/5
        public ActionResult XoaShipper(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SHIPPER shipper = db.SHIPPERs.Find(id);
            if (shipper == null)
            {
                return HttpNotFound();
            }
            return View(shipper);
        }

        // POST: QuanLyShipper/Delete/5
        [HttpPost, ActionName("XoaShipper")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SHIPPER shipper = db.SHIPPERs.Find(id);
            db.SHIPPERs.Remove(shipper);
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
