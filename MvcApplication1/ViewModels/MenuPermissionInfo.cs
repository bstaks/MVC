using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcApplication1.DataModels;

namespace MvcApplication1.ViewModels
{
    public class MenuPermissionInfo
    {
        public Menu Menu { get; set; }
        public List<Menu> ChildMenu { get; set; }
    }
}