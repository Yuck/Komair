using System.Linq.Expressions;
using Komair.Specifications.Abstract.Interfaces;
using Komair.Specifications.Validation.Abstractions.Rules;
using Komair.Specifications.Validation.Abstractions.Translations;
using Komair.Specifications.Validation.Abstractions.Translations.Abstract.Interfaces;
using Komair.Specifications.Validation.DataAnnotations.Internal;

namespace Komair.Specifications.Validation.DataAnnotations;

/// <summary>
/// Translates normalized validation rules into DataAnnotations artifacts.
/// </summary>
/// <typeparam name="T">The model type being validated.</typeparam>
public class DataAnnotationsBridge<T> : IValidationBridge<T, DataAnnotationsRuleArtifact<T>>
{
    /// <inheritdoc />
    public ValidationTranslationResult<DataAnnotationsRuleArtifact<T>> Translate(IEnumerable<ValidationRuleDescriptor<T>> rules)
    {
        ArgumentNullException.ThrowIfNull(rules);

        var artifacts = new List<DataAnnotationsRuleArtifact<T>>();
        var failures = new List<ValidationTranslationFailure>();
        var warnings = new List<ValidationTranslationWarning>();

        foreach (var rule in rules)
            artifacts.Add(CreateArtifact(rule, warnings, failures));

        return new ValidationTranslationResult<DataAnnotationsRuleArtifact<T>>(artifacts, warnings, failures);
    }

    /// <summary>
    /// Translates a specification into DataAnnotations artifacts.
    /// </summary>
    /// <param name="specification">The specification to translate.</param>
    /// <param name="messageTemplate">The message used when validation fails.</param>
    /// <param name="propertyPath">The optional explicit property path.</param>
    /// <param name="errorCode">The optional error code.</param>
    /// <param name="severity">The validation severity.</param>
    /// <param name="tags">The optional rule tags.</param>
    /// <returns>The translation result containing DataAnnotations artifacts.</returns>
    public ValidationTranslationResult<DataAnnotationsRuleArtifact<T>> Translate(ISpecification<T> specification, String messageTemplate, String? propertyPath = null, String? errorCode = null, ValidationSeverity severity = ValidationSeverity.Error, IEnumerable<String>? tags = null)
    {
        ArgumentNullException.ThrowIfNull(specification);
        ArgumentNullException.ThrowIfNull(messageTemplate);

        var rule = new ValidationRuleDescriptor<T>(specification.ToExpression(), messageTemplate, propertyPath, errorCode, severity, tags);

        return Translate([rule]);
    }

    private static Boolean ContainsDynamicInvocation(Expression expression)
    {
        return expression switch
        {
            BinaryExpression binaryExpression => ContainsDynamicInvocation(binaryExpression.Left) || ContainsDynamicInvocation(binaryExpression.Right),
            InvocationExpression => true,
            MethodCallExpression methodCallExpression => methodCallExpression.Arguments.Any(ContainsDynamicInvocation) || methodCallExpression.Object is not null && ContainsDynamicInvocation(methodCallExpression.Object),
            UnaryExpression unaryExpression => ContainsDynamicInvocation(unaryExpression.Operand),
            _ => false
        };
    }

    private static DataAnnotationsRuleArtifact<T> CreateArtifact(ValidationRuleDescriptor<T> rule, ICollection<ValidationTranslationWarning> warnings, ICollection<ValidationTranslationFailure> failures)
    {
        var path = ResolvePropertyPath(rule.Predicate, rule.PropertyPath);
        var identifier = rule.ErrorCode ?? path ?? "<anonymous>";

        if (IsCompositeExpression(rule.Predicate.Body))
        {
            failures.Add(new ValidationTranslationFailure(ValidationTranslationFailureReason.AmbiguousComposite, $"Composite rule '{identifier}' cannot be represented as a single DataAnnotations attribute.", identifier));
            warnings.Add(new ValidationTranslationWarning($"Rule '{identifier}' was exported as metadata-only due to composite expression.", identifier));

            return new DataAnnotationsRuleArtifact<T>(rule.Predicate, rule.MessageTemplate, path, rule.ErrorCode, ValidationSupportLevel.Partial, true);
        }

        if (String.IsNullOrWhiteSpace(path))
        {
            failures.Add(new ValidationTranslationFailure(ValidationTranslationFailureReason.MissingPropertyPath, $"Rule '{identifier}' does not expose a stable property path for DataAnnotations projection.", identifier));
            warnings.Add(new ValidationTranslationWarning($"Rule '{identifier}' was exported as metadata-only because no property path was available.", identifier));

            return new DataAnnotationsRuleArtifact<T>(rule.Predicate, rule.MessageTemplate, path, rule.ErrorCode, ValidationSupportLevel.Partial, true);
        }

        if (ContainsDynamicInvocation(rule.Predicate.Body))
        {
            failures.Add(new ValidationTranslationFailure(ValidationTranslationFailureReason.DynamicRuleNotSupported, $"Rule '{identifier}' uses dynamic invocation and cannot be represented as a stable DataAnnotations attribute.", identifier));
            warnings.Add(new ValidationTranslationWarning($"Rule '{identifier}' was exported as metadata-only due to dynamic invocation.", identifier));

            return new DataAnnotationsRuleArtifact<T>(rule.Predicate, rule.MessageTemplate, path, rule.ErrorCode, ValidationSupportLevel.None, true);
        }

        var attribute = new PredicateValidationAttribute<T>(rule.Predicate.Compile(), rule.MessageTemplate, path);

        return new DataAnnotationsRuleArtifact<T>(rule.Predicate, rule.MessageTemplate, path, rule.ErrorCode, ValidationSupportLevel.Full, false, attribute);
    }

    private static String? GetMemberPath(MemberExpression memberExpression)
    {
        Expression? current = memberExpression;

        var segments = new Stack<String>();

        while (current is MemberExpression member)
        {
            segments.Push(member.Member.Name);
            current = member.Expression;
        }

        return current is ParameterExpression ? String.Join(".", segments) : null;
    }

    private static Boolean IsCompositeExpression(Expression expression)
    {
        return expression switch
        {
            BinaryExpression { NodeType: ExpressionType.AndAlso or ExpressionType.OrElse } => true,
            UnaryExpression { NodeType: ExpressionType.Not } => true,
            BinaryExpression binaryExpression => IsCompositeExpression(binaryExpression.Left) || IsCompositeExpression(binaryExpression.Right),
            MethodCallExpression methodCallExpression => methodCallExpression.Arguments.Any(IsCompositeExpression) || methodCallExpression.Object is not null && IsCompositeExpression(methodCallExpression.Object),
            UnaryExpression unaryExpression => IsCompositeExpression(unaryExpression.Operand),
            _ => false
        };
    }

    private static String? ResolvePropertyPath(Expression<Func<T, Boolean>> predicate, String? explicitPath)
    {
        if ( ! String.IsNullOrWhiteSpace(explicitPath))
            return explicitPath;

        return TryExtractPropertyPath(predicate.Body);
    }

    private static String? TryExtractPropertyPath(Expression body)
    {
        return body switch
        {
            BinaryExpression binaryExpression => TryExtractPropertyPath(binaryExpression.Left) ?? TryExtractPropertyPath(binaryExpression.Right),
            MemberExpression memberExpression => GetMemberPath(memberExpression),
            MethodCallExpression methodCallExpression => methodCallExpression.Object is null ? null : TryExtractPropertyPath(methodCallExpression.Object),
            UnaryExpression unaryExpression => TryExtractPropertyPath(unaryExpression.Operand),
            _ => null
        };
    }
}
