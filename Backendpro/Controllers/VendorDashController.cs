using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Backendpro.Controllers
{
    [Authorize]
    public class VendorDashController : BaseCheckController
    {
        // GET: VendorDash
        public ActionResult Index()
        {
            return View();
        }
    }
}