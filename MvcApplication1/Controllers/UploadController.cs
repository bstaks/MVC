using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MvcApplication1.Controllers
{
    public class UploadController : ApiController
    {
        public void UploadPDFFile()
        {
            System.Web.HttpFileCollection fileObj = System.Web.HttpContext.Current.Request.Files;

        }
    }
}