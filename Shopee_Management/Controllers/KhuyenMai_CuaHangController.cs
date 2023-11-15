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
    public class KhuyenMai_CuaHangController : Controller
    {
        private TMDTdbEntities db = new TMDTdbEntities();


        // Hiển thị danh sách Voucher của người bán hàng
        public ActionResult Index()
        {
            // Lấy ID người bán hàng từ session
            string idNguoiBanHang = Session["StoreID"] as string;

            // Lấy danh sách Voucher của người bán hàng
            var voucherList = db.KHUYENMAIs.Where(v => v.id_nbh == idNguoiBanHang).ToList();

            return View(voucherList);
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
       
            return View();
        }

        // POST: KHUYENMAIs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_voucher,ty_le_giam,ngay_tao,ngay_bat_dau,ngay_ket_thuc,soluong")] KHUYENMAI voucher)
        {
            if (ModelState.IsValid)
            {
                // Lấy ID người bán hàng từ session
                string idNguoiBanHang = Session["StoreID"] as string;

                // Gán ID người bán hàng cho Voucher
                voucher.id_nbh = idNguoiBanHang;
                // Thêm Voucher mới vào cơ sở dữ liệu

                db.KHUYENMAIs.Add(voucher);
                db.SaveChanges();
                TempData["Success"] = "Thêm Voucher thành công!";
                return RedirectToAction("Index");
            }

          
            return View(voucher);
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
           
            return View(kHUYENMAI);
        }

        // POST: KHUYENMAIs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_voucher,ty_le_giam,ngay_tao,ngay_bat_dau,ngay_ket_thuc,soluong")] KHUYENMAI voucher)
        {
            if (ModelState.IsValid)
            {// Lấy ID người bán hàng từ session
                string idNguoiBanHang = Session["StoreID"] as string;

                // Gán ID người bán hàng cho Voucher
                voucher.id_nbh = idNguoiBanHang;

                db.Entry(voucher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
           
            return View(voucher);
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
