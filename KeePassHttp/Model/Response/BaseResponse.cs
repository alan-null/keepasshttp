namespace KeePassHttp.Model.Response
{
    public abstract class BaseResponse
    {
        /// <summary>
        /// Mirrors the request type of KeePassRequest
        /// </summary>
        public string RequestType;

        public bool Success = false;

        /// <summary>
        /// The user selected string as a result of 'associate',
        /// always returned on every request
        /// </summary>
        public string Id;

        /// <summary>
        /// response the current version of KeePassHttp
        /// </summary>
        public string Version = "";

        /// <summary>
        /// response an unique hash of the database composed of RootGroup UUid and RecycleBin UUid
        /// </summary>
        public string Hash = "";

        /// <summary>
        /// Nonce value used in conjunction with all encrypted fields,
        /// randomly generated for each request
        /// </summary>
        public string Nonce;

        /// <summary>
        /// Same purpose as Request.Verifier, but a new value
        /// </summary>
        public string Verifier;
    }
}
