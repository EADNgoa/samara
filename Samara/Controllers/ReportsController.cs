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
            ViewBag.lmd = lastMonDate;
            ViewBag.cmd = CurrMonDate;
            ViewBag.Yr = RptYr;
            ViewBag.OpeningBalance = db.Fetch<StockSummaryDet>("Select ItemName,sum(Qty) as Qty from StockSummary ss inner join Item i on ss.ItemID =i.ItemID Where Tdate= @0 Group By ItemName,Qty",lastMonDate);
            ViewBag.ClosingBalance = db.Fetch<StockSummaryDet>(" Select ItemName,Sum(Qty) as Qty from StockSummary ss inner join Item i on ss.ItemID =i.ItemID Where Tdate= @0 Group By ItemName,Qty", CurrMonDate);
            return View("OCbalance");
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
