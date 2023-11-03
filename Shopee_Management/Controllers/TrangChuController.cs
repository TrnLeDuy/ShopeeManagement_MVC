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
    public class TrangChuController : Controller
    {
        private TMDTdbEntities db = new TMDTdbEntities();

        // GET: TrangChu

        public ActionResult Index()
        {
            var model = new ModelTrangChu
            {
                ctsp = db.CHITIETSPs
                    .Include(c => c.BANGMAU)
                    .Include(c => c.THUONGHIEU)
                    .Include(c => c.XUATXU)
                    .Include(c => c.NGUOIBANHANG)
                    .Include(c => c.NGANHHANG)
                    .Include(c => c.NGANHHANGCON)
                    .Include(c => c.NGANHHANGCAP3)
                    .Include(c => c.KICHCO)
                    .Include(c => c.SANPHAM)
                    .ToList(),
                nganhhang = db.NGANHHANGs.ToList()
            };

            return View(model);
        }
    }
}