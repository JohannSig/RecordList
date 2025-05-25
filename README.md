# RecordList\<T>

A lightweight, record-friendly, immutable list type designed for **deep structural equality** comparisons, collection expressions support, and efficient JSON serialization.

---

## Overview

`RecordList<T>` wraps an immutable array to provide a list-like collection optimized for usage in **record types** and other immutable data structures. Its main purpose is to solve the common problem where standard collections like `List<T>` or arrays do **not** perform deep equality comparisons when used as record properties.

Key features:

- Implements `IReadOnlyList<T>`, `IEquatable<RecordList<T>>`, and supports collection expressions.
- Supports structural equality and stable hash code generation based on element contents.
- Optimized for immutability and compatibility with C# 12 collection expressions.
- Custom JSON converter for seamless serialization and deserialization.
- Minimal allocations with caching of the hash code.

---

## Important Considerations

### Immutability of `T`

- **`T` should be immutable** or at least behave as effectively immutable.  
- Ideally, use record types or other immutable types for `T` to ensure **deep structural equality** works as intended.
- Mutation of elements after the creation of a `RecordList<T>` will lead to **incorrect equality and hash codes** because the hash code is cached on first calculation.
- Nested `RecordList<T>` as elements of another `RecordList<T>` or record type is recommended for **full deep structural integrity**.

### Equality and Hash Code Behavior

- Equality and hash code operations are **O(n)** in the number of elements.
- The hash code is computed once and cached to optimize repeated calls.
- Do **not** modify elements after construction, or else the cached hash code and equality results may become invalid.

### Construction and Constructor Overloads

- `RecordList<T>` supports multiple constructors:  
  - From `IEnumerable<T>`  
  - From `params T[]` (note: may cause overload ambiguity with `IEnumerable<T>`)  
  - From `ReadOnlySpan<T>` (used internally for collection expression support)  
  - From `ImmutableArray<T>` (internal optimization)

- If you experience constructor ambiguity, prefer explicitly using `IEnumerable<T>` or factory methods.

---

## Usage

```csharp
var list = new RecordList<int>(new[] {1, 2, 3});
var empty = RecordList<int>.Empty;

// Equality works structurally
var list1 = new RecordList<string>(["a", "b"]);
var list2 = new RecordList<string>(["a", "b"]);
Console.WriteLine(list1.Equals(list2)); // True

// Use in records for deep equality
public record Group(string Name, RecordList<Person> Members);
```

## Extensions
An extension method is provided to convert any IEnumerable<T> to a RecordList<T>:
```csharp
var recordList = myEnumerable.ToRecordList();
```

Performance
- Equality and hashing are optimized but inherently linear time operations.
- Suitable for collections of moderate size where structural equality is required.
- For very large collections, consider the performance impact of equality and hashing.

## Serialization
- Supports JSON serialization and deserialization with a custom converter.
- Compatible with popular serializers that respect JsonConverter attributes.

## License
This project is licensed under the MIT License.

## Contributions
Feel free to contribute or report issues via GitHub.

## Disclaimer
RecordList<T> is a thin immutable wrapper around ImmutableArray<T> designed to improve the experience of using collections inside immutable record types. It does not prevent mutation of mutable T elements; that responsibility lies with the user to ensure immutability.

Always prefer immutable or record types for T to maintain integrity of equality semantics.
