using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Samara.Controllers
{
    public class EAController : Controller
    {
        // GET: EA
        protected Repository db;
        public EAController()
        {
            this.db = new Repository();
        }

        // GET: Agents
        protected IEnumerable<T> BaseIndex<T>(string TableWithWhere)
        {
            return db.Query<T>($"Select * from {TableWithWhere}");
        }

        protected T BaseCreateEdit<T>(int? id, string IDname)
        {
            T a;
            if (id.HasValue) //is edit
            {
                a = db.SingleOrDefault<T>($"where {IDname} = @0", id);
                return a;
            }
            return default(T);
        }

        protected ActionResult BaseSave<T>(T ObjToSave, bool isExisting)
        {
            if (ModelState.IsValid)
            {
                var r = (isExisting)? db.Update(ObjToSave): db.Insert(ObjToSave);
                return RedirectToAction("Index");
            }

            return View(ObjToSave);
        }


        // GET: EA
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //if (DateTime.Now.Date > DateTime.Parse("15 Aug 2017"))
            //{                
            //    filterContext.Result = new RedirectResult("~/Home/pli");
            //    return;
            //}

        }

        protected override void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                string controller = filterContext.RouteData.Values["controller"].ToString();
                string action = filterContext.RouteData.Values["action"].ToString();
                Exception ex = filterContext.Exception;

                filterContext.Result = new ViewResult
                {
                    ViewName = "Error",
                    ViewData = new ViewDataDictionary(new HandleErrorInfo(ex, controller, action))
                };
                filterContext.ExceptionHandled = true;
            }
        }
    }
}