namespace Komair.Specifications.Validation.Abstractions.Rules;

/// <summary>
/// Represents the severity level of a validation rule violation.
/// </summary>
public enum ValidationSeverity
{
    /// <summary>
    /// A validation failure that should be treated as an error.
    /// </summary>
    Error = 0,

    /// <summary>
    /// A validation failure that should be treated as a warning.
    /// </summary>
    Warning = 1
}
