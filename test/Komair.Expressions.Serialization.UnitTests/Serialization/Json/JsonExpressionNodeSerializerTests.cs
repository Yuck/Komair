using System.Linq.Expressions;
using Komair.Expressions.Abstract;
using Komair.Expressions.Mapping.Abstract.Interfaces;
using Komair.Expressions.Mapping.Mapster;
using Komair.Expressions.Serialization.Abstract.Interfaces;
using Komair.Expressions.Serialization.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Komair.Expressions.Serialization.UnitTests.Serialization.Json;

public class JsonExpressionNodeSerializerTests
{
    [Test]
    public void Roundtrips_Expression_Through_Json_Serializer()
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

        static IExpressionNodeMapper<Func<String, Boolean>> GetMapper() => new MapsterExpressionNodeMapper<Func<String, Boolean>>();
        static IExpressionNodeSerializer<JObject, ExpressionNodeBase> GetSerializer() => new JsonExpressionNodeSerializer<ExpressionNodeBase>(new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
    }
}
