using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace UniFirst.VehicleManagement
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    sealed public class VINValidationAttribute : ValidationAttribute
    {
        /// <summary>
        /// Vin must be 24 alphanumeric characters with a minimum of 8 alphas, ending with 5 numeric.
        /// </summary>
        /// <param name="value">The VIN string</param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var result = ValidationResult.Success;

            if (value != null)
            {
                if (value is String)
                {
                    var vin = (value as string).Trim();

                    if (vin.Length != 24)
                    {
                        result = new ValidationResult("VIN should be 24 characters in length.");
                    }
                    else if (vin.Count(c => char.IsLetter(c)) < 8 ||
                        !vin.Substring(vin.Length - 5).All(c => char.IsDigit(c)))
                    {
                        result = new ValidationResult("VIN must end with 5 digits and have at least 8 characters.");
                    }
                }
                else
                {
                    result = new ValidationResult("VIN should be a 24 character string.");
                }
            }

            return result;
        }
    }
}