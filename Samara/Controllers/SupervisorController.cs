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
    public class SupervisorsController : EAController
    {
        // GET: Clients
        public ActionResult Index(int? page, string SupName)
        {
            if (SupName?.Length > 0) page = 1;
            return View("Index", base.BaseIndex<Supervisor>(page, "Supervisor where Name like '%" + SupName + "%'"));
        }



        // GET: Clients/Create
        public ActionResult Manage(int? id)
        {
            return View(base.BaseCreateEdit<Supervisor>(id, "UserID"));
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage([Bind(Include = "UserID,Name")] Supervisor supervisor)
        {
            return base.BaseSave<Supervisor>(supervisor, supervisor.UserID > 0);
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
