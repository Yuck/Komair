using System.Linq.Expressions;

namespace Komair.Expressions.Exceptions;

public sealed class UnsupportedExpressionException(ExpressionType nodeType) : Exception($"Unsupported expression kind: {nodeType}.")
{
    public ExpressionType NodeType { get; } = nodeType;
}
