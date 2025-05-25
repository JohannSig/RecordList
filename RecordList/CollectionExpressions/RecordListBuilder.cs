namespace RecordList.CollectionExpressions;

/// <summary>
/// Collection expression builder.
/// </summary>
public static class RecordListBuilder
{
    // The builder method must be generic, 
    // but must _not_ be in a generic type
    public static RecordList<T> Create<T>(ReadOnlySpan<T> values) => values.Length == 0 
        ? RecordList<T>.Empty
        : new(values);
}
