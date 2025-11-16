using Newtonsoft.Json;
using System.Collections.Generic;

namespace KeePassHttp.Model.Request
{
    public sealed class SetLoginRequest : BaseRequest
    {
        public SetLoginRequest()
        {
            RequestType = RequestTypes.SET_LOGIN;
            StringFields = new Dictionary<string, string>();
        }

        [JsonProperty]
        public string Login { get; set; }

        [JsonProperty]
        public string Password { get; set; }

        [JsonProperty]
        public string Url { get; set; }

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string Uuid { get; set; }

        [JsonProperty]
        public string SubmitUrl { get; set; }

        /// <summary>
        /// Realm value used for filtering results.  Always encrypted.
        /// </summary>
        [JsonProperty]
        public string Realm { get; set; }

        [JsonProperty]
        public Dictionary<string,string> StringFields { get; set; }
    }
}
