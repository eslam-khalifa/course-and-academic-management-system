using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CAMS.BusinessLogic.Services.Validators
{
    public class NoNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            string str = value.ToString()!;
            if (Regex.IsMatch(str, @"\d"))
                return new ValidationResult("The field cannot contain numbers.");

            return ValidationResult.Success;
        }
    }
}
