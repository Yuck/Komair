using System.Linq.Expressions;
using System.Text.Json.Nodes;
using Komair.Expressions.Abstract;
using Komair.Expressions.Mapping.Mapster;
using Komair.Expressions.Serialization.Abstract.Interfaces;
using NUnit.Framework;

namespace Komair.Expressions.Serialization.Json.UnitTests;

public class ExpressionNodeSerializerTests
{
    private static IExpressionNodeSerializer<JsonObject, ExpressionNodeBase> GetSerializer()
    {
        return new ExpressionNodeSerializer<ExpressionNodeBase>();
    }

    [Test]
    public void Round_Trip_Expression_Through_Json_Serializer_Preserves_Evaluation()
    {
        const String value = "test";
        var mapper = new MapsterExpressionNodeMapper<Func<String, Boolean>>();
        var serializer = GetSerializer();

        Expression<Func<String, Boolean>> expression1 = t => t.Length > 0;
        var expected = expression1.Compile()(value);

        var node1 = mapper.ToExpressionNode(expression1);
        var serialized = serializer.Serialize(node1);
        var node2 = serializer.Deserialize(serialized);

        var expression2 = mapper.ToExpression(node2);
        var actual = expression2.Compile()(value);

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void Round_Trip_Expression_With_Boolean_Constant_Preserves_Value()
    {
        var mapper = new MapsterExpressionNodeMapper<Func<Boolean>>();
        var serializer = GetSerializer();
        Expression<Func<Boolean>> expression = () => true;

        var node = mapper.ToExpressionNode(expression);
        var serialized = serializer.Serialize(node);
        var deserialized = serializer.Deserialize(serialized);
        var roundTripped = mapper.ToExpression(deserialized);

        Assert.IsTrue(roundTripped.Compile()());
    }

    [Test]
    public void Round_Trip_Expression_With_Int32_Constant_Preserves_Value()
    {
        var mapper = new MapsterExpressionNodeMapper<Func<Int32>>();
        var serializer = GetSerializer();
        Expression<Func<Int32>> expression = () => 42;

        var node = mapper.ToExpressionNode(expression);
        var serialized = serializer.Serialize(node);
        var deserialized = serializer.Deserialize(serialized);
        var roundTripped = mapper.ToExpression(deserialized);

        Assert.AreEqual(42, roundTripped.Compile()());
    }

    [Test]
    public void Round_Trip_Expression_With_Int64_Constant_Preserves_Value()
    {
        var mapper = new MapsterExpressionNodeMapper<Func<Int64>>();
        var serializer = GetSerializer();
        Expression<Func<Int64>> expression = () => 42L;

        var node = mapper.ToExpressionNode(expression);
        var serialized = serializer.Serialize(node);
        var deserialized = serializer.Deserialize(serialized);
        var roundTripped = mapper.ToExpression(deserialized);

        Assert.AreEqual(42L, roundTripped.Compile()());
    }

    [Test]
    public void Round_Trip_Expression_With_Double_Constant_Preserves_Value()
    {
        var mapper = new MapsterExpressionNodeMapper<Func<Double>>();
        var serializer = GetSerializer();
        Expression<Func<Double>> expression = () => 3.14;

        var node = mapper.ToExpressionNode(expression);
        var serialized = serializer.Serialize(node);
        var deserialized = serializer.Deserialize(serialized);
        var roundTripped = mapper.ToExpression(deserialized);

        Assert.AreEqual(3.14, roundTripped.Compile()());
    }

    [Test]
    public void Round_Trip_Expression_With_Single_Constant_Preserves_Value()
    {
        var mapper = new MapsterExpressionNodeMapper<Func<Single>>();
        var serializer = GetSerializer();
        Expression<Func<Single>> expression = () => 3.14f;

        var node = mapper.ToExpressionNode(expression);
        var serialized = serializer.Serialize(node);
        var deserialized = serializer.Deserialize(serialized);
        var roundTripped = mapper.ToExpression(deserialized);

        Assert.AreEqual(3.14f, roundTripped.Compile()());
    }

    [Test]
    public void Round_Trip_Expression_With_String_Constant_Preserves_Value()
    {
        var mapper = new MapsterExpressionNodeMapper<Func<String>>();
        var serializer = GetSerializer();
        Expression<Func<String>> expression = () => "hello";

        var node = mapper.ToExpressionNode(expression);
        var serialized = serializer.Serialize(node);
        var deserialized = serializer.Deserialize(serialized);
        var roundTripped = mapper.ToExpression(deserialized);

        Assert.AreEqual("hello", roundTripped.Compile()());
    }

    [Test]
    public void Round_Trip_Expression_With_Enum_Constant_Preserves_Value()
    {
        var mapper = new MapsterExpressionNodeMapper<Func<ExpressionType>>();
        var serializer = GetSerializer();
        Expression<Func<ExpressionType>> expression = () => ExpressionType.Lambda;

        var node = mapper.ToExpressionNode(expression);
        var serialized = serializer.Serialize(node);
        var deserialized = serializer.Deserialize(serialized);
        var roundTripped = mapper.ToExpression(deserialized);

        Assert.AreEqual(ExpressionType.Lambda, roundTripped.Compile()());
    }

    [Test]
    public void Round_Trip_Expression_With_Null_Constant_Preserves_Value()
    {
        var mapper = new MapsterExpressionNodeMapper<Func<String?>>();
        var serializer = GetSerializer();
        Expression<Func<String?>> expression = () => null;

        var node = mapper.ToExpressionNode(expression);
        var serialized = serializer.Serialize(node);
        var deserialized = serializer.Deserialize(serialized);
        var roundTripped = mapper.ToExpression(deserialized);

        Assert.IsNull(roundTripped.Compile()());
    }

    [Test]
    public void Deserialize_WhenJsonEmpty_Throws()
    {
        var serializer = GetSerializer();
        var empty = new JsonObject();

        Assert.Throws<NotSupportedException>(() => serializer.Deserialize(empty));
    }
}
