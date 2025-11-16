namespace KeePassHttp
{
    public static class RequestTypes
    {
        public const string GET_LOGINS = "get-logins";
        public const string GET_LOGINS_COUNT = "get-logins-count";
        public const string GET_LOGINS_BY_NAMES = "get-logins-by-names";
        public const string GET_LOGIN_BY_UUID = "get-login-by-uuid";
        public const string GET_ALL_LOGINS = "get-all-logins";
        public const string SET_LOGIN = "set-login";
        public const string ASSOCIATE = "associate";
        public const string TEST_ASSOCIATE = "test-associate";
        public const string GENERATE_PASSWORD = "generate-password";
    }
}
