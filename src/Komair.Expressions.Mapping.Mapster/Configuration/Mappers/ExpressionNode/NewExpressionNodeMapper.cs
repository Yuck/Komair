using System.Linq.Expressions;
using Komair.Expressions.Mapping.Mapster.Configuration.Mappers.ExpressionNode.Abstract;
using Mapster;
using LinqExpression = System.Linq.Expressions.Expression;

namespace Komair.Expressions.Mapping.Mapster.Configuration.Mappers.ExpressionNode;

internal class NewExpressionNodeMapper(TypeAdapterConfig configuration) : ExpressionNodeMapperBase<NewExpressionNode, NewExpression>(configuration)
{
    public override NewExpression Map(NewExpressionNode source)
    {
        var constructor = source.Type.GetConstructor([.. source.ConstructorParameterTypes]) ?? throw new MissingMethodException(source.Type.FullName, ".ctor");
        var arguments = source.Arguments.Select(t => t.Adapt<LinqExpression>(Configuration)).ToArray();
        if (source.MemberNames.Count == 0)
            return LinqExpression.New(constructor, arguments);

        var members = source.MemberNames.Select(GetMember).ToArray();
        var result = LinqExpression.New(constructor, arguments, members);

        return result;

        System.Reflection.MemberInfo GetMember(String name)
        {
            return source.Type.GetMember(name).FirstOrDefault() ?? throw new MemberAccessException($"Member '{name}' was not found on type '{source.Type.FullName}'.");
        }
    }
}
