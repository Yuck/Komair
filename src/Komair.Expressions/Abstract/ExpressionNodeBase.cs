using System.Linq.Expressions;

namespace Komair.Expressions.Abstract;

/// <summary>
/// Base type for serializable snapshots of <see cref="System.Linq.Expressions.Expression"/> nodes.
/// </summary>
/// <param name="nodeType">The expression tree kind for this node.</param>
/// <param name="type">The CLR type of the expression.</param>
public abstract class ExpressionNodeBase(ExpressionType nodeType, Type type)
{
    /// <summary>
    /// Gets the expression tree kind for this node.
    /// </summary>
    public ExpressionType NodeType { get; } = nodeType;

    /// <summary>
    /// Gets the CLR type of the expression.
    /// </summary>
    public Type Type { get; } = type;
}
