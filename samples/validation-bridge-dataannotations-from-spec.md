# DataAnnotations From Specification

```csharp
using Komair.Specifications.Validation.DataAnnotations.Extensions;

var specification = RegistrationSpecifications.Create();
var translation = specification.ToDataAnnotationsArtifacts("Registration rules failed.", errorCode: "REG001");
```
