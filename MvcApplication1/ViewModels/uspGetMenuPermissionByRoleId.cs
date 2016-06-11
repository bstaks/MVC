using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.ViewModels
{
    public class uspGetMenuPermissionByRoleId
    {
        public string Name { get; set; }
        public int MenuId { get; set; }
        public int Permission { get; set; }

        public int MenuType { get; set; }

        public int ParentMenuId { get; set; }
    }
}