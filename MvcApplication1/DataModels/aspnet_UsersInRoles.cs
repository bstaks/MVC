using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcApplication1.DataModels
{
    public class aspnet_UsersInRoles
    {
        [Key]
        [Column(Order = 0)]
        public Guid UserId { get; set; }
        [Key]
        [Column(Order=1)]
        public Guid RoleId { get; set; }

        [ForeignKey("UserId")]
        public aspnet_users User { get; set; }
        [ForeignKey("RoleId")]
        public aspnet_Role Role { get; set; }
    }
}