using System.ComponentModel.DataAnnotations;
using BigO.Core.Extensions;

namespace BigO.Core.Tests;

public class TypeExtensionsTests
{
    public static IEnumerable<object[]> TypeExtensionsValueTypeTestData => new List<object[]>
    {
        new object[] { typeof(bool), false },
        new object[] { typeof(byte), (byte)0 },
        new object[] { typeof(char), '\0' },
        new object[] { typeof(decimal), (decimal)0 },
        new object[] { typeof(double), (double)0 },
        new object[] { typeof(float), (float)0 },
        new object[] { typeof(int), 0 },
        new object[] { typeof(long), (long)0 },
        new object[] { typeof(sbyte), (sbyte)0 },
        new object[] { typeof(short), (short)0 },
        new object[] { typeof(uint), (uint)0 },
        new object[] { typeof(ulong), (ulong)0 },
        new object[] { typeof(ushort), (ushort)0 }
    };


    [Fact]
    public void IsNullable_TypeIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        Type? type = null;

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => type.IsNullable());
    }

    [Fact]
    public void IsNullable_TypeIsNotNullable_ReturnsFalse()
    {
        // Arrange
        var type = typeof(int);

        // Act
        var result = type.IsNullable();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsNullable_TypeIsNullable_ReturnsTrue()
    {
        // Arrange
        var type = typeof(int?);

        // Act
        var result = type.IsNullable();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsOfNullableType_GivenNonNullableType_ReturnsFalse()
    {
        var result = "test".IsOfNullableType();

        Assert.False(result);
    }

    [Fact]
    public void IsOfNullableType2_GivenNonNullableType_ReturnsFalse()
    {
        var result = TypeExtensions.IsOfNullableType<string>();

        Assert.False(result);
    }

    [Fact]
    public void IsOfNullableType2_GivenNullableType_ReturnsTrue()
    {
        var result = TypeExtensions.IsOfNullableType<int?>();

        Assert.True(result);
    }

    [Fact]
    public void IsNumeric_GivenTypeThatIsNotNumeric_ReturnsFalse()
    {
        var result = typeof(string).IsNumeric();

        Assert.False(result);
    }

    [Fact]
    public void IsNumeric_GivenTypeThatIsNumeric_ReturnsTrue()
    {
        var result = typeof(int).IsNumeric();

        Assert.True(result);
    }

    [Fact]
    public void IsNumeric_GivenNullableTypeThatIsNumeric_ReturnsTrue()
    {
        var result = typeof(int?).IsNumeric();

        Assert.True(result);
    }

    [Fact]
    public void IsNumeric_GivenNullType_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ((Type?)null).IsNumeric());
    }

    [Fact]
    public void IsNumeric_GivenNullableTypeThatIsNumeric_ExcludesNullableTypes_ReturnsFalse()
    {
        var result = typeof(int?).IsNumeric(false);

        Assert.False(result);
    }

    [Fact]
    public void IsOpenGeneric_GivenTypeThatIsNotOpenGeneric_ReturnsFalse()
    {
        var result = typeof(string).IsOpenGeneric();

        Assert.False(result);
    }

    [Fact]
    public void IsOpenGeneric_GivenTypeThatIsOpenGeneric_ReturnsTrue()
    {
        var result = typeof(List<>).IsOpenGeneric();

        Assert.True(result);
    }

    [Fact]
    public void IsOpenGeneric_GivenNullType_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ((Type?)null)!.IsOpenGeneric());
    }

    [Fact]
    public void HasAttribute_GivenTypeThatDoesNotHaveAttribute_ReturnsFalse()
    {
        var result = typeof(string).HasAttribute(typeof(RequiredAttribute));

        Assert.False(result);
    }

    [Fact]
    public void HasAttribute_GivenNullType_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ((Type?)null)!.HasAttribute(typeof(TestAttribute)));
    }

    [Fact]
    public void HasAttribute_GivenTypeThatHasAttributeInheritedFromBaseType_ReturnsTrue()
    {
        var result = typeof(ClassWithInheritedAttribute).HasAttribute<TestAttribute>(a => true);

        Assert.True(result);
    }

    [Fact]
    public void HasAttribute_GivenTypeThatHasAttribute_WithPredicateThatDoesNotMatch_ReturnsFalse()
    {
        var result = typeof(ClassWithAttribute).HasAttribute<TestAttribute>(a => a.Value == "wrong");

        Assert.False(result);
    }

    [Test]
    private class ClassWithAttribute
    {
        [Test("test")]
        public void TestMethod()
        {
        }
    }

    private class ClassWithInheritedAttribute : ClassWithAttribute
    {
    }

    private class TestAttribute : Attribute
    {
        public TestAttribute()
        {
        }

        public TestAttribute(string value)
        {
            Value = value;
        }

        public string? Value { get; }
    }
}