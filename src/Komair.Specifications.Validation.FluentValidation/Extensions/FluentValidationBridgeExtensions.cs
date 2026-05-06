using FluentValidation;
using Komair.Specifications.Abstract.Interfaces;
using Komair.Specifications.Validation.Abstractions.Rules;

namespace Komair.Specifications.Validation.FluentValidation.Extensions;

/// <summary>
/// Extension helpers for creating FluentValidation validators from bridge inputs.
/// </summary>
public static class FluentValidationBridgeExtensions
{
    /// <summary>
    /// Builds a FluentValidation validator from normalized rule descriptors.
    /// </summary>
    /// <typeparam name="T">The model type being validated.</typeparam>
    /// <param name="rules">The normalized rules.</param>
    /// <returns>The FluentValidation validator.</returns>
    public static IValidator<T> ToFluentValidator<T>(this IEnumerable<ValidationRuleDescriptor<T>> rules)
    {
        ArgumentNullException.ThrowIfNull(rules);

        var bridge = new FluentValidationBridge<T>();
        var result = bridge.Translate(rules);

        return result.Artifacts.Single();
    }

    /// <summary>
    /// Builds a FluentValidation validator from a specification and rule metadata.
    /// </summary>
    /// <typeparam name="T">The model type being validated.</typeparam>
    /// <param name="specification">The specification to convert.</param>
    /// <param name="messageTemplate">The message used when validation fails.</param>
    /// <param name="propertyPath">The optional explicit property path.</param>
    /// <param name="errorCode">The optional error code.</param>
    /// <param name="severity">The validation severity.</param>
    /// <param name="tags">The optional rule tags.</param>
    /// <returns>The FluentValidation validator.</returns>
    public static IValidator<T> ToFluentValidator<T>(this ISpecification<T> specification, String messageTemplate, String? propertyPath = null, String? errorCode = null, ValidationSeverity severity = ValidationSeverity.Error, IEnumerable<String>? tags = null)
    {
        ArgumentNullException.ThrowIfNull(specification);
        ArgumentNullException.ThrowIfNull(messageTemplate);

        var bridge = new FluentValidationBridge<T>();
        var result = bridge.Translate(specification, messageTemplate, propertyPath, errorCode, severity, tags);

        return result.Artifacts.Single();
    }
}
