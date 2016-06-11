using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcApplication1.DataModels
{
    [Table("ValueTypeGroup")]
    public class ValueTypeGroupModels
    {
        [Key]
        public int ValueTypeGroupId { get; set; }
        [Required(ErrorMessage="This is requried!")]
        public string ValueTypeGroupName { get; set; }
       
        public int CreatedBy { get; set; }
        public DateTime CaretedDate
        { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool Active { get; set; }

        public  ICollection<ValueTypeModels> ValueType { get; set; }
    }
}