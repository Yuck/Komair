using System.Linq.Expressions;
using Komair.Expressions.Abstract;

namespace Komair.Expressions;

public class BinaryExpressionNode(ExpressionType nodeType, Type type) : ExpressionNodeBase(nodeType, type)
{
    public required ExpressionNodeBase Left { get; set; }

    public required ExpressionNodeBase Right { get; set; }
}
