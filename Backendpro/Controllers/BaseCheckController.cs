using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Backendpro.Controllers
{
    public class BaseCheckController : Controller
    {
        // GET: BaseCheck
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var result = filterContext.Result;
            List<string> lst = new List<string>();
            lst.Add("Customers");
            lst.Add("Locations");
            lst.Add("Products");
            lst.Add("Vendors");
            lst.Add("Watsapp");

            var temp = filterContext.Controller.ToString().Replace("Backendpro.Controllers.", "").Replace("Controller", "");
            if (System.Web.HttpContext.Current.Session["logintype"] != null)
            {
                if (System.Web.HttpContext.Current.Session["logintype"].ToString() == "Vendor" && lst.Contains(temp))
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(
                                new
                                {
                                    controller = "Account",
                                    action = "Login"
                                }));
                }
                else
                {
                    base.OnActionExecuting(filterContext);
                }


            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(
                                new
                                {
                                    controller = "Account",
                                    action = "Login"
                                }));
            }

        }
    }
}