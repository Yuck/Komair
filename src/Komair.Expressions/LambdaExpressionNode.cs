using System.Linq.Expressions;
using Komair.Expressions.Abstract;

namespace Komair.Expressions;

public class LambdaExpressionNode(ExpressionType nodeType, Type type) : ExpressionNodeBase(nodeType, type)
{
    public required ExpressionNodeBase Body { get; set; }
    public required IReadOnlyCollection<ParameterExpressionNode> Parameters { get; set; }
}
