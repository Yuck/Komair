# FluentValidator Class Integration

Use a specification-derived validator inside your normal `AbstractValidator<T>`.

```csharp
using FluentValidation;
using Komair.Specifications.Abstract;
using Komair.Specifications.Abstract.Interfaces;
using Komair.Specifications.Validation.FluentValidation.Extensions;
using System.Linq.Expressions;

public sealed class UserRegistrationRequest
{
    public Int32 Age { get; init; }

    public required String Email { get; init; }
}

public sealed class AdultUserSpecification : SpecificationBase<UserRegistrationRequest>
{
    public override Expression<Func<UserRegistrationRequest, Boolean>> ToExpression()
    {
        return t => t.Age >= 18;
    }
}

public sealed class CompanyEmailSpecification : SpecificationBase<UserRegistrationRequest>
{
    public override Expression<Func<UserRegistrationRequest, Boolean>> ToExpression()
    {
        return t => t.Email.EndsWith("@example.com");
    }
}

public static class RegistrationSpecifications
{
    public static ISpecification<UserRegistrationRequest> Create()
    {
        return new AdultUserSpecification().And(new CompanyEmailSpecification());
    }
}

public sealed class UserRegistrationRequestValidator : AbstractValidator<UserRegistrationRequest>
{
    public UserRegistrationRequestValidator()
    {
        Include(RegistrationSpecifications.Create().ToFluentValidator("Registration rules failed.", errorCode: "REG001"));

        RuleFor(t => t.Email).NotEmpty().EmailAddress();
        RuleFor(t => t.Age).InclusiveBetween(0, 120);
    }
}
```
