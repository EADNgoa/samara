﻿using Microsoft.AspNet.Identity;
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
            return View("Index", base.BaseIndex<SuppDet>(page, "SBillID,SupplierName,SiteName, Tdate", "SupplierBill as sb Inner Join Supplier as s on sb.SupplierID = s.SupplierID inner join Sites st on sb.SiteID = st.SiteID where SupplierName like '%" + SupplierName + "%'"));
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
        public ActionResult Manage([Bind(Include = "SBillID,SupplierID,SiteID,Tdate")] SupplierBill supplierBill)
        {
            supplierBill.TDSperc =(decimal) db.FirstOrDefault<Config>("Select TDSperc from Config").TDSperc;
            return base.BaseSave<SupplierBill>(supplierBill, supplierBill.SBillID > 0);
        }

        public ActionResult Details(int? id)
        {
            
             var viewdata = new Suplier_Bill
             {
                        SuplierDets = db.FirstOrDefault<SuppDet>("Select SBillID,SupplierName,Tdate,TDSperc from SupplierBill as sb Inner Join Supplier as s on sb.SupplierID =s.SupplierID where SBillID = @0", id),
                        SuplierBillDets = db.Fetch<SuppBillDet>("Select * From SupplierBillDetail sbd inner join Labour l on sbd.LabourID = l.LabourID Where SBillID = @0", id)
                 
            };
            ViewBag.LabourID = new SelectList(db.Fetch<Labour>("Select LabourID,LabourName from Labour"), "LabourID", "LabourName");
            ViewBag.tds = db.FirstOrDefault<decimal>("Select TDSperc from Config");
            ViewBag.SBillID = id;
            return View("Details", viewdata);            
        }
  
        [HttpPost]     
        public ActionResult Details([Bind(Include = "SBillDetailID,SBillID,LabourID,Job,Qty,UnitPrice,QtyRec,QtySold")] SupplierBillDetail supplierBillDetail)
        {
            var GetItem = db.FirstOrDefault<SupplierBillDetail>("Select * From SupplierBillDetail Where LabourID=@0 and SBillID =@0", supplierBillDetail.LabourID,supplierBillDetail.SBillID);
            if(GetItem ==  null)
            {
         
                supplierBillDetail.QtyRec = 0;
                ViewBag.Total = db.Fetch<SupplierBillDetail>("");
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
            ViewBag.LabourName = db.FirstOrDefault<SuppBillDet>("Select LabourName From SupplierBillDetail sbd inner  join  labour as l on sbd.LabourID = l.LabourID where SBillDetailID= @0", id).LabourName;
            ViewBag.tds = db.FirstOrDefault<decimal>("Select TDSperc from Config");
            var viewdata = new Suplier_Bill
                {
                    SuplierDets = db.FirstOrDefault<SuppDet>("Select SBillID,SupplierName,Tdate,TDSperc from SupplierBill as sb Inner Join Supplier as s on sb.SupplierID =s.SupplierID where SBillID = @0", id),
                    SuplierBillDets = db.Fetch<SuppBillDet>("Select * From SupplierBillDetail  Where SBillID = @0", id)
                };
              
               
                return View("Details", viewdata);            
        }
  
        [HttpPost]     
        public ActionResult ManageDetails([Bind(Include = "SBillDetailID,SBillID,LabourID,Job,Qty,UnitPrice,QtyRec,QtySold")] SuppBillDet supplierBillDetail)
        {
            var objToSave = new SupplierBillDetail()
            {
                SBillDetailID = supplierBillDetail.SBillDetailID,
                SBillID = supplierBillDetail.SBillID,
                LabourID = supplierBillDetail.LabourID,
                Job = supplierBillDetail.Job,
                Qty = supplierBillDetail.Qty,
                UnitPrice = supplierBillDetail.UnitPrice,
                QtyRec = supplierBillDetail.QtyRec,
                QtySold = supplierBillDetail.QtySold
            };

            base.BaseSave<SupplierBillDetail>(objToSave, supplierBillDetail.SBillDetailID > 0);
       
            return RedirectToAction("Details",new { id = supplierBillDetail.SBillID });
        }


        public ActionResult AutoCompleteSite(string term)
        {
            string UserID = User.Identity.GetUserId();
            var h = db.Fetch<AutoCompleteData>($"Select ss.SiteID as id, SiteName as value from SupervisorSites as ss Inner Join Sites s on s.SiteID = ss.SiteID inner join AspNetUsers anu on ss.UserID=anu.Id Where SiteName like '%{term}%' and ss.UserID = @0 ", UserID).ToList();
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
