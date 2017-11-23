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
    public class ClientBillController : EAController
    {
        public ActionResult Index(int? page, string ClientName)
        {
            if (ClientName?.Length > 0) page = 1;
            return View("Index", base.BaseIndex<ClientDet>(page, "CBillID,ClientName, Tdate,RetentionPerc,TaxPerc", "ClientBill as cb Inner Join Client as c on cb.ClientID = c.ClientID where ClientName like '%" + ClientName + "%'"));
        }



        // GET: Clients/Create
        public ActionResult Manage(int? id)
        {
            ViewBag.ClientID = new SelectList(db.Fetch<Client>("Select ClientID,ClientName from Client"), "ClientID", "ClientName");

            return View(base.BaseCreateEdit<ClientBill>(id, "CBillID"));
        }


        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage([Bind(Include = "CBillID,ClientID,Tdate,RetentionPerc,TaxPerc")] ClientBill cBill)
        {
            return base.BaseSave<ClientBill>(cBill, cBill.CBillID > 0);
        }

        public ActionResult Details(int? id)
        {
           // ViewBag.ClientBillDets = base.BaseCreateEdit<ClientBillDetail>(id, "CBillDetailID");
            ViewBag.gst = db.Fetch<decimal>("select TaxPerc From ClientBill",id);
            ViewBag.tan = db.FirstOrDefault<decimal>("select TANnumber From Config");
            ViewBag.pan = db.FirstOrDefault<decimal>("select PANnumber From Config");


            var viewdata = new Client_Bill
             {
                        ClientDets = db.FirstOrDefault<ClientDet>("Select CBillID,ClientName,Tdate,RetentionPerc,TaxPerc from ClientBill as cb Inner Join Client as c on cb.ClientID =c.ClientID where CBillID = @0", id),
                        ClientBillDets = db.Fetch<ClientBillDet>("Select * From ClientBillDetail  Where CBillID = @0", id)
                 
            };
                 decimal RetPerc = viewdata.ClientDets.RetentionPerc;
            decimal BefTaxDebit = db.FirstOrDefault<decimal>("select sum(Amount) as Amount from ClientBillDetail where BeforeTax = @0 and CBillID =@1 and DebitCredit =@2",1,id,1);
            decimal BefTaxCredit = db.FirstOrDefault<decimal>("select sum(Amount) as Amount from ClientBillDetail where BeforeTax = @0 and CBillID =@1 and DebitCredit =@2", 1, id, 0);
            decimal BefTax = BefTaxCredit - BefTaxDebit;

            decimal RetAmt = BefTax * RetPerc / 100;
            ViewBag.RetAmt = RetAmt;
            ViewBag.RetPerc = RetPerc;
                ViewBag.CBillID = id;
            ViewBag.gst = db.FirstOrDefault<decimal>("select TaxPerc From ClientBill",id);

            return View("Details", viewdata);            
        }
  
        [HttpPost]     
        public ActionResult Details([Bind(Include = "CBillDetailID,CBillID,Description,Amount,DebitCredit,BeforeTax")] ClientBillDetail clientBillDetail)
        {
            ViewBag.gst = db.FirstOrDefault<decimal>("select TaxPerc From ClientBill", clientBillDetail.CBillID);

            base.BaseSave<ClientBillDetail>(clientBillDetail, clientBillDetail.CBillDetailID > 0);
            return RedirectToAction("Details",new {id=clientBillDetail.CBillID });
        }
       
        public ActionResult ManageDetails(int? id)
        {
            ViewBag.ClientBillDets = base.BaseCreateEdit<ClientBillDetail>(id, "CBillDetailID");


            var viewdata = new Client_Bill
            {
                ClientDets = db.FirstOrDefault<ClientDet>("Select CBillID,ClientName,Tdate,RetentionPerc,TaxPerc from ClientBill as cb Inner Join Client as c on cb.ClientID =c.ClientID where CBillID = @0", id),
                ClientBillDets = db.Fetch<ClientBillDet>("Select * From ClientBillDetail  Where CBillID = @0", id)

            };
       


            return View("Details", viewdata);            
        }
  
        [HttpPost]     
        public ActionResult ManageDetails([Bind(Include = "CBillDetailID,CBillID,Description,Amount,DebitCredit,BeforeTax")] ClientBillDetail clientBillDetail)
        {
            base.BaseSave<ClientBillDetail>(clientBillDetail, clientBillDetail.CBillDetailID > 0);
       
            return RedirectToAction("Details",new { id = clientBillDetail.CBillID });
        }


        public ActionResult AutoCompleteItem(string term)
        {
            var h = db.Fetch<AutoCompleteData>($"Select ItemID as id, ItemName as value from Item where ItemName like '%{term}%'").ToList();
            return Json(h, JsonRequestBehavior.AllowGet);
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
