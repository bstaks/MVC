using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MvcApplication1.Models
{
    public class testModels
    {
        
        public string Id { get; set; }
        [Display(Name="Phone Number")]
        [Required]
        public string PhoneNumber { get; set; }
    }
}