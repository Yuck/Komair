# Komair.Specifications.Validation.DataAnnotations

DataAnnotations adapter for Komair specification validation bridge abstractions. This package translates normalized validation rules and specification predicates into runtime validation attributes when possible, and emits metadata-only artifacts with diagnostics when mapping is not safe.

## Key Types

- `DataAnnotationsBridge<T>`: Translates `ValidationRuleDescriptor<T>` rules into DataAnnotations translation artifacts.
- `DataAnnotationsRuleArtifact<T>`: Represents either a generated validation attribute artifact or a metadata-only fallback artifact.
- `DataAnnotationsBridgeExtensions`: Convenience extensions for building DataAnnotations translation results from descriptors or `ISpecification<T>` instances.

## Dependencies

- [Komair.Specifications](https://www.nuget.org/packages/Komair.Specifications/)
- [Komair.Specifications.Validation.Abstractions](https://www.nuget.org/packages/Komair.Specifications.Validation.Abstractions/)

## Installation

```bash
dotnet add package Komair.Specifications.Validation.DataAnnotations
```
