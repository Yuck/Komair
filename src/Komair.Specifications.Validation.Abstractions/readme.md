# Komair.Specifications.Validation.Abstractions

Core abstractions for bridging specification-based rules into validation-oriented outputs. This package defines normalized rule descriptors, translation result contracts, and extension interfaces used by validation adapters.

## Key Types

- `Rules.Abstract.Interfaces.IValidationRuleProvider<T>`: Provides normalized validation rule descriptors for a target model type.
- `Translations.Abstract.Interfaces.IValidationBridge<T, TArtifact>`: Translates normalized rules into framework-specific artifacts.
- `Rules.Abstract.Interfaces.IValidationAwareSpecification<T>`: Optional contract for specifications that can expose validation metadata.
- `Rules.ValidationRuleDescriptor<T>`: Canonical rule shape that combines predicate semantics and validation metadata.
- `Translations.ValidationTranslationResult<TArtifact>`: Structured translation output with artifacts, warnings, and failures.
- `Translations.ValidationTranslationFailure`: Structured translation failure details.
- `Translations.ValidationTranslationWarning`: Structured translation warning details.

## Usage

```csharp
using Komair.Specifications.Validation.Abstractions.Rules;

var rules = new[]
{
    new ValidationRuleDescriptor<User>(t => t.Age >= 18, "User must be an adult", "Age", "AGE001")
};
```

## Dependencies

- None

## Installation

```bash
dotnet add package Komair.Specifications.Validation.Abstractions
```
