using KeePassHttp.Attributes;
using Newtonsoft.Json;

namespace KeePassHttp.Model.Request
{
    public sealed class SetLoginRequest : BaseRequest
    {
        [JsonProperty, Required]
        public string Login { get; set; }

        [JsonProperty, Required]
        public string Password { get; set; }

        [JsonProperty, Required]
        public string Url { get; set; }

        [JsonProperty]
        public string Uuid { get; set; }

        [JsonProperty]
        public string SubmitUrl { get; set; }

        /// <summary>
        /// Realm value used for filtering results.  Always encrypted.
        /// </summary>
        [JsonProperty]
        public string Realm { get; set; }
    }
}
