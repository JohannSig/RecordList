namespace RecordList.Tests;

public sealed record RecordWithId(int Id, string Name);

public class RecordListFalsePositiveTests
{
    [Fact]
    public void DifferentOrder_ShouldNotBeEqual()
    {
        var list1 = new RecordList<int>(new[] { 1, 2, 3 });
        var list2 = new RecordList<int>(new[] { 3, 2, 1 });

        Assert.NotEqual(list1, list2);
    }

    [Fact]
    public void DifferentLength_ShouldNotBeEqual()
    {
        var list1 = new RecordList<string>(new[] { "a", "b" });
        var list2 = new RecordList<string>(new[] { "a", "b", "c" });

        Assert.NotEqual(list1, list2);
    }

    [Fact]
    public void SameLength_DifferentElements_ShouldNotBeEqual()
    {
        var list1 = new RecordList<string>(new[] { "a", "b", "c" });
        var list2 = new RecordList<string>(new[] { "a", "x", "c" });

        Assert.NotEqual(list1, list2);
    }

    [Fact]
    public void ReferenceEqualsButNotSameType_ShouldNotBeEqual()
    {
        var list = new RecordList<int>(new[] { 1, 2 });
        var unrelated = new object();

        Assert.False(list.Equals(unrelated));
    }

    [Fact]
    public void SameElements_DifferentReferenceTypes_ShouldNotBeEqual_UnlessEqualOverride()
    {
        var a1 = new RecordWithId(1, "A");
        var a2 = new RecordWithId(1, "A");

        Assert.NotSame(a1, a2);
        Assert.Equal(a1, a2); // record equality

        var list1 = new RecordList<RecordWithId>(new[] { a1 });
        var list2 = new RecordList<RecordWithId>(new[] { a2 });

        Assert.Equal(list1, list2); // deep equality should succeed
    }

    [Fact]
    public void CustomEqualityStructs_ShouldRespectValueSemantics()
    {
        var s1 = new CustomEquatableStruct(1);
        var s2 = new CustomEquatableStruct(2);

        var list1 = new RecordList<CustomEquatableStruct>(new[] { s1 });
        var list2 = new RecordList<CustomEquatableStruct>(new[] { s2 });

        Assert.NotEqual(list1, list2);
    }

    private readonly struct CustomEquatableStruct : IEquatable<CustomEquatableStruct>
    {
        public int Value { get; }

        public CustomEquatableStruct(int value) => Value = value;

        public bool Equals(CustomEquatableStruct other) => Value == other.Value;

        public override bool Equals(object? obj) => obj is CustomEquatableStruct other && Equals(other);

        public override int GetHashCode() => Value;
    }
}
