using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Samara.Controllers
{
    public class HomeController : EAController
    {
        public ActionResult Index(int? page)
        {
            page = 1;
          //  return View("Index", db.Fetch<ReorderDet>("Select ItemName,SiteName,Qty,ReorderLevel from Item as i inner join SiteCurrentStock as scs on i.ItemID = scs.ItemID inner join Sites s on s.SiteID = scs.SiteID Where Qty < ReorderLevel "));
            return View("Index", base.BaseIndex<ReorderDet>(page, "ItemName,SiteName,Qty,ReorderLevel", " Item as i inner join SiteCurrentStock as scs on i.ItemID = scs.ItemID inner join Sites s on s.SiteID = scs.SiteID Where Qty < ReorderLevel"));

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}