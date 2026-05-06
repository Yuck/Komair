namespace Komair.Specifications.Validation.Abstractions.Rules.Abstract.Interfaces;

/// <summary>
/// Provides normalized validation rules for a target model type.
/// </summary>
/// <typeparam name="T">The type being validated.</typeparam>
public interface IValidationRuleProvider<T>
{
    /// <summary>
    /// Returns the normalized validation rules for <typeparamref name="T"/>.
    /// </summary>
    /// <returns>A sequence of normalized validation rules.</returns>
    IEnumerable<ValidationRuleDescriptor<T>> GetValidationRules();
}
