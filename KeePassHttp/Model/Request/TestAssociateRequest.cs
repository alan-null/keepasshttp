namespace KeePassHttp.Model.Request
{
    public sealed class TestAssociateRequest : BaseRequest
    {
        public TestAssociateRequest()
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
