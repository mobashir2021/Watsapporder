using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Twilio;
using Twilio.AspNet.Mvc;
using Backendpro.Models;
using Twilio.AspNet.Common;
using Twilio.TwiML;
using Twilio.Rest.Api.V2010.Account;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;


namespace Backendpro.Controllers
{
    
    public class WatsappController : TwilioController
    {
        private moolansEntities1 db = new moolansEntities1();
        string accSid = "ACaa0d3f4c0b6a4ca0a0689977521620c1";
        string authToken = "b79fc9d05e7b04b1af70daffd603dffa";

        


        public TwiMLResult Index(SmsRequest incomingMessage)
        {

            
            var messagingResponse = new MessagingResponse();
            var mediaUrl = Request.Form["MediaUrl0"];
            var sid = Request.Form["MessageSid"];

            List<Customer> lst = db.Customers.ToList();
            List<Location> locations = db.Locations.ToList();
            List<MonthsCode> lstmonths = db.MonthsCodes.ToList();
            List<Vendor> lstvendors = db.Vendors.ToList();
            
            string path = ControllerContext.HttpContext.Server.MapPath("~/OrderFiles");
            
            var mainrootlocation = "";
            if (mediaUrl != null)
            {
                
                var dir = "/OrderFiles/";
                var filename = $"{sid}.jpg";
                var pathsave = Environment.CurrentDirectory  + dir;
                var saveLocation = path + "\\" + filename;
                mainrootlocation = "~/OrderFiles/" + filename;

                using (var web = new WebClient())
                {
                    byte[] imageBytes = web.DownloadData(mediaUrl);
                    FileStream fs = new FileStream(saveLocation, FileMode.Create);
                    BinaryWriter bw = new BinaryWriter(fs);
                    bw.Write(imageBytes);

                }
            }

            string parsedFrom = incomingMessage.From.Replace("whatsapp:", "");
            var customer = lst.Where(x => x.Customermobileno == parsedFrom).FirstOrDefault();
            if(customer == null)
            {
                Customer cust = new Customer();
                cust.Customermobileno = incomingMessage.From.Replace("whatsapp:", "");
                cust.Customername = incomingMessage.From.Replace("whatsapp:", "");
                db.Customers.Add(cust);
                db.SaveChanges();

                Order order = new Order();
                order.Customerid = cust.Customerid;
                order.OrderDate = DateTime.Now;
                order.Ordertext = incomingMessage.Body;
                if (mediaUrl != null)
                {
                    order.Imagepath = mainrootlocation;
                }
                order.Status = "New";
                db.Orders.Add(order);
                db.SaveChanges();

                



                messagingResponse.Message(GetproductTemplate());
                return TwiML(messagingResponse);
            }
            else
            {
                if (!customer.Locationid.HasValue)
                {
                    int iLocid = 0;
                    string strLocid = incomingMessage.Body.Replace(".", "").Trim();
                    if(int.TryParse(strLocid, out iLocid))
                    {
                        var locationcheck = locations.Where(x => x.Locationid == iLocid).FirstOrDefault();
                        if(locationcheck == null)
                        {
                            
                            messagingResponse.Message(GetproductTemplate("Kindly enter either number or name corresponding to below location"));
                            return TwiML(messagingResponse);
                        }
                        else
                        {
                            customer.Locationid = iLocid;



                            db.Entry(customer).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();

                            Order temporder = db.Orders.ToList().Where(x => x.Customerid.Value == customer.Customerid).First();
                            Location temploc = locations.Where(x => x.Locationid == customer.Locationid.Value).FirstOrDefault();
                            string Vendorno = "";
                            if (temploc != null)
                            {
                                
                                string loccode = temploc.LocationCode == null ? "" : temploc.LocationCode;
                                var mnth = lstmonths.Where(x => x.MonthsCodeId == DateTime.Now.Month).First();
                                temporder.OrderSerialNo = loccode + " " + mnth.MonthsCode1 + " " + DateTime.Now.Day.ToString() + " " + (temporder.Orderid + 99).ToString();
                                Vendorno = lstvendors.Where(x => x.Locationid.Value == temploc.Locationid).First().RegisteredMobileno;
                            }
                            
                            db.Entry(temporder).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            if(Vendorno != "")
                            {
                                messagingResponse.Message("Thanks for your order to Moolans Super Market. Your Order ID is <" + temporder.OrderSerialNo + ">.For any assistance, you may please contact to <" + Vendorno +">. \n\n Thank you... \n Moolans");
                            }
                            else
                            {
                                messagingResponse.Message("Thanks for your order to Moolans Super Market. Your Order ID is <" + temporder.OrderSerialNo + ">. \n\n Thank you... \n Moolans");
                            }
                            
                            return TwiML(messagingResponse);
                        }
                    }
                    else
                    {
                        var locfind = locations.Where(x => x.Location1.ToLower() == strLocid.Trim()).FirstOrDefault();
                        if(locfind != null)
                        {
                            customer.Locationid = locfind.Locationid;



                            db.Entry(customer).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();

                            Order temporder = db.Orders.ToList().Where(x => x.Customerid.Value == customer.Customerid).First();
                            
                            string loccode = locfind.LocationCode == null ? "" : locfind.LocationCode;
                            var mnth = lstmonths.Where(x => x.MonthsCodeId == DateTime.Now.Month).First();
                            temporder.OrderSerialNo = loccode + " " + mnth.MonthsCode1 + " " + DateTime.Now.Day.ToString() + " " + (temporder.Orderid + 99).ToString();
                            
                            db.Entry(temporder).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();

                            string Vendorno = lstvendors.Where(x => x.Locationid.Value == customer.Locationid.Value).First().RegisteredMobileno;
                            if (Vendorno != "")
                            {
                                messagingResponse.Message("Thanks for your order to Moolans Super Market. Your Order ID is <" + temporder.OrderSerialNo + ">.For any assistance, you may please contact to <" + Vendorno + ">. \n\n Thank you... \n Moolans");
                            }
                            else
                            {
                                messagingResponse.Message("Thanks for your order to Moolans Super Market. Your Order ID is <" + temporder.OrderSerialNo + ">. \n\n Thank you... \n Moolans");
                            }
                            return TwiML(messagingResponse);
                        }
                        else
                        {
                            messagingResponse.Message(GetproductTemplate("Kindly enter either number or name corresponding to below location"));
                            return TwiML(messagingResponse);
                        }
                        
                    }
                }
                else
                {
                    Order order = new Order();
                    order.Customerid = customer.Customerid;
                    
                    order.OrderDate = DateTime.Now;
                    order.Ordertext = incomingMessage.Body;
                    order.Status = "New";
                    db.Orders.Add(order);
                    db.SaveChanges();
                    Location temploc = locations.Where(x => x.Locationid == customer.Locationid.Value).FirstOrDefault();
                    string Vendorno = "";
                    if (temploc != null)
                    {
                        string loccode = temploc.LocationCode == null ? "" : temploc.LocationCode;
                        var mnth = lstmonths.Where(x => x.MonthsCodeId == DateTime.Now.Month).First();
                        order.OrderSerialNo = loccode + " " + mnth.MonthsCode1 + " " + DateTime.Now.Day.ToString() + " " + (order.Orderid + 99).ToString();
                        Vendorno = lstvendors.Where(x => x.Locationid.Value == customer.Locationid.Value).First().RegisteredMobileno;
                    }

                    if (mediaUrl != null)
                    {
                        order.Imagepath = mainrootlocation;
                    }
                    db.Entry(order).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    if (Vendorno != "")
                    {
                        messagingResponse.Message("Thanks for your order to Moolans Super Market. Your Order ID is <" + order.OrderSerialNo + ">.For any assistance, you may please contact to <" + Vendorno + ">. \n\n Thank you... \n Moolans");
                    }
                    else
                    {
                        messagingResponse.Message("Thanks for your order to Moolans Super Market. Your Order ID is <" + order.OrderSerialNo + ">. \n\n Thank you... \n Moolans");
                    }
                    return TwiML(messagingResponse);
                }
            }


            //messagingResponse.Message("Hey there, this is from web" + incomingMessage.Body + " from : " + incomingMessage.From + " , " + incomingMessage.To);
            //return TwiML(messagingResponse);
        }

        private void SendWatsappMessage(string to, string from, string message)
        {
            TwilioClient.Init(accSid, authToken);
            MessageResource.Create(
                body: message,
                from: new Twilio.Types.PhoneNumber(from),
                to: new Twilio.Types.PhoneNumber(to)
                );
        }

        private string GetproductTemplate(string locationval = "")
        {
            string returnvalue = "";
            List<Location> lst = db.Locations.ToList();
            returnvalue = "Kindly choose a location!\n";
            if(locationval == "")
                returnvalue = returnvalue + "Kindly Enter the number corresponding to location\n";
            foreach(var loc in lst)
            {
                returnvalue = returnvalue + loc.Locationid.ToString() + ". " + loc.Location1 + "\n";
            }

            return returnvalue;
        }
    }
}
