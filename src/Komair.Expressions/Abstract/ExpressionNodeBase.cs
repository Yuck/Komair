using System.Linq.Expressions;

namespace Komair.Expressions.Abstract;

public abstract class ExpressionNodeBase(ExpressionType nodeType, Type type)
{
    public ExpressionType NodeType { get; } = nodeType;

    public Type Type { get; } = type;
}
