using Komair.Expressions.Abstract;
using Komair.Expressions.Serialization.Abstract.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// TODO: Eventually try to use System.Text.Json, but for now its serializer is pretty hard to work with
namespace Komair.Expressions.Serialization.Json;

public class JsonExpressionNodeSerializer<TExpressionNode>(JsonSerializerSettings? settings = null) : IExpressionNodeSerializer<JObject, TExpressionNode> where TExpressionNode : ExpressionNodeBase
{
    private readonly JsonSerializerSettings? _settings = settings;

    public TExpressionNode Deserialize(JObject document)
    {
        return JsonConvert.DeserializeObject<TExpressionNode>(document.ToString(), _settings);
    }

    public JObject Serialize(TExpressionNode node) => JObject.Parse(JsonConvert.SerializeObject(node, _settings));
}
