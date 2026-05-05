using System.Linq.Expressions;

namespace Komair.Expressions.Mapping.Exceptions;

/// <summary>
/// Thrown when the root of a <see cref="Expression"/> tree is not a supported kind for mapping (expected lambda root).
/// </summary>
/// <param name="expression">The invalid root expression.</param>
public sealed class InvalidTreeRootException(Expression expression) : Exception(CreateMessage(expression))
{
    /// <summary>
    /// Gets the <see cref="ExpressionType"/> of the invalid root expression.
    /// </summary>
    public ExpressionType NodeType { get; } = Validate(expression).NodeType;

    private static String CreateMessage(Expression expression)
    {
        return $"Unsupported expression kind for mapping: {Validate(expression).NodeType}. Only {ExpressionType.Lambda} is supported at the root.";
    }

    private static Expression Validate(Expression expression)
    {
        ArgumentNullException.ThrowIfNull(expression);

        return expression;
    }
}
