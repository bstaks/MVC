

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace MvcApplication1.DataModels
{
    [Table("Customer", Schema = "Customer")]
    public class CustomerModels
    {
        [Key]
        public int CustomerId { get; set; }

        [RegularExpression("^(1)[0-9]{9}$",ErrorMessage="Invalid Customer Code")]
        public long CustomerCode { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        [Display(Name = "Name")]
        public string CustomerName { get; set; }
        public string Title { get; set; }

        public int locationId { get; set; }
        public int? UserId { get; set; }
        public DateTime CreateDate { get; set; }

        public int CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public IList<EntityAddressModels> Address { get; set; }
       
    }
}