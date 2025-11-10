using KeePassLib;

namespace KeePassHttp
{
    class PwEntryDatabase
    {
        private readonly PwDatabase _database;
        private readonly PwEntry _entry;
        private int _matchDistance;

        public PwEntry Entry
        {
            get { return _entry; }
        }

        public PwDatabase Database
        {
            get { return _database; }
        }

        internal int MatchDistance
        {
            get { return _matchDistance; }
            set { _matchDistance = value; }
        }

        public PwEntryDatabase(PwEntry e, PwDatabase db, int matchDistance = 0)
        {
            _entry = e;
            _database = db;
            _matchDistance = matchDistance;
        }
    }
}
