using RecordList;

namespace System.Linq
{
    public static class IEnumerableExtensions
    {
        public static RecordList<T> ToRecordList<T>(this IEnumerable<T> source)
        {
            ArgumentNullException.ThrowIfNull(source, nameof(source));

            return new RecordList<T>(source);
        }
    }
}
