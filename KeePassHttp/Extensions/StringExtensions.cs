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

        public static string RemoveUrlParameters(this string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return url;
            }

            Uri uri;
            if (!Uri.TryCreate(url, UriKind.Absolute, out uri))
            {
                throw new ArgumentException("Invalid URL format ", "url");
            }

            // Cut off everything starting from ? or # manually.
            int q = url.IndexOf('?');
            int h = url.IndexOf('#');

            int cutIndex;

            if (q == -1 && h == -1)
            {
                // no query or fragment → whole URL is already clean
                cutIndex = url.Length;
            }
            else if (q == -1)
            {
                cutIndex = h;     // only fragment exists
            }
            else if (h == -1)
            {
                cutIndex = q;     // only query exists
            }
            else
            {
                cutIndex = Math.Min(q, h); // both exist → cut at earliest
            }

            return url.Substring(0, cutIndex);
        }
    }
}
