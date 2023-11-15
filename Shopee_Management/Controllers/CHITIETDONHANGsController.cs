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
    public class CHITIETDONHANGsController : Controller
    {
        private TMDTdbEntities db = new TMDTdbEntities();

        // GET: CHITIETDONHANGs
        public ActionResult Index()
        {
            var cHITIETDONHANGs = db.CHITIETDONHANGs.Include(c => c.CHITIETSP).Include(c => c.DONHANG);
            return View(cHITIETDONHANGs.ToList());
        }

        // GET: CHITIETDONHANGs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CHITIETDONHANG cHITIETDONHANG = db.CHITIETDONHANGs.Find(id);
            if (cHITIETDONHANG == null)
            {
                return HttpNotFound();
            }
            return View(cHITIETDONHANG);
        }

        // GET: CHITIETDONHANGs/Create
        public ActionResult Create()
        {
            ViewBag.id_ctsp = new SelectList(db.CHITIETSPs, "id_ctsp", "id_nbh");
            ViewBag.id_don = new SelectList(db.DONHANGs, "id_don", "id_kh");
            return View();
        }

        // POST: CHITIETDONHANGs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_don,id_ctsp,id_nbh,so_luong,gia_tien")] CHITIETDONHANG cHITIETDONHANG)
        {
            if (ModelState.IsValid)
            {
                db.CHITIETDONHANGs.Add(cHITIETDONHANG);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_ctsp = new SelectList(db.CHITIETSPs, "id_ctsp", "id_nbh", cHITIETDONHANG.id_ctsp);
            ViewBag.id_don = new SelectList(db.DONHANGs, "id_don", "id_kh", cHITIETDONHANG.id_don);
            return View(cHITIETDONHANG);
        }

        // GET: CHITIETDONHANGs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CHITIETDONHANG cHITIETDONHANG = db.CHITIETDONHANGs.Find(id);
            if (cHITIETDONHANG == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_ctsp = new SelectList(db.CHITIETSPs, "id_ctsp", "id_nbh", cHITIETDONHANG.id_ctsp);
            ViewBag.id_don = new SelectList(db.DONHANGs, "id_don", "id_kh", cHITIETDONHANG.id_don);
            return View(cHITIETDONHANG);
        }

        // POST: CHITIETDONHANGs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_don,id_ctsp,id_nbh,so_luong,gia_tien")] CHITIETDONHANG cHITIETDONHANG)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cHITIETDONHANG).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_ctsp = new SelectList(db.CHITIETSPs, "id_ctsp", "id_nbh", cHITIETDONHANG.id_ctsp);
            ViewBag.id_don = new SelectList(db.DONHANGs, "id_don", "id_kh", cHITIETDONHANG.id_don);
            return View(cHITIETDONHANG);
        }

        // GET: CHITIETDONHANGs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CHITIETDONHANG cHITIETDONHANG = db.CHITIETDONHANGs.Find(id);
            if (cHITIETDONHANG == null)
            {
                return HttpNotFound();
            }
            return View(cHITIETDONHANG);
        }

        // POST: CHITIETDONHANGs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CHITIETDONHANG cHITIETDONHANG = db.CHITIETDONHANGs.Find(id);
            db.CHITIETDONHANGs.Remove(cHITIETDONHANG);
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
