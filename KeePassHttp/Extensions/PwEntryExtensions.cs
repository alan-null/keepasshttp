using KeePassHttp.Model.Entry;
using KeePassLib;
using KeePassLib.Security;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KeePassHttp.Extensions
{
    internal static class PwEntryExtensions
    {
        private static readonly JsonSerializerSettings DefaultSerializerSettings = new JsonSerializerSettings
        {
            DefaultValueHandling = DefaultValueHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore
        };

        internal static JsonSerializer NewJsonSerializer()
        {
            return JsonSerializer.Create(DefaultSerializerSettings);
        }

        private static bool UpdateSet(ref HashSet<string> target, IEnumerable<string> source)
        {
            if (source == null)
            {
                return false;
            }
            var newSet = new HashSet<string>(source);
            if (target == null || !newSet.SetEquals(target))
            {
                target = newSet;
                return true;
            }
            return false;
        }

        public static KeePassHttpEntryConfig GetEntryConfig(this PwEntry e)
        {
            if (e.Strings.Exists(Constants.KEEPASSHTTP_NAME))
            {
                var json = e.Strings.ReadSafe(Constants.KEEPASSHTTP_NAME);
                if (string.IsNullOrEmpty(json))
                {
                    return null;
                }
                using (var reader = new JsonTextReader(new StringReader(json)))
                {
                    return NewJsonSerializer().Deserialize<KeePassHttpEntryConfig>(reader);
                }
            }
            return null;
        }

        public static void SetEntryConfig(this PwEntry e, KeePassHttpEntryConfig c)
        {
            using (var writer = new StringWriter())
            {
                NewJsonSerializer().Serialize(writer, c);
                e.Strings.Set(Constants.KEEPASSHTTP_NAME, new ProtectedString(false, writer.ToString()));
            }
            e.Touch(true);
        }

        private static bool SetIfChanged(PwEntry entry, string key, string newValue)
        {
            if (newValue == null)
            {
                return false;
            }
            var existing = entry.Strings.Get(key);
            var existingValue = existing != null ? existing.ReadString() : string.Empty;
            if (existingValue == newValue)
            {
                return false;
            }
            entry.Strings.Set(key, new ProtectedString(false, newValue));
            return true;
        }

        public static bool SetEntry(this PwEntry entry, EntryModel model)
        {
            bool modified = false;

            modified |= SetIfChanged(entry, PwDefs.TitleField, model.Title);
            modified |= SetIfChanged(entry, PwDefs.UserNameField, model.Username);
            modified |= SetIfChanged(entry, PwDefs.PasswordField, model.Password);
            modified |= SetIfChanged(entry, PwDefs.UrlField, model.Url);

            bool hasConfigUpdate = model.Config != null;
            bool hasFieldUpdates = model.StringFields != null && model.StringFields.Any();

            if (hasConfigUpdate)
            {
                var config = GetEntryConfig(entry) ?? new KeePassHttpEntryConfig();
                bool configChanged = false;

                if (model.Config.Allow != null)
                {
                    if (UpdateSet(ref config.Allow, model.Config.Allow))
                    {
                        configChanged = true;
                    }
                }

                if (model.Config.Deny != null)
                {
                    if (UpdateSet(ref config.Deny, model.Config.Deny))
                    {
                        configChanged = true;
                    }
                }

                if (model.Config.Realm != null && config.Realm != model.Config.Realm)
                {
                    config.Realm = model.Config.Realm;
                    configChanged = true;
                }

                if (configChanged)
                {
                    SetEntryConfig(entry, config);
                    modified = true;
                }
            }

            if (hasFieldUpdates)
            {
                foreach (var sf in model.StringFields)
                {
                    if (sf.Key == null)
                    {
                        continue;
                    }

                    if (sf.Value == null)
                    {
                        entry.Strings.Remove(sf.Key);
                        modified = true;
                    }
                    else
                    {
                        if (SetIfChanged(entry, sf.Key, sf.Value))
                        {
                            modified = true;
                        }
                    }
                }
            }

            if (modified)
            {
                entry.Touch(true);
            }
            return modified;
        }
    }
}
