# Komair.Expressions.Serialization

An abstraction library for serializing `Komair.Expressions.ExpressionNode` objects to and from other representations (including `System.Linq.Expressions`).

## Key Concepts

- **serialization boundary** &ndash; defines how `ExpressionNodeBase` graphs are converted to transport-friendly types
- **pluggable formats** &ndash; concrete serializers (such as JSON) live in separate packages and implement the abstraction interfaces

Typical usage is to pair this package with `Komair.Expressions.Serialization.Json` in applications that need to send or persist expression trees.

## Installation

```shell
dotnet add package Komair.Expressions.Serialization
```
