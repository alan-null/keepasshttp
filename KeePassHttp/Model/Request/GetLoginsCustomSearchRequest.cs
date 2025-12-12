using KeePassHttp.Attributes;
using KeePassLib;
using Newtonsoft.Json;
using System;

namespace KeePassHttp.Model.Request
{
    public sealed class GetLoginsCustomSearchRequest : BaseRequest
    {

        [JsonProperty, Required]
        public string SearchString { get; set; }

        [JsonProperty]
        public bool SearchInTitles { get; set; }

        [JsonProperty]
        public bool SearchInUserNames { get; set; }

        [JsonProperty]
        public bool SearchInPasswords { get; set; }

        [JsonProperty]
        public bool SearchInUrls { get; set; }

        [JsonProperty]
        public bool SearchInNotes { get; set; }

        [JsonProperty]
        public bool SearchInOther { get; set; }

        [JsonProperty]
        public bool SearchInStringNames { get; set; }

        [JsonProperty]
        public bool SearchInTags { get; set; }

        [JsonProperty]
        public bool SearchInUuids { get; set; }

        [JsonProperty]
        public bool SearchInGroupPaths { get; set; }

        [JsonProperty]
        public bool SearchInGroupNames { get; set; }

        [JsonProperty]
        public bool SearchInHistory { get; set; }

        [JsonProperty]
        public PwSearchMode SearchMode { get; set; }

        [JsonProperty]
        public bool ExcludeExpired { get; set; }

        [JsonProperty]
        public bool RespectEntrySearchingDisabled { get; set; }

        [JsonProperty]
        public StringComparison ComparisonMode { get; set; }

        public GetLoginsCustomSearchRequest()
        {
            RequestType = RequestTypes.GET_LOGINS_CUSTOM_SEARCH;
            SearchInTitles = true;
            SearchInUserNames = true;
            SearchInPasswords = false;
            SearchInUrls = true;
            SearchInNotes = true;
            SearchInOther = true;
            SearchInStringNames = false;
            SearchInTags = true;
            SearchInUuids = false;
            SearchInGroupPaths = false;
            SearchInGroupNames = false;
            SearchInHistory = false;
            ExcludeExpired = false;
            RespectEntrySearchingDisabled = true;
            SearchMode = PwSearchMode.Simple;
            ComparisonMode = StringComparison.InvariantCultureIgnoreCase;
        }
    }
}
