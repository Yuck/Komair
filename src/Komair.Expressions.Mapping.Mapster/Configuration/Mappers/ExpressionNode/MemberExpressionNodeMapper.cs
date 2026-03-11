using System.Linq.Expressions;
using Komair.Expressions.Mapping.Mapster.Configuration.Mappers.ExpressionNode.Abstract;
using Mapster;
using LinqExpression = System.Linq.Expressions.Expression;

namespace Komair.Expressions.Mapping.Mapster.Configuration.Mappers.ExpressionNode;

internal class MemberExpressionNodeMapper(TypeAdapterConfig configuration) : ExpressionNodeMapperBase<MemberExpressionNode, MemberExpression>(configuration)
{
    public override MemberExpression Map(MemberExpressionNode source)
    {
        var expression = source.Expression.Adapt<LinqExpression>(Configuration);
        var type = source.Expression?.Type ?? throw new NullReferenceException();
        var member = type.GetMember(source.MemberName).FirstOrDefault() ?? throw new MemberAccessException($"Member '{source.MemberName}' was not found on type '{type.FullName}'.");
        var result = LinqExpression.MakeMemberAccess(expression, member);

        return result;
    }
}
