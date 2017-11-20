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
    public class SitePictureController : EAController
    {
     
        public ActionResult Index(int? page ,string SiteName )

        {
           if(SiteName?.Length >0) page = 1;
           return View("Index", base.BaseIndex<SitePictureDet>(page, "SitePictureID,DTime,Picture,SiteName,Email,Comment", "SitePictures sp inner join Sites s on sp.SiteID = s.SiteID inner join AspNetUsers asu on sp.UserID = asu.Id where SiteName like '%" + SiteName + "%'  "));
        }


       

        public ActionResult Manage(int? id)
        {

            var rec = base.BaseCreateEdit<SitePicture>(id, "SitePictureID");


            if (id != null)
            {
                SitePictureImg sp = new SitePictureImg()
                {
                   Comment = rec.Comment,
                   DTime =(DateTime) rec.DTime,
                   Picture = rec.Picture,
                   SitePictureID =rec.SitePictureID,
                   SiteID =(int)rec.SiteID,
                   UserID= rec.UserID                
                };
                ViewBag.FromSiteName = db.SingleOrDefault<string>("Select SiteName from Sites where SiteID = @0", rec.SiteID);

                return View(sp);
            } else
            {
                SitePictureImg sp = new SitePictureImg() { UserID = User.Identity.GetUserId()};
                return View(sp);
            }
            
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage([Bind(Include = "SitePictureID,UserID,DTime,SiteID,,Picture,UploadedFile,Comment")] SitePictureImg sitePicture)
      {


                if (sitePicture.UploadedFile != null || sitePicture.SitePictureID > 0)

                {
                    SitePicture res = new SitePicture
                    {
                        Comment = sitePicture.Comment,
                        DTime = DateTime.Now,
                        SiteID = sitePicture.SiteID,
                        SitePictureID = sitePicture.SitePictureID,
                        UserID = sitePicture.UserID
                    };

                    if (sitePicture.UploadedFile != null)
                    {
                        string fn = sitePicture.UploadedFile.FileName.Substring(sitePicture.UploadedFile.FileName.LastIndexOf('\\') + 1);
                        fn = sitePicture.SiteID + "_" + fn;
                        string SavePath = System.IO.Path.Combine(Server.MapPath("~/Images"), fn);
                        sitePicture.UploadedFile.SaveAs(SavePath);

                        //System.Drawing.Bitmap upimg = new System.Drawing.Bitmap(siteTransaction.UploadedFile.InputStream);
                        //System.Drawing.Bitmap svimg = MyExtensions.CropUnwantedBackground(upimg);
                        //svimg.Save(System.IO.Path.Combine(Server.MapPath("~/Images"), fn));

                        res.Picture = fn;
                    }
                    else
                    {
                        res.Picture = sitePicture.Picture;
                    }

                    base.BaseSave<SitePicture>(res, sitePicture.SitePictureID > 0);
                }

            return RedirectToAction("Index");
                               
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
