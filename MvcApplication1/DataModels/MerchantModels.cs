using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcApplication1.DataModels
{
    [Table("Merchant")]
    public class MerchantModels
    {
        [Key]
        public int MerchantId { get; set; }
        [Required]
        public long MerchantCode { get; set; }
        [Required]
        public string MerchantName { get; set; }
        public string Title { get; set; }
        [Required]
        public int locationId { get; set; }
        public int? UserId { get; set; }
        public DateTime CreateDate { get; set; }

        public int CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public IList<EntityAddressModels> Address { get; set; }
    }
}