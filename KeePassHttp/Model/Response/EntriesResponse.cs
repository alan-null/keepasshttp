using System.Collections.Generic;

namespace KeePassHttp.Model.Response
{
    public class EntriesResponse : BaseResponse
    {
        /// <summary>
        /// The resulting entries for a get-login request
        /// </summary>
        public List<ResponseEntry> Entries { get; private set; }

        /// <summary>
        /// response to get-logins-count, number of entries for requested Url
        /// </summary>
        public int Count = 0;

        public EntriesResponse()
        {
            Entries = new List<ResponseEntry>();
        }
    }
}
