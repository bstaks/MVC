using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.Custom
{
    public class CustomPartialView : ViewResultBase 
    {

        protected override ViewEngineResult FindView(ControllerContext context)
        {
            throw new NotImplementedException();
        }
    }
}