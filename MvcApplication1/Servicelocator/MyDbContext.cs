using MvcApplication1.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Servicelocator
{
    public class MyDbContext 
    {
        public ShoppingCart GetDbContext()
        {
            return new ShoppingCart();
        }
    }
}