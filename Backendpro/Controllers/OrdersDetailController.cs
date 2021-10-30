using Backendpro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Backendpro.Controllers
{
    [Authorize]
    public class OrdersDetailController : BaseCheckController
    {
        string accSid = "ACaa0d3f4c0b6a4ca0a0689977521620c1";
        string authToken = "b79fc9d05e7b04b1af70daffd603dffa";
        string fromwatsapp = "whatsapp:+14155238886";
        private moolansEntities1 db = new moolansEntities1();
        // GET: OrdersDetail
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NewOrders()
        {
            ViewBag.logintype = Session["logintype"].ToString();
            ViewBag.Start = null;
            ViewBag.End = null;
            
            List<OrderVM> final = new List<OrderVM>();
            if(Session["logintype"].ToString() == "Admin")
            {
                List<Order> lst = db.Orders.Where(x => x.Status == "New").OrderByDescending(x => x.OrderDate).ToList();
                List<Customer> customers = db.Customers.ToList();
                List<Location> locations = db.Locations.ToList();
                foreach (var ord in lst)
                {
                    OrderVM vm = new OrderVM();
                    vm.Orderid = ord.Orderid; vm.OrderSerialno = ""; vm.OrderImage = ord.Imagepath; vm.Ordertext = ord.Ordertext; vm.Status = ord.Status;
                    vm.Customerid = ord.Customerid.Value;
                    if (string.IsNullOrEmpty(vm.Ordertext))
                    {
                        vm.Isphoto = true;
                    }
                    else
                    {
                        vm.Isphoto = false;
                    }
                    if (vm.OrderImage == null)
                    {
                        vm.OrderImage = @"~/OrderFiles/defaultorderimage.jpg";
                    }
                    Customer cust = customers.Where(x => x.Customerid == vm.Customerid).FirstOrDefault();
                    if (cust != null)
                    {
                        vm.CustomerMobileno = cust.Customermobileno;
                        vm.Customername = cust.Customername;
                        if (ord.Locationid.HasValue)
                        {
                            int temploc = ord.Locationid.Value;
                            Location loc = locations.Where(x => x.Locationid == temploc).FirstOrDefault();
                            if (loc != null)
                            {
                                vm.Locationid = loc.Locationid; vm.Location = loc.Location1;
                                final.Add(vm);
                            }
                        }
                        else
                        {
                            Location loc = locations.Where(x => cust.Locationid.HasValue && x.Locationid == cust.Locationid).FirstOrDefault();
                            if (loc != null)
                            {
                                vm.Locationid = loc.Locationid; vm.Location = loc.Location1;
                                final.Add(vm);
                            }
                        }
                        
                    }
                    
                }
            }else if (Session["logintype"].ToString() == "Vendor")
            {
                int locationid = Convert.ToInt32(Session["location"].ToString());
                List<Order> lst = db.Orders.Where(x => x.Status == "New").OrderByDescending(x => x.OrderDate).ToList();
                List<Customer> customers = db.Customers.Where(x => x.Locationid.HasValue && x.Locationid.Value == locationid).ToList();
                lst = (from ls in lst
                       join cs in customers on ls.Customerid equals cs.Customerid
                       select ls).ToList();
                List < Location > locations = db.Locations.ToList();
                foreach (var ord in lst)
                {
                    OrderVM vm = new OrderVM();
                    vm.Orderid = ord.Orderid; vm.OrderSerialno = ""; vm.OrderImage = ord.Imagepath; vm.Ordertext = ord.Ordertext; vm.Status = ord.Status;
                    vm.Customerid = ord.Customerid.Value;
                    if (string.IsNullOrEmpty(vm.Ordertext))
                    {
                        vm.Isphoto = true;
                    }
                    else
                    {
                        vm.Isphoto = false;
                    }
                    if (vm.OrderImage == null)
                    {
                        vm.OrderImage = @"~/OrderFiles/defaultorderimage.jpg";
                    }
                    Customer cust = customers.Where(x => x.Customerid == vm.Customerid).FirstOrDefault();
                    if (cust != null)
                    {
                        vm.CustomerMobileno = cust.Customermobileno;
                        vm.Customername = cust.Customername;
                        if (ord.Locationid.HasValue)
                        {
                            int temploc = ord.Locationid.Value;
                            Location loc = locations.Where(x => x.Locationid == temploc).FirstOrDefault();
                            if (loc != null)
                            {
                                vm.Locationid = loc.Locationid; vm.Location = loc.Location1;
                                final.Add(vm);
                            }
                        }
                        else
                        {
                            Location loc = locations.Where(x => cust.Locationid.HasValue && x.Locationid == cust.Locationid).FirstOrDefault();
                            if (loc != null)
                            {
                                vm.Locationid = loc.Locationid; vm.Location = loc.Location1;
                                final.Add(vm);
                            }
                        }

                    }

                }
            }
            
            
            return View(final);
        }

        public ActionResult PreviousOrders()
        {
            ViewBag.logintype = Session["logintype"].ToString();
            ViewBag.Start = null;
            ViewBag.End = null;
            List<OrderVM> final = new List<OrderVM>();
            if (Session["logintype"].ToString() == "Admin")
            {
                List<Order> lst = db.Orders.Where(x => x.Status != "New").OrderByDescending(x => x.OrderDate).ToList();
                List<Customer> customers = db.Customers.ToList();
                List<Location> locations = db.Locations.ToList();
                foreach (var ord in lst)
                {
                    OrderVM vm = new OrderVM();
                    vm.Orderid = ord.Orderid; vm.OrderSerialno = ""; vm.OrderImage = ord.Imagepath; vm.Ordertext = ord.Ordertext; vm.Status = ord.Status;
                    vm.Customerid = ord.Customerid.Value;
                    if (string.IsNullOrEmpty(vm.Ordertext))
                    {
                        vm.Isphoto = true;
                    }
                    else
                    {
                        vm.Isphoto = false;
                    }
                    if (ord.Status == "Completed")
                    {
                        vm.isButtonDisabled = true;
                    }
                    if (vm.OrderImage == null)
                    {
                        vm.OrderImage = @"~/OrderFiles/defaultorderimage.jpg";
                    }
                    Customer cust = customers.Where(x => x.Customerid == vm.Customerid).FirstOrDefault();
                    if (cust != null)
                    {
                        vm.CustomerMobileno = cust.Customermobileno;
                        vm.Customername = cust.Customername;
                        if (ord.Locationid.HasValue)
                        {
                            int temploc = ord.Locationid.Value;
                            Location loc = locations.Where(x => x.Locationid == temploc).FirstOrDefault();
                            if (loc != null)
                            {
                                vm.Locationid = loc.Locationid; vm.Location = loc.Location1;
                                final.Add(vm);
                            }
                        }
                        else
                        {
                            Location loc = locations.Where(x => cust.Locationid.HasValue && x.Locationid == cust.Locationid).FirstOrDefault();
                            if (loc != null)
                            {
                                vm.Locationid = loc.Locationid; vm.Location = loc.Location1;
                                final.Add(vm);
                            }
                        }

                    }

                }
            }
            else if (Session["logintype"].ToString() == "Vendor")
            {
                List<Order> lst = db.Orders.Where(x => x.Status != "New").OrderByDescending(x => x.OrderDate).ToList();
                int locationid = Convert.ToInt32(Session["location"].ToString());
                List<Customer> customers = db.Customers.Where(x => x.Locationid.HasValue && x.Locationid == locationid).ToList();
                lst = (from ls in lst
                       join cs in customers on ls.Customerid equals cs.Customerid
                       select ls).ToList();
                List<Location> locations = db.Locations.ToList();
                foreach (var ord in lst)
                {
                    OrderVM vm = new OrderVM();
                    vm.Orderid = ord.Orderid; vm.OrderSerialno = ""; vm.OrderImage = ord.Imagepath; vm.Ordertext = ord.Ordertext; vm.Status = ord.Status;
                    vm.Customerid = ord.Customerid.Value;
                    if (string.IsNullOrEmpty(vm.Ordertext))
                    {
                        vm.Isphoto = true;
                    }
                    else
                    {
                        vm.Isphoto = false;
                    }
                    if (ord.Status == "Completed")
                    {
                        vm.isButtonDisabled = true;
                    }
                    if (vm.OrderImage == null)
                    {
                        vm.OrderImage = @"~/OrderFiles/defaultorderimage.jpg";
                    }
                    Customer cust = customers.Where(x => x.Customerid == vm.Customerid).FirstOrDefault();
                    if (cust != null)
                    {
                        vm.CustomerMobileno = cust.Customermobileno;
                        vm.Customername = cust.Customername;
                        if (ord.Locationid.HasValue)
                        {
                            int temploc = ord.Locationid.Value;
                            Location loc = locations.Where(x => x.Locationid == temploc).FirstOrDefault();
                            if (loc != null)
                            {
                                vm.Locationid = loc.Locationid; vm.Location = loc.Location1;
                                final.Add(vm);
                            }
                        }
                        else
                        {
                            Location loc = locations.Where(x => cust.Locationid.HasValue && x.Locationid == cust.Locationid).FirstOrDefault();
                            if (loc != null)
                            {
                                vm.Locationid = loc.Locationid; vm.Location = loc.Location1;
                                final.Add(vm);
                            }
                        }

                    }

                }
            }


            return View(final);
        }

        public ActionResult AcceptOrder(int Orderid)
        {
            ViewBag.logintype = Session["logintype"].ToString();
            Order ord = db.Orders.Where(x => x.Orderid == Orderid).FirstOrDefault();
            Vendor loc = db.Vendors.ToList().Where(x => x.Locationid.Value == Convert.ToInt32(Session["location"].ToString())).First();
            if(ord != null)
            {
                ord.Status = "Pending";
                db.Entry(ord).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            string regmobileno = loc.RegisteredMobileno;
            if (!regmobileno.Contains("+91"))
            {
                regmobileno = "+91" + regmobileno;
            }

            Customer cust = db.Customers.ToList().Where(x => x.Customerid == ord.Customerid.Value).First();
            string custmobileno = cust.Customermobileno.Contains("+91") ? "whatsapp:" + cust.Customermobileno : "whatsapp:+91" + cust.Customermobileno;
            TwilioClient.Init(accSid, authToken);
            MessageResource.Create(
                body: "We are processing your order with OrderId" + ord.OrderSerialNo + ". For any assistance, you may please contact : " + regmobileno,
                from: new Twilio.Types.PhoneNumber(fromwatsapp),
                to: new Twilio.Types.PhoneNumber(custmobileno)
                ) ;
            return RedirectToAction("NewOrders");
        }

        public ActionResult RejectOrder(int Orderid)
        {
            ViewBag.logintype = Session["logintype"].ToString();
            Order ord = db.Orders.Where(x => x.Orderid == Orderid).FirstOrDefault();
            Vendor loc = db.Vendors.ToList().Where(x => x.Locationid.Value == Convert.ToInt32(Session["location"].ToString())).First();
            if (ord != null)
            {
                ord.Status = "Rejected";
                db.Entry(ord).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            string regmobileno = loc.RegisteredMobileno;
            if (!regmobileno.Contains("+91"))
            {
                regmobileno = "+91" + regmobileno;
            }
            TwilioClient.Init(accSid, authToken);
            MessageResource.Create(
                body: "Your order has been rejected due to some inconvenience! Please try again.",
                from: new Twilio.Types.PhoneNumber(fromwatsapp),
                to: new Twilio.Types.PhoneNumber("watsapp:" + regmobileno)
                );
            return RedirectToAction("NewOrders");
        }

        public ActionResult DispatchedOrder(int Orderid)
        {
            ViewBag.logintype = Session["logintype"].ToString();
            Order ord = db.Orders.Where(x => x.Orderid == Orderid).FirstOrDefault();
            Vendor loc = db.Vendors.ToList().Where(x => x.Locationid.Value == Convert.ToInt32(Session["location"].ToString())).First();
            if (ord != null)
            {
                ord.Status = "Dispatched";
                db.Entry(ord).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            string regmobileno = loc.RegisteredMobileno;
            if (!regmobileno.Contains("+91"))
            {
                regmobileno = "+91" + regmobileno;
            }
            TwilioClient.Init(accSid, authToken);
            MessageResource.Create(
                body: "Your order (" + ord.OrderSerialNo +") is dispatched. Team Moolans will deliver your order Shortly.",
                from: new Twilio.Types.PhoneNumber(fromwatsapp),
                to: new Twilio.Types.PhoneNumber("watsapp:" + regmobileno)
                );
            return RedirectToAction("NewOrders");
        }

        public ActionResult CompleteOrder(int Orderid)
        {
            ViewBag.logintype = Session["logintype"].ToString();
            Order ord = db.Orders.Where(x => x.Orderid == Orderid).FirstOrDefault();
            Vendor loc = db.Vendors.ToList().Where(x => x.Locationid.Value == Convert.ToInt32(Session["location"].ToString())).First();
            if (ord != null)
            {
                ord.Status = "Completed";
                db.Entry(ord).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            string regmobileno = loc.RegisteredMobileno;
            if (!regmobileno.Contains("+91"))
            {
                regmobileno = "+91" + regmobileno;
            }
            TwilioClient.Init(accSid, authToken);
            MessageResource.Create(
                body: "Your order (" + ord.OrderSerialNo + ") is dispatched. Team Moolans will deliver your order Shortly.",
                from: new Twilio.Types.PhoneNumber(fromwatsapp),
                to: new Twilio.Types.PhoneNumber("watsapp:" + regmobileno)
                );
            return RedirectToAction("PreviousOrders");
        }

        [HttpGet]
        public ActionResult FilterdateNew(string startdate, string enddate)
        {

            if(startdate == "" || enddate == "")
            {
                return View("NewOrders");
            }
            ViewBag.Start = startdate;
            ViewBag.End = enddate;
            DateTime start = Convert.ToDateTime(startdate);
            DateTime end = Convert.ToDateTime(enddate).AddMinutes(1439);
            ViewBag.logintype = Session["logintype"].ToString();

            List<OrderVM> final = new List<OrderVM>();
            if (Session["logintype"].ToString() == "Admin")
            {
                List<Order> lst = db.Orders.Where(x => x.Status == "New" && x.OrderDate.Value >= start && x.OrderDate.Value <= end).OrderByDescending(x => x.OrderDate).ToList();
                List<Customer> customers = db.Customers.ToList();
                List<Location> locations = db.Locations.ToList();
                foreach (var ord in lst)
                {
                    OrderVM vm = new OrderVM();
                    vm.Orderid = ord.Orderid; vm.OrderSerialno = ""; vm.OrderImage = ord.Imagepath; vm.Ordertext = ord.Ordertext; vm.Status = ord.Status;
                    vm.Customerid = ord.Customerid.Value;
                    if (string.IsNullOrEmpty(vm.Ordertext))
                    {
                        vm.Isphoto = true;
                    }
                    else
                    {
                        vm.Isphoto = false;
                    }
                    if (ord.Status == "Completed")
                    {
                        vm.isButtonDisabled = true;
                    }
                    if (vm.OrderImage == null)
                    {
                        vm.OrderImage = @"~/OrderFiles/defaultorderimage.jpg";
                    }
                    Customer cust = customers.Where(x => x.Customerid == vm.Customerid).FirstOrDefault();
                    if (cust != null)
                    {
                        vm.CustomerMobileno = cust.Customermobileno;
                        vm.Customername = cust.Customername;
                        if (ord.Locationid.HasValue)
                        {
                            int temploc = ord.Locationid.Value;
                            Location loc = locations.Where(x => x.Locationid == temploc).FirstOrDefault();
                            if (loc != null)
                            {
                                vm.Locationid = loc.Locationid; vm.Location = loc.Location1;
                                final.Add(vm);
                            }
                        }
                        else
                        {
                            Location loc = locations.Where(x => cust.Locationid.HasValue && x.Locationid == cust.Locationid).FirstOrDefault();
                            if (loc != null)
                            {
                                vm.Locationid = loc.Locationid; vm.Location = loc.Location1;
                                final.Add(vm);
                            }
                        }

                    }

                }
            }
            else if (Session["logintype"].ToString() == "Vendor")
            {
                int locationid = Convert.ToInt32(Session["location"].ToString());
                List<Order> lst = db.Orders.Where(x => x.Status == "New" && x.OrderDate.Value >= start && x.OrderDate.Value <= end).OrderByDescending(x => x.OrderDate).ToList();
                List<Customer> customers = db.Customers.Where(x => x.Locationid.HasValue && x.Locationid.Value == locationid).ToList();
                lst = (from ls in lst
                       join cs in customers on ls.Customerid equals cs.Customerid
                       select ls).ToList();
                List<Location> locations = db.Locations.ToList();
                foreach (var ord in lst)
                {
                    OrderVM vm = new OrderVM();
                    vm.Orderid = ord.Orderid; vm.OrderSerialno = ""; vm.OrderImage = ord.Imagepath; vm.Ordertext = ord.Ordertext; vm.Status = ord.Status;
                    vm.Customerid = ord.Customerid.Value;
                    if (string.IsNullOrEmpty(vm.Ordertext))
                    {
                        vm.Isphoto = true;
                    }
                    else
                    {
                        vm.Isphoto = false;
                    }
                    if (ord.Status == "Completed")
                    {
                        vm.isButtonDisabled = true;
                    }
                    if (vm.OrderImage == null)
                    {
                        vm.OrderImage = @"~/OrderFiles/defaultorderimage.jpg";
                    }
                    Customer cust = customers.Where(x => x.Customerid == vm.Customerid).FirstOrDefault();
                    if (cust != null)
                    {
                        vm.CustomerMobileno = cust.Customermobileno;
                        vm.Customername = cust.Customername;
                        if (ord.Locationid.HasValue)
                        {
                            int temploc = ord.Locationid.Value;
                            Location loc = locations.Where(x => x.Locationid == temploc).FirstOrDefault();
                            if (loc != null)
                            {
                                vm.Locationid = loc.Locationid; vm.Location = loc.Location1;
                                final.Add(vm);
                            }
                        }
                        else
                        {
                            Location loc = locations.Where(x => cust.Locationid.HasValue && x.Locationid == cust.Locationid).FirstOrDefault();
                            if (loc != null)
                            {
                                vm.Locationid = loc.Locationid; vm.Location = loc.Location1;
                                final.Add(vm);
                            }
                        }

                    }

                }
            }
            return View("NewOrders", final);
        }

        [HttpGet]
        public ActionResult FilterdatePre(string startdate, string enddate)
        {

            if (startdate == "" || enddate == "")
            {
                return View("NewOrders");
            }
            ViewBag.Start = startdate;
            ViewBag.End = enddate;
            DateTime start = Convert.ToDateTime(startdate);
            DateTime end = Convert.ToDateTime(enddate).AddMinutes(1439);
            ViewBag.logintype = Session["logintype"].ToString();

            List<OrderVM> final = new List<OrderVM>();
            if (Session["logintype"].ToString() == "Admin")
            {
                List<Order> lst = db.Orders.Where(x => x.Status != "New" && x.OrderDate.Value >= start && x.OrderDate.Value <= end).OrderByDescending(x => x.OrderDate).ToList();
                List<Customer> customers = db.Customers.ToList();
                List<Location> locations = db.Locations.ToList();
                foreach (var ord in lst)
                {
                    OrderVM vm = new OrderVM();
                    vm.Orderid = ord.Orderid; vm.OrderSerialno = ""; vm.OrderImage = ord.Imagepath; vm.Ordertext = ord.Ordertext; vm.Status = ord.Status;
                    vm.Customerid = ord.Customerid.Value;
                    if (string.IsNullOrEmpty(vm.Ordertext))
                    {
                        vm.Isphoto = true;
                    }
                    else
                    {
                        vm.Isphoto = false;
                    }
                    if (vm.OrderImage == null)
                    {
                        vm.OrderImage = @"~/OrderFiles/defaultorderimage.jpg";
                    }
                    Customer cust = customers.Where(x => x.Customerid == vm.Customerid).FirstOrDefault();
                    if (cust != null)
                    {
                        vm.CustomerMobileno = cust.Customermobileno;
                        vm.Customername = cust.Customername;
                        if (ord.Locationid.HasValue)
                        {
                            int temploc = ord.Locationid.Value;
                            Location loc = locations.Where(x => x.Locationid == temploc).FirstOrDefault();
                            if (loc != null)
                            {
                                vm.Locationid = loc.Locationid; vm.Location = loc.Location1;
                                final.Add(vm);
                            }
                        }
                        else
                        {
                            Location loc = locations.Where(x => cust.Locationid.HasValue && x.Locationid == cust.Locationid).FirstOrDefault();
                            if (loc != null)
                            {
                                vm.Locationid = loc.Locationid; vm.Location = loc.Location1;
                                final.Add(vm);
                            }
                        }

                    }

                }
            }
            else if (Session["logintype"].ToString() == "Vendor")
            {
                int locationid = Convert.ToInt32(Session["location"].ToString());
                List<Order> lst = db.Orders.Where(x => x.Status != "New" && x.OrderDate.Value >= start && x.OrderDate.Value <= end).OrderByDescending(x => x.OrderDate).ToList();
                List<Customer> customers = db.Customers.Where(x => x.Locationid.HasValue && x.Locationid.Value == locationid).ToList();
                lst = (from ls in lst
                       join cs in customers on ls.Customerid equals cs.Customerid
                       select ls).ToList();
                List<Location> locations = db.Locations.ToList();
                foreach (var ord in lst)
                {
                    OrderVM vm = new OrderVM();
                    vm.Orderid = ord.Orderid; vm.OrderSerialno = ""; vm.OrderImage = ord.Imagepath; vm.Ordertext = ord.Ordertext; vm.Status = ord.Status;
                    vm.Customerid = ord.Customerid.Value;
                    if (string.IsNullOrEmpty(vm.Ordertext))
                    {
                        vm.Isphoto = true;
                    }
                    else
                    {
                        vm.Isphoto = false;
                    }
                    if (vm.OrderImage == null)
                    {
                        vm.OrderImage = @"~/OrderFiles/defaultorderimage.jpg";
                    }
                    Customer cust = customers.Where(x => x.Customerid == vm.Customerid).FirstOrDefault();
                    if (cust != null)
                    {
                        vm.CustomerMobileno = cust.Customermobileno;
                        vm.Customername = cust.Customername;
                        if (ord.Locationid.HasValue)
                        {
                            int temploc = ord.Locationid.Value;
                            Location loc = locations.Where(x => x.Locationid == temploc).FirstOrDefault();
                            if (loc != null)
                            {
                                vm.Locationid = loc.Locationid; vm.Location = loc.Location1;
                                final.Add(vm);
                            }
                        }
                        else
                        {
                            Location loc = locations.Where(x => cust.Locationid.HasValue && x.Locationid == cust.Locationid).FirstOrDefault();
                            if (loc != null)
                            {
                                vm.Locationid = loc.Locationid; vm.Location = loc.Location1;
                                final.Add(vm);
                            }
                        }

                    }

                }
            }
            return View("PreviousOrders", final);
        }

        public PartialViewResult SeeOrderImage(int orderid)
        {
            Order order = db.Orders.Where(x => x.Orderid == orderid).First();
            OrderVM vm = new OrderVM();
            if (string.IsNullOrEmpty(order.Imagepath))
            {
                vm.OrderImage = @"~/OrderFiles/defaultorderimage.jpg";
            }
            else
            {
                vm.OrderImage = order.Imagepath;
            }
            

            return PartialView(vm);
        }

        public PartialViewResult SeeOrderDetails(int orderid)
        {
            OrderVM vm = new OrderVM();
            
            Order order = db.Orders.ToList().Where(x => x.Orderid == orderid).First();
                

            vm.Ordertext = order.Ordertext;
            return PartialView(vm);
            
        }
    }
}