using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using Samara.Models;

namespace Samara.Controllers
{
    public class ItemsController : EAController
    {
        // GET: Clients
        public ActionResult Index(int? page ,string ItemName )
        {
            return View("Index", base.BaseIndex<Item>(page, "Item where ItemName like '%" + ItemName + "%'"));
        }



        // GET: Clients/Create
        public ActionResult Manage(int? id)
        {
            ViewBag.UnitID = new SelectList(db.Fetch<Unit>("Select UnitID,UnitName from Units"));
            return View(base.BaseCreateEdit<Item>(id, "ItemID"));
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage([Bind(Include = "ItemID,ItemName,UnitID,ReorderLevel,TaxPerc")] Item item)
        {
            ViewBag.UnitID = new SelectList(db.Fetch<Unit>("Select UnitID,UnitName from Units"),item.UnitID);
            return base.BaseSave<Item>(item, item.ItemID > 0);
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
