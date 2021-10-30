using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backendpro.Models
{
    public class CustomerVM
    {
        public int Customerid { get; set; }
        public string CustomerName { get; set; }
        public string CustomerMobileno { get; set; }

        public string Location { get; set; }

    }
}