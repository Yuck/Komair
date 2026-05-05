using System.Linq.Expressions;
using Komair.Expressions.Mapping.Mapster.Configuration.Mappers.ExpressionNode.Abstract;
using Mapster;
using LinqExpression = System.Linq.Expressions.Expression;

namespace Komair.Expressions.Mapping.Mapster.Configuration.Mappers.ExpressionNode;

internal class ListInitExpressionNodeMapper(TypeAdapterConfig configuration) : ExpressionNodeMapperBase<ListInitExpressionNode, ListInitExpression>(configuration)
{
    public override ListInitExpression Map(ListInitExpressionNode source)
    {
        var newExpression = source.NewExpression.Adapt<NewExpression>(Configuration);
        var initializers = source.Initializers.Select(MapInitializer).ToArray();
        var result = LinqExpression.ListInit(newExpression, initializers);

        return result;

        ElementInit MapInitializer(ElementInitNode initializer)
        {
            var arguments = initializer.Arguments.Select(t => t.Adapt<LinqExpression>(Configuration)).ToArray();
            var method = source.Type.GetMethods().FirstOrDefault(t => t.Name == initializer.AddMethodName && t.GetParameters().Length == arguments.Length) ?? throw new MissingMethodException(source.Type.FullName, initializer.AddMethodName);

            return LinqExpression.ElementInit(method, arguments);
        }
    }
}
