using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;
using KeePassLib;
using KeePassHttp.Extensions;
using System.Linq;
using KeePassHttp.Model.Entry;

namespace KeePassHttp.Tests
{
    public class PwEntryExtensionsTests
    {
        private static EntryModel NewModel(
            string title = null,
            string user = null,
            string pass = null,
            string url = null,
            KeePassHttpEntryConfig cfg = null,
            Dictionary<string, string> strFields = null)
        {
            return new EntryModel { Title = title, Username = user, Password = pass, Url = url, Config = cfg, StringFields = strFields };
        }

        private static PwEntry NewEntry()
        {
            return new PwEntry(true, true);
        }

        [Fact]
        public void SetEntry_BasicFields_Modifies()
        {
            var entry = NewEntry();
            var model = NewModel("T", "U", "P", "http://x");
            var modified = entry.SetEntry(model);
            Assert.True(modified);
            Assert.Equal("T", entry.Strings.ReadSafe(PwDefs.TitleField));
            Assert.Equal("U", entry.Strings.ReadSafe(PwDefs.UserNameField));
            Assert.Equal("P", entry.Strings.ReadSafe(PwDefs.PasswordField));
            Assert.Equal("http://x", entry.Strings.ReadSafe(PwDefs.UrlField));
        }

        [Fact]
        public void SetEntry_NoChanges_ReturnsFalse()
        {
            var entry = NewEntry();
            var first = NewModel("A", "B", "C", "D");
            Assert.True(entry.SetEntry(first));
            var before = entry.LastModificationTime;
            Thread.Sleep(15);
            var second = NewModel("A", "B", "C", "D");
            Assert.False(entry.SetEntry(second));
            Assert.Equal(before, entry.LastModificationTime);
        }

        [Fact]
        public void SetEntry_ConfigAllowDenyRealm_Updated()
        {
            var entry = NewEntry();
            var cfg = new KeePassHttpEntryConfig
            {
                Allow = new HashSet<string> { "a.com", "b.com" },
                Deny = new HashSet<string> { "x.com" },
                Realm = "r1"
            };
            Assert.True(entry.SetEntry(NewModel(cfg: cfg)));
            var stored = entry.GetEntryConfig();
            Assert.NotNull(stored);
            Assert.Equal("r1", stored.Realm);
            Assert.True(stored.Allow.SetEquals(cfg.Allow));
            Assert.True(stored.Deny.SetEquals(cfg.Deny));
        }

        [Fact]
        public void SetEntry_Config_NoChange_ReturnsFalse()
        {
            var entry = NewEntry();
            var cfg = new KeePassHttpEntryConfig { Realm = "r1" };
            Assert.True(entry.SetEntry(NewModel(cfg: cfg)));
            var before = entry.LastModificationTime;
            Thread.Sleep(15);
            var cfgSame = new KeePassHttpEntryConfig { Realm = "r1" };
            Assert.False(entry.SetEntry(NewModel(cfg: cfgSame)));
            Assert.Equal(before, entry.LastModificationTime);
        }

        [Fact]
        public void SetEntry_Config_PartialUpdate()
        {
            var entry = NewEntry();
            var initial = new KeePassHttpEntryConfig { Allow = new HashSet<string> { "a" }, Realm = "r1" };
            Assert.True(entry.SetEntry(NewModel(cfg: initial)));
            var update = new KeePassHttpEntryConfig { Allow = null, Deny = new HashSet<string> { "x" } }; // Allow/Realm null -> unchanged
            Assert.True(entry.SetEntry(NewModel(cfg: update)));
            var stored = entry.GetEntryConfig();
            Assert.True(stored.Allow.SetEquals(new HashSet<string> { "a" }));
            Assert.True(stored.Deny.SetEquals(new HashSet<string> { "x" }));
            Assert.Equal("r1", stored.Realm);
        }

        [Fact]
        public void SetEntry_StringFields_NullValueNotStored()
        {
            var entry = NewEntry();
            var model = NewModel(strFields: new Dictionary<string, string>{
                { "Custom1", null },
                { "Custom2", "X" }
            });
            Assert.True(entry.SetEntry(model));
            // fallback - empty string expected
            Assert.Equal(string.Empty, entry.Strings.ReadSafe("Custom1"));
            Assert.Equal(new[] { "Custom2" }, entry.Strings.GetKeys());
            Assert.Equal("X", entry.Strings.ReadSafe("Custom2"));
        }

        [Fact]
        public void SetEntry_StringFields_FieldRemoval()
        {
            var entry = NewEntry();
            var initial = new Dictionary<string, string> { { "Custom1", "Value1" }, { "Custom2", "Value2" } };
            Assert.True(entry.SetEntry(NewModel(strFields: initial)));
            var update = new Dictionary<string, string> { { "Custom1", "Value1" }, { "Custom2", null } };
            Assert.True(entry.SetEntry(NewModel(strFields: update)));
            var stored = entry.Strings;
            Assert.True(stored.Count() == 1);
            Assert.Equal(new[] { "Custom1" }, entry.Strings.GetKeys());
            Assert.Equal("Value1", stored.Get("Custom1").ReadString());
        }

        [Fact]
        public void GetEntryConfig_RoundTrip()
        {
            var entry = NewEntry();
            var cfg = new KeePassHttpEntryConfig
            {
                Allow = new HashSet<string> { "a" },
                Deny = new HashSet<string> { "d" },
                Realm = "realm"
            };
            entry.SetEntryConfig(cfg);
            var read = entry.GetEntryConfig();
            Assert.NotNull(read);
            Assert.Equal("realm", read.Realm);
            Assert.True(read.Allow.SetEquals(cfg.Allow));
            Assert.True(read.Deny.SetEquals(cfg.Deny));
        }

        [Fact]
        public void SetEntry_ReturnsTrue_WhenOnlyConfigChanges()
        {
            var entry = NewEntry();
            var first = NewModel(title: "T");
            Assert.True(entry.SetEntry(first));
            var cfgUpdate = new KeePassHttpEntryConfig { Realm = "new" };
            Assert.True(entry.SetEntry(NewModel(cfg: cfgUpdate))); // Only config change
            var stored = entry.GetEntryConfig();
            Assert.Equal("new", stored.Realm);
        }
    }
}
