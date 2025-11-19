namespace KeePassHttp.Abstraction
{
    public interface IConfigProvider
    {
        bool ReceiveCredentialNotification { get; set; }
        bool UnlockDatabaseRequest { get; set; }
        bool SpecificMatchingOnly { get; set; }
        bool AlwaysAllowAccess { get; set; }
        bool AlwaysAllowUpdates { get; set; }
        bool SearchInAllOpenedDatabases { get; set; }
        bool HideExpired { get; set; }
        bool MatchSchemes { get; set; }
        bool ReturnStringFields { get; set; }
        bool ReturnStringFieldsWithKphOnly { get; set; }
        bool SortResultByUsername { get; set; }

        bool ActivateHttpListener { get; set; }
        long ListenerPortHttp { get; set; }
        string ListenerHostHttp { get; set; }

        bool ActivateHttpsListener { get; set; }
        long ListenerPortHttps { get; set; }
        string ListenerHostHttps { get; set; }

        bool CheckUpdates { get; set; }
    }
}
