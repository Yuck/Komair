using System.Linq.Expressions;
using Komair.Expressions.Abstract;
using Komair.Expressions.Mapping.Mapster.Configuration.Mappers.Expression.Abstract;
using Mapster;

namespace Komair.Expressions.Mapping.Mapster.Configuration.Mappers.Expression;

internal class ListInitExpressionMapper(TypeAdapterConfig configuration) : ExpressionMapperBase<ListInitExpression, ListInitExpressionNode>(configuration)
{
    public override ListInitExpressionNode Map(ListInitExpression source)
    {
        var nodeType = source.NodeType;
        var type = source.Type;
        var initializers = source.Initializers.Select(MapInitializer).ToArray();
        var result = new ListInitExpressionNode(nodeType, type)
        {
            NewExpression = source.NewExpression.Adapt<NewExpressionNode>(Configuration),
            Initializers = initializers
        };

        return result;
    }

    private ElementInitNode MapInitializer(ElementInit source)
    {
        var result = new ElementInitNode
        {
            AddMethodName = source.AddMethod.Name,
            Arguments = source.Arguments.Select(t => t.Adapt<ExpressionNodeBase>(Configuration)).ToArray()
        };

        return result;
    }
}
