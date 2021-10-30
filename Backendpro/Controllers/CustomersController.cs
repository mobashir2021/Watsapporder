using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Backendpro.Models;

namespace Backendpro.Controllers
{
    [Authorize]
    public class CustomersController : BaseCheckController
    {
        private moolansEntities1 db = new moolansEntities1();

        // GET: Customers
        public ActionResult Index()
        {
            ViewBag.logintype = Session["logintype"].ToString();
            List<CustomerVM> final = new List<CustomerVM>();
            List<Location> locs = db.Locations.ToList();
            List<Customer> cus = db.Customers.ToList();
            if (Session["logintype"].ToString() == "Admin")
            {
                foreach(var cusvar in cus)
                {
                    CustomerVM cs = new CustomerVM();
                    cs.Customerid = cusvar.Customerid;
                    cs.CustomerMobileno = cusvar.Customermobileno;
                    cs.CustomerName = cusvar.Customername;
                    Location fnloc = locs.Where(x => x.Locationid == cusvar.Locationid.Value).FirstOrDefault();
                    if(fnloc != null)
                    {
                        cs.Location = fnloc.Location1;
                    }
                    final.Add(cs);
                }
                
            }
            else if(Session["logintype"].ToString() == "Vendor")
            {
                int locationid = Convert.ToInt32(Session["location"].ToString());
                cus = cus.Where(x => x.Locationid.Value == locationid).ToList();
                foreach (var cusvar in cus)
                {
                    CustomerVM cs = new CustomerVM();
                    cs.Customerid = cusvar.Customerid;
                    cs.CustomerMobileno = cusvar.Customermobileno;
                    cs.CustomerName = cusvar.Customername;
                    Location fnloc = locs.Where(x => x.Locationid == cusvar.Locationid.Value).FirstOrDefault();
                    if (fnloc != null)
                    {
                        cs.Location = fnloc.Location1;
                    }
                    final.Add(cs);
                }

            }
            return View(final);
        }

        public ActionResult SeeOrders(int Customerid)
        {
            ViewBag.logintype = Session["logintype"].ToString();
            var lst = db.Orders.Where(x => x.Customerid.Value == Customerid).ToList();
            List<OrderVM> final = new List<OrderVM>();
            Customer cust = db.Customers.Where(x => x.Customerid == Customerid).FirstOrDefault();

            List<Location> locations = db.Locations.ToList();
            foreach (var ord in lst)
            {
                OrderVM vm = new OrderVM();
                vm.Orderid = ord.Orderid; vm.OrderSerialno = ""; vm.OrderImage = ord.Imagepath; vm.Ordertext = ord.Ordertext; vm.Status = ord.Status;
                vm.Customerid = ord.Customerid.Value;
                if (vm.OrderImage == null)
                {
                    vm.OrderImage = @"~/OrderFiles/defaultorderimage.jpg";
                }
                
                if (cust != null)
                {
                    vm.CustomerMobileno = cust.Customermobileno;
                    Location loc = locations.Where(x => x.Locationid == cust.Locationid).FirstOrDefault();
                    if (loc != null)
                    {
                        vm.Locationid = loc.Locationid; vm.Location = loc.Location1;
                    }
                }
                final.Add(vm);
            }
            return View(final);
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Customerid,Customername,Customermobileno,Locationid")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Customerid,Customername,Customermobileno,Locationid")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
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
