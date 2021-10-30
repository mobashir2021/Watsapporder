using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Backendpro.Models;
using Backendpro.common;
using Newtonsoft.Json;
using System.Drawing;
using System.IO;
using System.Web;

namespace Backendpro.Controllers
{
    public class BackendapiController : ApiController
    {
        moolansEntities1 db = new moolansEntities1();
        private string baseWebsite = "http://moolans.azurewebsites.net";

        [AllowCrossSiteJson]
        [HttpGet]
        public HttpResponseMessage GetLocations()
        {
            var lst = db.Locations.ToList();
            try
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(lst));
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return response;
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        [AllowCrossSiteJson]
        [HttpGet]
        public HttpResponseMessage GetProducts()
        {
            var lst = db.Products.ToList();
            try
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(lst));
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return response;
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        [AllowCrossSiteJson]
        [HttpGet]
        public HttpResponseMessage GetOrderList(string Customerid)
        {
            List<OrderVM> final = new List<OrderVM>();
            List<Customer> customers = db.Customers.ToList();
            int custid = Convert.ToInt32(Customerid);
            var lst = db.Orders.Where(x => x.Customerid.Value == custid).ToList();
            Customer cust = customers.Where(x => x.Customerid == custid).FirstOrDefault();
            //var lst = db.Orders.ToList();
            List<Location> locations = db.Locations.ToList();
            foreach (var ord in lst)
            {
                OrderVM vm = new OrderVM();
                vm.Orderid = ord.Orderid; vm.OrderSerialno = "";  vm.Ordertext = ord.Ordertext; vm.Status = ord.Status;
                vm.Customerid = ord.Customerid.Value;
                if (string.IsNullOrEmpty(ord.Imagepath))
                {
                    vm.OrderImage = baseWebsite + "/OrderFiles/defaultorderimage.jpg";
                }
                else
                {
                    vm.OrderImage = baseWebsite + ord.Imagepath.Replace("~","");
                }
                
                if (cust != null)
                {
                    vm.CustomerMobileno = cust.Customermobileno;
                    vm.Customername = cust.Customername;
                    Location loc = locations.Where(x => ord.Locationid.HasValue && x.Locationid == ord.Locationid.Value).FirstOrDefault();
                    if (loc != null)
                    {
                        vm.Locationid = loc.Locationid; vm.Location = loc.Location1;
                        
                    }
                }
                final.Add(vm);
            }


            try
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(final));
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return response;
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        [AllowCrossSiteJson]
        [HttpGet]
        public HttpResponseMessage Getlogin(string username, string password)
        {
            var lst = db.Customers.Where(x => x.Customermobileno == username && password == x.Password).FirstOrDefault();

            var message = "";
            if(lst != null)
            {
                
            }
            else
            {
                Customer cust = new Customer();
                cust.Customerid = -1;
                lst = cust;
            }

            try
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                
                response.Content = new StringContent(JsonConvert.SerializeObject(lst));
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return response;
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        [AllowCrossSiteJson]
        [HttpGet]
        public HttpResponseMessage RegisterCustomer(string name, string mobileno, string password, string selectedlocation, string address)
        {
            List<Customer> lst = db.Customers.ToList();
            var cnt = lst.Where(x => x.Customermobileno == mobileno).Count();
            var locs = db.Locations.ToList().Where(x => x.Location1.Trim().ToLower() == selectedlocation.Trim().ToLower()).First();
            var message = "";
            Customer returnvalue = new Customer();
            if (cnt == 0)
            {
                Customer cust = new Customer();
                cust.Customermobileno = mobileno;
                cust.Customername = name;
                cust.Password = password;
                cust.Address = address;
                cust.Locationid = locs.Locationid;
                db.Customers.Add(cust);
                db.SaveChanges();
                returnvalue = cust;
            }
            else
            {
                returnvalue.Customerid = -1;
            }
            
            
            try
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(returnvalue));
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return response;
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        [AllowCrossSiteJson]
        [HttpGet]
        public HttpResponseMessage GetOrder(string Customerid, string location, string ordertext)
        {
            List<MonthsCode> lstmonths = db.MonthsCodes.ToList();
            int locid = Convert.ToInt32(location);
            Location loc = db.Locations.Where(x => x.Locationid == locid).First();
            Order order = new Order();
            order.Customerid = Convert.ToInt32(Customerid);
            order.Locationid = loc.Locationid;
            order.Status = "New";
            order.OrderDate = DateTime.Now;
            order.Ordertext = ordertext;

            db.Orders.Add(order);
            db.SaveChanges();
            var mnth = lstmonths.Where(x => x.MonthsCodeId == DateTime.Now.Month).First();
            order.OrderSerialNo = loc.LocationCode + " " + mnth.MonthsCode1 + " " + DateTime.Now.Day.ToString() + " " + (order.Orderid + 99).ToString();
            db.Entry(order).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            var message = "Order Sent for approval successfully.";


            try
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(message));
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return response;
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        [AllowCrossSiteJson]
        [HttpGet]
        public HttpResponseMessage Updatepassword(string Customerid, string password)
        {
            
            var message = "Order Sent for approval successfully.";
            int cid = Convert.ToInt32(Customerid);

            var cust = db.Customers.Where(x => x.Customerid == cid).First();
            cust.Password = password;
            db.Entry(cust).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            try
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(cust));
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return response;
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        [AllowCrossSiteJson]
        [HttpGet]
        public HttpResponseMessage Updatelocation(string Customerid, string location)
        {
            int cid = Convert.ToInt32(Customerid.Trim());

            var cust = db.Customers.Where(x => x.Customerid == cid).First();
            var loc = db.Locations.Where(x => x.Location1.Trim().ToLower() == location.Trim().ToLower()).First();
            cust.Locationid = loc.Locationid;
            db.Entry(cust).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            try
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(cust));
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return response;
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }



