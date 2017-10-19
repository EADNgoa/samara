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
    public class ClientsController : EAController
    {        
        // GET: Clients
        public ActionResult Index(int? page, string PropName)
        {            
            return View("Index", base.BaseIndex<Client>(page, "Client where ClientName like '%"+PropName+"%'"));            
        }

       

        // GET: Clients/Create
        public ActionResult Manage(int? id)
        {
            return View(base.BaseCreateEdit<Client>(id, "ClientId"));
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage([Bind(Include = "ClientId,ClientName,Address")] Client client)
        {         
            return base.BaseSave<Client>(client, client.ClientId >0);            
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
