using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MvcApplication1.DataModels
{
    public class Location
    {
    [Key]
        public int LocationId { get; set; }
        [StringLength(200)]
        [Required]
        public string LocatioName { get; set; }
        [StringLength(2000)]
        public string Description { get; set; }
        public int? ParentLocationid { get; set; }
        public int LocationType { get; set; }
        public int? SortOrder { get; set; }
        public bool Active { get; set; }
    }
}