using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using Samara.Models;
using Microsoft.AspNet.Identity;

namespace Samara.Controllers
{
    [Authorize(Roles = "Boss")]
    public class SiteTransController : EAController
    {
     
        public ActionResult Index(int? page ,string SiteName )

        {
           if(SiteName?.Length >0) page = 1;
           return View("Index", base.BaseIndex<BossTransDet>(page, "SiteTransID,Tdate,Email,SiteName,ItemName,QtyAdded,Remarks", "SiteTransasction Inner Join Sites on SiteTransasction.SiteID = Sites.SiteID Inner Join Item on Item.ItemID = SiteTransasction.ItemID Inner Join AspNetUsers on AspNetUsers.Id = SiteTransasction.UserID  Where SiteName like '%" + SiteName + "%' and  QtyRemoved < 0"));
        }
        public ActionResult IssueIndex(int? page , string SiteName)
        {
            if (SiteName?.Length > 0) page = 1;
            return View("IssueIndex", base.BaseIndex<IssueTransDet>(page, "SiteTransID,Tdate,Email,SiteName,ItemName, QtyAdded, QtyRemoved,Remarks", "SiteTransasction Inner Join Sites on SiteTransasction.SiteID = Sites.SiteID Inner Join Item on Item.ItemID = SiteTransasction.ItemID Inner Join AspNetUsers on AspNetUsers.Id = SiteTransasction.UserID Where SiteName like '%" + SiteName + "%' and QtyRemoved >= 0 and ToSiteID IS Null"));
        }
        public ActionResult TransferIndex(int? page, string SiteName)
        {
            page = 1;
            return View("TransferIndex", base.BaseIndex<SiteTransfers>(page, "SiteTransID,Tdate,Email,sf.SiteName as CurrentSite,ItemName, QtyAdded, QtyRemoved,st.SiteName as ToSite,Remarks ", " SiteTransasction stf Inner Join Sites sf on stf.SiteID = sf.SiteID Inner Join Item i on i.ItemID = stf.ItemID inner join Sites as st on st.SiteID = stf.ToSiteID Inner Join AspNetUsers ans on ans.Id = stf.UserID Where sf.SiteName like '%" + SiteName + "%' and ToSiteID > 0"));
        }



