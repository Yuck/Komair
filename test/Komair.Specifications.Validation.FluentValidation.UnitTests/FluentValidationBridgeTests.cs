using System.Linq.Expressions;
using FluentValidation;
using Komair.Specifications.Abstract;
using Komair.Specifications.Validation.Abstractions.Rules;
using Komair.Specifications.Validation.FluentValidation.Extensions;
using NUnit.Framework;

namespace Komair.Specifications.Validation.FluentValidation.UnitTests;

public class FluentValidationBridgeTests
{
    [Test]
    public void ToFluentValidator_WhenExtensionReceivesDescriptors_ReturnsValidator()
    {
        var validator = new[]
        {
            new ValidationRuleDescriptor<Person>(t => t.Age >= 18, "Age must be >= 18", "Age")
        }.ToFluentValidator();

        var result = validator.Validate(new Person { Age = 10, Name = "Alex" });

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Select(t => t.PropertyName), Is.EquivalentTo(["Age"]));
    }

    [Test]
    public void ToFluentValidator_WhenExtensionReceivesSpecification_ReturnsValidator()
    {
        var specification = new AdultSpecification();
        var validator = specification.ToFluentValidator("Must be an adult", "Age", "AGE001");
        var result = validator.Validate(new Person { Age = 10, Name = "Alex" });

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors[0].ErrorCode, Is.EqualTo("AGE001"));
        Assert.That(result.Errors[0].PropertyName, Is.EqualTo("Age"));
    }

    [Test]
    public void Translate_WhenDescriptorHasNoExplicitPath_InfersPropertyName()
    {
        var bridge = new FluentValidationBridge<Person>();
        var descriptors = new[]
        {
            new ValidationRuleDescriptor<Person>(t => t.Age >= 18, "Age must be >= 18")
        };
        var translation = bridge.Translate(descriptors);
        var validator = translation.Artifacts.Single();
        var result = validator.Validate(new Person { Age = 10, Name = "Alex" });

        Assert.That(translation.Succeeded, Is.True);
        Assert.That(translation.Warnings, Is.Empty);
        Assert.That(result.Errors[0].PropertyName, Is.EqualTo("Age"));
    }

    [Test]
    public void Translate_WhenDescriptorIsWarning_MapsSeverityToFluentValidationWarning()
    {
        var bridge = new FluentValidationBridge<Person>();
        var descriptors = new[]
        {
            new ValidationRuleDescriptor<Person>(t => t.Name.Length >= 3, "Name is short", severity: ValidationSeverity.Warning)
        };
        var validator = bridge.Translate(descriptors).Artifacts.Single();
        var result = validator.Validate(new Person { Age = 20, Name = "Al" });

        Assert.That(result.Errors[0].Severity, Is.EqualTo(Severity.Warning));
    }

    [Test]
    public void Translate_WhenDescriptorPathCannotBeInferred_EmitsWarningAndValidatesObjectLevel()
    {
        var bridge = new FluentValidationBridge<Person>();
        var descriptors = new[]
        {
            new ValidationRuleDescriptor<Person>(t => String.Equals(t.Name, "ALEX", StringComparison.OrdinalIgnoreCase), "Name must be Alex")
        };
        var translation = bridge.Translate(descriptors);
        var validator = translation.Artifacts.Single();
        var result = validator.Validate(new Person { Age = 20, Name = "Bob" });

        Assert.That(translation.Succeeded, Is.True);
        Assert.That(translation.Warnings, Has.Count.EqualTo(1));
        Assert.That(result.IsValid, Is.False);
    }

    private sealed class AdultSpecification : SpecificationBase<Person>
    {
        public override Expression<Func<Person, Boolean>> ToExpression()
        {
            return t => t.Age >= 18;
        }
    }

    private sealed class Person
    {
        public Int32 Age { get; init; }

        public required String Name { get; init; }
    }
}
