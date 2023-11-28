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
    public class USERTINTUCsController : Controller
    {
        private TMDTdbEntities1 db = new TMDTdbEntities1();

        // GET: USERTINTUCs
        public ActionResult Index()
        {
            var tINTUCs = db.TINTUCs.Include(t => t.THELOAITIN);
            return View(tINTUCs.ToList());
        }

        // GET: USERTINTUCs/Details/5
        public ActionResult Details(int? id)
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
    }

}
