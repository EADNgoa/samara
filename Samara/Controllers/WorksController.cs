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
    public class WorksController : EAController
    {
        public ActionResult Index(int? page, string WorkName)
        {
            if (WorkName?.Length > 0) page = 1;
            return View("Index", base.BaseIndex<MasterWork>(page, "WorkID,WorkName, UnitName,Rate", "Work w Inner Join Units u on w.UnitID = u.UnitID where WorkName like '%" + WorkName + "%'"));
        }



        // GET: Clients/Create
        public ActionResult Manage(int? id)
        {
            ViewBag.UnitID = new SelectList(db.Fetch<Unit>("Select UnitID,UnitName from Units"), "UnitID", "UnitName");

            return View(base.BaseCreateEdit<Work>(id, "WorkID"));
        }


        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage([Bind(Include = "WorkID,WorkName,UnitID,Rate")] Work work)
        {
            work.Rate = 0;
            return base.BaseSave<Work>(work, work.WorkID > 0);
        }

        public ActionResult Details(int? id)
        {
            
             var viewdata = new MAsterWork_Details
             {
                        Work = db.FirstOrDefault<MasterWork>("Select WorkID,WorkName,UnitName,Rate from Work w Inner Join Units u on w.UnitID =u.UnitID where WorkID = @0", id),
                        WorkDets = db.Fetch<MasterWorkDetails>("Select * From WorkDetails wd inner join Item i on wd.ItemID = i.ItemID inner join Units u on i.UnitID = u.UnitID Where WorkID = @0", id)
                 
            };
            ViewBag.UnitID = new SelectList(db.Fetch<Unit>("Select UnitID,UnitName from Units"), "UnitID", "UnitName");
            ViewBag.WorkID = id;
                    
            return View("Details", viewdata);            
        }
  
        [HttpPost]     
        public ActionResult Details([Bind(Include = "WorkDetailID,WorkID,UnitID,Rate,ItemID,Qty,Amount")] WorkDetail workDetail)
        {            
            using (var transaction = db.GetTransaction())
            {
                try
                {
                    var getWR = db.FirstOrDefault<decimal>("Select Rate From Work Where WorkID= @0", workDetail.WorkID);
                    var getItemRate = db.FirstOrDefault<decimal>("Select Rate From Item Where ItemID = @0",workDetail.ItemID);

                    workDetail.Amount = workDetail.Qty * getItemRate;
                    base.BaseSave<WorkDetail>(workDetail, workDetail.WorkDetailID > 0);
                    db.Update("Work", "WorkID", new { Rate = workDetail.Amount + getWR}, workDetail.WorkID);
                    transaction.Complete();

                }
                
                catch (Exception ex)
            {
                db.AbortTransaction();
                throw ex;
            }


        }
            return RedirectToAction("Details",new {id=workDetail.WorkID });
        }

       
        public ActionResult ManageDetails(int? id)
        {
            ViewBag.WorkDets = base.BaseCreateEdit<WorkDetail>(id, "WorkDetailID");
            ViewBag.WorkName = db.FirstOrDefault<string>("Select itemName From WorkDetails wd inner  join  Item as i on wd.ItemID = i.ItemID where WorkDetailID= @0", id);

            var viewdata = new MAsterWork_Details
            {
                Work = db.FirstOrDefault<MasterWork>("Select WorkID,WorkName,UnitName,Rate from Work w Inner Join Units u on w.UnitID =u.UnitID where WorkID = @0", id),
                WorkDets = db.Fetch<MasterWorkDetails>("Select * From WorkDetails wd inner join Item i on wd.ItemID = i.ItemID inner join Units u on i.UnitID = u.UnitID Where WorkID = @0", id)
            };

            
            return View("Details", viewdata);            
        }
  
        [HttpPost]
        public ActionResult ManageDetails([Bind(Include = "WorkDetailID,WorkID,ItemID,Qty,Rate,Amount")] WorkDetail workDetail,Decimal? or)
        {
            var getWR = db.FirstOrDefault<Work>("Select Rate From Work Where WorkID= @0", workDetail.WorkID);
            using (var transaction = db.GetTransaction())
            {
                try
                {
                    workDetail.Amount = workDetail.Qty * workDetail.Rate;
                    db.Update("Work", "WorkID", new { Rate = workDetail.Amount + getWR.Rate -or },workDetail.WorkID);
                    base.BaseSave<WorkDetail>(workDetail, workDetail.WorkDetailID > 0);
                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    db.AbortTransaction();
                    throw ex;
                }
            }
            return RedirectToAction("Details",new { id = workDetail.WorkID });
        }




        public ActionResult LabourDetails(int? id)
        {

            var viewdata = new MAsterWork_Details
            {
                Work = db.FirstOrDefault<MasterWork>("Select WorkID,WorkName,UnitName,Rate from Work w Inner Join Units u on w.UnitID =u.UnitID where WorkID = @0", id),
                WorkDets = db.Fetch<MasterWorkDetails>("Select * From WorkDetails wd inner join Labour l on wd.LabourID = l.LabourID  Where WorkID = @0", id)

            };
            ViewBag.LabourID = new SelectList(db.Fetch<Labour>("Select LabourID,LabourName from Labour"), "LabourID", "LabourName");
            ViewBag.WorkID = id;

            return View("LabourDetails", viewdata);
        }

        [HttpPost]
        public ActionResult LabourDetails([Bind(Include = "WorkDetailID,WorkID,UnitID,LabourID,Qty,Rate,Amount")] WorkDetail workDetail)
        {
            var getWR = db.FirstOrDefault<decimal>("Select Rate From Work Where WorkID= @0", workDetail.WorkID);
            var labourRate = db.FirstOrDefault<decimal>("Select Rate From Labour where LabourID = @0", workDetail.LabourID);
            using (var transaction = db.GetTransaction())
            {
                try
                {
                    workDetail.Amount = workDetail.Qty * labourRate;
                    base.BaseSave<WorkDetail>(workDetail, workDetail.WorkDetailID > 0);
                    db.Update("Work", "WorkID", new { Rate = workDetail.Amount + getWR }, workDetail.WorkID);
                    transaction.Complete();

                }

                catch (Exception ex)
                {
                    db.AbortTransaction();
                    throw ex;
                }


            }
            return RedirectToAction("LabourDetails", new { id = workDetail.WorkID });
        }
        public ActionResult LabourManageDetails(int? id)
        {
            ViewBag.WorkDets = base.BaseCreateEdit<WorkDetail>(id, "WorkDetailID");
            ViewBag.WorkName = db.FirstOrDefault<string>("Select LabourName From WorkDetails wd inner  join  Labour as l on wd.LabourID = l.LabourID where WorkDetailID= @0", id);

            var viewdata = new MAsterWork_Details
            {
                Work = db.FirstOrDefault<MasterWork>("Select WorkID,WorkName,UnitName,Rate from Work w Inner Join Units u on w.UnitID =u.UnitID where WorkID = @0", id),
                WorkDets = db.Fetch<MasterWorkDetails>("Select * From WorkDetails wd inner join Labour l on l.LabourID =  wd.LabourID Where WorkID = @0", id)
            };


            return View("LabourDetails", viewdata);
        }

        [HttpPost]
        public ActionResult LabourManageDetails([Bind(Include = "WorkDetailID,WorkID,LabourID,Qty,Rate,Amount")] WorkDetail workDetail, Decimal? or)
        {
            var getWR = db.FirstOrDefault<Work>("Select Rate From Work Where WorkID= @0", workDetail.WorkID);
            using (var transaction = db.GetTransaction())
            {
                try
                {
                    var labourRate = db.FirstOrDefault<decimal>("Select Rate From Labour where LabourID = @0",workDetail.LabourID);
                    workDetail.Amount = workDetail.Qty * labourRate;
                    db.Update("Work", "WorkID", new { Rate = workDetail.Amount + getWR.Rate - or }, workDetail.WorkID);
                    base.BaseSave<WorkDetail>(workDetail, workDetail.WorkDetailID > 0);
                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    db.AbortTransaction();
                    throw ex;
                }
            }
            return RedirectToAction("LabourDetails", new { id = workDetail.WorkID });
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
