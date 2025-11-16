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

        public AssociateRequest()
        {
            // backward compatibility
            // if 'Id' is missing or empty, set it to "UndefinedKeyPlaceholder" to match previous behavior and pass validation ('Required' attribute)
            if (string.IsNullOrWhiteSpace(Id))
            {
                Id = Constants.UndefinedKeyPlaceholder;
            }
        }
    }
}
