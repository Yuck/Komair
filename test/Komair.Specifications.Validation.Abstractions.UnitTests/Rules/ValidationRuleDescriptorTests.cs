using Komair.Specifications.Validation.Abstractions.Rules;
using NUnit.Framework;

namespace Komair.Specifications.Validation.Abstractions.UnitTests.Rules;

public class ValidationRuleDescriptorTests
{
    [Test]
    public void Constructor_WhenMessageTemplateIsNull_ThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => _ = new ValidationRuleDescriptor<String>(t => t.Length > 0, null!));

        Assert.That(exception!.ParamName, Is.EqualTo("messageTemplate"));
    }

    [Test]
    public void Constructor_WhenPredicateIsNull_ThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => _ = new ValidationRuleDescriptor<String>(null!, "message"));

        Assert.That(exception!.ParamName, Is.EqualTo("predicate"));
    }

    [Test]
    public void Constructor_WhenTagsAreNotProvided_UsesEmptyTagsCollection()
    {
        var descriptor = new ValidationRuleDescriptor<String>(t => t.Length > 0, "message");

        Assert.That(descriptor.Tags, Is.Empty);
    }

    [Test]
    public void Constructor_WhenValuesAreProvided_SetsProperties()
    {
        var descriptor = new ValidationRuleDescriptor<String>(t => t.Length > 0, "message", "Name", "ERR001", ValidationSeverity.Warning, ["tag1", "tag2"]);

        Assert.That(descriptor.ErrorCode, Is.EqualTo("ERR001"));
        Assert.That(descriptor.MessageTemplate, Is.EqualTo("message"));
        Assert.That(descriptor.PropertyPath, Is.EqualTo("Name"));
        Assert.That(descriptor.Severity, Is.EqualTo(ValidationSeverity.Warning));
        Assert.That(descriptor.Tags, Is.EquivalentTo(["tag1", "tag2"]));
    }
}
