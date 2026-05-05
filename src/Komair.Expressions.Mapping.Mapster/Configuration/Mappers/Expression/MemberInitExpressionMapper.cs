using System.Linq.Expressions;
using Komair.Expressions.Abstract;
using Komair.Expressions.Mapping.Mapster.Configuration.Mappers.Expression.Abstract;
using Mapster;

namespace Komair.Expressions.Mapping.Mapster.Configuration.Mappers.Expression;

internal class MemberInitExpressionMapper(TypeAdapterConfig configuration) : ExpressionMapperBase<MemberInitExpression, MemberInitExpressionNode>(configuration)
{
    public override MemberInitExpressionNode Map(MemberInitExpression source)
    {
        var nodeType = source.NodeType;
        var type = source.Type;
        var bindings = source.Bindings
                             .OfType<MemberAssignment>()
                             .Select(MapBinding)
                             .ToArray();
        var result = new MemberInitExpressionNode(nodeType, type)
        {
            NewExpression = source.NewExpression.Adapt<NewExpressionNode>(Configuration),
            Bindings = bindings
        };

        return result;
    }

    private MemberAssignmentNode MapBinding(MemberAssignment source)
    {
        var result = new MemberAssignmentNode
        {
            MemberName = source.Member.Name,
            Expression = source.Expression.Adapt<ExpressionNodeBase>(Configuration)
        };

        return result;
    }
}
