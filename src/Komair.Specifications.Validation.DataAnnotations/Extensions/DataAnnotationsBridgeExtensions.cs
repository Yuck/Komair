using Komair.Specifications.Abstract.Interfaces;
using Komair.Specifications.Validation.Abstractions.Rules;
using Komair.Specifications.Validation.Abstractions.Translations;

namespace Komair.Specifications.Validation.DataAnnotations.Extensions;

/// <summary>
/// Extension helpers for creating DataAnnotations translation artifacts from bridge inputs.
/// </summary>
public static class DataAnnotationsBridgeExtensions
{
    /// <summary>
    /// Builds DataAnnotations translation artifacts from normalized rules.
    /// </summary>
    /// <typeparam name="T">The model type being validated.</typeparam>
    /// <param name="rules">The normalized rules.</param>
    /// <returns>The translation result.</returns>
    public static ValidationTranslationResult<DataAnnotationsRuleArtifact<T>> ToDataAnnotationsArtifacts<T>(this IEnumerable<ValidationRuleDescriptor<T>> rules)
    {
        ArgumentNullException.ThrowIfNull(rules);

        var bridge = new DataAnnotationsBridge<T>();

        return bridge.Translate(rules);
    }

    /// <summary>
    /// Builds DataAnnotations translation artifacts from a specification.
    /// </summary>
    /// <typeparam name="T">The model type being validated.</typeparam>
    /// <param name="specification">The specification to convert.</param>
    /// <param name="messageTemplate">The message used when validation fails.</param>
    /// <param name="propertyPath">The optional explicit property path.</param>
    /// <param name="errorCode">The optional error code.</param>
    /// <param name="severity">The validation severity.</param>
    /// <param name="tags">The optional rule tags.</param>
    /// <returns>The translation result.</returns>
    public static ValidationTranslationResult<DataAnnotationsRuleArtifact<T>> ToDataAnnotationsArtifacts<T>(this ISpecification<T> specification, String messageTemplate, String? propertyPath = null, String? errorCode = null, ValidationSeverity severity = ValidationSeverity.Error, IEnumerable<String>? tags = null)
    {
        ArgumentNullException.ThrowIfNull(specification);
        ArgumentNullException.ThrowIfNull(messageTemplate);

        var bridge = new DataAnnotationsBridge<T>();

        return bridge.Translate(specification, messageTemplate, propertyPath, errorCode, severity, tags);
    }
}
