using System;
using System.Security.Cryptography;
using System.Text;

namespace KeePassHttp.Extensions
{
    internal static class StringExtensions
    {
        public static string DecryptString(this string base64Encrypted, Aes aes)
        {
            if (base64Encrypted == null)
            {
                return null;
            }
            return CryptoTransform(base64Encrypted, true, false, aes, CMode.DECRYPT);
        }

        public static string EncryptString(this string plain, Aes aes)
        {
            if (plain == null)
            {
                return null;
            }
            return CryptoTransform(plain, false, true, aes, CMode.ENCRYPT);
        }

        private static string CryptoTransform(string input, bool base64in, bool base64out, Aes cipher, CMode mode)
        {
            byte[] bytes;
            if (base64in)
            {
                bytes = Convert.FromBase64String(input);
            }
            else
            {
                bytes = Encoding.UTF8.GetBytes(input);
            }

            using (var c = mode == CMode.ENCRYPT ? cipher.CreateEncryptor() : cipher.CreateDecryptor())
            {
                var buf = c.TransformFinalBlock(bytes, 0, bytes.Length);
                return base64out ? Convert.ToBase64String(buf) : Encoding.UTF8.GetString(buf);
            }
        } 
    }
}
