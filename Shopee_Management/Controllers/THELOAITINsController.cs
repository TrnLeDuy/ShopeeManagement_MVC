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
    public class THELOAITINsController : Controller
    {
        private TMDTdbEntities1 db = new TMDTdbEntities1();

        // GET: THELOAITINs
        public ActionResult Index()
        {
            return View(db.THELOAITINs.ToList());
        }

        // GET: THELOAITINs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            THELOAITIN tHELOAITIN = db.THELOAITINs.Find(id);
            if (tHELOAITIN == null)
            {
                return HttpNotFound();
            }
            return View(tHELOAITIN);
        }

        // GET: THELOAITINs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: THELOAITINs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_the_loai,ten_the_loai")] THELOAITIN tHELOAITIN)
        {
            if (ModelState.IsValid)
            {
                db.THELOAITINs.Add(tHELOAITIN);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tHELOAITIN);
        }

        // GET: THELOAITINs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            THELOAITIN tHELOAITIN = db.THELOAITINs.Find(id);
            if (tHELOAITIN == null)
            {
                return HttpNotFound();
            }
            return View(tHELOAITIN);
        }

        // POST: THELOAITINs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_the_loai,ten_the_loai")] THELOAITIN tHELOAITIN)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tHELOAITIN).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tHELOAITIN);
        }

        // GET: THELOAITINs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            THELOAITIN tHELOAITIN = db.THELOAITINs.Find(id);
            if (tHELOAITIN == null)
            {
                return HttpNotFound();
            }
            return View(tHELOAITIN);
        }

        // POST: THELOAITINs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            THELOAITIN tHELOAITIN = db.THELOAITINs.Find(id);
            db.THELOAITINs.Remove(tHELOAITIN);
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
