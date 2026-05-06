using Komair.Specifications.Validation.Abstractions.Translations;
using NUnit.Framework;

namespace Komair.Specifications.Validation.Abstractions.UnitTests.Translations;

public class ValidationTranslationResultTests
{
    [Test]
    public void Constructor_WhenFailuresArePresent_SucceededReturnsFalse()
    {
        var result = new ValidationTranslationResult<String>(failures: [new ValidationTranslationFailure(ValidationTranslationFailureReason.DynamicRuleNotSupported, "failed")]);

        Assert.That(result.Succeeded, Is.False);
    }

    [Test]
    public void Constructor_WhenNoCollectionsProvided_InitializesEmptyCollections()
    {
        var result = new ValidationTranslationResult<String>();

        Assert.That(result.Artifacts, Is.Empty);
        Assert.That(result.Failures, Is.Empty);
        Assert.That(result.Warnings, Is.Empty);
    }

    [Test]
    public void Constructor_WhenWarningsAndArtifactsProvided_SetsCollections()
    {
        var result = new ValidationTranslationResult<String>(["artifact"], [new ValidationTranslationWarning("warn")]);

        Assert.That(result.Artifacts, Is.EquivalentTo(["artifact"]));
        Assert.That(result.Failures, Is.Empty);
        Assert.That(result.Succeeded, Is.True);
        Assert.That(result.Warnings, Has.Count.EqualTo(1));
    }
}
