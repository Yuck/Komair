# Komair.Expressions.Mapping

A .NET abstraction library for mapping `Komair.Expressions.ExpressionNode` objects to and from other representations (in particular `System.Linq.Expressions`).

## Key Concepts

- **mapping abstraction** &ndash; defines contracts for converting between serializable expression nodes and runtime expression trees
- **expression-node centric design** &ndash; focuses on `ExpressionNodeBase` and its derived node types as the mapping boundary

This package does not depend on any specific mapping framework; concrete implementations live in separate packages such as `Komair.Expressions.Mapping.Mapster`.

## Installation

```shell
dotnet add package Komair.Expressions.Mapping
```
