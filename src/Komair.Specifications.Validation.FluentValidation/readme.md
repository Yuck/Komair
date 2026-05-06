# Komair.Specifications.Validation.FluentValidation

FluentValidation adapter for Komair specification validation bridge abstractions. This package translates normalized validation rules and specification predicates into FluentValidation validators while preserving rule metadata where possible.

## Key Types

- `FluentValidationBridge<T>`: Translates `ValidationRuleDescriptor<T>` rules into a FluentValidation `IValidator<T>` artifact.
- `FluentValidationBridgeExtensions`: Convenience extensions for building validators from descriptors or `ISpecification<T>` instances.

## Dependencies

- [FluentValidation](https://www.nuget.org/packages/FluentValidation/)
- [Komair.Specifications](https://www.nuget.org/packages/Komair.Specifications/)
- [Komair.Specifications.Validation.Abstractions](https://www.nuget.org/packages/Komair.Specifications.Validation.Abstractions/)

## Installation

```bash
dotnet add package Komair.Specifications.Validation.FluentValidation
```
