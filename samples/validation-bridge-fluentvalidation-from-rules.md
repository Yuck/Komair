# FluentValidation From Rule Descriptors

```csharp
using Komair.Specifications.Validation.Abstractions.Rules;
using Komair.Specifications.Validation.FluentValidation.Extensions;

var rules = new[]
{
    new ValidationRuleDescriptor<UserRegistrationRequest>(t => t.Age >= 18, "User must be an adult.", "Age", "AGE001"),
    new ValidationRuleDescriptor<UserRegistrationRequest>(t => t.Email.EndsWith("@example.com"), "Email must be a company address.", "Email", "EMAIL001")
};

var validator = rules.ToFluentValidator();
```
