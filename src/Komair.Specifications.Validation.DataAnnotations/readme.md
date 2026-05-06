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

## Usage

```csharp
using Komair.Specifications.Validation.Abstractions.Rules;
using Komair.Specifications.Validation.DataAnnotations.Extensions;

var rules = new[]
{
    new ValidationRuleDescriptor<User>(t => t.Age >= 18, "User must be an adult", "Age", "AGE001")
};
var translation = rules.ToDataAnnotationsArtifacts();
```

```csharp
using System.Linq.Expressions;
using Komair.Specifications.Abstract;
using Komair.Specifications.Validation.DataAnnotations.Extensions;

public sealed class AdultUserSpecification : SpecificationBase<User>
{
    public override Expression<Func<User, Boolean>> ToExpression()
    {
        return t => t.Age >= 18;
    }
}

var specification = new AdultUserSpecification();
var translationFromSpecification = specification.ToDataAnnotationsArtifacts("User must be an adult", errorCode: "AGE001");
```
