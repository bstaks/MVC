using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcApplication1.DataModels
{
    public class aspnet_Role
    {
        [Key]
        public Guid RoleId { get; set; }
        [StringLength(300)]
        public string RoleName { get; set; }
        public IList<aspnet_UsersInRoles> UserInRoles { get; set; }

        public IList<aspnet_MenuPermission> aspnetMenuPermission { get; set; }
    }
}