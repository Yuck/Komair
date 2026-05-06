namespace Komair.Specifications.Validation.Abstractions.Translations;

/// <summary>
/// Represents a fatal translation failure for a rule or construct.
/// </summary>
public sealed class ValidationTranslationFailure
{
    /// <summary>
    /// Gets the failure message.
    /// </summary>
    public String Message { get; }

    /// <summary>
    /// Gets the failure reason category.
    /// </summary>
    public ValidationTranslationFailureReason Reason { get; }

    /// <summary>
    /// Gets the optional rule identifier associated with the failure.
    /// </summary>
    public String? RuleId { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationTranslationFailure"/> class.
    /// </summary>
    /// <param name="reason">The failure reason category.</param>
    /// <param name="message">The failure message.</param>
    /// <param name="ruleId">The optional rule identifier associated with the failure.</param>
    public ValidationTranslationFailure(ValidationTranslationFailureReason reason, String message, String? ruleId = null)
    {
        ArgumentNullException.ThrowIfNull(message);

        Reason = reason;
        Message = message;
        RuleId = ruleId;
    }
}
