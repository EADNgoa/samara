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
    [Authorize(Roles = "Boss")]
    public class SitesController : EAController
    {
        // GET: Clients
        public ActionResult Index(int? page ,string SiteName )

        {
            if (SiteName?.Length > 0) page = 1;
            return View("Index", base.BaseIndex<SiteDetail>(page, "SiteID,Name,SiteName", "Sites Inner join Project on Sites.projectId=Project.ProjectID where SiteName like '%" + SiteName + "%'"));
        }



        // GET: Clients/Create
        public ActionResult Manage(int? id)
        {
            ViewBag.ProjectID = new SelectList(db.Fetch<Project>("Select ProjectID,Name from Project"), "ProjectID", "Name");
                        
            return View(base.BaseCreateEdit<Site>(id, "SiteID"));
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage([Bind(Include = "SiteID,ProjectID,SiteName")] Site site)
        {            
            return base.BaseSave<Site>(site, site.SiteID > 0);
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
