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
    public class UnitsController : EAController
    {
        // GET: Clients
        public ActionResult Index(int? page, string UnitName)
        {
            return View("Index", base.BaseIndex<Unit>(page, "Units where UnitName like '%" + UnitName + "%'"));
        }



        // GET: Clients/Create
        public ActionResult Manage(int? id)
        {
            return View(base.BaseCreateEdit<Unit>(id, "UnitID"));
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage([Bind(Include = "UnitID,UnitName")] Unit unit)
        {
            return base.BaseSave<Unit>(unit, unit.UnitID > 0);
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
