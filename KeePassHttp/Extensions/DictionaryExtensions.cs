using System.Collections.Generic;
using System.Security.Cryptography;

namespace KeePassHttp.Extensions
{
    internal static class DictionaryExtensions
    {
        public static Dictionary<string, string> DecryptDictionary(this Dictionary<string, string> base64Encrypted, Aes aes)
        {
            if (base64Encrypted == null)
            {
                return null;
            }

            Dictionary<string, string> result = new Dictionary<string, string>();
            if (base64Encrypted != null && base64Encrypted.Count > 0)
            {
                foreach (var kvp in base64Encrypted)
                {
                    if (kvp.Key != null)
                    {
                        string decK = kvp.Key.DecryptString(aes);
                        string decV = kvp.Value.DecryptString(aes);
                        if (decK != null)
                        {
                            result[decK] = decV;
                        }
                    }
                }
            }
            return result;
        }
    }
}
