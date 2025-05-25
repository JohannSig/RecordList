using System.Collections.Immutable;
using System.Text.Json;
using FluentAssertions;

namespace RecordList.Tests;

public sealed record Person(string Name, int Age);

public class RecordListTests
{
    [Fact]
    public void Constructor_FromIEnumerable_WorksCorrectly()
    {
        var people = new[] { new Person("Alice", 30), new Person("Bob", 25) };
        var list = new RecordList<Person>(people);

        Assert.Equal(2, list.Count);
        Assert.Equal(people[0], list[0]);
        Assert.Equal(people[1], list[1]);
    }

    [Fact]
    public void Constructor_FromImmutableArray_WorksCorrectly()
    {
        var array = ImmutableArray.Create(new Person("Alice", 30));
        var list = new RecordList<Person>(array);

        Assert.Single(list);
        Assert.Equal(array[0], list[0]);
    }

    [Fact]
    public void Constructor_FromReadOnlySpan_WorksCorrectly()
    {
        var array = new[] { new Person("Alice", 30), new Person("Bob", 25) };
        var list = new RecordList<Person>(array.AsSpan());

        Assert.Equal(2, list.Count);
        Assert.Equal(array[0], list[0]);
        Assert.Equal(array[1], list[1]);
    }

    [Fact]
    public void Equality_WorksCorrectly()
    {
        var a = new RecordList<int>(new[] { 1, 2, 3 });
        var b = new RecordList<int>(ImmutableArray.Create(1, 2, 3));
        var c = new RecordList<int>(new[] { 3, 2, 1 });

        Assert.Equal(a, b);
        Assert.NotEqual(a, c);
        Assert.True(a.Equals(b));
        Assert.False(a.Equals(c));
    }

    [Fact]
    public void HashCode_IsStable()
    {
        var list1 = new RecordList<string>(new[] { "a", "b" });
        var list2 = new RecordList<string>(new[] { "a", "b" });

        Assert.Equal(list1.GetHashCode(), list2.GetHashCode());
    }

    [Fact]
    public void Contains_Works()
    {
        var list = new RecordList<int>(new[] { 10, 20, 30 });

        Assert.True(list.Contains(20));
        Assert.False(list.Contains(99));
    }

    [Fact]
    public void IndexOf_Works()
    {
        var list = new RecordList<string>(new[] { "a", "b", "c" });

        Assert.Equal(1, list.IndexOf("b"));
        Assert.Equal(-1, list.IndexOf("z"));
    }

    [Fact]
    public void LastIndexOf_Works()
    {
        var list = new RecordList<string>(new[] { "x", "y", "x" });

        Assert.Equal(2, list.LastIndexOf("x"));
    }

    [Fact]
    public void ToImmutableArray_ReturnsCorrectArray()
    {
        var original = new[] { 1, 2, 3 };
        var list = new RecordList<int>(original);

        var array = list.ToImmutableArray();

        Assert.Equal(original, array);
    }

    [Fact]
    public void Empty_ReturnsSameInstance()
    {
        var a = RecordList<int>.Empty;
        var b = RecordList<int>.Empty;

        Assert.Same(a, b);
        Assert.Empty(a);
    }

    [Fact]
    public void Serialization_RoundTrip_Works()
    {
        var options = new JsonSerializerOptions { WriteIndented = false };
        var original = new RecordList<Person>(new[] { new Person("A", 1), new Person("B", 2) });

        var json = JsonSerializer.Serialize(original, options);
        var deserialized = JsonSerializer.Deserialize<RecordList<Person>>(json, options);

        Assert.Equal(original, deserialized);
    }

    [Fact]
    public void ToString_ReturnsUsefulDebugInfo()
    {
        var list = new RecordList<int>(new[] { 1, 2, 3 });

        Assert.StartsWith("RecordList<Int32>[3]", list.ToString());
    }

    [Fact]
    public void SerializationTest()
    {
        RecordList<string> recordList = ["Item1", "Item2", "Item3"];

        string json = JsonSerializer.Serialize(recordList);

        json.Should().Be("[\"Item1\",\"Item2\",\"Item3\"]");
    }

    [Fact]
    public void DeserializationTest()
    {
        string json = "[\"Item1\",\"Item2\",\"Item3\"]";
        var recordList = JsonSerializer.Deserialize<RecordList<string>>(json);
        recordList.Should().NotBeNull();
        recordList.Should().HaveCount(3);
        recordList[0].Should().Be("Item1");
        recordList[1].Should().Be("Item2");
        recordList[2].Should().Be("Item3");
    }
}