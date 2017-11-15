using System;
//using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
//using System.Web;
using System.Web.Mvc;

/*
namespace Samara.Controllers
{
    public class ClientBillController : EAController
    {
        public ActionResult Index(int? page, string ClientName)
        {
            if (ClientName?.Length > 0) page = 1;
            return View("Index", base.BaseIndex<ClientDet>(page, "CBillID,ClientName, Tdate,RetentionPerc", "ClientBill as cb Inner Join Client as c on cb.ClientID = c.ClientID where ClientName like '%" + ClientName + "%'"));
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
        public ActionResult Manage([Bind(Include = "CBillID,ClientID,Tdate,RetentionPerc")] ClientBill cBill)
        {
            return base.BaseSave<ClientBill>(cBill, cBill.CBillID > 0);
        }

        public ActionResult Details(int? id)
        {
            
             var viewdata = new Client_Bill
             {
                        ClientDets = db.FirstOrDefault<ClientDet>("Select CBillID,ClientName,Tdate,RetentionPerc from ClientBill as cb Inner Join Client as c on cb.ClientID =c.ClientID where CBillID = @0", id),
                        ClientBillDets = db.Fetch<ClientBillDet>("Select * From ClientBillDetail  Where CBillID = @0", id)
                 
            };
                ViewBag.CBillID = id;
                return View("Details", viewdata);            
        }
  
        [HttpPost]     
        public ActionResult Details([Bind(Include = "CBillDetailID,CBillID,ItemID,Qty,UnitCostPrice,UnitSellPrice,TaxPerc")] ClientBillDetail clientBillDetail)
        {
            var GetItem = db.FirstOrDefault<ClientBillDetail>("Select * From ClientBillDetail Where  CBillID =@0 and ItemID=@1", clientBillDetail.CBillID, clientBillDetail.ItemID);
            if(GetItem ==  null)
            {
                base.BaseSave<ClientBillDetail>(clientBillDetail, clientBillDetail.CBillDetailID > 0);
            }
            else
            {
                db.Update("ClientBillDetail", "CBillDetailID", new { Qty = GetItem.Qty + clientBillDetail.Qty ,UnitSellPrice = clientBillDetail.UnitSellPrice }, GetItem.CBillDetailID);
            }

            return RedirectToAction("Details",new {id=clientBillDetail.CBillID });
        }

        public ActionResult ManageDetails(int? id)
        {
            ViewBag.ClientBillDets = base.BaseCreateEdit<ClientBillDetail>(id, "CBillDetailID");
            ViewBag.ItemName = db.FirstOrDefault<ClientBillDet>("Select itemName From ClientBillDetail cbd inner  join  Item as i on cbd.ItemID = i.ItemID where CBillDetailID= @0", id).ItemName;


            var viewdata = new Client_Bill
            {
                ClientDets = db.FirstOrDefault<ClientDet>("Select CBillID,ClientName,Tdate,RetentionPerc from ClientBill as cb Inner Join Client as c on cb.ClientID =c.ClientID where CBillID = @0", id),
                ClientBillDets = db.Fetch<ClientBillDet>("Select * From ClientBillDetail  Where CBillID = @0", id)

            };

            return View("Details", viewdata);            
        }
  
        [HttpPost]     
        public ActionResult ManageDetails([Bind(Include = "CBillDetailID,CBillID,ItemID,Qty,UnitCostPrice,UnitSellPrice,TaxPerc")] ClientBillDetail clientBillDetail)
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
*/