using System.Linq.Expressions;
using Komair.Expressions.Abstract;
using Komair.Expressions.Mapping.Mapster.Configuration.Mappers.Expression.Abstract;
using Mapster;

namespace Komair.Expressions.Mapping.Mapster.Configuration.Mappers.Expression;

internal class MemberExpressionMapper(TypeAdapterConfig configuration) : ExpressionMapperBase<MemberExpression, MemberExpressionNode>(configuration)
{
    public override MemberExpressionNode Map(MemberExpression source)
    {
        var expression = source.Expression ?? throw new InvalidOperationException($"Member '{source.Member.Name}' does not have an owning expression.");
        var nodeType = source.NodeType;
        var type = source.Type;
        var result = new MemberExpressionNode(nodeType, type)
        {
            Expression = expression.Adapt<ExpressionNodeBase>(Configuration),
            MemberName = source.Member.Name
        };

        return result;
    }
}
