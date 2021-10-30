using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Backendpro.Models;

namespace Backendpro.Controllers
{
    [Authorize]
    public class HomeController : BaseCheckController
    {
        moolansEntities1 db = new moolansEntities1();
        public ActionResult Index()
        {
            //List<OrderVM> final = new List<OrderVM>();
            //List<Order> lstord = db.Orders.ToList();
            //List<Customer> customers = db.Customers.ToList();
            ////List<Location> locs = db.Locations.ToList();
            //List<Vendor> ven = db.Vendors.ToList();
            //List<VendorRows> rows = new List<VendorRows>();
            //foreach(var v in ven)
            //{
            //    OrderVM vm = new OrderVM();
            //    vm.vendorname = v.Vendorname;
            //    vm.Vendormobileno = v.RegisteredMobileno;
            //    int neword = 0;
            //    int pending = 0;
            //    int completed = 0;
            //    var res = (from c in customers
            //               join o in lstord on c.Customerid equals o.Customerid
            //               where c.Locationid.HasValue && c.Locationid == v.Locationid.Value
            //               select o).ToList();

            //    foreach(var o in res)
            //    {
            //        if (o.Status == "New")
            //            neword = neword + 1;
            //        else if (o.Status == "Pending")
            //            pending = pending + 1;
            //        else if (o.Status == "Completed")
            //            completed = completed + 1;

            //    }

            //    vm.Neworders = neword.ToString();
            //    vm.Pendingorders = pending.ToString();
            //    vm.Completedorders = completed.ToString();
            //    if(rows.Count == 0)
            //    {
            //        VendorRows vr = new VendorRows();
            //        vr.colone = vm;
            //        rows.Add(vr);
                    
            //    }
            //    else
            //    {
            //        VendorRows last = rows.Last();
                    
                    
            //        if(last.colone == null)
            //        {
            //            last.colone = vm;
            //        }
            //        else if(last.coltwo == null)
            //        {
            //            last.coltwo = vm;
            //        }
            //        else if(last.colthree == null)
            //        {
            //            last.colthree = vm;
            //        }
            //        else
            //        {
            //            VendorRows vr = new VendorRows();
            //            rows.Add(vr);
            //            vr.colone = vm;
            //        }
            //    }
                

            //    //final.Add(vm);
            //}
            return View();
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