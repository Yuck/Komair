using System.Linq.Expressions;
using Komair.Expressions.Abstract;

namespace Komair.Expressions;

/// <summary>
/// Represents a block <see cref="System.Linq.Expressions.Expression"/> in serializable form.
/// </summary>
/// <param name="nodeType">The expression tree kind for this node.</param>
/// <param name="type">The CLR type of the expression.</param>
public class BlockExpressionNode(ExpressionType nodeType, Type type) : ExpressionNodeBase(nodeType, type)
{
    /// <summary>
    /// Gets or sets expressions executed by the block.
    /// </summary>
    public required IReadOnlyCollection<ExpressionNodeBase> Expressions { get; set; }

    /// <summary>
    /// Gets or sets variables declared in the block scope.
    /// </summary>
    public required IReadOnlyCollection<ParameterExpressionNode> Variables { get; set; }
}
