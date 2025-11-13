using System.Collections.Generic;
using KeePassHttp.Attributes;
using Newtonsoft.Json;

namespace KeePassHttp.Model.Request
{
    public sealed class GetLoginsByNamesRequest : BaseRequest
    {
        [JsonProperty, Required]
        public List<string> Names { get; set; }
    }
}
