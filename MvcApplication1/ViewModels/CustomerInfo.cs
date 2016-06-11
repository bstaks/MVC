using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcApplication1.DataModels;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MvcApplication1.ViewModels
{
    public class CustomerInfo
    {
        public CustomerModels Customer { get; set; }
        public EntityAddressModels Address { get; set; }
        public aspnet_users User { get; set; }
        public aspnet_Membership MemberShip { get; set; }
        public int CountryId { get; set; }
      
    
    }

}