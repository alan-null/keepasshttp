using System.Collections.Generic;

namespace KeePassHttp.Extensions
{
    internal static class ObjectExtensions
    {
        public static List<string> GetNonEmptyFields(this object model)
        {
            var fieldsToUpdate = new List<string>();
            if (model == null) return fieldsToUpdate;

            var properties = model.GetType().GetProperties();
            foreach (var property in properties)
            {
                var value = property.GetValue(model, null);

                // Try string
                var strValue = value as string;
                if (strValue != null)
                {
                    fieldsToUpdate.Add(property.Name);
                    continue;
                }

                // Try dictionary<string,string>
                var dictValue = value as Dictionary<string, string>;
                if (dictValue != null)
                {
                    if (dictValue.Count != 0)
                    {
                        var keys = string.Join(",", dictValue.Keys);
                        fieldsToUpdate.Add(string.Format("{0}({1})", property.Name, keys));
                    }
                    continue;
                }

                // Any other non-null value type or reference type
                if (value != null)
                    fieldsToUpdate.Add(property.Name);
            }
            return fieldsToUpdate;
        }
    }
}
