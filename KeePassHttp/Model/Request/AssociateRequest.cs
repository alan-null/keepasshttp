using KeePassHttp.Attributes;
using Newtonsoft.Json;

namespace KeePassHttp.Model.Request
{
    //TODO base
    public sealed class AssociateRequest : BaseRequest
    {
        /// <summary>
        /// Base64 AES key
        /// </summary>
        [JsonProperty, Required]
        public string Key { get; set; }
    }
}