        public System.Drawing.Image Base64ToImage(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

            ms.Write(imageBytes, 0, imageBytes.Length);
            return System.Drawing.Image.FromStream(ms, true);
        }

        [AllowCrossSiteJson]
        [HttpPost]
        public HttpResponseMessage GetOrderPhoto()
        {

            try
            {
                List<MonthsCode> lstmonths = db.MonthsCodes.ToList();
                var img = HttpContext.Current.Request.Params["base64image"];
                var location = HttpContext.Current.Request.Params["location"].ToString();
                int locid = Convert.ToInt32(location);
                var Customerid = HttpContext.Current.Request.Params["Customerid"].ToString();
                Location loc = db.Locations.Where(x => x.Locationid == locid).First();
                Order order = new Order();
                order.Customerid = Convert.ToInt32(Customerid);
                order.Locationid = loc.Locationid;
                order.Status = "New";
                order.OrderDate = DateTime.Now;
                //order.Ordertext = ordertext;
                string path = HttpContext.Current.Server.MapPath("~/OrderFiles");
                var dir = "/OrderFiles/";
                var filename = DateTime.Now.Ticks.ToString() + ".jpg";
                var pathsave = Environment.CurrentDirectory + dir;
                var saveLocation = path + "\\" + filename;
                byte[] bytes = Convert.FromBase64String(img);
                FileStream fs = new FileStream(saveLocation, FileMode.Create);
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Write(bytes);
                var mainrootlocation = "~/OrderFiles/" + filename;
                order.Imagepath = mainrootlocation;


                db.Orders.Add(order);
                db.SaveChanges();
                var mnth = lstmonths.Where(x => x.MonthsCodeId == DateTime.Now.Month).First();
                order.OrderSerialNo = loc.LocationCode + " " + mnth.MonthsCode1 + " " + DateTime.Now.Day.ToString() + " " + (order.Orderid + 99).ToString();
                db.Entry(order).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                var message = "Order Sent for approval successfully.";


                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(message));
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return response;
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }



        [HttpGet]
        public string Testapi(string values)
        {
            return values + " data";
        }

        

    }
}
