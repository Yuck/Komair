namespace Komair.Specifications.Validation.Abstractions.Rules.Abstract.Interfaces;

/// <summary>
/// Represents a specification that can expose validation metadata in addition to predicate behavior.
/// </summary>
/// <typeparam name="T">The type being validated.</typeparam>
public interface IValidationAwareSpecification<T>
{
    /// <summary>
    /// Creates a normalized validation rule descriptor for the specification.
    /// </summary>
    /// <returns>The normalized validation rule descriptor.</returns>
    ValidationRuleDescriptor<T> GetValidationRule();
}
