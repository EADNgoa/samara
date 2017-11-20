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
    public class ConfigsController : EAController
    {        
        // GET: Clients
        public ActionResult Index()
        {
           
            return View("Index", base.BaseIndex<Config>(null,"Config "));            
        }

       

        // GET: Clients/Create
        public ActionResult Manage(int? id)
        {
            return View(base.BaseCreateEdit<Config>(id, "ConfigID"));
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage([Bind(Include = "ConfigID,PANnumber,TANnumber,RowsPerPage")] Config config)
        {         
            return base.BaseSave<Config>(config, config.ConfigID >0);            
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
