namespace KeePassHttp.Model.Response
{
    public class GetLoginsCountResponse : BaseResponse
    {
        /// <summary>
        /// response to get-logins-count, number of entries for requested Url
        /// </summary>
        public int Count = 0;
    }
}
