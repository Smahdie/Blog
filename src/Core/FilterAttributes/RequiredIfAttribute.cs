using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;


namespace Core.FilterAttributes
{
    public class RequiredIfAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly string DependentProperty;
        private readonly object DependentPropertyValue;

        public RequiredIfAttribute(string dependentProperty, object dependentPropertyValue)
        {
            DependentProperty = dependentProperty;
            DependentPropertyValue = dependentPropertyValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var properties = validationContext.ObjectType.GetRuntimeProperties();
            var property = properties.FirstOrDefault(a => a.Name == DependentProperty);
            if (property == null)
            {
                return new ValidationResult("Unknown property");
            }

            var otherPropertyValue = property.GetValue(validationContext.ObjectInstance, null);

            if (otherPropertyValue == null)
            {
                return ValidationResult.Success;
            }

            var intOtherPropertyValue = otherPropertyValue;

            if (intOtherPropertyValue == DependentPropertyValue)
            {
                if (value == null || value as string == string.Empty)
                {
                    var message = string.Format(CultureInfo.CurrentCulture,
                    FormatErrorMessage(validationContext.DisplayName),
                    new object[] { DependentProperty });

                    return new ValidationResult(message);
                }
            }

            return ValidationResult.Success;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            var message = string.Format(CultureInfo.CurrentCulture,
                    FormatErrorMessage(context.ModelMetadata.GetDisplayName()),
                    new object[] { DependentProperty });

            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-requiredIf", message);
            context.Attributes.Add("data-val-requiredIf-otherPropertyName", DependentProperty);
            context.Attributes.Add("data-val-requiredIf-otherPropertyValue", DependentPropertyValue.ToString());
        }
    }
}
