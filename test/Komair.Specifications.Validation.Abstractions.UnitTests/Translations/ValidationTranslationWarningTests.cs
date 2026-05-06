using Komair.Specifications.Validation.Abstractions.Translations;
using NUnit.Framework;

namespace Komair.Specifications.Validation.Abstractions.UnitTests.Translations;

public class ValidationTranslationWarningTests
{
    [Test]
    public void Constructor_WhenMessageIsNull_ThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => _ = new ValidationTranslationWarning(null!));

        Assert.That(exception!.ParamName, Is.EqualTo("message"));
    }

    [Test]
    public void Constructor_WhenValuesAreProvided_SetsProperties()
    {
        var warning = new ValidationTranslationWarning("warn", "rule-1");

        Assert.That(warning.Message, Is.EqualTo("warn"));
        Assert.That(warning.RuleId, Is.EqualTo("rule-1"));
    }
}
