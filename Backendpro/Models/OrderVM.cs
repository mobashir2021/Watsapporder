using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backendpro.Models
{
    public class OrderVM
    {
        public int Orderid { get; set; }
        public string OrderSerialno { get; set; }
        public int Customerid { get; set; }
        public string CustomerMobileno { get; set; }
        public string Location { get; set; }
        public string vendorname { get; set; }
        public string Vendormobileno { get; set; }
        public int Locationid { get; set; }
        public string Ordertext { get; set; }
        public string OrderImage { get; set; }

        public bool Isphoto { get; set; }
        public string Customername { get; set; }

        public string Status { get; set; }

        public string Neworders { get; set; }
        public string Pendingorders { get; set; }
        public string Completedorders { get; set; }

        public bool IsOrdertext { get; set; }

        public bool isButtonDisabled { get; set; }

    }
}