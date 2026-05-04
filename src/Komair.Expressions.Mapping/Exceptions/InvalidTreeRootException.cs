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
    public ExpressionType NodeType { get; } = expression.NodeType;

    private static String CreateMessage(Expression expression)
    {
        ArgumentNullException.ThrowIfNull(expression);

        return $"Unsupported expression kind for mapping: {expression.NodeType}. Only {ExpressionType.Lambda} is supported at the root.";
    }
}
