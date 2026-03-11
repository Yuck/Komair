using System.Linq.Expressions;
using Komair.Expressions.Abstract;

namespace Komair.Expressions;

public class ParameterExpressionNode(ExpressionType nodeType, Type type) : ExpressionNodeBase(nodeType, type)
{
    public String? Name { get; set; }
}
