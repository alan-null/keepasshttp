using KeePassLib;

namespace KeePassHttp
{
    class PwEntryDatabase
    {
        private readonly PwDatabase _database;
        private readonly PwEntry _entry;

        public PwEntry Entry
        {
            get { return _entry; }
        }

        public PwDatabase Database
        {
            get { return _database; }
        }

        public PwEntryDatabase(PwEntry e, PwDatabase db)
        {
            _entry = e;
            _database = db;
        }
    }
}
