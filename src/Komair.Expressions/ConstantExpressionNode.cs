using System.Linq.Expressions;
using Komair.Expressions.Abstract;

namespace Komair.Expressions;

public class ConstantExpressionNode(ExpressionType nodeType, Type type) : ExpressionNodeBase(nodeType, type)
{
    public Object? Value { get; set; }
}
