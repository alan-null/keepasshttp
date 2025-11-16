using KeePassHttp.Extensions;
using System;
using System.Security.Cryptography;
using Xunit;

namespace KeePassHttp.Tests.StringExtensions
{
    public class EncryptDecryptString
    {
        private const string nullString = null;

        private static Aes NewAes()
        {
            var aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = new byte[32]; // all zeros
            aes.IV = new byte[16];  // all zeros
            return aes;
        }

        [Fact]
        public void Encrypt_Null_ReturnsNull()
        {
            using (var aes = NewAes())
            {
                Assert.Null(nullString.EncryptString(aes));
            }
        }

        [Fact]
        public void Decrypt_Null_ReturnsNull()
        {
            using (var aes = NewAes())
            {
                Assert.Null(nullString.DecryptString(aes));
            }
        }

        [Fact]
        public void RoundTrip_Encrypt_Decrypt()
        {
            const string plain = "Hello World!";
            using (var aes = NewAes())
            {
                var enc = plain.EncryptString(aes);
                Assert.NotNull(enc);
                Assert.NotEqual(plain, enc);
                var dec = enc.DecryptString(aes);
                Assert.Equal(plain, dec);
            }
        }

        [Fact]
        public void Encrypt_ProducesBase64()
        {
            using (var aes = NewAes())
            {
                var enc = "Data".EncryptString(aes);
                Assert.NotNull(enc);
                // Validate Base64 by trying to convert
                var bytes = Convert.FromBase64String(enc);
                Assert.True(bytes.Length > 0);
            }
        }

        [Fact]
        public void Decrypt_InvalidBase64_Throws()
        {
            using (var aes = NewAes())
            {
                Assert.Throws<FormatException>(() => "%%%".DecryptString(aes));
            }
        }

        [Fact]
        public void Deterministic_WithSameKeyAndIv()
        {
            using (var aes = NewAes())
            {
                var c1 = "Same".EncryptString(aes);
                var c2 = "Same".EncryptString(aes);
                Assert.Equal(c1, c2); // same IV yields same ciphertext
            }
        }
    }
}
