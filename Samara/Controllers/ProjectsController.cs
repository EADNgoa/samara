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
    public class ProjectsController : EAController
    {
        // GET: Clients
        public ActionResult Index(int? page, string ProjName)
        {
            if (ProjName?.Length > 0) page = 1;
            return View("Index", base.BaseIndex<Project>(page, "Project where Name like '%" + ProjName + "%'"));
        }



        // GET: Clients/Create
        public ActionResult Manage(int? id)
        {
            return View(base.BaseCreateEdit<Project>(id, "ProjectID"));
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage([Bind(Include = "ProjectID,Name")] Project project)
        {
            return base.BaseSave<Project>(project, project.ProjectID > 0);
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
