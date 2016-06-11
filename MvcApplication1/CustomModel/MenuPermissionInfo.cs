using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.CustomModel
{
    public class MenuPermissionInfo
    {
        public MenuPermissionInfo()
        {
            MenuPermissionInfos = new List<MenuPermissionInfo>();
        }
        public string Name { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public int Permission { get; set; }

        public Guid? RoleId { get; set; }
        public int MenuId { get; set; }
        public int ParentMenuId { get; set; }
        public int MenuType { get; set; }
        public bool isChecked { get; set; }

        public List<MenuPermissionInfo> MenuPermissionInfos { get; set; }
    }
}