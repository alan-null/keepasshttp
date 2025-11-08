using System;
using System.Globalization;
using KeePass.App.Configuration;
using KeePassHttp.Abstraction;

namespace KeePassHttp.Configuration
{
    internal sealed class EnvironmentConfigProvider : IConfigProvider
    {
        private bool _receiveCredentialNotification;
        private bool _unlockDatabaseRequest;
        private bool _specificMatchingOnly;
        private bool _alwaysAllowAccess;
        private bool _alwaysAllowUpdates;
        private bool _searchInAllOpenedDatabases;
        private bool _hideExpired;
        private bool _matchSchemes;
        private bool _returnStringFields;
        private bool _returnStringFieldsWithKphOnly;
        private bool _sortResultByUsername;
        private long _listenerPort;
        private string _listenerHost;

        private readonly DefaultConfigProvider _defaults;

        public EnvironmentConfigProvider(AceCustomConfig cfg)
        {
            _defaults = new DefaultConfigProvider(cfg);

            _receiveCredentialNotification = _defaults.ReceiveCredentialNotification;
            _unlockDatabaseRequest = _defaults.UnlockDatabaseRequest;
            _specificMatchingOnly = _defaults.SpecificMatchingOnly;
            _alwaysAllowAccess = _defaults.AlwaysAllowAccess;
            _alwaysAllowUpdates = _defaults.AlwaysAllowUpdates;
            _searchInAllOpenedDatabases = _defaults.SearchInAllOpenedDatabases;
            _hideExpired = _defaults.HideExpired;
            _matchSchemes = _defaults.MatchSchemes;
            _returnStringFields = _defaults.ReturnStringFields;
            _returnStringFieldsWithKphOnly = _defaults.ReturnStringFieldsWithKphOnly;
            _sortResultByUsername = _defaults.SortResultByUsername;
            _listenerPort = _defaults.ListenerPort;
            _listenerHost = _defaults.ListenerHost;
        }

        public bool ReceiveCredentialNotification
        {
            get { bool v; return TryGetEnvBool("ReceiveCredentialNotification", out v) ? v : _receiveCredentialNotification; }
            set { _receiveCredentialNotification = value; }
        }

        public bool UnlockDatabaseRequest
        {
            get { bool v; return TryGetEnvBool("UnlockDatabaseRequest", out v) ? v : _unlockDatabaseRequest; }
            set { _unlockDatabaseRequest = value; }
        }

        public bool SpecificMatchingOnly
        {
            get { bool v; return TryGetEnvBool("SpecificMatchingOnly", out v) ? v : _specificMatchingOnly; }
            set { _specificMatchingOnly = value; }
        }

        public bool AlwaysAllowAccess
        {
            get { bool v; return TryGetEnvBool("AlwaysAllowAccess", out v) ? v : _alwaysAllowAccess; }
            set { _alwaysAllowAccess = value; }
        }

        public bool AlwaysAllowUpdates
        {
            get { bool v; return TryGetEnvBool("AlwaysAllowUpdates", out v) ? v : _alwaysAllowUpdates; }
            set { _alwaysAllowUpdates = value; }
        }

        public bool SearchInAllOpenedDatabases
        {
            get { bool v; return TryGetEnvBool("SearchInAllOpenedDatabases", out v) ? v : _searchInAllOpenedDatabases; }
            set { _searchInAllOpenedDatabases = value; }
        }

        public bool HideExpired
        {
            get { bool v; return TryGetEnvBool("HideExpired", out v) ? v : _hideExpired; }
            set { _hideExpired = value; }
        }

        public bool MatchSchemes
        {
            get { bool v; return TryGetEnvBool("MatchSchemes", out v) ? v : _matchSchemes; }
            set { _matchSchemes = value; }
        }

        public bool ReturnStringFields
        {
            get { bool v; return TryGetEnvBool("ReturnStringFields", out v) ? v : _returnStringFields; }
            set { _returnStringFields = value; }
        }

        public bool ReturnStringFieldsWithKphOnly
        {
            get { bool v; return TryGetEnvBool("ReturnStringFieldsWithKphOnly", out v) ? v : _returnStringFieldsWithKphOnly; }
            set { _returnStringFieldsWithKphOnly = value; }
        }

        public bool SortResultByUsername
        {
            get { bool v; return TryGetEnvBool("SortResultByUsername", out v) ? v : _sortResultByUsername; }
            set { _sortResultByUsername = value; }
        }

        public long ListenerPort
        {
            get { long l; return TryGetEnvLong("ListenerPort", out l) ? l : _listenerPort; }
            set { _listenerPort = value; }
        }

        public string ListenerHost
        {
            get { string s; return TryGetEnvString("ListenerHost", out s) ? s : _listenerHost; }
            set { _listenerHost = value; }
        }

        private static bool TryGetEnvBool(string name, out bool value)
        {
            value = false;
            var s = Environment.GetEnvironmentVariable("KPH_" + name);
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }

            s = s.Trim();
            if (s.Equals("1", StringComparison.OrdinalIgnoreCase) ||
                s.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                s.Equals("yes", StringComparison.OrdinalIgnoreCase))
            {
                value = true;
                return true;
            }
            if (s.Equals("0", StringComparison.OrdinalIgnoreCase) ||
                s.Equals("false", StringComparison.OrdinalIgnoreCase) ||
                s.Equals("no", StringComparison.OrdinalIgnoreCase))
            {
                value = false;
                return true;
            }
            return false;
        }

        private static bool TryGetEnvLong(string name, out long value)
        {
            value = 0;
            var s = Environment.GetEnvironmentVariable("KPH_" + name);
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }

            return long.TryParse(s.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out value);
        }

        private static bool TryGetEnvString(string name, out string value)
        {
            value = Environment.GetEnvironmentVariable("KPH_" + name);
            return !string.IsNullOrEmpty(value);
        }
    }
}
