namespace Komair.Specifications.Validation.Abstractions.Translations;

/// <summary>
/// Represents how fully a construct can be translated to a specific validation target.
/// </summary>
public enum ValidationSupportLevel
{
    /// <summary>
    /// The construct cannot be translated.
    /// </summary>
    None = 0,

    /// <summary>
    /// The construct can be translated with reduced fidelity.
    /// </summary>
    Partial = 1,

    /// <summary>
    /// The construct can be translated with full fidelity.
    /// </summary>
    Full = 2
}
