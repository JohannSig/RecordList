using System.Collections;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using RecordList.CollectionExpressions;
using RecordList.Serializations;

namespace RecordList;

/// <summary>
/// An attempt to create a record-friendly list type that supports collection expressions,
/// immutability, equality comparisons and JSON serialization.
/// </summary>
/// <typeparam name="T"></typeparam>
[CollectionBuilder(typeof(RecordListBuilder), nameof(RecordListBuilder.Create))]
[JsonConverter(typeof(RecordListJsonConverterFactory))]
public sealed class RecordList<T> : IReadOnlyList<T>, IEquatable<RecordList<T>>
{
    public static RecordList<T> Empty { get; } = new();

    private readonly ImmutableArray<T> _items;

    private RecordList()
    {
        _items = [];
    }

    public RecordList(IEnumerable<T> items)
    {
        ArgumentNullException.ThrowIfNull(items, nameof(items));
     
        if (items is ImmutableArray<T> immutableArray)
        {
            // If the items are already an ImmutableArray, we can use it directly
            _items = immutableArray;
            return;
        }
        else if (items is T[] array)
        {
            _items = ImmutableArray.Create(array);
            return;
        }

        this._items = [.. items];
    }

    public RecordList(params T[]? items)
    {
        this._items = ImmutableArray.Create(items);
    }

    public RecordList(ReadOnlySpan<T> items)
    {
        _items = ImmutableArray.Create(items);
    }

    // IReadOnlyList
    public T this[int index] => _items[index];
    public int Count => _items.Length;

    // Equality
    public bool Equals(RecordList<T>? other)
    {
        if (ReferenceEquals(this, other)) return true;
        if (other is null || Count != other.Count) return false;
        return _items.SequenceEqual(other._items);
    }

    public override bool Equals(object? obj) => obj is RecordList<T> other && this.Equals(other);

    private int? hashCode;

    public override int GetHashCode()
    {
        hashCode ??= this.GetHashCodeInternal();

        return hashCode.Value;
    }

    private int GetHashCodeInternal()
    {
        // Use a hash combiner to get a stable value
        var hash = new HashCode();

        hash.Add(_items.Length);

        foreach (var item in _items)
        {
            hash.Add(item, EqualityComparer<T>.Default);
        }

        return hash.ToHashCode();
    }

    public IEnumerator<T> GetEnumerator()
    {
        return ((IEnumerable<T>)_items).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_items).GetEnumerator();
    }

    public ImmutableArray<T> ToImmutableArray() => _items;

    public bool Contains(T item) => _items.Contains(item);

    public int IndexOf(T item) => _items.IndexOf(item);

    public int LastIndexOf(T item) => _items.LastIndexOf(item);

    public override string ToString() => $"RecordList<{typeof(T).Name}>[{this.Count}]";
}
