using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Xunit;
using KeePassHttp.Extensions;

namespace KeePassHttp.Tests
{
    public class DictionaryExtensionsTests
    {
        private Aes CreateAes()
        {
            Aes aes = Aes.Create();
            // For reproducibility use fixed key/IV (not for production security).
            aes.Key = new byte[32]; // 256-bit zero key
            aes.IV = new byte[16];  // 128-bit zero IV
            return aes;
        }

        private string EncryptString(string plainText, Aes aes)
        {
            // Helper mirrors assumed DecryptString implementation (AES + base64).
            if (plainText == null)
            {
                return null;
            }
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] inputBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] cipherBytes;
            using (var ms = new System.IO.MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    cs.Write(inputBytes, 0, inputBytes.Length);
                    cs.FlushFinalBlock();
                    cipherBytes = ms.ToArray();
                }
            }
            return Convert.ToBase64String(cipherBytes);
        }

        [Fact]
        public void DecryptDictionary_ReturnsNull_WhenInputIsNull()
        {
            Aes aes = CreateAes();
            Dictionary<string, string> @null = null;
            Dictionary<string, string> decrypted = @null.DecryptDictionary(aes);
            Assert.Null(decrypted);
        }

        [Fact]
        public void DecryptDictionary_ReturnsEmpty_WhenInputIsEmpty()
        {
            Aes aes = CreateAes();
            Dictionary<string, string> empty = new Dictionary<string, string>();
            Dictionary<string, string> decrypted = empty.DecryptDictionary(aes);
            Assert.NotNull(decrypted);
            Assert.True(decrypted.Count == 0);
        }

        [Fact]
        public void DecryptDictionary_ReturnsDecryptedPairs()
        {
            Aes aes = CreateAes();
            Dictionary<string, string> encrypted = new Dictionary<string, string>();
            string plainKey = "Site";
            string plainValue = "Password123";
            encrypted[EncryptString(plainKey, aes)] = EncryptString(plainValue, aes);

            Dictionary<string, string> result = encrypted.DecryptDictionary(aes);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.True(result.ContainsKey(plainKey));
            Assert.Equal(plainValue, result[plainKey]);
        }

        [Fact]
        public void DecryptDictionary_OverwritesDuplicateDecryptedKeys()
        {
            Aes aes = CreateAes();
            Dictionary<string, string> encrypted = new Dictionary<string, string>();
            string duplicateKeyPlain = "Alpha";
            encrypted[EncryptString(duplicateKeyPlain, aes)] = EncryptString("First", aes);
            encrypted[EncryptString(duplicateKeyPlain, aes)] = EncryptString("Second", aes);

            Dictionary<string, string> result = encrypted.DecryptDictionary(aes);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Second", result[duplicateKeyPlain]);
        }
    }
}
