using Komair.Expressions.Abstract;

namespace Komair.Expressions.Mapping.Exceptions;

public sealed class InvalidNodeRootException(ExpressionNodeBase node) : Exception(CreateMessage(node))
{
    public Type ActualNodeType { get; } = node.GetType();

    private static String CreateMessage(ExpressionNodeBase node)
    {
        ArgumentNullException.ThrowIfNull(node);

        return $"Unsupported expression node kind: {node.GetType().FullName}. Only {nameof(LambdaExpressionNode)} is supported at the mapping root.";
    }
}
