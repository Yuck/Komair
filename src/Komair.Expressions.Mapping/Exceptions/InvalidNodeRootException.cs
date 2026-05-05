using Komair.Expressions.Abstract;

namespace Komair.Expressions.Mapping.Exceptions;

/// <summary>
/// Thrown when the root of an expression node graph is not a supported node kind for mapping (expected lambda root).
/// </summary>
/// <param name="node">The invalid root node.</param>
public sealed class InvalidNodeRootException(ExpressionNodeBase node) : Exception(CreateMessage(node))
{
    /// <summary>
    /// Gets the runtime type of the invalid root node.
    /// </summary>
    public Type ActualNodeType { get; } = Validate(node).GetType();

    private static String CreateMessage(ExpressionNodeBase node)
    {
        return $"Unsupported expression node kind: {Validate(node).GetType().FullName}. Only {nameof(LambdaExpressionNode)} is supported at the mapping root.";
    }

    private static ExpressionNodeBase Validate(ExpressionNodeBase node)
    {
        ArgumentNullException.ThrowIfNull(node);

        return node;
    }
}
