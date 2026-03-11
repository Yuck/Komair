# komair.expressions.serialization.json

A JSON-based implementation of Komair expression serialization using `Newtonsoft.Json`.

## key types

- **`JsonExpressionNodeSerializer<TNode>`** &ndash; serializes and deserializes expression node graphs to and from `JObject` using `Json.NET` type metadata

## dependencies

- [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json)
- `Komair.Expressions`
- `Komair.Expressions.Serialization`

## installation

```shell
dotnet add package Komair.Expressions.Serialization.Json
```
