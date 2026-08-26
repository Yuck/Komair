# Komair.Specifications.Validation.Abstractions

Core abstractions for bridging specification-based rules into validation-oriented outputs. This package defines normalized rule descriptors, translation result contracts, and extension interfaces used by validation adapters.

## Key Types

- **`IValidationRuleProvider<T>`** (`Rules.Abstract.Interfaces`) &ndash; provides normalized validation rule descriptors for a target model type
- **`IValidationBridge<T, TArtifact>`** (`Translations.Abstract.Interfaces`) &ndash; translates normalized rules into framework-specific artifacts
- **`IValidationAwareSpecification<T>`** (`Rules.Abstract.Interfaces`) &ndash; optional contract for specifications that can expose validation metadata
- **`ValidationRuleDescriptor<T>`** (`Rules`) &ndash; canonical rule shape that combines predicate semantics and validation metadata
- **`ValidationTranslationResult<TArtifact>`** (`Translations`) &ndash; structured translation output with artifacts, warnings, and failures
- **`ValidationTranslationFailure`** (`Translations`) &ndash; structured translation failure details
- **`ValidationTranslationWarning`** (`Translations`) &ndash; structured translation warning details

## Usage

```csharp
using Komair.Specifications.Validation.Abstractions.Rules;

var rules = new[]
{
    new ValidationRuleDescriptor<User>(t => t.Age >= 18, "User must be an adult", "Age", "AGE001")
};
```

## Installation

```shell
dotnet add package Komair.Specifications.Validation.Abstractions
```
