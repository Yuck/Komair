using System.Linq.Expressions;

namespace Komair.Expressions.Mapping.Exceptions;

public sealed class InvalidTreeRootException(Expression expression) : Exception(CreateMessage(expression))
{
    public ExpressionType NodeType { get; } = expression.NodeType;

    private static String CreateMessage(Expression expression)
    {
        ArgumentNullException.ThrowIfNull(expression);

        return $"Unsupported expression kind for mapping: {expression.NodeType}. Only {ExpressionType.Lambda} is supported at the root.";
    }
}
