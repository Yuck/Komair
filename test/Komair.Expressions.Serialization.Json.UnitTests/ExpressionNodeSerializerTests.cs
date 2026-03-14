using System.Linq.Expressions;
using System.Text.Json.Nodes;
using Komair.Expressions.Abstract;
using Komair.Expressions.Mapping.Abstract.Interfaces;
using Komair.Expressions.Mapping.Mapster;
using Komair.Expressions.Serialization.Abstract.Interfaces;
using NUnit.Framework;

namespace Komair.Expressions.Serialization.Json.UnitTests;

public class ExpressionNodeSerializerTests
{
    [Test]
    public void Round_Trip_Expression_Through_Json_Serializer_Preserves_Evaluation()
    {
        const String value = "test";

        var mapper = GetMapper();
        var serializer = GetSerializer();

        Expression<Func<String, Boolean>> expression1 = t => t.Length > 0;
        var expected = expression1.Compile()(value);

        var node1 = mapper.ToExpressionNode(expression1);
        var serialized = serializer.Serialize(node1);
        var node2 = serializer.Deserialize(serialized);

        var expression2 = mapper.ToExpression(node2);
        var actual = expression2.Compile()(value);

        Assert.AreEqual(expected, actual);

        static IExpressionNodeMapper<Func<String, Boolean>> GetMapper()
        {
            return new MapsterExpressionNodeMapper<Func<String, Boolean>>();
        }

        static IExpressionNodeSerializer<JsonObject, ExpressionNodeBase> GetSerializer()
        {
            return new ExpressionNodeSerializer<ExpressionNodeBase>();
        }
    }
}
