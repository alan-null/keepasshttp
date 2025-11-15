namespace KeePassHttp.Model.Response
{
    public class ErrorResponse : BaseResponse
    {
        public ErrorResponse() { }
        public ErrorResponse(string error)
        {
            Error = error;
            Success = false;
        }
        public string Error = null;
    }
}
