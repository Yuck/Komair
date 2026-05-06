using Komair.Specifications.Validation.Abstractions.Rules;

namespace Komair.Specifications.Validation.Abstractions.Translations.Abstract.Interfaces;

/// <summary>
/// Translates normalized validation rules into framework-specific validation artifacts.
/// </summary>
/// <typeparam name="T">The model type described by the input rules.</typeparam>
/// <typeparam name="TArtifact">The framework-specific output artifact type.</typeparam>
public interface IValidationBridge<T, TArtifact>
{
    /// <summary>
    /// Translates rules to target artifacts and emits diagnostics for partial or failed translation cases.
    /// </summary>
    /// <param name="rules">The rules to translate.</param>
    /// <returns>The translation result.</returns>
    ValidationTranslationResult<TArtifact> Translate(IEnumerable<ValidationRuleDescriptor<T>> rules);
}
