using KeePassHttp.Attributes;
using Newtonsoft.Json;

namespace KeePassHttp.Model.Request
{
    public sealed class GetLoginByUuidRequest : BaseRequest
    {
        [JsonProperty, Required]
        public string Uuid { get; set; }
    }
}
