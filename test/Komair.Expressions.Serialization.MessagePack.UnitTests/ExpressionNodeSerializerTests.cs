using System.Linq.Expressions;
using Komair.Expressions.Abstract;
using Komair.Expressions.Mapping.Mapster;
using Komair.Expressions.Serialization;
using Komair.Expressions.Serialization.Abstract.Interfaces;
using Komair.Expressions.Serialization.Exceptions;
using MessagePack;
using NUnit.Framework;

namespace Komair.Expressions.Serialization.MessagePack.UnitTests;

public class ExpressionNodeSerializerTests
{
    private static IExpressionNodeSerializer<Byte[], ExpressionNodeBase> GetSerializer()
    {
        return new ExpressionNodeSerializer<ExpressionNodeBase>();
    }

    [Test]
    public void Deserialize_WhenDocumentEmpty_ThrowsExpressionSerializationException()
    {
        var serializer = GetSerializer();

        Assert.Throws<ExpressionSerializationException>(() => serializer.Deserialize([]));
    }

    [Test]
    public void Deserialize_WhenEnvelopeMissingNode_ThrowsExpressionSerializationException()
    {
        var serializer = GetSerializer();
        var document = MessagePackSerializer.Serialize(new Object[] { ExpressionSerializationWireFormat.CurrentSchemaVersion });

        Assert.Throws<ExpressionSerializationException>(() => serializer.Deserialize(document));
    }

    [Test]
    public void Deserialize_WhenSchemaVersionUnsupported_ThrowsExpressionSerializationException()
    {
        var mapper = new MapsterExpressionNodeMapper<Func<Int32>>();
        var serializer = GetSerializer();

        var node = mapper.ToExpressionNode(CreateExpression());
        var envelope = serializer.Serialize(node);
        var reader = new MessagePackReader(envelope);

        reader.ReadArrayHeader();
        reader.ReadInt32();

        var nodeBytes = reader.ReadRaw();
        var buffer = new System.Buffers.ArrayBufferWriter<Byte>();
        var writer = new MessagePackWriter(buffer);

        writer.WriteArrayHeader(2);
        writer.Write(99);
        writer.WriteRaw(nodeBytes);
        writer.Flush();

        var exception = Assert.Throws<ExpressionSerializationException>(() => serializer.Deserialize(buffer.WrittenSpan.ToArray()));

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

        Assert.That(roundTripped.Compile()(), Is.True);

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

        Assert.That(roundTripped.Compile()(), Is.EqualTo(3.14));

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

        Assert.That(roundTripped.Compile()(), Is.EqualTo(ExpressionType.Lambda));

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

        Assert.That(roundTripped.Compile()(), Is.EqualTo(42));

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

        Assert.That(roundTripped.Compile()(), Is.EqualTo(42L));

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

        Assert.That(actual, Is.EqualTo(expected));

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

        Assert.That(actual, Is.EqualTo(expected));

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

        Assert.That(roundTripped.Compile()(), Is.Null);

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

        Assert.That(roundTripped.Compile()(), Is.EqualTo(3.14f));

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

        Assert.That(roundTripped.Compile()(), Is.EqualTo("hello"));

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
        var reader = new MessagePackReader(serialized);

        Assert.That(reader.ReadArrayHeader(), Is.EqualTo(2));
        Assert.That(reader.ReadInt32(), Is.EqualTo(ExpressionSerializationWireFormat.CurrentSchemaVersion));
        Assert.That(reader.ReadArrayHeader(), Is.EqualTo(2));
        Assert.That(reader.ReadString(), Is.EqualTo("Lambda"));

        return;

        Expression<Func<Int32>> CreateExpression() => () => 42;
    }

    public class TestModel
    {
        public Int32 Value { get; set; }
    }
}
