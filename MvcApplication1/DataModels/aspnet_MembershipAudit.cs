using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcApplication1.DataModels
{
    public class aspnet_MembershipAudit
    {
        public string EmailId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? LastLockOutDate { get; set; }
        public DateTime CreatedDate { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int aspnet_MemberShipAuditId { get; set; }

        public int EntityId { get; set; }
        public int EntityType { get; set; }

        [StringLength(200)]
        public string LastLoginIP { get; set; }
        
    }
}