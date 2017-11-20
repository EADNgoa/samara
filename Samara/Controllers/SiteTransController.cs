using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


using Microsoft.AspNet.Identity;

namespace Samara.Controllers
{
    [Authorize(Roles = "Boss")]
    public class SiteTransController : EAController
    {
     
        public ActionResult Index(int? page ,string SiteName )

        {
           if(SiteName?.Length >0) page = 1;
           return View("Index", base.BaseIndex<BossTransDet>(page, "SiteTransID,Tdate,Email,SiteName,ItemName,SupplierName,QtyAdded,Remarks", "SiteTransasction Inner Join Sites on SiteTransasction.SiteID = Sites.SiteID Inner Join Item on Item.ItemID = SiteTransasction.ItemID Inner Join Supplier on Supplier.SupplierID = SiteTransasction.SupplierID Inner Join AspNetUsers on AspNetUsers.Id = SiteTransasction.UserID  Where SiteName like '%" + SiteName + "%' and  QtyRemoved = 0"));
        }
        public ActionResult IssueIndex(int? page , string SiteName)
        {
            if (SiteName?.Length > 0) page = 1;
            return View("IssueIndex", base.BaseIndex<IssueTransDet>(page, "SiteTransID,Tdate,Email,SiteName,ClientName,ItemName, QtyRemoved,Remarks", "SiteTransasction Inner Join Sites on SiteTransasction.SiteID = Sites.SiteID Inner Join Item on Item.ItemID = SiteTransasction.ItemID Inner Join AspNetUsers on AspNetUsers.Id = SiteTransasction.UserID Inner join Client on Client.ClientID =SiteTransasction.ClientID Where SiteName like '%" + SiteName + "%' and QtyRemoved >= 0 and ToSiteID IS Null and COALESCE(QtyRemoved,0)>0 and TositeID is null"));
        }
        public ActionResult TransferIndex(int? page, string SiteName)
        {
            page = 1;
            return View("TransferIndex", base.BaseIndex<SiteTransfers>(page, "SiteTransID,Tdate,Email,sf.SiteName as CurrentSite,ItemName, QtyAdded, QtyRemoved,st.SiteName as ToSite,Remarks ", " SiteTransasction stf Inner Join Sites sf on stf.SiteID = sf.SiteID Inner Join Item i on i.ItemID = stf.ItemID inner join Sites as st on st.SiteID = stf.ToSiteID Inner Join AspNetUsers ans on ans.Id = stf.UserID Where sf.SiteName like '%" + SiteName + "%' and ToSiteID IS NOT NULL"));
        }



