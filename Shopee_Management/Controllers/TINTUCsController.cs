using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Windows.Interop;
using Shopee_Management.Models;

namespace Shopee_Management.Controllers
{
    public class TINTUCsController : Controller
    {
        private TMDTdbEntities db = new TMDTdbEntities();

        // GET: TINTUCs
        public ActionResult Index()
        {
            var tINTUCs = db.TINTUCs.Include(t => t.THELOAITIN);
            return View(tINTUCs.ToList());
        }

        // GET: TINTUCs/Details/5

        
        [HttpGet]
        public ActionResult Details(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tINTUC = db.TINTUCs.Find(id); // Lấy dữ liệu tin tức từ cơ sở dữ liệu dựa trên id.
            if (tINTUC == null)
            {
                return HttpNotFound(); // Hoặc trả về một trang lỗi 404 nếu không tìm thấy tin tức.
            }
            return PartialView("_Details", tINTUC); // Trả về một PartialView cho nội dung chi tiết.
        }




        // GET: TINTUCs/Create
        public ActionResult Create()
        {
            ViewBag.id_theloai = new SelectList(db.THELOAITINs, "id_the_loai", "ten_the_loai");
            return View();
        }

        // POST: TINTUCs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_tin_tuc,tieu_de,noi_dung,ngay_dang,id_theloai,image_tintuc")] TINTUC tINTUC, HttpPostedFileBase image_tintuc)
        {
            if (ModelState.IsValid)
            {
                if(image_tintuc != null)
                {
                    var fileName = Path.GetFileName(image_tintuc.FileName);

                    var path = Path.Combine(Server.MapPath("imagesTINTUC") + fileName);
                    tINTUC.image_tintuc = fileName;
                    image_tintuc.SaveAs(path);
                }
                db.TINTUCs.Add(tINTUC);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_theloai = new SelectList(db.THELOAITINs, "id_the_loai", "ten_the_loai", tINTUC.id_theloai);
            return View(tINTUC);
        }

        // GET: TINTUCs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TINTUC tINTUC = db.TINTUCs.Find(id);
            if (tINTUC == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_theloai = new SelectList(db.THELOAITINs, "id_the_loai", "ten_the_loai", tINTUC.id_theloai);
            return View(tINTUC);
        }

        // POST: TINTUCs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_tin_tuc,tieu_de,noi_dung,ngay_dang,id_theloai,image_tintuc")] TINTUC tINTUC, HttpPostedFileBase image_tintuc)
        {
            if (ModelState.IsValid)
            {
                if (image_tintuc != null)
                {
                    var fileName = Path.GetFileName(image_tintuc.FileName);

                    var path = Path.Combine(Server.MapPath("imagesTINTUC"), fileName);
                    tINTUC.image_tintuc = fileName;
                    image_tintuc.SaveAs(path);
                }

                db.Entry(tINTUC).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_theloai = new SelectList(db.THELOAITINs, "id_the_loai", "ten_the_loai", tINTUC.id_theloai);
            return View(tINTUC);
        }
       


        // GET: TINTUCs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TINTUC tINTUC = db.TINTUCs.Find(id);
            if (tINTUC == null)
            {
                return HttpNotFound();
            }
            return View(tINTUC);
        }

        // POST: TINTUCs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TINTUC tINTUC = db.TINTUCs.Find(id);
            db.TINTUCs.Remove(tINTUC);
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


