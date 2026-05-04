using System.Linq.Expressions;
using Komair.Expressions.Abstract;

namespace Komair.Expressions.Mapping.Abstract.Interfaces;

/// <summary>
/// Maps between <see cref="ExpressionNodeBase"/> graphs and <see cref="Expression{T}"/> trees.
/// </summary>
/// <typeparam name="T">The delegate type of the expression (for example a <c>Func&lt;T, TResult&gt;</c> signature).</typeparam>
public interface IExpressionNodeMapper<T>
{
    /// <summary>
    /// Maps a serializable node graph to a <see cref="Expression{T}"/> tree.
    /// </summary>
    /// <param name="node">The root of the node graph (typically a <see cref="Komair.Expressions.LambdaExpressionNode"/>).</param>
    /// <returns>The corresponding expression tree.</returns>
    Expression<T> ToExpression(ExpressionNodeBase node);

    /// <summary>
    /// Maps an expression tree to a serializable node graph.
    /// </summary>
    /// <param name="expression">The expression tree.</param>
    /// <returns>The root of the node graph.</returns>
    ExpressionNodeBase ToExpressionNode(Expression<T> expression);
}
