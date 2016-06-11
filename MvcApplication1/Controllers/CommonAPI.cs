using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using MvcApplication1.DataModels;
using MvcApplication1.EnumsType;

namespace MvcApplication1.Controllers
{
    [Authorize]
    public class CommonAPIController : ApiController
    {
        public List<ValueTypeGroupModels> GetValueTypeGroup() {

            using (ShoppingCart dbContext = new ShoppingCart())
            {
                return dbContext.ValueTypeGroup.Select(m => m).ToList();
            }
        }

      
        public List<Location> GetCountry(int LocationType)
        {

            using (ShoppingCart dbContext = new ShoppingCart())
            {
                return dbContext.Location.Where(m => m.LocationType == LocationType).OrderBy(m => m.LocatioName).ToList();
            }
        }

        public List<Location> GetLocation(int LocationType,int ParentLocationId)
        {

            return GetCountry(LocationType).Where(m => m.ParentLocationid == ParentLocationId).OrderBy(m => m.LocatioName).ToList();
        } 
    }
}