using System.Linq.Expressions;
using Komair.Expressions.Mapping.Mapster.Configuration.Mappers.ExpressionNode.Abstract;
using Mapster;
using LinqExpression = System.Linq.Expressions.Expression;

namespace Komair.Expressions.Mapping.Mapster.Configuration.Mappers.ExpressionNode;

internal class MemberInitExpressionNodeMapper(TypeAdapterConfig configuration) : ExpressionNodeMapperBase<MemberInitExpressionNode, MemberInitExpression>(configuration)
{
    public override MemberInitExpression Map(MemberInitExpressionNode source)
    {
        var newExpression = source.NewExpression.Adapt<NewExpression>(Configuration);
        var bindings = source.Bindings.Select(MapBinding).ToArray();
        var result = LinqExpression.MemberInit(newExpression, bindings);

        return result;

        MemberAssignment MapBinding(MemberAssignmentNode binding)
        {
            var member = source.Type.GetMember(binding.MemberName).FirstOrDefault() ?? throw new MemberAccessException($"Member '{binding.MemberName}' was not found on type '{source.Type.FullName}'.");
            var expression = binding.Expression.Adapt<LinqExpression>(Configuration);

            return LinqExpression.Bind(member, expression);
        }
    }
}
