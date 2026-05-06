using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Komair.Specifications.Validation.Abstractions.Translations;

namespace Komair.Specifications.Validation.DataAnnotations;

/// <summary>
/// Represents a translated DataAnnotations artifact for a validation rule.
/// </summary>
/// <typeparam name="T">The model type being validated.</typeparam>
public sealed class DataAnnotationsRuleArtifact<T>
{
    /// <summary>
    /// Gets the generated attribute. When <see cref="IsMetadataOnly"/> is <see langword="true"/>, this value is <see langword="null"/>.
    /// </summary>
    public ValidationAttribute? Attribute { get; }

    /// <summary>
    /// Gets a value indicating whether this artifact is metadata-only.
    /// </summary>
    public Boolean IsMetadataOnly { get; }

    /// <summary>
    /// Gets the message template for this rule.
    /// </summary>
    public String MessageTemplate { get; }

    /// <summary>
    /// Gets the optional error code for this rule.
    /// </summary>
    public String? ErrorCode { get; }

    /// <summary>
    /// Gets the predicate associated with the rule.
    /// </summary>
    public Expression<Func<T, Boolean>> Predicate { get; }

    /// <summary>
    /// Gets the optional property path for this artifact.
    /// </summary>
    public String? PropertyPath { get; }

    /// <summary>
    /// Gets the support level for this artifact.
    /// </summary>
    public ValidationSupportLevel SupportLevel { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataAnnotationsRuleArtifact{T}"/> class.
    /// </summary>
    /// <param name="predicate">The rule predicate.</param>
    /// <param name="messageTemplate">The message template.</param>
    /// <param name="propertyPath">The optional property path.</param>
    /// <param name="errorCode">The optional error code.</param>
    /// <param name="supportLevel">The support level for this artifact.</param>
    /// <param name="isMetadataOnly">Whether this artifact is metadata-only.</param>
    /// <param name="attribute">The generated attribute.</param>
    public DataAnnotationsRuleArtifact(Expression<Func<T, Boolean>> predicate, String messageTemplate, String? propertyPath, String? errorCode, ValidationSupportLevel supportLevel, Boolean isMetadataOnly, ValidationAttribute? attribute = null)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(messageTemplate);

        Attribute = attribute;
        IsMetadataOnly = isMetadataOnly;
        MessageTemplate = messageTemplate;
        ErrorCode = errorCode;
        Predicate = predicate;
        PropertyPath = propertyPath;
        SupportLevel = supportLevel;
    }
}
