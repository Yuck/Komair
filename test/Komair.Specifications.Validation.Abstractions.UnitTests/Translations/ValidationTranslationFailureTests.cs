using Komair.Specifications.Validation.Abstractions.Translations;
using NUnit.Framework;

namespace Komair.Specifications.Validation.Abstractions.UnitTests.Translations;

public class ValidationTranslationFailureTests
{
    [Test]
    public void Constructor_WhenMessageIsNull_ThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => _ = new ValidationTranslationFailure(ValidationTranslationFailureReason.UnsupportedNodeType, null!));

        Assert.That(exception!.ParamName, Is.EqualTo("message"));
    }

    [Test]
    public void Constructor_WhenValuesAreProvided_SetsProperties()
    {
        var failure = new ValidationTranslationFailure(ValidationTranslationFailureReason.AmbiguousComposite, "failed", "rule-1");

        Assert.That(failure.Message, Is.EqualTo("failed"));
        Assert.That(failure.Reason, Is.EqualTo(ValidationTranslationFailureReason.AmbiguousComposite));
        Assert.That(failure.RuleId, Is.EqualTo("rule-1"));
    }
}
