# DataAnnotations From Rule Descriptors

```csharp
using System.ComponentModel.DataAnnotations;
using Komair.Specifications.Validation.Abstractions.Rules;
using Komair.Specifications.Validation.DataAnnotations.Extensions;

var rules = new[]
{
    new ValidationRuleDescriptor<UserRegistrationRequest>(t => t.Age >= 18, "User must be an adult.", "Age", "AGE001")
};

var translation = rules.ToDataAnnotationsArtifacts();
var artifact = translation.Artifacts.Single();
var context = new ValidationContext(new UserRegistrationRequest { Age = 17, Email = "dev@other.com" });
var validation = artifact.Attribute?.GetValidationResult(null, context);
```
