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
    public class SupplierBillController : EAController
    {
        public ActionResult Index(int? page, string SupplierName)
        {
            if (SupplierName?.Length > 0) page = 1;
            return View("Index", base.BaseIndex<SuppDet>(page, "SBillID,SupplierName, Tdate", "SupplierBill as sb Inner Join Supplier as s on sb.SupplierID = s.SupplierID where SupplierName like '%" + SupplierName + "%'"));
        }



        // GET: Clients/Create
        public ActionResult Manage(int? id)
        {
            ViewBag.SupplierID = new SelectList(db.Fetch<Supplier>("Select SupplierID,SupplierName from Supplier"), "SupplierID", "SupplierName");

            return View(base.BaseCreateEdit<SupplierBill>(id, "SBillID"));
        }


        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage([Bind(Include = "SBillID,SupplierID,Tdate")] SupplierBill supplierBill)
        {
            return base.BaseSave<SupplierBill>(supplierBill, supplierBill.SBillID > 0);
        }

        public ActionResult Details(int? id)
        {
            
             var viewdata = new Suplier_Bill
             {
                        SuplierDets = db.FirstOrDefault<SuppDet>("Select SBillID,SupplierName,Tdate from SupplierBill as sb Inner Join Supplier as s on sb.SupplierID =s.SupplierID where SBillID = @0", id),
                        SuplierBillDets = db.Fetch<SuppBillDet>("Select * From SupplierBillDetail  Where SBillID = @0", id)
                 
            };
                ViewBag.SBillID = id;
                return View("Details", viewdata);            
        }
  
        [HttpPost]     
        public ActionResult Details([Bind(Include = "SBillDetailID,SBillID,ItemID,Qty,UnitPrice,QtyRec,QtySold")] SupplierBillDetail supplierBillDetail)
        {
            var GetItem = db.FirstOrDefault<SupplierBillDetail>("Select * From SupplierBillDetail Where ItemID=@0 and SBillID =@0",supplierBillDetail.ItemID,supplierBillDetail.SBillID);
            if(GetItem ==  null)
            {
                supplierBillDetail.QtyRec = 0;
                base.BaseSave<SupplierBillDetail>(supplierBillDetail, supplierBillDetail.SBillDetailID > 0);
            }
            else
            {
                db.Update("SupplierBillDetail", "SBillDetailID", new { Qty = GetItem.Qty + supplierBillDetail.Qty ,UnitPrice =supplierBillDetail.UnitPrice }, GetItem.SBillDetailID);
            }
           

            return RedirectToAction("Details",new {id=supplierBillDetail.SBillID });
        }

        public ActionResult ManageDetails(int? id)
        {
            ViewBag.SupBillDets = base.BaseCreateEdit<SupplierBillDetail>(id, "SBillDetailID");
            ViewBag.ItemName = db.FirstOrDefault<SuppBillDet>("Select itemName From SupplierBillDetail sbd inner  join  Item as i on sbd.ItemID = i.ItemID where SBillDetailID= @0", id).ItemName;

            var viewdata = new Suplier_Bill
                {
                    SuplierDets = db.FirstOrDefault<SuppDet>("Select SBillID,SupplierName,Tdate from SupplierBill as sb Inner Join Supplier as s on sb.SupplierID =s.SupplierID where SBillID = @0", id),
                    SuplierBillDets = db.Fetch<SuppBillDet>("Select * From SupplierBillDetail  Where SBillID = @0", id)
                };
              
               
                return View("Details", viewdata);            
        }
  
        [HttpPost]     
        public ActionResult ManageDetails([Bind(Include = "SBillDetailID,SBillID,ItemID,Qty,UnitPrice,QtyRec,QtySold")] SupplierBillDetail supplierBillDetail)
        {
            base.BaseSave<SupplierBillDetail>(supplierBillDetail, supplierBillDetail.SBillDetailID > 0);
       
            return RedirectToAction("Details",new { id = supplierBillDetail.SBillID });
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
