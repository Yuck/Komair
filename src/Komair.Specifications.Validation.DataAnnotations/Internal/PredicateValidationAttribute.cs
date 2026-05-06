using System.ComponentModel.DataAnnotations;

namespace Komair.Specifications.Validation.DataAnnotations.Internal;

internal sealed class PredicateValidationAttribute<T>(Func<T, Boolean> predicate, String message, String? propertyPath) : ValidationAttribute(message)
{
    protected override ValidationResult? IsValid(Object? value, ValidationContext validationContext)
    {
        if (validationContext.ObjectInstance is not T candidate)
            return ValidationResult.Success;

        if (predicate(candidate))
            return ValidationResult.Success;

        return String.IsNullOrWhiteSpace(propertyPath) ? new ValidationResult(ErrorMessageString) : new ValidationResult(ErrorMessageString, [propertyPath]);
    }
}
