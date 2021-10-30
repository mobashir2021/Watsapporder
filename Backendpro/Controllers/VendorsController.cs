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
    public class VendorsController : BaseCheckController
    {
        private moolansEntities1 db = new moolansEntities1();

        // GET: Vendors
        public ActionResult Index()
        {
            return View(db.Vendors.ToList());
        }

        // GET: Vendors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendor vendor = db.Vendors.Find(id);
            if (vendor == null)
            {
                return HttpNotFound();
            }
            return View(vendor);
        }

        // GET: Vendors/Create
        public ActionResult Create()
        {
            List<Location> locationlist = db.Locations.ToList();
            ViewBag.locationlist = new SelectList(locationlist, "Locationid", "Location1");
            return View();
        }

        // POST: Vendors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Vendorid,Vendorname,RegisteredMobileno,Locationid,Password")] Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                db.Vendors.Add(vendor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vendor);
        }

        // GET: Vendors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendor vendor = db.Vendors.Find(id);
            if (vendor == null)
            {
                return HttpNotFound();
            }
            return View(vendor);
        }

        // POST: Vendors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Vendorid,Vendorname,RegisteredMobileno,Locationid,Password")] Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vendor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vendor);
        }

        // GET: Vendors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendor vendor = db.Vendors.Find(id);
            if (vendor == null)
            {
                return HttpNotFound();
            }
            return View(vendor);
        }

        // POST: Vendors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vendor vendor = db.Vendors.Find(id);
            db.Vendors.Remove(vendor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult EnableVendor()
        {
            List<Vendor> vendorlist = db.Vendors.Where(x => x.Status == null).ToList();
            ViewBag.vendorslist = new SelectList(vendorlist, "Vendorid", "RegisteredMobileno");
            return View();
        }
        [HttpPost]
        public ActionResult EnableVendor(string vendorsvalue)
        {
            if (!string.IsNullOrEmpty(vendorsvalue))
            {
                Vendor vd = db.Vendors.ToList().Where(x => x.Vendorid == Convert.ToInt32(vendorsvalue)).First();
                vd.Status = "Active";
                db.Entry(vd).State = EntityState.Modified;
                db.SaveChanges();
            }
            List<Vendor> vendorlist = db.Vendors.Where(x => x.Status == null).ToList();
            ViewBag.vendorslist = new SelectList(vendorlist, "Vendorid", "RegisteredMobileno");
            return View();
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
