using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Shopee_Management.Models;

namespace Shopee_Management.Controllers
{
    public class TINTUCsController : Controller
    {
        private TMDTdbEntities1 db = new TMDTdbEntities1();

        // GET: TINTUCs
        public ActionResult Index()
        {
            var tINTUCs = db.TINTUCs.Include(t => t.THELOAITIN);
            return View(tINTUCs.ToList());
        }

        // GET: TINTUCs/Details/5

        /*        public ActionResult Details(int? id)
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
                }*/

        //--------------------------------------------
        [HttpGet]
        public ActionResult Details(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TINTUC tINTUC = db.TINTUCs.Find(id); // Lấy dữ liệu tin tức từ cơ sở dữ liệu dựa trên id.
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_tin_tuc,tieu_de,noi_dung,ngay_dang,id_theloai")] TINTUC tINTUC)
        {
            if (ModelState.IsValid)
            {
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_tin_tuc,tieu_de,noi_dung,ngay_dang,id_theloai")] TINTUC tINTUC)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tINTUC).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_theloai = new SelectList(db.THELOAITINs, "id_the_loai", "ten_the_loai", tINTUC.id_theloai);
            return View(tINTUC);
        }
        //-----------------------------------------------------------------------------------------------

        /*   public ActionResult Edit(int? id)
           {
               if (id == null)
               {
                   return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
               }
               TINTUC tintuc = db.TINTUCs.Find(id);
               if (tintuc == null)
               {
                   return HttpNotFound();
               }
               return PartialView("_Edit", tintuc);
           }*/
        /*        public ActionResult Edit(int id)
                {
                    // Lấy dữ liệu cần thiết, bao gồm danh sách thể loại
                    var model = db.TINTUCs.Find(id); // Thay thế db.TINTUCs bằng context của bạn
                    var theLoaiList = db.THELOAITINs.ToList(); // Thay thế db.THELOAITINs bằng context của bạn

                    // Chuyển danh sách thể loại vào ViewBag
                    ViewBag.TheloaiList = new SelectList(theLoaiList, "id_the_loai", "ten_the_loai");

                    return PartialView("_Edit", model);
                }


                // POST: TINTUCs/Edit/5
                [HttpPost]
                [ValidateAntiForgeryToken]
                public ActionResult Edit([Bind(Include = "id_tin_tuc,tieu_de,noi_dung,ngay_dang,id_the_loai")] TINTUC tintuc)
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(tintuc).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    return PartialView("_Edit", tintuc);
                }*/
        //reup


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
        //---------------------------------------------------------------------------------
        // GET: TINTUCs/Delete/5
        /*        public ActionResult Delete(int? id)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    TINTUC tintuc = db.TINTUCs.Find(id);
                    if (tintuc == null)
                    {
                        return HttpNotFound();
                    }
                    return PartialView("_Delete", tintuc);
                }

                // POST: TINTUCs/Delete/5
                [HttpPost, ActionName("_Delete")]
                [ValidateAntiForgeryToken]
                public ActionResult DeleteConfirmed(int id)
                {
                    TINTUC tintuc = db.TINTUCs.Find(id);
                    db.TINTUCs.Remove(tintuc);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }*/




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


