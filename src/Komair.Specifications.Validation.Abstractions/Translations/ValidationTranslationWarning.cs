namespace Komair.Specifications.Validation.Abstractions.Translations;

/// <summary>
/// Represents a non-fatal warning produced during translation.
/// </summary>
public sealed class ValidationTranslationWarning
{
    /// <summary>
    /// Gets the warning message.
    /// </summary>
    public String Message { get; }

    /// <summary>
    /// Gets the optional rule identifier associated with the warning.
    /// </summary>
    public String? RuleId { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationTranslationWarning"/> class.
    /// </summary>
    /// <param name="message">The warning message.</param>
    /// <param name="ruleId">The optional rule identifier associated with the warning.</param>
    public ValidationTranslationWarning(String message, String? ruleId = null)
    {
        ArgumentNullException.ThrowIfNull(message);

        Message = message;
        RuleId = ruleId;
    }
}
