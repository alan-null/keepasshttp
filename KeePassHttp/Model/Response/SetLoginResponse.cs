using Newtonsoft.Json;

namespace KeePassHttp.Model.Response
{
    public class SetLoginResponse : BaseResponse
    {
        [JsonProperty]
        public string Uuid { get; set; }
    }
}
