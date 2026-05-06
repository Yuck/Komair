# DataAnnotations Class Integration

Use specification-derived DataAnnotations artifacts inside a model class that implements `IValidatableObject`.

```csharp
using System.ComponentModel.DataAnnotations;
using Komair.Specifications.Abstract.Interfaces;
using Komair.Specifications.Validation.DataAnnotations.Extensions;

public sealed class UserRegistrationRequest : IValidatableObject
{
    public Int32 Age { get; init; }

    public required String Email { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        ISpecification<UserRegistrationRequest> specification = RegistrationSpecifications.Create();
        var translation = specification.ToDataAnnotationsArtifacts("Registration rules failed.", errorCode: "REG001");

        foreach (var failure in translation.Failures)
            yield return new ValidationResult(failure.Message);

        foreach (var artifact in translation.Artifacts.Where(t => t.Attribute is not null))
        {
            var result = artifact.Attribute!.GetValidationResult(null, validationContext);

            if (result is not null && result != ValidationResult.Success)
                yield return result;
        }
    }
}
```

`IValidatableObject` is automatically invoked by ASP.NET Core model validation, so no custom `[Validate]` attribute is required:

```csharp
[ApiController]
[Route("api/users")]
public sealed class UsersController : ControllerBase
{
    [HttpPost("register")]
    public IActionResult Register([FromBody] UserRegistrationRequest request)
    {
        if ( ! ModelState.IsValid)
            return ValidationProblem(ModelState);

        return Ok();
    }
}
```
