using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.CustomModel
{
    public class CustomerInfoModels
    {
        public long CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string StateName { get; set; }

        public string CityName { get; set; }

        public string AddressLine { get; set; }

        public string AddressLine2 { get; set; }

        public int Pincode { get; set; }

        public string EmailId { get; set; }

        public string Mobile { get; set; }
    }
}