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
    public class ReportsController : EAController
    {
        // GET: Clients
        public ActionResult Stock(int? page ,string ItemName )
        {
            ViewBag.ItemStock = db.Fetch<SiteCurrentStockDet>("Select ItemName,Sum(Qty) as Qty from SiteCurrentStock  Inner Join Item  on SiteCurrentStock.ItemID = Item.ItemID  Group By ItemName");
            return View("Stock", base.BaseIndex<SiteCurrentStockDet>(page, "SiteName,ItemName,Sum(Qty) as Qty", "SiteCurrentStock scs inner join Item i on scs.ItemID=i.ItemID Inner Join Sites s on  scs.SiteID = s.SiteID Group By SiteName,ItemName,Qty"));
        }
        public ActionResult RptRq(int? page)
        {
            ViewBag.YearBox = MyExtensions.MakeYrRq(8, 1, DateTime.Today.Year);
            ViewBag.MonthBox = MyExtensions.MonthList();
            ViewBag.ReturnAction = "OCbalance";
            return View();
           
        }
        public ActionResult OCbalance(int? page,FormCollection fm)
        {
            if (fm["RptYear"] == null || fm["RptMonth"] == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            int RptYr = int.Parse(fm["RptYear"]);
            int RptMon = int.Parse(fm["RptMonth"]);
            DateTime lastMonDate = DateTime.Parse($"01/{RptMon}/{RptYr}").AddDays(-1);
            int LastDate = DateTime.DaysInMonth(RptYr, RptMon);
            DateTime CurrMonDate = DateTime.Parse($"{LastDate}/{RptMon}/{RptYr}");
            ViewBag.RptMon = MyExtensions.MonthFromInt(RptMon);
            ViewBag.RptYr = RptYr;
            
            ViewBag.OpeningBalance = db.Fetch<StockSummaryDetails>("Select i.itemId, ItemName,sum(Qty) as Qty from StockSummary ss inner join Item i on ss.ItemID =i.ItemID Where Tdate= @0 Group By i.itemId, ItemName", lastMonDate).ToDictionary(i => i.ItemId);
            ViewBag.ClosingBalance = db.Fetch<StockSummaryDetails>(" Select i.itemId, ItemName,Sum(Qty) as Qty from StockSummary ss inner join Item i on ss.ItemID =i.ItemID Where Tdate= @0 Group By i.itemId, ItemName", CurrMonDate).ToDictionary(i => i.ItemId);
            return View("OCbalance");
        }

        public ActionResult RptRqPls(int? page)
        {
            ViewBag.FromSiteName = 1;
            ViewBag.ReturnAction = "ProfitLossSite";
            return View("RptRq");

        }

        public ActionResult ProfitLossSite(FormCollection fm)
        {
            if (fm["SiteName"] ==  null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
 
            int Site = int.Parse(fm["SiteID"]);
            ViewBag.sn = fm["SiteName"];
            ViewBag.Expenditure = db.Fetch<BossTransDet>("Select SupplierName,ItemName,Sum(QtyAdded) as QtyAdded,sum(QtyAdded * Price)  as Amount from SiteTransasction st inner join item i on st.ItemID = i.ItemID inner join Supplier s on s.SupplierID = st.SupplierID Where SiteID = 1 and Price IS NOT NULL Group BY  SupplierName,ItemName ", Site);
            ViewBag.Income = db.Fetch<SuppBillDet>("Select SupplierName,LabourName,Sum(Qty) as Qty,sum(Qty  *UnitPrice) as Amount from SupplierBill sb inner join SupplierBillDetail sbd on sb.SBillID = sbd.SBillID inner join Sites s on s.SiteID = sb.SiteID inner join Labour l on l.LabourID = sbd.LabourID inner join Supplier sp on sp.SupplierID =sb.SupplierID where sb.SiteID = @0 Group By SupplierName,LabourName", Site);
            return View();
        }

        public ActionResult ProfitLossSummary()
        {    
            ViewBag.Expenditure = db.Fetch<BossTransDet>("Select SiteName,Sum(QtyAdded) as QtyAdded,sum(QtyAdded*Price) as Amount from SiteTransasction st inner join item i on st.ItemID = i.ItemID  inner join Sites ss on st.SiteID = ss.SiteID where  Price IS NOT NULL Group BY  SiteName ");
            ViewBag.Income = db.Fetch<SuppBillDet>("Select SiteName,Sum(Qty) as Qty,sum(Qty * UnitPrice) as Amount from SupplierBill sb inner join SupplierBillDetail sbd on sb.SBillID = sbd.SBillID inner join Sites s on s.SiteID = sb.SiteID   Group By SiteName");
            return View();
        }

        public ActionResult RetentionReport(int? id)
        {
            if (id != null)
            {
                ClientBill clientbill = db.SingleOrDefault<ClientBill>("select * from ClientBill where CBillID =@0", id);
                clientbill.RetentionAmtIsPaid = true;
                base.BaseSave<ClientBill>(clientbill, true);
            }
            return View("RetentionReport", base.BaseIndex<ClientDet>(null, " *  ","ClientBill cb inner join Client c on cb.ClientID = c.ClientID where RetentionAmtIsPaid = '0' "));
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
