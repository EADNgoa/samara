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
    public class RateAdditionController : EAController
    {        
        // GET: Clients
        public ActionResult Index(int? page, string PropName)
        {
            if (PropName?.Length > 0) page = 1;
            return View("Index", base.BaseIndex<RateAddition>(page, "RateAdditions where RateAdditionDesc like '%" + PropName+"%'"));            
        }

       

        // GET: Clients/Create
        public ActionResult Manage(int? id)
        {
            return View(base.BaseCreateEdit<RateAddition>(id, "RateAdditionID"));
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage([Bind(Include = "RateAdditionID,RateAdditionDesc,Percentage")] RateAddition rateAddition)
        {         
            return base.BaseSave<RateAddition>(rateAddition, rateAddition.RateAdditionID > 0);            
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
