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

## Usage

```csharp
using Komair.Specifications.Validation.Abstractions.Rules;
using Komair.Specifications.Validation.FluentValidation.Extensions;

var rules = new[]
{
    new ValidationRuleDescriptor<User>(t => t.Age >= 18, "User must be an adult", "Age", "AGE001")
};
var validator = rules.ToFluentValidator();
```

```csharp
using System.Linq.Expressions;
using Komair.Specifications.Abstract;
using Komair.Specifications.Validation.FluentValidation.Extensions;

public sealed class AdultUserSpecification : SpecificationBase<User>
{
    public override Expression<Func<User, Boolean>> ToExpression()
    {
        return t => t.Age >= 18;
    }
}

var specification = new AdultUserSpecification();
var validatorFromSpecification = specification.ToFluentValidator("User must be an adult", errorCode: "AGE001");
```
