using KeePassHttp.Attributes;
using Newtonsoft.Json;

namespace KeePassHttp.Model.Request
{
    public sealed class GetLoginsRequest : BaseRequest
    {
        [JsonProperty, Required]
        public string Url { get; set; }

        [JsonProperty]
        public string Realm { get; set; }

        [JsonProperty]
        public string SubmitUrl { get; set; }
    }
}
