using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CAMS.BusinessLogic.Services.Validators
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class EndDateAfterAttribute : ValidationAttribute
    {
        private readonly string _startDatePropertyName;

        public EndDateAfterAttribute(string startDatePropertyName)
        {
            _startDatePropertyName = startDatePropertyName;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            PropertyInfo? startDateProp = validationContext.ObjectType.GetProperty(_startDatePropertyName);
            if (startDateProp == null)
                return new ValidationResult($"Unknown property: {_startDatePropertyName}");

            var startDateValue = startDateProp.GetValue(validationContext.ObjectInstance);
            if (startDateValue == null || value == null)
                return ValidationResult.Success;

            if (startDateValue is DateTime startDate && value is DateTime endDate)
            {
                if (endDate <= startDate)
                    return new ValidationResult(ErrorMessage ?? $"End date must be after {_startDatePropertyName}.");
            }

            return ValidationResult.Success;
        }
    }
}
