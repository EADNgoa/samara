using Microsoft.AspNet.Identity;
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
            return View("Index", base.BaseIndex<ClientDet>(page, "CBillID,ClientName,SiteName, Tdate,RetentionPerc,TaxPerc", "ClientBill as cb Inner Join Client as c on cb.ClientID = c.ClientID inner join sites s on s.SiteID = cb.SiteID where ClientName like '%" + ClientName + "%'"));
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
        public ActionResult Manage([Bind(Include = "CBillID,ClientID,SiteID,Tdate,RetentionPerc,TaxPerc")] ClientBill cBill)
        {
            return base.BaseSave<ClientBill>(cBill, cBill.CBillID > 0);
        }

        public ActionResult Details(int? id)
        {
           // ViewBag.ClientBillDets = base.BaseCreateEdit<ClientBillDetail>(id, "CBillDetailID");
            ViewBag.gst = db.Fetch<decimal>("select TaxPerc From ClientBill where CBillID = @0",id);
            var confi = db.FirstOrDefault<Config>("select * From Config");
            ViewBag.tan = confi.TANnumber;
            ViewBag.pan = confi.PANnumber;


            var viewdata = new Client_Bill
             {
                        ClientDets = db.FirstOrDefault<ClientDet>("Select CBillID,ClientName,Tdate,RetentionPerc,RetentionAmt, RetentionAmtIsPaid,TaxPerc from ClientBill as cb Inner Join Client as c on cb.ClientID =c.ClientID where CBillID = @0", id),
                        ClientBillDets = db.Fetch<ClientBillDet>("Select * From ClientBillDetail  Where CBillID = @0", id)
                 
            };
            
            ViewBag.RetAmt = viewdata.ClientDets.RetentionAmt;
            ViewBag.RetPerc = viewdata.ClientDets.RetentionPerc;
            ViewBag.RetAmtIsPAid = viewdata.ClientDets.RetentionAmtIsPaid;
            ViewBag.CBillID = id;
            ViewBag.gst = db.FirstOrDefault<decimal>("select TaxPerc From ClientBill",id);

            return View("Details", viewdata);            
        }
  
        [HttpPost]     
        public ActionResult Details([Bind(Include = "CBillDetailID,CBillID,Description,Amount,DebitCredit,BeforeTax")] ClientBillDetail clientBillDetail, decimal RetentionPerc)
        {
            ViewBag.gst = db.FirstOrDefault<decimal>("select TaxPerc From ClientBill", clientBillDetail.CBillID);

            using (var transaction = db.GetTransaction())
            {
                try
                {
                    base.BaseSave<ClientBillDetail>(clientBillDetail, clientBillDetail.CBillDetailID > 0);

                    //Set Retention Amount
                    decimal RetPerc = db.FirstOrDefault<decimal>("Select RetentionPerc From ClientBill Where CBillID = @0", clientBillDetail.CBillID);
                    decimal BefTaxDebit = db.FirstOrDefault<decimal>("select COALESCE(sum(Amount),0) as Amount from ClientBillDetail where BeforeTax = @0 and CBillID =@1 and DebitCredit =@2", 1, clientBillDetail.CBillID, 1);
                    decimal BefTaxCredit = db.FirstOrDefault<decimal>("select COALESCE(sum(Amount),0) as Amount from ClientBillDetail where BeforeTax = @0 and CBillID =@1 and DebitCredit =@2", 1, clientBillDetail.CBillID, 0);
                    decimal BefTax = BefTaxCredit - BefTaxDebit;
                    decimal RetAmt = BefTax * (RetPerc / 100);
                    decimal AftTaxDebit = db.FirstOrDefault<decimal>("select COALESCE(sum(Amount),0) as Amount from ClientBillDetail where BeforeTax = @0 and CBillID =@1 and DebitCredit =@2", 0, clientBillDetail.CBillID, 1);
                    decimal AftTaxCredit = db.FirstOrDefault<decimal>("select COALESCE(sum(Amount),0) as Amount from ClientBillDetail where BeforeTax = @0 and CBillID =@1 and DebitCredit =@2", 0, clientBillDetail.CBillID, 0);
                    decimal AftTax = AftTaxCredit - AftTaxDebit;
                    decimal GT = BefTax + AftTax;

                    var TaxPerc = db.FirstOrDefault<decimal>("Select TaxPerc From ClientBill where CBillID=@0", clientBillDetail.CBillID);
                    var TaxAmt = BefTax * (TaxPerc / 100);

                    db.Update("ClientBill", "CBillID", new { RetentionAmt = RetAmt, TaxAmt = TaxAmt, GrandTotalNoTax = GT }, clientBillDetail.CBillID);

                    transaction.Complete();                    
                    return RedirectToAction("Details",new {id=clientBillDetail.CBillID });
                }
                catch (Exception ex)
                {
                    db.AbortTransaction();
                    throw ex;
                }
            }
        }
       
        public ActionResult ManageDetails(int? id)
        {
            ViewBag.ClientBillDets = base.BaseCreateEdit<ClientBillDetail>(id, "CBillDetailID");


            //var viewdata = new Client_Bill();
            //viewdata.ClientBillDets = db.Fetch<ClientBillDet>("Select * From ClientBillDetail  Where CBillDetailID = @0", id);
            //viewdata.ClientDets = db.FirstOrDefault<ClientDet>("Select CBillID,ClientName,Tdate,RetentionPerc,RetentionAmt, RetentionAmtIsPaid,TaxPerc from ClientBill as cb Inner Join Client as c on cb.ClientID =c.ClientID where CBillID = @0", viewdata.ClientBillDets.First<ClientBillDet>().CBillID);

            return View("Details");            
        }
  
        [HttpPost]     
        public ActionResult ManageDetails([Bind(Include = "CBillDetailID,CBillID,Description,Amount,DebitCredit,BeforeTax")] ClientBillDetail clientBillDetail)
        {
            using (var transaction = db.GetTransaction())
            {
                try
                {
                    base.BaseSave<ClientBillDetail>(clientBillDetail, clientBillDetail.CBillDetailID > 0);

                    //Set Retention Amount
                    decimal RetPerc = db.FirstOrDefault<decimal>("Select RetentionPerc From ClientBill Where CBillID = @0", clientBillDetail.CBillID);
                    decimal BefTaxDebit = db.FirstOrDefault<decimal>("select COALESCE(sum(Amount),0) as Amount from ClientBillDetail where BeforeTax = @0 and CBillID =@1 and DebitCredit =@2", 1, clientBillDetail.CBillID, 1);
                    decimal BefTaxCredit = db.FirstOrDefault<decimal>("select COALESCE(sum(Amount),0) as Amount from ClientBillDetail where BeforeTax = @0 and CBillID =@1 and DebitCredit =@2", 1, clientBillDetail.CBillID, 0);
                    decimal BefTax = BefTaxCredit - BefTaxDebit;
                    decimal RetAmt = BefTax * (RetPerc / 100);
                    decimal AftTaxDebit = db.FirstOrDefault<decimal>("select COALESCE(sum(Amount),0) as Amount from ClientBillDetail where BeforeTax = @0 and CBillID =@1 and DebitCredit =@2", 0, clientBillDetail.CBillID, 1);
                    decimal AftTaxCredit = db.FirstOrDefault<decimal>("select COALESCE(sum(Amount),0) as Amount from ClientBillDetail where BeforeTax = @0 and CBillID =@1 and DebitCredit =@2", 0, clientBillDetail.CBillID, 0);
                    decimal AftTax = AftTaxCredit - AftTaxDebit;
                    decimal GT = BefTax + AftTax;

                    var TaxPerc = db.FirstOrDefault<decimal>("Select TaxPerc From ClientBill where CBillID=@0", clientBillDetail.CBillID);
                    var TaxAmt = BefTax * (TaxPerc / 100);

                    db.Update("ClientBill", "CBillID", new { RetentionAmt = RetAmt, TaxAmt= TaxAmt, GrandTotalNoTax= GT }, clientBillDetail.CBillID);

                    transaction.Complete();
                    return RedirectToAction("Details", new { id = clientBillDetail.CBillID });
                }
                catch (Exception ex)
                {
                    db.AbortTransaction();
                    throw ex;
                }
            }
                
        }

        public ActionResult AutoCompleteSite(string term)
        {
            string UserID = User.Identity.GetUserId();
            var h = db.Fetch<AutoCompleteData>($"Select ss.SiteID as id, SiteName as value from SupervisorSites as ss Inner Join Sites s on s.SiteID = ss.SiteID inner join AspNetUsers anu on ss.UserID=anu.Id Where SiteName like '%{term}%' and ss.UserID = @0 ", UserID).ToList();
            return Json(h, JsonRequestBehavior.AllowGet);
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
