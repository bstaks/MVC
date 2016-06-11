using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcApplication1.DataModels
{
    public class aspnet_MenuPermission
    {
        [Key]
        [Column(Order = 0)]
        public Guid RoleId { get; set; }
        [Key]
        [Column(Order = 1)]
        public int MenuId { get; set; }
        public int Permission { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate{get;set;}

        public bool Active { get; set; }
        [ForeignKey("MenuId")]
        public virtual Menu Menus { get; set; }
        [ForeignKey("RoleId")]
        public virtual aspnet_Role aspnet_Roles { get; set; }
    }
}