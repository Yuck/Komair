using System.Linq.Expressions;
using System.Text.Json.Nodes;
using Komair.Expressions.Abstract;
using Komair.Expressions.Mapping.Mapster;
using Komair.Expressions.Serialization;
using Komair.Expressions.Serialization.Abstract.Interfaces;
using Komair.Expressions.Serialization.Exceptions;
using NUnit.Framework;

namespace Komair.Expressions.Serialization.Json.UnitTests;

public class ExpressionNodeSerializerTests
{
    private static IExpressionNodeSerializer<JsonObject, ExpressionNodeBase> GetSerializer()
    {
        return new ExpressionNodeSerializer<ExpressionNodeBase>();
    }

    [Test]
    public void Deserialize_WhenEnvelopeMissingNode_ThrowsExpressionSerializationException()
    {
        var serializer = GetSerializer();
        var document = new JsonObject
        {
            [ExpressionSerializationWireFormat.SchemaPropertyName] = ExpressionSerializationWireFormat.CurrentSchemaVersion
        };

        var exception = Assert.Throws<ExpressionSerializationException>(() => serializer.Deserialize(document));

        Assert.That(exception!.Message, Does.Contain(ExpressionSerializationWireFormat.NodePropertyName));
    }

    [Test]
    public void Deserialize_WhenJsonEmpty_ThrowsExpressionSerializationException()
    {
        var serializer = GetSerializer();
        var empty = new JsonObject();

        Assert.Throws<ExpressionSerializationException>(() => serializer.Deserialize(empty));
    }

    [Test]
    public void Deserialize_WhenLegacyBareNodeFormat_RoundTrips()
    {
        var mapper = new MapsterExpressionNodeMapper<Func<Int32>>();
        var serializer = GetSerializer();

        var node = mapper.ToExpressionNode(CreateExpression());
        var envelope = serializer.Serialize(node);
        var legacy = envelope[ExpressionSerializationWireFormat.NodePropertyName]!.AsObject();

        var deserialized = serializer.Deserialize(legacy);
        var roundTripped = mapper.ToExpression(deserialized);

        Assert.AreEqual(42, roundTripped.Compile()());

        return;

        Expression<Func<Int32>> CreateExpression() => () => 42;
    }

    [Test]
    public void Deserialize_WhenSchemaVersionUnsupported_ThrowsExpressionSerializationException()
    {
        var mapper = new MapsterExpressionNodeMapper<Func<Int32>>();
        var serializer = GetSerializer();

        var node = mapper.ToExpressionNode(CreateExpression());
        var envelope = serializer.Serialize(node);

        envelope[ExpressionSerializationWireFormat.SchemaPropertyName] = 99;

        var exception = Assert.Throws<ExpressionSerializationException>(() => serializer.Deserialize(envelope));

        Assert.That(exception!.Message, Does.Contain("99"));

        return;

        Expression<Func<Int32>> CreateExpression() => () => 42;
    }

    [Test]
    public void Serialize_WhenExpressionRoundTripsWithBooleanConstant_ReturnsTrue()
    {
        var mapper = new MapsterExpressionNodeMapper<Func<Boolean>>();
        var serializer = GetSerializer();

        var node = mapper.ToExpressionNode(CreateExpression());
        var serialized = serializer.Serialize(node);
        var deserialized = serializer.Deserialize(serialized);
        var roundTripped = mapper.ToExpression(deserialized);

        Assert.IsTrue(roundTripped.Compile()());

        return;

        Expression<Func<Boolean>> CreateExpression() => () => true;
    }

    [Test]
    public void Serialize_WhenExpressionRoundTripsWithDoubleConstant_ReturnsCompiledValue()
    {
        var mapper = new MapsterExpressionNodeMapper<Func<Double>>();
        var serializer = GetSerializer();

        var node = mapper.ToExpressionNode(CreateExpression());
        var serialized = serializer.Serialize(node);
        var deserialized = serializer.Deserialize(serialized);
        var roundTripped = mapper.ToExpression(deserialized);

        Assert.AreEqual(3.14, roundTripped.Compile()());

        return;

        Expression<Func<Double>> CreateExpression() => () => 3.14;
    }

    [Test]
    public void Serialize_WhenExpressionRoundTripsWithEnumConstant_ReturnsCompiledValue()
    {
        var mapper = new MapsterExpressionNodeMapper<Func<ExpressionType>>();
        var serializer = GetSerializer();

        var node = mapper.ToExpressionNode(CreateExpression());
        var serialized = serializer.Serialize(node);
        var deserialized = serializer.Deserialize(serialized);
        var roundTripped = mapper.ToExpression(deserialized);

        Assert.AreEqual(ExpressionType.Lambda, roundTripped.Compile()());

        return;

        Expression<Func<ExpressionType>> CreateExpression() => () => ExpressionType.Lambda;
    }

    [Test]
    public void Serialize_WhenExpressionRoundTripsWithInt32Constant_ReturnsCompiledValue()
    {
        var mapper = new MapsterExpressionNodeMapper<Func<Int32>>();
        var serializer = GetSerializer();

        var node = mapper.ToExpressionNode(CreateExpression());
        var serialized = serializer.Serialize(node);
        var deserialized = serializer.Deserialize(serialized);
        var roundTripped = mapper.ToExpression(deserialized);

        Assert.AreEqual(42, roundTripped.Compile()());

        return;

        Expression<Func<Int32>> CreateExpression() => () => 42;
    }

