# Komair.Specifications.Validation.Abstractions

Core abstractions for bridging specification-based rules into validation-oriented outputs. This package defines normalized rule descriptors, translation result contracts, and extension interfaces used by validation adapters.

## Key Types

- `IValidationRuleProvider<T>`: Provides normalized validation rule descriptors for a target model type.
- `IValidationBridge<T, TArtifact>`: Translates normalized rules into framework-specific artifacts.
- `IValidationAwareSpecification<T>`: Optional contract for specifications that can expose validation metadata.
- `ValidationRuleDescriptor<T>`: Canonical rule shape that combines predicate semantics and validation metadata.
- `ValidationTranslationResult<TArtifact>`: Structured translation output with artifacts, warnings, and failures.
- `ValidationTranslationFailure`: Structured translation failure details.
- `ValidationTranslationWarning`: Structured translation warning details.

## Dependencies

- None

## Installation

```bash
dotnet add package Komair.Specifications.Validation.Abstractions
```
