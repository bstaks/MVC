using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcApplication1.DataModels
{
    [Table("Menu")]
    public class Menu
    {
        [Key]
        public int MenuId { get; set; }
        [StringLength(500)]
        [Required(ErrorMessage = "{0} is required!")]
        public string Name { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        public int? Order { get; set; }
        [Display(Name = "Menu Type")]
        public int MenuType { get; set; }
        [Display(Name = "Parent Menu")]
        public int ParentMenuId { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        [Display(Name = "Controller Name")]
        public string ControllerName { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        [Display(Name = "Action Name")]
        public string ActionName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool Active { get; set; }

        public IList<aspnet_MenuPermission> aspnet_MenuPermissions { get; set; }
    }
}