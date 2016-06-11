using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcApplication1.DataModels
{
    [Table("ValueType")]
    public class ValueTypeModels
    {
        [Key]
        [Display(Name="Code")]
        [Required]
        public int ValueTypeId { get; set; }
        [Required(ErrorMessage="{0} is required!")]
        public string ValueTypeName { get; set; }
        [Display(Name="Value Type Group")]
        public int ValueTypeGroupId { get; set; }
        public int? SortOrder { get; set; }
        public bool Active { get; set; }

        [ForeignKey("ValueTypeGroupId")]
        public ValueTypeGroupModels ValueTypeGroup { get; set; }
    }
}