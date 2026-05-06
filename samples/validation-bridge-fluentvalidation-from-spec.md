# FluentValidation From Specification

```csharp
using Komair.Specifications.Validation.FluentValidation.Extensions;

var specification = RegistrationSpecifications.Create();
var validator = specification.ToFluentValidator("Registration rules failed.", errorCode: "REG001");
var result = validator.Validate(new UserRegistrationRequest { Age = 17, Email = "dev@other.com" });
```