        public ActionResult ManageTransfer(int? id)
        {
            ViewBag.SiteID = new SelectList(db.Fetch<Site>("Select SiteID,SiteName from Sites"), "SiteID", "SiteName");
            ViewBag.ToSiteID = new SelectList(db.Fetch<Site>("Select SiteID,SiteName from Sites"), "SiteID", "SiteName");
            ViewBag.ItemID = new SelectList(db.Fetch<Item>("Select ItemID,ItemName from Item"), "ItemID", "ItemName");

            return View(base.BaseCreateEdit<SiteTransasction>(id, "SiteTransID"));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageTransfer([Bind(Include = "SiteTransID,UserID,Tdate,SiteID,ItemID,QtyAdded,QtyRemoved,ToSiteID,Remarks")] SiteTransasction siteTransaction)
        {


            siteTransaction.Tdate = DateTime.Now;
            siteTransaction.UserID = User.Identity.GetUserId();


            var checkItem = db.Exists<SiteCurrentStock>(" SiteID = @0 and ItemID= @1", siteTransaction.SiteID, siteTransaction.ItemID);
            var checkSite = db.Exists<SiteCurrentStock>(" SiteID = @0 and ItemID= @1", siteTransaction.ToSiteID, siteTransaction.ItemID);
           
            if (siteTransaction.SiteTransID == 0 && checkItem == true)
            {

                var getStock = db.FirstOrDefault<SiteCurrentStock>("Select SiteStockID,Qty From SiteCurrentStock Where  SiteID=@0 and ItemID =@1", siteTransaction.SiteID, siteTransaction.ItemID);
                db.Update("SiteCurrentStock", "SiteStockID", new { Qty = getStock.Qty - siteTransaction.QtyRemoved }, getStock.SiteStockID);
                siteTransaction.QtyAdded = getStock.Qty - siteTransaction.QtyRemoved;

                if (checkSite == true)
                {
                    var TransferStock = db.FirstOrDefault<SiteCurrentStock>("Select SiteStockID,Qty From SiteCurrentStock Where  SiteID=@0 and ItemID =@1", siteTransaction.ToSiteID, siteTransaction.ItemID);
                    db.Update("SiteCurrentStock", "SiteStockID", new { Qty = TransferStock.Qty + siteTransaction.QtyRemoved }, TransferStock.SiteStockID);
                   
                }
                else
                {

                    var item = new SiteCurrentStock { SiteID = siteTransaction.ToSiteID, ItemID = siteTransaction.ItemID, Qty = siteTransaction.QtyRemoved };
                   
                    db.Save(item);

                }
                base.BaseSave<SiteTransasction>(siteTransaction, siteTransaction.SiteTransID > 0);
                return RedirectToAction("TransferIndex");
            }
            return RedirectToAction("ManageTransfer");
        }

        public ActionResult ManageIssue(int? id)
        {
            ViewBag.SiteID = new SelectList(db.Fetch<Site>("Select SiteID,SiteName from Sites"), "SiteID", "SiteName");
            ViewBag.ItemID = new SelectList(db.Fetch<Item>("Select ItemID,ItemName from Item"), "ItemID", "ItemName");
          
            return View(base.BaseCreateEdit<SiteTransasction>(id, "SiteTransID"));
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageIssue([Bind(Include = "SiteTransID,UserID,Tdate,SiteID,ItemID,QtyAdded,QtyRemoved,Remarks")] SiteTransasction siteTransaction)
        {

            siteTransaction.Tdate = DateTime.Now;
            siteTransaction.UserID = User.Identity.GetUserId();
           

            var checkItem = db.Exists<SiteCurrentStock>(" SiteID = @0 and ItemID= @1", siteTransaction.SiteID, siteTransaction.ItemID);
          
                if (siteTransaction.SiteTransID == 0 && checkItem == true)
                {
                
                        var getStock = db.FirstOrDefault<SiteCurrentStock>("Select SiteStockID,Qty From SiteCurrentStock Where  SiteID=@0 and ItemID =@1", siteTransaction.SiteID, siteTransaction.ItemID);
                        db.Update("SiteCurrentStock", "SiteStockID", new { Qty = getStock.Qty - siteTransaction.QtyRemoved }, getStock.SiteStockID);
                        siteTransaction.QtyAdded = getStock.Qty - siteTransaction.QtyRemoved;
                        base.BaseSave<SiteTransasction>(siteTransaction, siteTransaction.SiteTransID > 0);
                       return RedirectToAction("IssueIndex");
            }

            return RedirectToAction("Manage");
        }

        public ActionResult Manage(int? id)
        {
            ViewBag.SiteID = new SelectList(db.Fetch<Site>("Select SiteID,SiteName from Sites"), "SiteID", "SiteName");
            ViewBag.ItemID = new SelectList(db.Fetch<Item>("Select ItemID,ItemName from Item"), "ItemID", "ItemName");
            return View(base.BaseCreateEdit<SiteTransasction>(id, "SiteTransID"));
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage([Bind(Include = "SiteTransID,UserID,Tdate,SiteID,ItemID,QtyAdded,Remarks")] SiteTransasction siteTransaction)
      {
           
            siteTransaction.Tdate = DateTime.Now;
            siteTransaction.UserID = User.Identity.GetUserId();
            siteTransaction.QtyRemoved = -1;
            var checkItem = db.Exists<SiteCurrentStock>(" SiteID = @0 and ItemID= @1",siteTransaction.SiteID,siteTransaction.ItemID);     
                if (siteTransaction.SiteTransID == 0)
                {
                    if (checkItem == false)
                    {
                        var item = new SiteCurrentStock { SiteID = siteTransaction.SiteID, ItemID = siteTransaction.ItemID, Qty = siteTransaction.QtyAdded };
                        db.Save(item);
                    }
                    else
                    {
                        var getStock = db.FirstOrDefault<SiteCurrentStock>("Select SiteStockID,Qty From SiteCurrentStock Where  SiteID=@0 and ItemID =@1", siteTransaction.SiteID, siteTransaction.ItemID);
                        db.Update("SiteCurrentStock", "SiteStockID", new { Qty = getStock.Qty + siteTransaction.QtyAdded }, getStock.SiteStockID);
                    }
                }
                else
                {
                    int id = db.FirstOrDefault<SiteCurrentStock>("Select SiteStockID From SiteCurrentStock Where SiteID=@0 and ItemID =@1", siteTransaction.SiteID, siteTransaction.ItemID).SiteStockID;
                    db.Update("SiteCurrentStock", "SiteStockID", new { Qty = siteTransaction.QtyAdded }, id);
                } 
            return base.BaseSave<SiteTransasction>(siteTransaction, siteTransaction.SiteTransID >0);
        }

        public ActionResult AutoCompleteSite()
        {
            string UserID = User.Identity.GetUserId();
            var h = db.Fetch<SiteIDdropdown>("Select ss.SiteID as Id, SiteName as Value from SupervisorSites as ss Inner Join Sites s on s.SiteID = ss.SiteID inner join AspNetUsers anu on ss.UserID=anu.Id Where ss.UserID = @0 ",UserID).ToList();
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
