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
    public class SHIPPERsController : Controller
    {
        private TMDTdbEntities db = new TMDTdbEntities();

        // GET: SHIPPERs
        public ActionResult Index()
        {
            var sHIPPERs = db.SHIPPERs.Include(s => s.CTYVANCHUYEN);
            return View(sHIPPERs.ToList());
        }

        // GET: SHIPPERs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SHIPPER sHIPPER = db.SHIPPERs.Find(id);
            if (sHIPPER == null)
            {
                return HttpNotFound();
            }
            return View(sHIPPER);
        }

        // GET: SHIPPERs/Create
        public ActionResult Create()
        {
            ViewBag.id_cty = new SelectList(db.CTYVANCHUYENs, "id_cty", "ten_cty");
            return View();
        }

        // POST: SHIPPERs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_shipper,ho_ten,sdt,email,dia_chi,trang_thai_shipper,id_cty")] SHIPPER sHIPPER)
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

        // GET: SHIPPERs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SHIPPER sHIPPER = db.SHIPPERs.Find(id);
            if (sHIPPER == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_cty = new SelectList(db.CTYVANCHUYENs, "id_cty", "ten_cty", sHIPPER.id_cty);
            return View(sHIPPER);
        }

        // POST: SHIPPERs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_shipper,ho_ten,sdt,email,dia_chi,trang_thai_shipper,id_cty")] SHIPPER sHIPPER)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sHIPPER).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_cty = new SelectList(db.CTYVANCHUYENs, "id_cty", "ten_cty", sHIPPER.id_cty);
            return View(sHIPPER);
        }

        // GET: SHIPPERs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SHIPPER sHIPPER = db.SHIPPERs.Find(id);
            if (sHIPPER == null)
            {
                return HttpNotFound();
            }
            return View(sHIPPER);
        }

        // POST: SHIPPERs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SHIPPER sHIPPER = db.SHIPPERs.Find(id);
            db.SHIPPERs.Remove(sHIPPER);
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
