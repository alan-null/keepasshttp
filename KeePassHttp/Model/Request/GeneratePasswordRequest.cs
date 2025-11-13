namespace KeePassHttp.Model.Request
{
    public sealed class GeneratePasswordRequest : BaseRequest
    {
        /// <summary>
        /// response to get-logins-count, number of entries for requested Url
        /// </summary>
        public int Count = 0;
    }
}
