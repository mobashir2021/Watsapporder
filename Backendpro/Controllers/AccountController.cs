using Backendpro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Backendpro.Controllers
{
    public class AccountController : Controller
    {
        private moolansEntities1 db = new moolansEntities1();
        // GET: Account
        public ActionResult Login()
        {
            List<Location> locationlist = db.Locations.ToList();
            ViewBag.locationlist = new SelectList(locationlist, "Locationid", "Location1");
            ViewBag.Message = "";
            return View();
        }

        [HttpPost]
        public ActionResult Login(string name, string pass, string go, string next, string logintype, string regname, string regpass, string Location,string vendorname)
        {
            List<Location> locationlist = db.Locations.ToList();
            if (go != null)
            {
                if(logintype == "Admin")
                {
                    adminTable ad = db.adminTables.ToList().Where(x => x.Mobileno == name.Trim() && x.Password == pass).FirstOrDefault();
                    if(ad != null)
                    {
                        FormsAuthentication.SetAuthCookie("admin", false);
                        Session["logintype"] = "Admin";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.locationlist = new SelectList(locationlist, "Locationid", "Location1");
                        ViewBag.Message = "Wrong Credential";
                        return View();
                    }
                    
                }
                else if(logintype == "Vendor")
                {
                    Vendor vd = db.Vendors.ToList().Where(x => x.RegisteredMobileno == name && pass == x.Password && x.Status == "Active").FirstOrDefault();
                    if(vd != null)
                    {
                        FormsAuthentication.SetAuthCookie(vd.RegisteredMobileno, false);
                        Session["logintype"] = "Vendor";
                        Session["location"] = vd.Locationid.ToString();
                        return RedirectToAction("Index", "VendorDash");
                    }
                    else
                    {
                        
                        ViewBag.locationlist = new SelectList(locationlist, "Locationid", "Location1");
                        ViewBag.Message = "Wrong Credential or Not Active";
                        return View();
                    }
                }
                else
                {
                    
                    ViewBag.locationlist = new SelectList(locationlist, "Locationid", "Location1");
                    ViewBag.Message = "Wrong Username/Password";
                    return View();
                }
            }
            else if(next != null)
            {
                Vendor vendor = new Vendor();
                if (string.IsNullOrEmpty(vendorname))
                {
                    vendorname = regname;
                }
                vendor.RegisteredMobileno = regname;
                vendor.Password = regpass;
                vendor.Vendorname = vendorname;
                Location lct = db.Locations.Where(x => x.Location1.ToLower().Trim() == Location.ToLower().Trim()).FirstOrDefault();
                if(lct != null)
                {
                    vendor.Locationid = lct.Locationid;
                    db.Vendors.Add(vendor);
                    db.SaveChanges();
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.locationlist = new SelectList(locationlist, "Locationid", "Location1");
                    ViewBag.Message = "Choose correct Location";
                    return View();
                }
                
            }
            
            ViewBag.locationlist = new SelectList(locationlist, "Locationid", "Location1");
            ViewBag.Message = "";
            return View();
        }

        public ActionResult Logout()
        {
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            FormsAuthentication.SignOut();
            Session.Remove("logintype");
            Session.Remove("location");
            Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpPost]
        public JsonResult GetLocation(string Prefix)
        {
            //Note : you can bind same list from database  
            List<Location> objLocation = db.Locations.ToList();
            
            //Searching records from list using LINQ query  
            var Name = (from N in objLocation
                        where N.Location1.ToLower().StartsWith(Prefix.ToLower())
                        select new { N.Location1 });
            return Json(Name, JsonRequestBehavior.AllowGet);
        }
    }
}