using System.Collections.Generic;

namespace KeePassHttp.Model.Entry
{
    public class EntryModel
    {
        public string Title { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Url { get; set; }
        public KeePassHttpEntryConfig Config { get; set; }
        public Dictionary<string, string> StringFields { get; set; }
    }
}
