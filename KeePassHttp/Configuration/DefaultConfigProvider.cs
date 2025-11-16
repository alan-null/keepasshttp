using KeePass.App.Configuration;
using KeePassHttp.Abstraction;

namespace KeePassHttp.Configuration
{
    internal sealed class DefaultConfigProvider : IConfigProvider
    {
        private readonly ConfigOpt _opt;
        public DefaultConfigProvider(AceCustomConfig cfg)
        {
            _opt = new ConfigOpt(cfg);
        }

        public bool ReceiveCredentialNotification { get { return _opt.ReceiveCredentialNotification; } set { _opt.ReceiveCredentialNotification = value; } }
        public bool UnlockDatabaseRequest { get { return _opt.UnlockDatabaseRequest; } set { _opt.UnlockDatabaseRequest = value; } }
        public bool SpecificMatchingOnly { get { return _opt.SpecificMatchingOnly; } set { _opt.SpecificMatchingOnly = value; } }
        public bool AlwaysAllowAccess { get { return _opt.AlwaysAllowAccess; } set { _opt.AlwaysAllowAccess = value; } }
        public bool AlwaysAllowUpdates { get { return _opt.AlwaysAllowUpdates; } set { _opt.AlwaysAllowUpdates = value; } }
        public bool SearchInAllOpenedDatabases { get { return _opt.SearchInAllOpenedDatabases; } set { _opt.SearchInAllOpenedDatabases = value; } }
        public bool HideExpired { get { return _opt.HideExpired; } set { _opt.HideExpired = value; } }
        public bool MatchSchemes { get { return _opt.MatchSchemes; } set { _opt.MatchSchemes = value; } }
        public bool ReturnStringFields { get { return _opt.ReturnStringFields; } set { _opt.ReturnStringFields = value; } }
        public bool ReturnStringFieldsWithKphOnly { get { return _opt.ReturnStringFieldsWithKphOnly; } set { _opt.ReturnStringFieldsWithKphOnly = value; } }
        public bool SortResultByUsername { get { return _opt.SortResultByUsername; } set { _opt.SortResultByUsername = value; } }
        public long ListenerPort { get { return _opt.ListenerPort; } set { _opt.ListenerPort = value; } }
        public string ListenerHost { get { return _opt.ListenerHost; } set { _opt.ListenerHost = value; } }
        public bool CheckUpdates { get { return _opt.CheckUpdates; } set { _opt.CheckUpdates = value; } }
    }
}
