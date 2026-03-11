using System.Linq.Expressions;
using Komair.Expressions.Abstract;

namespace Komair.Expressions.Mapping.Abstract.Interfaces;

public interface IExpressionNodeMapper<T>
{
    Expression<T> ToExpression(ExpressionNodeBase node);
    ExpressionNodeBase ToExpressionNode(Expression<T> expression);
}
