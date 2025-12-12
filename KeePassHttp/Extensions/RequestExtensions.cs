using KeePassHttp.Model.Request;
using KeePassLib;

namespace KeePassHttp.Extensions
{
    internal static class RequestExtensions
    {
        public static SearchParameters BuildSearchParameters(this GetLoginsCustomSearchRequest r)
        {
            var p = new SearchParameters();
            p.SearchInTitles = r.SearchInTitles;
            p.SearchInUserNames = r.SearchInUserNames;
            p.SearchInPasswords = r.SearchInPasswords;
            p.SearchInUrls = r.SearchInUrls;
            p.SearchInNotes = r.SearchInNotes;
            p.SearchInOther = r.SearchInOther;
            p.SearchInStringNames = r.SearchInStringNames;
            p.SearchInTags = r.SearchInTags;
            p.SearchInUuids = r.SearchInUuids;
            p.SearchInGroupPaths = r.SearchInGroupPaths;
            p.SearchInGroupNames = r.SearchInGroupNames;
            p.SearchInHistory = r.SearchInHistory;
            p.RespectEntrySearchingDisabled = r.RespectEntrySearchingDisabled;
            p.ExcludeExpired = r.ExcludeExpired;
            p.SearchMode = r.SearchMode;
            p.ComparisonMode = r.ComparisonMode;
            return p;
        }
    }
}
