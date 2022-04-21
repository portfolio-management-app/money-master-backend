using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PublicAPI.Attributes
{
    public class CustomAllowedInputValidationAttribute : ValidationAttribute
    {
        public string[] AllowableValues { get; set; }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (AllowableValues?.Contains(value?.ToString()) is true) return ValidationResult.Success;

            var msg = $"Allowable values are {string.Join(", ", AllowableValues ?? new string[] { "No values" })}";
            return new ValidationResult(msg);
        }
    }
}