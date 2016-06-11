using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcApplication1.Custom
{
    public class DropdownValidation : ValidationAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            return base.FormatErrorMessage(name);
        }

        public override bool IsValid(object value)
        {
            int myValue = Convert.ToInt32(value);
            if (myValue != 0)
            {
                return true;
            }
            return false;
        }

       
    }
}