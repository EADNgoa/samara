using System;
//using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
//using System.Web;
using System.Web.Mvc;


namespace Samara.Controllers
{
    public class ItemsController : EAController
    {
        // GET: Clients
        public ActionResult Index(int? page ,string ItemName )
        {
            if (ItemName?.Length > 0) page = 1;
            return View("Index", base.BaseIndex<ItemDetail>(page, "ItemId, ItemName, UnitName, ReorderLevel, TaxPerc", "Item inner join Units on item.unitId=Units.UnitId where ItemName like '%" + ItemName + "%'"));
        }



        // GET: Clients/Create
        public ActionResult Manage(int? id)
        {
            ViewBag.UnitID = new SelectList(db.Fetch<Unit>("Select UnitID,UnitName from Units"), "UnitID", "UnitName");
            
            return View(base.BaseCreateEdit<Item>(id, "ItemID"));
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage([Bind(Include = "ItemID,ItemName,UnitID,ReorderLevel,TaxPerc")] Item item)
        {            
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
