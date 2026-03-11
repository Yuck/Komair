using System.Linq.Expressions;
using Komair.Expressions.Abstract;

namespace Komair.Expressions;

public class MemberExpressionNode(ExpressionType nodeType, Type type) : ExpressionNodeBase(nodeType, type)
{
    public required ExpressionNodeBase Expression { get; set; }
    public required String MemberName { get; set; }
}
