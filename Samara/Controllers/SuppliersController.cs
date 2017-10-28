using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace Samara.Controllers
{
    public class SuppliersController : EAController
    {
        // GET: Clients
        public ActionResult Index(int? page, string SupName)
        {
            if (SupName?.Length > 0) page = 1;
            return View("Index", base.BaseIndex<Supplier>(page, "Supplier where SupplierName like '%" + SupName + "%'"));
        }



        // GET: Clients/Create
        public ActionResult Manage(int? id)
        {
            return View(base.BaseCreateEdit<Supplier>(id, "SupplierID"));
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage([Bind(Include = "SupplierID,SupplierName")] Supplier supplier)
        {
            return base.BaseSave<Supplier>(supplier, supplier.SupplierID > 0);
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
