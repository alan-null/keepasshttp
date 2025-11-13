using KeePassHttp.Attributes;
using Newtonsoft.Json;

namespace KeePassHttp.Model.Request
{
    public sealed class GetLoginsCountRequest : BaseRequest
    {
        [JsonProperty, Required]
        public string Url { get; set; }
    }
}
