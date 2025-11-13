using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using KeePassHttp.Attributes;

namespace KeePassHttp.Validation
{
    internal static class RequestValidator
    {
        private delegate bool AttributeCheck(object value, out string error);

        private static readonly Dictionary<Type, AttributeCheck> _validators = new Dictionary<Type, AttributeCheck>
        {
            { typeof(RequiredAttribute), RequiredCheck }
        };

        internal static List<string> Validate(object request)
        {
            var errors = new List<string>();
            if (request == null)
            {
                return errors;
            }

            Type t = request.GetType();
            PropertyInfo[] props = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var prop in props)
            {
                object[] atts = prop.GetCustomAttributes(false);
                if (atts == null || atts.Length == 0)
                {
                    continue;
                }

                object value = null;
                bool gotValue = false;
                try
                {
                    value = prop.GetValue(request, null);
                    gotValue = true;
                }
                catch { /* ignore getter exceptions */ }

                foreach (var att in atts)
                {
                    AttributeCheck check;
                    if (_validators.TryGetValue(att.GetType(), out check))
                    {
                        string error;
                        if (!check(gotValue ? value : null, out error))
                        {
                            errors.Add(prop.Name + ": " + error);
                        }
                    }
                }
            }
            return errors;
        }

        private static bool RequiredCheck(object value, out string error)
        {
            // null
            if (value == null)
            {
                error = "required (null)";
                return false;
            }

            // string
            var s = value as string;
            if (s != null)
            {
                if (s.Length == 0 || s.Trim().Length == 0)
                {
                    error = "required (empty)";
                    return false;
                }
                error = null;
                return true;
            }

            // IEnumerable (but not string which handled above)
            var enumerable = value as IEnumerable;
            if (enumerable != null)
            {
                var hasItem = false;
                var e = enumerable.GetEnumerator();
                try
                {
                    if (e.MoveNext())
                    {
                        hasItem = true;
                    }
                }
                finally
                {
                    var disp = e as IDisposable;
                    if (disp != null)
                    {
                        disp.Dispose();
                    }
                }
                if (!hasItem)
                {
                    error = "required (empty collection)";
                    return false;
                }
            }

            // ProtectedString or other types just need to be non-null.
            error = null;
            return true;
        }
    }
}
