using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MvcApplication1.DataModels
{
    public class aspnet_Membership
    {
        [Key]
        //[DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        //public int aspnetMemberShipId { get; set; }
        public Guid UserId { get; set; }
        [Required(ErrorMessage="{0} is required!")]
        public string Password { get; set; }
        public int PasswordFormat { get; set; }
        public string PasswordSalt { get; set; }
        public bool isLive { get; set; }
        public bool IslockedOut { get; set; }
        [Required(ErrorMessage="{0} is required!")]
        [StringLength(300)]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string Emailid { get; set; }
        public string SecurityQuestion
        {
            get;
            set;
        }
        public string SecurityAns { get; set; }
        public int? FailedPasswordAttemtptCount { get; set; }
        public DateTime? LastLockedDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        [StringLength(200)]
        public string LastLoginIP { get; set; }
        [ForeignKey("UserId")]
        public virtual aspnet_users User { get; set; }
    }
}