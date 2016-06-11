using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MvcApplication1.Custom;

namespace MvcApplication1.DataModels
{
    [Table("EntityAddress")]
    public class EntityAddressModels
    {
        [Key]
        public int Addressid { get; set; }
        [StringLength(1000)]
        [Required(ErrorMessage = "{0} is rquried!")]
        [Display(Name = "Address Line1")]
        public string AddressLine { get; set; }
        [StringLength(1000)]
        public string AddressLine2 { get; set; }
        [Required]

        public int EntityId { get; set; }
        [Required]
        public int AddressTypeId { get; set; }

        [DropdownValidation(ErrorMessage="{0} is required!")]
        [Required(ErrorMessage = "{0} is required!")]
        [Display(Name = "State")]
        public int StateId { get; set; }

        [Required(ErrorMessage = "City is requied!")]
        [DropdownValidation(ErrorMessage="{0} is required!")]
        [Display(Name = "City")]
        public int CityId { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        public int Pincode { get; set; }
        public string Emailid { get; set; }
        [StringLength(200)]
        [Required(ErrorMessage = "{0} is requried!")]
        [RegularExpression("^(7|8|9)[0-9]{9}$", ErrorMessage = "{0} is invalid!")]
        public string Mobile { get; set; }

        public int EntityType { get; set; }
        
        [ForeignKey("EntityId")]
        public virtual CustomerModels Customer { get; set; }
       
        [ForeignKey("EntityId")]
        public virtual MerchantModels Merchant { get; set; }

    }
}