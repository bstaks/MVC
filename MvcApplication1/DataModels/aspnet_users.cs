using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcApplication1.DataModels
{
    public class aspnet_users
    {
        [Key]
        public Guid UserId { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NumericUserId { get; set; }
        [Required(ErrorMessage = "{0} is rquired!")]
        [StringLength(200)]
        public string UserName { get; set; }
        [StringLength(200)]
        public string LowerUserName { get; set; }
        public DateTime CreatedDate { get; set; }
        public IList<aspnet_UsersInRoles> UserInRoles { get; set; }

    }
}