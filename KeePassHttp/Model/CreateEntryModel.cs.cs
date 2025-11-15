namespace KeePassHttp.Model
{
    public class CreateEntryModel
    {
        public string Title { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Url { get; set; }
        public string Realm { get; set; }
        public string SubmitUrl { get; set; }
    }
}
