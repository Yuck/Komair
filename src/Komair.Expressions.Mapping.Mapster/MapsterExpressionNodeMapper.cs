using System.Linq.Expressions;
using Komair.Expressions.Abstract;
using Komair.Expressions.Mapping.Abstract.Interfaces;
using Komair.Expressions.Mapping.Mapster.Configuration;
using Mapster;

namespace Komair.Expressions.Mapping.Mapster;

/// <summary>
/// Maps between <see cref="ExpressionNodeBase"/> graphs and <see cref="Expression{T}"/> trees using Mapster.
/// </summary>
/// <typeparam name="T">The delegate type of the expression (for example a <c>Func&lt;T, TResult&gt;</c> signature).</typeparam>
/// <param name="configuration">Optional Mapster configuration; a default configuration is used when <see langword="null"/>.</param>
public class MapsterExpressionNodeMapper<T>(TypeAdapterConfig? configuration = null) : IExpressionNodeMapper<T>
{
    private readonly TypeAdapterConfig _configuration = configuration ?? new DefaultTypeAdapterConfiguration<T>();

    /// <inheritdoc />
    public Expression<T> ToExpression(ExpressionNodeBase node) => node.Adapt<Expression<T>>(_configuration);

    /// <inheritdoc />
    public ExpressionNodeBase ToExpressionNode(Expression<T> expression) => expression.Adapt<ExpressionNodeBase>(_configuration);
}
