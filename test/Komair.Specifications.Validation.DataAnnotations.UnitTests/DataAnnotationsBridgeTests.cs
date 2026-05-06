using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Komair.Specifications.Abstract;
using Komair.Specifications.Validation.Abstractions.Rules;
using Komair.Specifications.Validation.Abstractions.Translations;
using Komair.Specifications.Validation.DataAnnotations.Extensions;
using NUnit.Framework;

namespace Komair.Specifications.Validation.DataAnnotations.UnitTests;

public class DataAnnotationsBridgeTests
{
    [Test]
    public void ToDataAnnotationsArtifacts_WhenExtensionReceivesRuleDescriptors_TranslatesRule()
    {
        var descriptors = new[]
        {
            new ValidationRuleDescriptor<Person>(t => t.Age >= 18, "Adult required", "Age", "AGE001")
        };
        var translation = descriptors.ToDataAnnotationsArtifacts();

        Assert.That(translation.Artifacts, Has.Count.EqualTo(1));
        Assert.That(translation.Failures, Is.Empty);
        Assert.That(translation.Warnings, Is.Empty);
        Assert.That(translation.Artifacts.Single().ErrorCode, Is.EqualTo("AGE001"));
    }

    [Test]
    public void ToDataAnnotationsArtifacts_WhenExtensionReceivesSpecification_TranslatesRule()
    {
        var specification = new AdultSpecification();
        var translation = specification.ToDataAnnotationsArtifacts("Adult required", "Age", "AGE001");

        Assert.That(translation.Artifacts, Has.Count.EqualTo(1));
        Assert.That(translation.Failures, Is.Empty);
        Assert.That(translation.Artifacts.Single().ErrorCode, Is.EqualTo("AGE001"));
    }

    [Test]
    public void Translate_WhenDynamicInvocationIsUsed_ProducesMetadataOnlyWithDynamicRuleNotSupportedFailure()
    {
        var bridge = new DataAnnotationsBridge<Person>();
        Func<Person, Boolean> predicate = t => t.Age >= 18;

        var rule = new ValidationRuleDescriptor<Person>(t => predicate(t), "Adult required", "Age", "AGE001");
        var translation = bridge.Translate([rule]);
        var artifact = translation.Artifacts.Single();
        var failure = translation.Failures.Single();
        var warning = translation.Warnings.Single();

        Assert.That(translation.Succeeded, Is.False);
        Assert.That(failure.Reason, Is.EqualTo(ValidationTranslationFailureReason.DynamicRuleNotSupported));
        Assert.That(failure.RuleId, Is.EqualTo("AGE001"));
        Assert.That(warning.RuleId, Is.EqualTo("AGE001"));
        Assert.That(artifact.Attribute, Is.Null);
        Assert.That(artifact.IsMetadataOnly, Is.True);
        Assert.That(artifact.SupportLevel, Is.EqualTo(ValidationSupportLevel.None));
    }

    [Test]
    public void Translate_WhenExplicitPathIsProvided_UsesExplicitPathOverInferredPath()
    {
        var bridge = new DataAnnotationsBridge<Person>();
        var rule = new ValidationRuleDescriptor<Person>(t => t.Age >= 18, "Adult required", "Person.Age");
        var translation = bridge.Translate([rule]);

        Assert.That(translation.Succeeded, Is.True);
        Assert.That(translation.Artifacts.Single().PropertyPath, Is.EqualTo("Person.Age"));
    }

    [Test]
    public void Translate_WhenPathCanBeInferred_ProducesAttributeArtifact()
    {
        var bridge = new DataAnnotationsBridge<Person>();
        var rule = new ValidationRuleDescriptor<Person>(t => t.Age >= 18, "Adult required");
        var translation = bridge.Translate([rule]);
        var artifact = translation.Artifacts.Single();
        var context = new ValidationContext(new Person { Age = 16, Name = "Alex" });
        var validationResult = artifact.Attribute!.GetValidationResult(null, context);

        Assert.That(translation.Succeeded, Is.True);
        Assert.That(artifact.IsMetadataOnly, Is.False);
        Assert.That(artifact.PropertyPath, Is.EqualTo("Age"));
        Assert.That(validationResult, Is.Not.Null);
    }

    [Test]
    public void Translate_WhenPathCanBeInferredAndValidationPasses_ReturnsSuccessValidationResult()
    {
        var bridge = new DataAnnotationsBridge<Person>();
        var rule = new ValidationRuleDescriptor<Person>(t => t.Age >= 18, "Adult required");
        var translation = bridge.Translate([rule]);
        var artifact = translation.Artifacts.Single();
        var context = new ValidationContext(new Person { Age = 21, Name = "Alex" });
        var validationResult = artifact.Attribute!.GetValidationResult(null, context);

        Assert.That(validationResult, Is.EqualTo(ValidationResult.Success));
    }

    [Test]
    public void Translate_WhenPredicateIsComposite_ProducesMetadataOnlyWithAmbiguousCompositeFailure()
    {
        var bridge = new DataAnnotationsBridge<Person>();
        var rule = new ValidationRuleDescriptor<Person>(t => t.Age >= 18 && t.Name.StartsWith("A"), "Adult A required", "Age");
        var translation = bridge.Translate([rule]);
        var artifact = translation.Artifacts.Single();

        Assert.That(translation.Succeeded, Is.False);
        Assert.That(translation.Failures.Single().Reason, Is.EqualTo(ValidationTranslationFailureReason.AmbiguousComposite));
        Assert.That(artifact.Attribute, Is.Null);
        Assert.That(artifact.IsMetadataOnly, Is.True);
        Assert.That(artifact.SupportLevel, Is.EqualTo(ValidationSupportLevel.Partial));
    }

    [Test]
    public void Translate_WhenPropertyPathIsMissing_ProducesMetadataOnlyWithMissingPropertyPathFailure()
    {
        var bridge = new DataAnnotationsBridge<Person>();
        var rule = new ValidationRuleDescriptor<Person>(t => String.Equals(t.Name, "Alex", StringComparison.OrdinalIgnoreCase), "Name must match");
        var translation = bridge.Translate([rule]);
        var artifact = translation.Artifacts.Single();

        Assert.That(translation.Succeeded, Is.False);
        Assert.That(translation.Failures.Single().Reason, Is.EqualTo(ValidationTranslationFailureReason.MissingPropertyPath));
        Assert.That(translation.Warnings.Single().RuleId, Is.EqualTo("<anonymous>"));
        Assert.That(artifact.Attribute, Is.Null);
        Assert.That(artifact.IsMetadataOnly, Is.True);
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
