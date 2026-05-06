namespace Komair.Specifications.Validation.Abstractions.Translations;

/// <summary>
/// Represents the structured result of translating normalized rules to target artifacts.
/// </summary>
/// <typeparam name="TArtifact">The output artifact type emitted by a target adapter.</typeparam>
public sealed class ValidationTranslationResult<TArtifact>
{
    /// <summary>
    /// Gets the translated artifacts.
    /// </summary>
    public IReadOnlyCollection<TArtifact> Artifacts { get; }

    /// <summary>
    /// Gets translation failures.
    /// </summary>
    public IReadOnlyCollection<ValidationTranslationFailure> Failures { get; }

    /// <summary>
    /// Gets a value indicating whether translation completed without failures.
    /// </summary>
    public Boolean Succeeded => Failures.Count is 0;

    /// <summary>
    /// Gets translation warnings.
    /// </summary>
    public IReadOnlyCollection<ValidationTranslationWarning> Warnings { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationTranslationResult{TArtifact}"/> class.
    /// </summary>
    /// <param name="artifacts">The translated artifacts.</param>
    /// <param name="warnings">The translation warnings.</param>
    /// <param name="failures">The translation failures.</param>
    public ValidationTranslationResult(IEnumerable<TArtifact>? artifacts = null, IEnumerable<ValidationTranslationWarning>? warnings = null, IEnumerable<ValidationTranslationFailure>? failures = null)
    {
        Artifacts = artifacts?.ToArray() ?? [];
        Warnings = warnings?.ToArray() ?? [];
        Failures = failures?.ToArray() ?? [];
    }
}
