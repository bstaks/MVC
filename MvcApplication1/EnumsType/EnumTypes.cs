using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.EnumsType
{
    public class EnumTypes
    {
        public enum MenuType
        {
            Parent=101,
            Child =102,
            SubChild =103

        }

        public enum EntityType
        {

            Customer=201,
            Merchant= 202,
            Admin =203
        }

       

        public enum AccountType
        {


        }

        public enum TransactionType
        {


        }

        public enum LocationType
        {

            Country = 501,
            State = 502,
            City = 503
        }

        public enum AddressType
        {
            Permanant=601,
            Communication =602
        }

        public enum PermissionType
        {
            View =1,
            Add =2,
            Edit =4,
            Delete =8,
        }

    }
}