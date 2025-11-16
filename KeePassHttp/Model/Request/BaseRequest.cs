using KeePassHttp.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KeePassHttp.Model.Request
{
    [JsonObject(MemberSerialization.OptIn)]
    public class BaseRequest
    {
        [JsonProperty]
        public string RequestType { get; set; }

        /// <summary>
        /// Always required, an identifier given by the KeePass user
        /// </summary>
        [JsonProperty, Required]
        public string Id { get; set; }

        /// <summary>
        /// Trigger unlock of database even if feature is disabled in KPH (because of user interaction to fill-in)
        /// </summary>
        [JsonProperty]
        public string TriggerUnlock { get; set; }

        /// <summary>
        /// A value used to ensure that the correct key has been chosen,
        /// it is always the value of Nonce encrypted with Key
        /// </summary>
        [JsonProperty]
        public string Verifier { get; set; }

        /// <summary>
        /// Nonce value used in conjunction with all encrypted fields,
        /// randomly generated for each request
        /// </summary>
        [JsonProperty]
        public string Nonce { get; set; }

        internal static BaseRequest Factory(JObject jo, JsonSerializer serializer)
        {
            var type = (string)jo["RequestType"] ?? "";
            BaseRequest request;
            switch (type)
            {
                case RequestTypes.GET_LOGINS: request = new GetLoginsRequest(); break;
                case RequestTypes.GET_LOGINS_COUNT: request = new GetLoginsCountRequest(); break;
                case RequestTypes.GET_LOGINS_BY_NAMES: request = new GetLoginsByNamesRequest(); break;
                case RequestTypes.GET_LOGIN_BY_UUID: request = new GetLoginByUuidRequest(); break;
                case RequestTypes.GET_ALL_LOGINS: request = new GetAllLoginsRequest(); break;
                case RequestTypes.SET_LOGIN: request = new SetLoginRequest(); break;
                case RequestTypes.ASSOCIATE: request = new AssociateRequest(); break;
                case RequestTypes.TEST_ASSOCIATE: request = new TestAssociateRequest(); break;
                case RequestTypes.GENERATE_PASSWORD: request = new GeneratePasswordRequest(); break;
                default: request = new UnknownRequest(); break;
            }
            using (var jr = jo.CreateReader())
            {
                serializer.Populate(jr, request);
            }

            return request;
        }
    }
}
