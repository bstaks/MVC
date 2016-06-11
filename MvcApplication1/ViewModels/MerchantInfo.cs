using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcApplication1.DataModels;

namespace MvcApplication1.ViewModels
{
    public class MerchantInfo
    {
        public MerchantModels Merchant { get; set; }
        public EntityAddressModels Address { get; set; }
        public aspnet_users User { get; set; }
        public aspnet_Membership MemberShip { get; set; }
        public int CountryId { get; set; }
    }
}