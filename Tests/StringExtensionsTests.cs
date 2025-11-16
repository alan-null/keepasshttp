using System;
using KeePassHttp.Extensions;
using Xunit;

namespace KeePassHttp.Tests
{
    public class StringExtensionsTests
    {
        [Fact]
        public void RemoveUrlParameters_RemovesQuery()
        {
            var url = "https://example.com/path/to/res?x=1&y=2";
            var result = StringExtensions.RemoveUrlParameters(url);
            Assert.Equal("https://example.com/path/to/res", result);
        }

        [Fact]
        public void RemoveUrlParameters_RemovesFragment()
        {
            var url = "https://example.com/path/sub/#section";
            var result = StringExtensions.RemoveUrlParameters(url);
            Assert.Equal("https://example.com/path/sub/", result);
        }

        [Fact]
        public void RemoveUrlParameters_RootWithQuery()
        {
            var url = "https://example.com?x=1";
            var result = StringExtensions.RemoveUrlParameters(url);
            Assert.Equal("https://example.com", result);
        }

        [Fact]
        public void RemoveUrlParameters_TrailingSlashWithQuery()
        {
            var url = "https://example.com/path/?a=1";
            var result = StringExtensions.RemoveUrlParameters(url);
            Assert.Equal("https://example.com/path/", result);
        }

        [Fact]
        public void RemoveUrlParameters_EncodedPathPreserved()
        {
            var url = "https://example.com/a%20b/c%2Fd?x=1#frag";
            var result = StringExtensions.RemoveUrlParameters(url);
            Assert.Equal("https://example.com/a%20b/c%2Fd", result);
        }

        [Fact]
        public void RemoveUrlParameters_Null_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => StringExtensions.RemoveUrlParameters(null));
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void RemoveUrlParameters_Whitespace_Throws(string input)
        {
            Assert.Throws<ArgumentNullException>(() => StringExtensions.RemoveUrlParameters(input));
        }

        [Theory]
        [InlineData("ht!tp://bad")]
        [InlineData("://missing-scheme")]
        [InlineData("http:/ /space")]
        public void RemoveUrlParameters_InvalidUrl_Throws(string input)
        {
            Assert.Throws<ArgumentException>(() => StringExtensions.RemoveUrlParameters(input));
        }
    }
}
