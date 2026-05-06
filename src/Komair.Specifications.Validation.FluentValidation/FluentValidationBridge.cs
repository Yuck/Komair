using System.Linq.Expressions;
using FluentValidation;
using Komair.Specifications.Abstract.Interfaces;
using Komair.Specifications.Validation.Abstractions.Rules;
using Komair.Specifications.Validation.Abstractions.Translations;
using Komair.Specifications.Validation.Abstractions.Translations.Abstract.Interfaces;

namespace Komair.Specifications.Validation.FluentValidation;

/// <summary>
/// Translates normalized rules into a FluentValidation validator artifact.
/// </summary>
/// <typeparam name="T">The model type being validated.</typeparam>
public class FluentValidationBridge<T> : IValidationBridge<T, IValidator<T>>
{
    /// <inheritdoc />
    public ValidationTranslationResult<IValidator<T>> Translate(IEnumerable<ValidationRuleDescriptor<T>> rules)
    {
        ArgumentNullException.ThrowIfNull(rules);

        var failures = new List<ValidationTranslationFailure>();
        var warnings = new List<ValidationTranslationWarning>();
        var validator = new DescriptorValidator(rules, warnings);

        return new ValidationTranslationResult<IValidator<T>>([validator], warnings, failures);
    }

    /// <summary>
    /// Translates a specification into a FluentValidation validator artifact.
    /// </summary>
    /// <param name="specification">The specification to translate.</param>
    /// <param name="messageTemplate">The message used when validation fails.</param>
    /// <param name="propertyPath">The optional explicit property path.</param>
    /// <param name="errorCode">The optional error code.</param>
    /// <param name="severity">The validation severity.</param>
    /// <param name="tags">The optional rule tags.</param>
    /// <returns>The translation result containing a FluentValidation validator artifact.</returns>
    public ValidationTranslationResult<IValidator<T>> Translate(ISpecification<T> specification, String messageTemplate, String? propertyPath = null, String? errorCode = null, ValidationSeverity severity = ValidationSeverity.Error, IEnumerable<String>? tags = null)
    {
        ArgumentNullException.ThrowIfNull(specification);
        ArgumentNullException.ThrowIfNull(messageTemplate);

        var rule = new ValidationRuleDescriptor<T>(specification.ToExpression(), messageTemplate, propertyPath, errorCode, severity, tags);

        return Translate([rule]);
    }

    private sealed class DescriptorValidator : AbstractValidator<T>
    {
        public DescriptorValidator(IEnumerable<ValidationRuleDescriptor<T>> rules, ICollection<ValidationTranslationWarning> warnings)
        {
            foreach (var rule in rules)
                AddRule(rule, warnings);
        }

        private void AddRule(ValidationRuleDescriptor<T> rule, ICollection<ValidationTranslationWarning> warnings)
        {
            var predicate = rule.Predicate.Compile();
            var path = ResolvePropertyPath(rule.Predicate, rule.PropertyPath);
            var identifier = rule.ErrorCode ?? path ?? "<anonymous>";
            var builder = RuleFor(t => t).Must(t => predicate(t))
                                         .WithMessage(rule.MessageTemplate)
                                         .WithSeverity(MapSeverity(rule.Severity));

            if (! String.IsNullOrWhiteSpace(rule.ErrorCode))
                builder = builder.WithErrorCode(rule.ErrorCode);

            if (String.IsNullOrWhiteSpace(path))
            {
                warnings.Add(new ValidationTranslationWarning($"Rule '{identifier}' does not expose a stable property path. Falling back to object-level validation.", identifier));

                return;
            }

            builder.OverridePropertyName(path);
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

        private static Severity MapSeverity(ValidationSeverity severity)
        {
            return severity switch
            {
                ValidationSeverity.Warning => Severity.Warning,
                _ => Severity.Error
            };
        }

        private static String? ResolvePropertyPath(Expression<Func<T, Boolean>> predicate, String? explicitPath)
        {
            if (! String.IsNullOrWhiteSpace(explicitPath))
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
}
