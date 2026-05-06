using System.Linq.Expressions;

namespace Komair.Specifications.Validation.Abstractions.Rules;

/// <summary>
/// Represents a normalized validation rule combining predicate semantics and validation metadata.
/// </summary>
/// <typeparam name="T">The model type validated by the rule.</typeparam>
public sealed class ValidationRuleDescriptor<T>
{
    /// <summary>
    /// Gets the optional error code associated with the rule.
    /// </summary>
    public String? ErrorCode { get; }

    /// <summary>
    /// Gets the message template used when the rule fails.
    /// </summary>
    public String MessageTemplate { get; }

    /// <summary>
    /// Gets the predicate represented by this rule.
    /// </summary>
    public Expression<Func<T, Boolean>> Predicate { get; }

    /// <summary>
    /// Gets the optional property path associated with the rule.
    /// </summary>
    public String? PropertyPath { get; }

    /// <summary>
    /// Gets the severity of the rule.
    /// </summary>
    public ValidationSeverity Severity { get; }

    /// <summary>
    /// Gets optional tags associated with the rule.
    /// </summary>
    public IReadOnlyCollection<String> Tags { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationRuleDescriptor{T}"/> class.
    /// </summary>
    /// <param name="predicate">The predicate represented by this rule.</param>
    /// <param name="messageTemplate">The message template used when the rule fails.</param>
    /// <param name="propertyPath">The optional property path associated with the rule.</param>
    /// <param name="errorCode">The optional error code associated with the rule.</param>
    /// <param name="severity">The severity of the rule.</param>
    /// <param name="tags">Optional tags associated with the rule.</param>
    public ValidationRuleDescriptor(Expression<Func<T, Boolean>> predicate, String messageTemplate, String? propertyPath = null, String? errorCode = null, ValidationSeverity severity = ValidationSeverity.Error, IEnumerable<String>? tags = null)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(messageTemplate);

        Predicate = predicate;
        MessageTemplate = messageTemplate;
        PropertyPath = propertyPath;
        ErrorCode = errorCode;
        Severity = severity;
        Tags = tags?.ToArray() ?? [];
    }
}
