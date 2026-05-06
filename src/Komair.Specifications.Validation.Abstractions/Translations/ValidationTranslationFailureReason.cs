namespace Komair.Specifications.Validation.Abstractions.Translations;

/// <summary>
/// Represents known categories for validation translation failures.
/// </summary>
public enum ValidationTranslationFailureReason
{
    /// <summary>
    /// The source expression node kind is unsupported by the target adapter.
    /// </summary>
    UnsupportedNodeType = 0,

    /// <summary>
    /// The adapter could not derive a stable property path from the source rule.
    /// </summary>
    MissingPropertyPath = 1,

    /// <summary>
    /// The composition of the source rule is ambiguous for target translation.
    /// </summary>
    AmbiguousComposite = 2,

    /// <summary>
    /// The source rule depends on runtime behavior that cannot be represented by the target adapter.
    /// </summary>
    DynamicRuleNotSupported = 3
}
