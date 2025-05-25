namespace RecordList.Tests;

public class RecordListCollectionExpressionTests
{
    [Fact]
    public void CanUseCollectionExpression_WithPrimitives()
    {
        RecordList<int> list = [1, 2, 3];

        Assert.Equal(3, list.Count);
        Assert.Equal(1, list[0]);
        Assert.Equal(2, list[1]);
        Assert.Equal(3, list[2]);
    }

    [Fact]
    public void CollectionExpression_CreatesCorrectEquality()
    {
        RecordList<string> list1 = ["a", "b", "c"];
        RecordList<string> list2 = new(new[] { "a", "b", "c" });

        Assert.Equal(list1, list2);
    }

    [Fact]
    public void CollectionExpression_WorksWithRecords()
    {
        var a = new Person("Alice", 30);
        var b = new Person("Bob", 25);

        RecordList<Person> list = [a, b];

        Assert.Equal(2, list.Count);
        Assert.Equal(a, list[0]);
        Assert.Equal(b, list[1]);
    }

    [Fact]
    public void CollectionExpression_EmptyList_Works()
    {
        RecordList<int> empty = [];

        Assert.Same(RecordList<int>.Empty, empty);
        Assert.Empty(empty);
    }

    [Fact]
    public void CollectionExpression_MixedLiterals_AndVariables()
    {
        var x = 99;
        RecordList<int> list = [1, x, 3];

        Assert.Equal([1, 99, 3], list.ToImmutableArray());
    }

    public sealed record Person(string Name, int Age);
}