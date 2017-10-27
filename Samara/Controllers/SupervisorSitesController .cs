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
    [Authorize(Roles = "Boss")]
    public class SupervisorSitesController : EAController
    {
        // GET: Clients
        public ActionResult Index(int? page,string SiteName  )
        {
             if(SiteName?.Length >0)page = 1;
            return View("Index", base.BaseIndex<SupervisorSitesDet>(page, "Email,SiteName", "AspNetUsers Inner join SupervisorSites on AspNetUsers.Id=SupervisorSites.UserID Left Outer Join Sites on Sites.SiteID = SupervisorSites.SiteID where SiteName like '%" + SiteName + "%' "));
        }



        // GET: Clients/Create
        public ActionResult Manage(int? Id)
        {
            ViewBag.UserID = new SelectList(db.Fetch<AspNetUser>("Select Id,Email from AspNetUSers"), "Id", "Email");
            ViewBag.SiteID = new SelectList(db.Fetch<Site>("Select SiteID,SiteName from Sites"), "SiteID", "SiteName");
            return View(base.BaseCreateEdit<SupervisorSite>(Id,"SiteID"));
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage([Bind(Include = "UserID,SiteID")] SupervisorSite supervisorSite)
        {
            return base.BaseSave<SupervisorSite>(supervisorSite, db.Exists<SupervisorSite>("UserId=@0 and SiteID=@1", supervisorSite.UserID, supervisorSite.SiteID));
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
