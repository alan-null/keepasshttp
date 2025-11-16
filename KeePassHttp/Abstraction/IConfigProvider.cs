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
        long ListenerPort { get; set; }
        string ListenerHost { get; set; }
        bool CheckUpdates { get; set; }
    }
}
