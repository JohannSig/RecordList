# RecordList

`RecordList<T>` is an immutable, record-friendly collection type for C# designed to enable deep structural equality comparison and seamless integration with record types. It wraps an `ImmutableArray<T>` internally, ensuring efficient, thread-safe, and allocation-friendly operations.

## Why RecordList?

C# record types automatically implement value-based equality for their properties, but many collection types do not behave well when used as record properties — they default to reference equality rather than element-wise comparison.

`RecordList<T>` solves this by providing a collection that:

- Supports deep equality comparison of elements,
- Is immutable and thread-safe,
- Supports collection expressions with a custom builder,
- Integrates with JSON serialization via a custom `JsonConverter`,
- Provides multiple constructors for flexible instantiation,
- Can be nested inside other record types without losing value semantics.

## Features

- **Immutable collection** backed by `ImmutableArray<T>`
- **Deep equality** and hashing based on elements
- Support for **collection expressions** (enables `{ ... }` initializer syntax)
- Constructors accepting `IEnumerable<T>`, `params T[]`, `ImmutableArray<T>`, and `ReadOnlySpan<T>`
- Seamless JSON serialization/deserialization support
- Predefined `Empty` singleton for zero-allocation empty lists

## Installation

To use `RecordList` in your project, clone the repository or add the source directly. (NuGet package coming soon!)

## Usage

```csharp
var list1 = new RecordList<int>(new[] { 1, 2, 3 });
var list2 = new RecordList<int> { 1, 2, 3 }; // Using collection expression

bool areEqual = list1 == list2; // true, deep equality comparison

// Using in records:
public record Group(string Name, RecordList<Person> Members);
