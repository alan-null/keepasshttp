using System.Collections.Generic;
using System.Linq;
using Xunit;
using KeePassHttp.Extensions;

namespace KeePassHttp.Tests
{
    public class ObjectExtensionsTests
    {
        [Fact]
        public void NullModel_ReturnsEmptyList()
        {
            object model = null;
            var result = model.GetNonEmptyFields();
            Assert.Empty(result);
        }

        [Fact]
        public void EmptyString_Included()
        {
            var model = new { Empty = "" };
            var result = model.GetNonEmptyFields();
            Assert.Contains("Empty", result);
        }

        [Fact]
        public void NonEmptyString_Included()
        {
            var model = new { Value = "a" };
            var result = model.GetNonEmptyFields();
            Assert.Contains("Value", result);
        }

        [Fact]
        public void EmptyDictionary_Included()
        {
            var model = new { Dict = new Dictionary<string, string>() };
            var result = model.GetNonEmptyFields();
            Assert.Contains("Dict", result);
        }

        [Fact]
        public void NonEmptyDictionary_Included()
        {
            var model = new { Dict = new Dictionary<string, string> { { "a", "b" } } };
            var result = model.GetNonEmptyFields();
            Assert.Equal(new[] { "Dict" }, result);
        }

        [Fact]
        public void ZeroInt_Included()
        {
            var model = new { IntValue = 0 };
            var result = model.GetNonEmptyFields();
            Assert.Contains("IntValue", result);
        }

        [Fact]
        public void NullableInt_Null_Excluded()
        {
            var model = new { IntValue = (int?)null };
            var result = model.GetNonEmptyFields();
            Assert.DoesNotContain("IntValue", result);
            Assert.Empty(result);
        }

        [Fact]
        public void Object_NonNull_Included()
        {
            var model = new { Obj = new object() };
            var result = model.GetNonEmptyFields();
            Assert.Contains("Obj", result);
        }

        [Fact]
        public void MixedProperties_ReturnsOnlyNonEmpty()
        {
            var model = new
            {
                S1 = "",
                S2 = "abc",
                D1 = new Dictionary<string, string>(),
                D2 = new Dictionary<string, string> { { "k", "v" } },
                I1 = 0,
                I2 = (int?)null,
                O1 = (object)null,
                O2 = new object()
            };
            var result = model.GetNonEmptyFields();
            var expected = new[] { "S1", "S2", "D1", "D2", "I1", "O2" };
            Assert.Equal(expected.OrderBy(x => x), result.OrderBy(x => x));
            Assert.DoesNotContain("I2", result);
            Assert.DoesNotContain("O1", result);
        }
    }
}