    [Test]
    public void Serialize_WhenExpressionRoundTripsWithInt64Constant_ReturnsCompiledValue()
    {
        var mapper = new MapsterExpressionNodeMapper<Func<Int64>>();
        var serializer = GetSerializer();

        var node = mapper.ToExpressionNode(CreateExpression());
        var serialized = serializer.Serialize(node);
        var deserialized = serializer.Deserialize(serialized);
        var roundTripped = mapper.ToExpression(deserialized);

        Assert.AreEqual(42L, roundTripped.Compile()());

        return;

        Expression<Func<Int64>> CreateExpression() => () => 42L;
    }

    [Test]
    public void Serialize_WhenExpressionRoundTripsWithLengthPredicate_PreservesEvaluation()
    {
        const String value = "test";
        var mapper = new MapsterExpressionNodeMapper<Func<String, Boolean>>();
        var serializer = GetSerializer();

        var expected = CreateExpression().Compile()(value);

        var node1 = mapper.ToExpressionNode(CreateExpression());
        var serialized = serializer.Serialize(node1);
        var node2 = serializer.Deserialize(serialized);

        var expression2 = mapper.ToExpression(node2);
        var actual = expression2.Compile()(value);

        Assert.AreEqual(expected, actual);

        return;

        Expression<Func<String, Boolean>> CreateExpression() => t => t.Length > 0;
    }

    [Test]
    public void Serialize_WhenExpressionRoundTripsWithMemberInit_PreservesEvaluation()
    {
        const String value = "test";
        var mapper = new MapsterExpressionNodeMapper<Func<String, TestModel>>();
        var serializer = GetSerializer();

        var expected = CreateExpression().Compile()(value).Value;

        var node1 = mapper.ToExpressionNode(CreateExpression());
        var serialized = serializer.Serialize(node1);
        var node2 = serializer.Deserialize(serialized);

        var expression2 = mapper.ToExpression(node2);
        var actual = expression2.Compile()(value).Value;

        Assert.AreEqual(expected, actual);

        return;

        Expression<Func<String, TestModel>> CreateExpression() => t => new TestModel { Value = t.Length };
    }

    [Test]
    public void Serialize_WhenExpressionRoundTripsWithNullConstant_ReturnsNull()
    {
        var mapper = new MapsterExpressionNodeMapper<Func<String?>>();
        var serializer = GetSerializer();

        var node = mapper.ToExpressionNode(CreateExpression());
        var serialized = serializer.Serialize(node);
        var deserialized = serializer.Deserialize(serialized);
        var roundTripped = mapper.ToExpression(deserialized);

        Assert.IsNull(roundTripped.Compile()());

        return;

        Expression<Func<String?>> CreateExpression() => () => null;
    }

    [Test]
    public void Serialize_WhenExpressionRoundTripsWithSingleConstant_ReturnsCompiledValue()
    {
        var mapper = new MapsterExpressionNodeMapper<Func<Single>>();
        var serializer = GetSerializer();

        var node = mapper.ToExpressionNode(CreateExpression());
        var serialized = serializer.Serialize(node);
        var deserialized = serializer.Deserialize(serialized);
        var roundTripped = mapper.ToExpression(deserialized);

        Assert.AreEqual(3.14f, roundTripped.Compile()());

        return;

        Expression<Func<Single>> CreateExpression() => () => 3.14f;
    }

    [Test]
    public void Serialize_WhenExpressionRoundTripsWithStringConstant_ReturnsCompiledValue()
    {
        var mapper = new MapsterExpressionNodeMapper<Func<String>>();
        var serializer = GetSerializer();

        var node = mapper.ToExpressionNode(CreateExpression());
        var serialized = serializer.Serialize(node);
        var deserialized = serializer.Deserialize(serialized);
        var roundTripped = mapper.ToExpression(deserialized);

        Assert.AreEqual("hello", roundTripped.Compile()());

        return;

        Expression<Func<String>> CreateExpression() => () => "hello";
    }

    [Test]
    public void Serialize_WhenNodeSerialized_IncludesSchemaAndNodeEnvelope()
    {
        var mapper = new MapsterExpressionNodeMapper<Func<Int32>>();
        var serializer = GetSerializer();

        var node = mapper.ToExpressionNode(CreateExpression());
        var serialized = serializer.Serialize(node);

        Assert.AreEqual(ExpressionSerializationWireFormat.CurrentSchemaVersion, serialized[ExpressionSerializationWireFormat.SchemaPropertyName]!.GetValue<Int32>());
        Assert.IsInstanceOf<JsonObject>(serialized[ExpressionSerializationWireFormat.NodePropertyName]);
        Assert.AreEqual("Lambda", serialized[ExpressionSerializationWireFormat.NodePropertyName]!["$type"]!.GetValue<String>());

        return;

        Expression<Func<Int32>> CreateExpression() => () => 42;
    }

    public class TestModel
    {
        public Int32 Value { get; set; }
    }
}