        public ActionResult ManageTransfer(int? id)
        {
            var rec = base.BaseCreateEdit<SiteTransasction>(id, "SiteTransID");

            //Since UserID and QtyRemoved is [required] we have to set it beofre loading the form
            if (rec == null) //We are in create mode
            {
                rec = new SiteTransasction { UserID = User.Identity.GetUserId(), QtyAdded = 0 };
                ViewBag.OriginalQty = 0;
            }
            else
            {
                //In edit mode We need to fetch the name values of the autocomplete boxes
                ViewBag.FromSiteName = db.SingleOrDefault<string>("Select SiteName from Sites where SiteID = @0", rec.SiteID);
                ViewBag.ItemName = db.SingleOrDefault<string>("Select ItemName from Item where ItemID = @0", rec.ItemID);
                ViewBag.ToSiteName = db.SingleOrDefault<string>("Select SiteName from Sites where SiteID = @0", rec.SiteID);

                ViewBag.OriginalQty = rec.QtyRemoved;
            }
            return View(rec);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageTransfer([Bind(Include = "SiteTransID,UserID,Tdate,SiteID,ItemID,QtyAdded,QtyRemoved,ToSiteID,Remarks")] SiteTransasction siteTransaction,int OriginalQty)
        {
            siteTransaction.Tdate = DateTime.Now;
         


            var getStock = db.FirstOrDefault<SiteCurrentStock>("Select SiteStockID,Qty From SiteCurrentStock Where  SiteID=@0 and ItemID =@1", siteTransaction.SiteID, siteTransaction.ItemID);
            var TransferStock = db.FirstOrDefault<SiteCurrentStock>("Select SiteStockID,Qty From SiteCurrentStock Where  SiteID=@0 and ItemID =@1", siteTransaction.ToSiteID, siteTransaction.ItemID);
            using (var transaction = db.GetTransaction())
            {
                try
                {
                    if (getStock != null)
                    {
                        if(siteTransaction.SiteTransID>0)
                            db.Update("SiteCurrentStock", "SiteStockID", new { Qty = getStock.Qty + OriginalQty - siteTransaction.QtyRemoved }, getStock.SiteStockID);
                        else  
                            db.Update("SiteCurrentStock", "SiteStockID", new { Qty = getStock.Qty - siteTransaction.QtyRemoved }, getStock.SiteStockID);
                       
                        if (TransferStock != null)
                        {
                            if (siteTransaction.SiteTransID > 0)
                                db.Update("SiteCurrentStock", "SiteStockID", new { Qty = TransferStock.Qty - OriginalQty + siteTransaction.QtyRemoved }, TransferStock.SiteStockID);
                            else
                                db.Update("SiteCurrentStock", "SiteStockID", new { Qty = TransferStock.Qty + siteTransaction.QtyRemoved }, TransferStock.SiteStockID);
                        }
                        else
                        {
                            var item = new SiteCurrentStock { SiteID = siteTransaction.ToSiteID, ItemID = siteTransaction.ItemID, Qty = siteTransaction.QtyRemoved };
                            db.Save(item);
                        }
                        base.BaseSave<SiteTransasction>(siteTransaction, siteTransaction.SiteTransID > 0);
                        transaction.Complete();

                    }
                }
                catch (Exception ex)
                {
                    db.AbortTransaction();
                    throw ex;
                }
            
         
            }

            return RedirectToAction("ManageTransfer");
        }

        public ActionResult ManageIssue(int? id)
        {

            var rec = base.BaseCreateEdit<SiteTransasction>(id, "SiteTransID");

            //Since UserID and QtyRemoved is [required] we have to set it beofre loading the form
            if (rec == null) //We are in create mode
            {
                rec = new SiteTransasction { UserID = User.Identity.GetUserId(), QtyAdded = 0 };
                ViewBag.OriginalQty = 0;
                ViewBag.ClientID = new SelectList(db.Fetch<Client>("Select ClientID,ClientName from Client"), "ClientID", "ClientName");

            }
            else
            {
                //In edit mode We need to fetch the name values of the autocomplete boxes
                ViewBag.FromSiteName = db.SingleOrDefault<string>("Select SiteName from Sites where SiteID = @0", rec.SiteID);
                ViewBag.ItemName = db.SingleOrDefault<string>("Select ItemName from Item where ItemID = @0", rec.ItemID);
                ViewBag.ClientName = db.SingleOrDefault<string>("Select ClientName from Client where ClientID = @0", rec.ClientID);

                ViewBag.OriginalQty = rec.QtyRemoved;
            }


            return View(rec);
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598y
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageIssue([Bind(Include = "SiteTransID,UserID,Tdate,SiteID,ClientID,ItemID,QtyAdded,QtyRemoved,Remarks")] SiteTransasction siteTransaction,int OriginalQty)
        {

            siteTransaction.Tdate = DateTime.Now;
           


            var getStock = db.FirstOrDefault<SiteCurrentStock>("Select SiteStockID,Qty From SiteCurrentStock Where  SiteID=@0 and ItemID =@1", siteTransaction.SiteID, siteTransaction.ItemID);
            using (var transaction = db.GetTransaction())
            {
                try
                {
                    if (getStock != null)
                    {
                        if (siteTransaction.SiteTransID > 0)
                        {
                            db.Update("SiteCurrentStock", "SiteStockID", new { Qty = getStock.Qty + OriginalQty - siteTransaction.QtyRemoved }, getStock.SiteStockID);

                        }
                        else
                        {

                            db.Update("SiteCurrentStock", "SiteStockID", new { Qty = getStock.Qty - siteTransaction.QtyRemoved }, getStock.SiteStockID);
                          
                        }
                        base.BaseSave<SiteTransasction>(siteTransaction, siteTransaction.SiteTransID > 0);
                        transaction.Complete();
                        return RedirectToAction("IssueIndex");
                    }
                }
                catch (Exception ex)
                {
                    db.AbortTransaction();
                    throw ex;
                }
            

              }

            return RedirectToAction("Manage");
        }

        public ActionResult Manage(int? id)
        {

            var rec = base.BaseCreateEdit<SiteTransasction>(id, "SiteTransID");

            //Since UserID and QtyRemoved is [required] we have to set it before loading the form
            if (rec == null) //We are in create mode
            {
                //rec = new SiteTransasction { UserID = User.Identity.GetUserId(), QtyRemoved = 0 };
                ViewBag.OriginalQty = 0;
               
                ViewBag.SupplierID = new SelectList(db.Fetch<Supplier>("Select SupplierID,SupplierName from Supplier"), "SupplierID", "SupplierName");

            }
            else
            {
                //In edit mode We need to fetch the name values of the autocomplete boxes
                ViewBag.FromSiteName = db.SingleOrDefault<string>("Select SiteName from Sites where SiteID = @0", rec.SiteID);
                ViewBag.ItemName = db.SingleOrDefault<string>("Select ItemName from Item where ItemID = @0", rec.ItemID);
                ViewBag.SupplierName = db.SingleOrDefault<string>("Select SupplierName from Supplier where SupplierID = @0", rec.SupplierID);
                ViewBag.OriginalQty = rec.QtyAdded;
                ViewBag.SupplierID = new SelectList(db.Fetch<Supplier>("Select SupplierID,SupplierName from Supplier"), "SupplierID", "SupplierName");
                
            }

            if (id != null)
            {
                ChallanImg ci = new ChallanImg()
                {
                    ItemID = rec.ItemID.Value,
                    QtyAdded = (int)rec.QtyAdded,
                    QtyRemoved = (int)rec.QtyRemoved,
                    Remarks = rec.Remarks,
                    SiteID = (int)rec.SiteID,
                    SiteTransID = rec.SiteTransID,
                    SupplierID = (int)rec.SupplierID,
                    Tdate = (DateTime)rec.Tdate,
                    UserID = User.Identity.GetUserId(),
                    path = rec.path                    
                };
                return View(ci);
            } else
            {
                ChallanImg ci = new ChallanImg() { UserID = User.Identity.GetUserId(), QtyRemoved = 0 };
                return View(ci);
            }
            
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage([Bind(Include = "SiteTransID,UserID,Tdate,SiteID,SupplierID,Path,UploadedFile,ItemID,QtyAdded,Remarks,QtyRemoved")] ChallanImg siteTransaction, int OriginalQty)
      {
           
            siteTransaction.Tdate = DateTime.Now;            
            var currentStock = db.SingleOrDefault<SiteCurrentStock>("select * from SiteCurrentStock where SiteID = @0 and ItemID= @1", siteTransaction.SiteID,siteTransaction.ItemID);

            using (var transaction = db.GetTransaction())
            {
                try
                {                    
                    

                    //on 14 Nov 2017 stanley said suppliers would make their own bills, hence the below was commented out
                    //var sup = db.FirstOrDefault<SupplierBill>("Select SBillID,SupplierID from SupplierBill Where SupplierID = @0", siteTransaction.SupplierID);
                    //var getSupBill = db.FirstOrDefault<SupplierBillDetail>("Select SBillDetailID,SBillID,ItemID,Qty,UnitPrice,QtyRec from SupplierBillDetail Where ItemID= @0 and SBillID = @1", siteTransaction.ItemID, sup.SBillID);
                    //if (siteTransaction.SiteTransID>0)
                    //{
                    //    if (getSupBill != null)
                    //    {
                    //        db.Update("SupplierBillDetail", "SBillDetailID", new { QtyRec =  siteTransaction.QtyAdded + getSupBill.QtyRec - OriginalQty }, getSupBill.SBillDetailID);
                    //        siteTransaction.SBillDetailID = getSupBill.SBillDetailID;
                    //    }
                    //}
                    //else
                    //{
                    //  if (getSupBill != null)
                    //    {
                    //        if (getSupBill.Qty != getSupBill.QtyRec)
                    //        {
                    //            db.Update("SupplierBillDetail", "SBillDetailID", new { QtyRec =siteTransaction.QtyAdded+getSupBill.QtyRec}, getSupBill.SBillDetailID);
                    //        }

                    //        siteTransaction.SBillDetailID = getSupBill.SBillDetailID;
                    //    }

                    //}

                   
                        if (currentStock == null)//this is the first purchase of the item at thissite
                        {
                            // var getSupBill = db.FirstOrDefault<SupplierBillDetail>("SBillDetailID,SBillID,ItemID,Qty,UnitPrice,QtyRec","Supplier as sp inner join SupplierBillDetail as sbd on sp.SBillID = sbd.SBillID where Supplier Where ItemID= @0 and SupplierID=@0",siteTransaction.ItemID,siteTransaction.SupplierID);


                            var item = new SiteCurrentStock { SiteID = siteTransaction.SiteID, ItemID = siteTransaction.ItemID, Qty = siteTransaction.QtyAdded };
                            db.Save(item);
                        }
                        else
                        {
                            if (siteTransaction.SiteTransID > 0) //Edit mode
                                db.Update("SiteCurrentStock", "SiteStockID", new { Qty = currentStock.Qty - OriginalQty + siteTransaction.QtyAdded }, currentStock.SiteStockID);
                            else
                                db.Update("SiteCurrentStock", "SiteStockID", new { Qty = currentStock.Qty + siteTransaction.QtyAdded }, currentStock.SiteStockID);
                        }


                        SiteTransasction res = new SiteTransasction
                        {
                            ItemID = siteTransaction.ItemID,
                            QtyAdded = siteTransaction.QtyAdded,
                            QtyRemoved = siteTransaction.QtyRemoved,
                            Remarks = siteTransaction.Remarks,
                            SiteID = siteTransaction.SiteID,
                            SiteTransID = siteTransaction.SiteTransID,
                            SupplierID = siteTransaction.SupplierID,
                            Tdate = siteTransaction.Tdate,
                            UserID = siteTransaction.UserID
                        };

                        if (siteTransaction.UploadedFile != null)
                        {
                            string fn = siteTransaction.UploadedFile.FileName.Substring(siteTransaction.UploadedFile.FileName.LastIndexOf('\\') + 1);
                            fn = siteTransaction.SiteID + "_" + fn;
                            string SavePath = System.IO.Path.Combine(Server.MapPath("~/Images"), fn);
                            siteTransaction.UploadedFile.SaveAs(SavePath);

                            //System.Drawing.Bitmap upimg = new System.Drawing.Bitmap(siteTransaction.UploadedFile.InputStream);
                            //System.Drawing.Bitmap svimg = MyExtensions.CropUnwantedBackground(upimg);
                            //svimg.Save(System.IO.Path.Combine(Server.MapPath("~/Images"), fn));

                            res.path = fn;
                        }
                        else
                        {
                            res.path = siteTransaction.path;
                        }

                        base.BaseSave<SiteTransasction>(res, siteTransaction.SiteTransID > 0);
                        transaction.Complete();
                        return RedirectToAction("Index");
                                        
                    
                }
                catch (Exception ex)
                {
                    db.AbortTransaction();
                    throw ex;
                }
            }
        }

        public ActionResult AutoCompleteItem(string term)
        {        
            var h = db.Fetch<AutoCompleteData>($"Select ItemID as id, ItemName as value from Item where ItemName like '%{term}%'" ).ToList();
            return Json(h, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AutoCompleteToSite(string term)
        {
            var h = db.Fetch<AutoCompleteData>($"Select SiteID as id, SiteName as value from Sites where SiteName like '%{term}%'").ToList();
            return Json(h, JsonRequestBehavior.AllowGet);
        }
 

        public ActionResult AutoCompleteSite(string term)
        {
            string UserID = User.Identity.GetUserId();
            var h = db.Fetch<AutoCompleteData>($"Select ss.SiteID as id, SiteName as value from SupervisorSites as ss Inner Join Sites s on s.SiteID = ss.SiteID inner join AspNetUsers anu on ss.UserID=anu.Id Where SiteName like '%{term}%' and ss.UserID = @0 ",UserID).ToList();
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
