namespace RecordList.Tests;

public class RecordListNestedEqualityTests
{
    public sealed record Person(string Name, int Age);

    public sealed record Group(string GroupName, RecordList<Person> Members);

    public sealed record NestedWrapper(Group GroupData);

    [Fact]
    public void Equality_Works_AsRecordProperty()
    {
        var a1 = new Person("Alice", 30);
        var b1 = new Person("Bob", 25);
        var list1 = new RecordList<Person>([a1, b1]);

        var a2 = new Person("Alice", 30);
        var b2 = new Person("Bob", 25);
        var list2 = new RecordList<Person>([a2, b2]);

        var group1 = new Group("TestGroup", list1);
        var group2 = new Group("TestGroup", list2);

        Assert.Equal(group1, group2);
    }

    [Fact]
    public void Inequality_DifferentContent()
    {
        var list1 = new RecordList<string>(["one", "two"]);
        var list2 = new RecordList<string>(["one", "three"]);

        var group1 = new Group("Mismatch", list1.Select(s => new Person(s, 1)).ToRecordList());
        var group2 = new Group("Mismatch", list2.Select(s => new Person(s, 1)).ToRecordList());

        Assert.NotEqual(group1, group2);
    }

    [Fact]
    public void Inequality_DifferentLength()
    {
        var group1 = new Group("Test", new RecordList<Person>([new("A", 1)]));
        var group2 = new Group("Test", new RecordList<Person>([new("A", 1), new("B", 2)]));

        Assert.NotEqual(group1, group2);
    }

    [Fact]
    public void Equality_Works_InsideNestedRecord()
    {
        var a = new Person("Alice", 30);
        var b = new Person("Bob", 25);
        var list = new RecordList<Person>([a, b]);

        var nested1 = new NestedWrapper(new Group("DeepGroup", list));
        var nested2 = new NestedWrapper(new Group("DeepGroup", list));

        Assert.Equal(nested1, nested2);
    }

    [Fact]
    public void Inequality_Works_InsideNestedRecord()
    {
        var list1 = new RecordList<Person>([new("A", 1)]);
        var list2 = new RecordList<Person>([new("B", 2)]);

        var nested1 = new NestedWrapper(new Group("Group", list1));
        var nested2 = new NestedWrapper(new Group("Group", list2));

        Assert.NotEqual(nested1, nested2);
    }

    [Fact]
    public void RecordListReference_ShouldNotAffectEquality()
    {
        var person = new Person("Same", 42);

        var sharedList = new RecordList<Person>([person]);

        var g1 = new Group("G", sharedList);
        var g2 = new Group("G", new RecordList<Person>([new Person("Same", 42)]));

        // Records should match structurally, even though the RecordList instances are different.
        Assert.Equal(g1, g2);
    }
}