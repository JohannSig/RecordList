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
    /// <summary>
    /// An empty instance of RecordList<T> for convenience and performance.
    /// </summary>
    public static RecordList<T> Empty { get; } = new();

    /// <summary>
    /// Backing field for the items in the list.
    /// </summary>
    private readonly ImmutableArray<T> _items;

    /// <summary>
    /// Private constructor for the empty list.
    /// </summary>
    private RecordList()
    {
        _items = ImmutableArray<T>.Empty;
    }

    /// <summary>
    /// Generic constructor for any IEnumerable<T> type.
    /// </summary>
    /// <param name="items"></param>
    public RecordList(IEnumerable<T> items)
    {
        ArgumentNullException.ThrowIfNull(items, nameof(items));

        _items = items switch
        {
            ImmutableArray<T> immutableArray => immutableArray,
            T[] array => ImmutableArray.Create(array),
            _ => ImmutableArray.CreateRange(items)
        };
    }

    /// <summary>
    /// Constructor for new T[] { x, y, z, .. } arguments.
    /// </summary>
    /// <param name="items"></param>
    public RecordList(params T[]? items)
    {
        this._items = ImmutableArray.Create(items);
    }

    /// <summary>
    /// Constructor for collection expressions.
    /// </summary>
    /// <param name="items"></param>
    public RecordList(ReadOnlySpan<T> items)
    {
        _items = ImmutableArray.Create(items);
    }

    /// <summary>
    /// Gets the item at the specified index in the list.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public T this[int index] => _items[index];

    /// <summary>
    /// Gets the number of items in the list.
    /// </summary>
    public int Count => _items.Length;

    /// <summary>
    /// Checks if this RecordList<T> is equal to another RecordList<T> instance.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(RecordList<T>? other)
    {
        if (ReferenceEquals(this, other)) return true;
        if (other is null || Count != other.Count) return false;
        return _items.SequenceEqual(other._items);
    }

    /// <summary>
    /// Checks if this RecordList<T> is equal to another object.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj) => obj is RecordList<T> other && this.Equals(other);

    /// <summary>
    /// The idempotent hash code for this RecordList<T> instance.
    /// </summary>
    private int? hashCode;

    /// <summary>
    /// Gets the hash code for this RecordList<T> instance.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        hashCode ??= this.GetHashCodeInternal();

        return hashCode.Value;
    }

    /// <summary>
    /// Internal method to compute the hash code for this RecordList<T> instance.
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Returns an enumerator that iterates through the RecordList<T> instance.
    /// </summary>
    /// <returns></returns>
    public IEnumerator<T> GetEnumerator()
    {
        return ((IEnumerable<T>)_items).GetEnumerator();
    }

    /// <summary>
    /// Returns an enumerator that iterates through the RecordList<T> instance.
    /// </summary>
    /// <returns></returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_items).GetEnumerator();
    }

    /// <summary>
    /// Returns the backing ImmutableArray<T> for this RecordList<T> instance.
    /// </summary>
    /// <returns></returns>
    public ImmutableArray<T> ToImmutableArray() => _items;

    /// <summary>
    /// Checks if the RecordList<T> contains a specific item.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool Contains(T item) => _items.Contains(item);

    /// <summary>
    /// Finds the index of the first occurrence of a specific item in the RecordList<T>.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int IndexOf(T item) => _items.IndexOf(item);

    /// <summary>
    /// Finds the index of the last occurrence of a specific item in the RecordList<T>.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int LastIndexOf(T item) => _items.LastIndexOf(item);

    /// <summary>
    /// Returns a string representation of the RecordList<T> instance, including its type and count.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"RecordList<{typeof(T).Name}>[{this.Count}]";
}
