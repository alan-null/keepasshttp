using System.Security.Cryptography;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;

using KeePass.Plugins;
using KeePassLib.Collections;
using KeePassLib.Security;
using KeePassLib.Utility;
using KeePassLib;

using Newtonsoft.Json;
using KeePass.UI;
using KeePass;
using KeePassLib.Cryptography.PasswordGenerator;
using KeePassLib.Cryptography;
using KeePass.Util.Spr;
using KeePassHttp.Abstraction;
using KeePassHttp.Extensions;
using KeePassHttp.Model.Request;
using KeePassHttp.Model;
using KeePassHttp.Model.Response;

namespace KeePassHttp
{
    public sealed partial class KeePassHttpExt : Plugin
    {
        private string GetHost(string uri)
        {
            var host = uri;
            try
            {
                var url = new Uri(uri);
                host = url.Host;

                if (!url.IsDefaultPort)
                {
                    host += ":" + url.Port.ToString();
                }
            }
            catch
            {
                // ignore exception, not a URI, assume input is host
            }
            return host;
        }

        private string GetScheme(string uri)
        {
            var scheme = "";
            try
            {
                var url = new Uri(uri);
                scheme = url.Scheme;
            }
            catch
            {
                // ignore exception, not a URI, assume input is host
            }
            return scheme;
        }

        private List<PwDatabase> GetDatabases(IConfigProvider configOpt)
        {
            var listDatabases = new List<PwDatabase>();
            if (configOpt.SearchInAllOpenedDatabases)
            {
                foreach (PwDocument doc in host.MainWindow.DocumentManager.Documents)
                {
                    if (doc.Database.IsOpen)
                    {
                        listDatabases.Add(doc.Database);
                    }
                }
            }
            else
            {
                listDatabases.Add(host.Database);
            }

            return listDatabases;
        }

        private BaseResponse GetAllLoginsHandler(BaseRequest br, Aes aes)
        {
            if (!VerifyRequest(br, aes))
            {
                return new ErrorResponse { RequestType = RequestTypes.GET_ALL_LOGINS, Error = "Couldn't verify request" };
            }

            var resp = new GetAllLoginsResponse();
            var r = br as GetAllLoginsRequest;

            var list = host.Database.RootGroup.GetEntries(true);
            var config = GetConfigProvider();

            CompleteGetLoginsResult(list.Select(p => new PwEntryDatabase(p, host.Database)), config, resp, r.Id, null, aes);
            return resp;
        }

        private List<PwEntryDatabase> FindMatchingEntries(EntryQuery r, Aes aes)
        {
            if (r == null)
            {
                return new List<PwEntryDatabase>();
            }

            string submitHost = null;
            string realm = null;

            var url = r.Url.DecryptString(aes);
            var formHost = GetHost(url);
            var hostScheme = GetScheme(url);

            if (r.SubmitUrl != null)
            {
                submitHost = GetHost(r.SubmitUrl.DecryptString(aes));
            }

            if (r.Realm != null)
            {
                realm = r.Realm.DecryptString(aes);
            }

            var parms = MakeSearchParameters();
            var configOpt = GetConfigProvider();

            var candidates = new List<PwEntryDatabase>();
            string origSearchHost = formHost;

            foreach (PwDatabase db in GetDatabases(configOpt))
            {
                string searchHost = origSearchHost;
                int before = candidates.Count;
                //get all possible entries for given host-name
                while (candidates.Count == before && (origSearchHost == searchHost || searchHost.IndexOf('.') != -1))
                {
                    parms.SearchString = string.Format("^{0}$|/{0}/?", searchHost);
                    var found = new PwObjectList<PwEntry>();
                    db.RootGroup.SearchEntries(parms, found);
                    foreach (var e in found)
                    {
                        candidates.Add(new PwEntryDatabase(e, db));
                    }

                    int dot = searchHost.IndexOf('.');
                    if (dot == -1)
                    {
                        break;
                    }

                    searchHost = searchHost.Substring(dot + 1);
                }
            }

            var filtered = new List<PwEntryDatabase>(candidates.Count);
            DateTime nowUtc = DateTime.UtcNow;

            foreach (var ed in candidates)
            {
                var entry = ed.Entry;
                var title = entry.Strings.ReadSafe(PwDefs.TitleField) ?? "";
                var entryUrl = entry.Strings.ReadSafe(PwDefs.UrlField);
                var cfg = GetEntryConfig(entry);

                // 1. Expiry validation
                if (IsExpired(entry, configOpt, nowUtc))
                {
                    continue;
                }

                // 2. Config deny / realm mismatch
                if (IsDeniedByConfig(cfg, formHost, submitHost))
                {
                    continue;
                }

                if (RealmMismatch(cfg, realm))
                {
                    continue;
                }

                // 3. Explicit allow
                if (ConfigExplicitAllow(cfg, formHost, submitHost))
                {
                    if (configOpt.MatchSchemes && !SchemeMatches(hostScheme, entryUrl, title))
                    {
                        continue;
                    }

                    filtered.Add(ed);
                    continue;
                }

                // 4. Host match
                if (!HostMatches(formHost, title, entryUrl))
                {
                    continue;
                }

                // 5. Scheme (optional)
                if (configOpt.MatchSchemes && !SchemeMatches(hostScheme, entryUrl, title))
                {
                    continue;
                }

                filtered.Add(ed);
            }

            return filtered;
        }

        private static readonly string[] _UriPrefixes = { "http://", "https://", "ftp://", "sftp://" };

        private static bool IsLikelyUri(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }

            foreach (var p in _UriPrefixes)
            {
                if (s.StartsWith(p, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        private bool SchemeMatches(string hostScheme, string entryUrl, string title)
        {
            if (!string.IsNullOrEmpty(entryUrl) && GetScheme(entryUrl) == hostScheme)
            {
                return true;
            }

            return GetScheme(title) == hostScheme;
        }

        private static bool IsExpired(PwEntry e, IConfigProvider cfg, DateTime nowUtc)
        {
            return cfg.HideExpired && e.Expires && e.ExpiryTime <= nowUtc;
        }
        private static bool IsDeniedByConfig(KeePassHttpEntryConfig cfg, string formHost, string submitHost)
        {
            if (cfg == null)
            {
                return false;
            }

            if (cfg.Deny.Contains(formHost))
            {
                return true;
            }

            if (submitHost != null && cfg.Deny.Contains(submitHost))
            {
                return true;
            }

            return false;
        }
        private static bool RealmMismatch(KeePassHttpEntryConfig cfg, string realm)
        {
            return realm != null && cfg != null && cfg.Realm != realm;
        }
        private static bool ConfigExplicitAllow(KeePassHttpEntryConfig cfg, string formHost, string submitHost)
        {
            if (cfg == null)
            {
                return false;
            }

            if (!cfg.Allow.Contains(formHost))
            {
                return false;
            }

            if (submitHost == null)
            {
                return true;
            }

            return cfg.Allow.Contains(submitHost);
        }

        private static bool HostMatches(string formHost, string title, string entryUrl)
        {
            if (string.IsNullOrEmpty(formHost))
            {
                return false;
            }

            return CheckMatch(entryUrl, formHost) || CheckMatch(title, formHost);
        }

        private static bool CheckMatch(string input, string formHost)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            if (IsLikelyUri(input))
            {
                var host = GetStaticHost(input);
                if (!string.IsNullOrEmpty(host) && formHost.EndsWith(host))
                {
                    return true;
                }
            }

            return formHost.Contains(input);
        }

        private static string GetStaticHost(string uri)
        {
            try
            {
                var u = new Uri(uri);
                return u.Host;
            }
            catch { return ""; }
        }

        private BaseResponse GetLoginsCountHandler(BaseRequest br, Aes aes)
        {
            if (!VerifyRequest(br, aes))
            {
                return new ErrorResponse { RequestType = RequestTypes.GET_LOGINS_COUNT, Error = "Couldn't verify request" };
            }

            var resp = new GetLoginsCountResponse();
            var r = br as GetLoginsCountRequest;

            resp.Success = true;
            resp.Id = r.Id;
            var items = FindMatchingEntries(new EntryQuery { Url = r.Url }, aes);
            SetResponseVerifier(resp, aes);
            resp.Count = items.Count;
            return resp;
        }

        private BaseResponse GetLoginsHandler(BaseRequest br, Aes aes)
        {

            if (!VerifyRequest(br, aes))
            {
                return new ErrorResponse { RequestType = RequestTypes.GET_LOGINS, Error = "Couldn't verify request" };
            }

            var resp = new GetLoginsResponse();
            var r = br as GetLoginsRequest;

            string submitHost = null;
            var host = GetHost(r.Url.DecryptString(aes));
            if (r.SubmitUrl != null)
            {
                submitHost = GetHost(r.SubmitUrl.DecryptString(aes));
            }

            var itemsList = FindMatchingEntries(new EntryQuery { SubmitUrl = r.SubmitUrl, Url = r.Url, Realm = r.Realm }, aes);
            if (itemsList.Count == 0)
            {
                resp.Success = true;
                resp.Id = r.Id;
                SetResponseVerifier(resp, aes);
                return resp;
            }

            var configOpt = GetConfigProvider();
            var config = GetConfigEntry(true);
            bool autoAllow = !string.IsNullOrWhiteSpace(config.Strings.ReadSafe("Auto Allow")) || configOpt.AlwaysAllowAccess;

            var needPrompting = new List<PwEntryDatabase>();
            if (!autoAllow)
            {
                foreach (var ed in itemsList)
                {
                    var e = ed.Entry;
                    var c = GetEntryConfig(e);
                    var title = e.Strings.ReadSafe(PwDefs.TitleField);
                    var entryUrl = e.Strings.ReadSafe(PwDefs.UrlField);
                    bool requires;
                    if (c != null)
                    {
                        requires = (title != host && entryUrl != host && !c.Allow.Contains(host)) ||
                                   (submitHost != null && !c.Allow.Contains(submitHost) &&
                                    submitHost != title && submitHost != entryUrl);
                    }
                    else
                    {
                        requires = (title != host && entryUrl != host) ||
                                   (submitHost != null &&
                                   submitHost != title && submitHost != entryUrl);
                    }
                    if (requires)
                    {
                        needPrompting.Add(ed);
                    }
                }
            }

            if (needPrompting.Count > 0)
            {
                var win = this.host.MainWindow;
                using (var f = new AccessControlForm())
                {
                    win.Invoke((MethodInvoker)delegate
                    {
                        f.Icon = win.Icon;
                        f.Plugin = this;
                        f.Entries = needPrompting.Select(e => e.Entry).ToList();
                        f.Host = submitHost ?? host;
                        f.Load += delegate { f.Activate(); };
                        f.ShowDialog(win);

                        if (f.Remember && (f.Allowed || f.Denied))
                        {
                            foreach (var ed in needPrompting)
                            {
                                var cfg = GetEntryConfig(ed.Entry) ?? new KeePassHttpEntryConfig();
                                var set = f.Allowed ? cfg.Allow : cfg.Deny;
                                set.Add(host);
                                if (submitHost != null && submitHost != host)
                                {
                                    set.Add(submitHost);
                                }

                                SetEntryConfig(ed.Entry, cfg);
                            }
                        }

                        if (!f.Allowed)
                        {
                            var denySet = new HashSet<PwEntryDatabase>(needPrompting);
                            itemsList.RemoveAll(ed => denySet.Contains(ed));
                        }
                    });
                }
            }

            if (itemsList.Count == 0)
            {
                resp.Success = true;
                resp.Id = r.Id;
                SetResponseVerifier(resp, aes);
                return resp;
            }

            string compareToUrl = (r.SubmitUrl != null ? r.SubmitUrl.DecryptString(aes) : r.Url.DecryptString(aes)).ToLowerInvariant();

            foreach (var entryDatabase in itemsList)
            {
                var entryUrl = entryDatabase.Entry.Strings.ReadSafe(PwDefs.UrlField);
                if (string.IsNullOrEmpty(entryUrl))
                {
                    entryUrl = entryDatabase.Entry.Strings.ReadSafe(PwDefs.TitleField);
                }

                entryUrl = entryUrl.ToLowerInvariant();
                entryDatabase.MatchDistance = LevenshteinDistance(compareToUrl, entryUrl);
            }

            if (configOpt.SpecificMatchingOnly && itemsList.Count > 1)
            {
                int min = itemsList.Min(e => e.MatchDistance);
                itemsList = itemsList.Where(e => e.MatchDistance == min).ToList();
            }

            CompleteGetLoginsResult(itemsList, configOpt, resp, r.Id, host, aes);
            return resp;
        }

        private BaseResponse GetLoginsByNamesHandler(BaseRequest br, Aes aes)
        {
            if (!VerifyRequest(br, aes))
            {
                return new ErrorResponse { RequestType = RequestTypes.GET_LOGINS_BY_NAMES, Error = "Couldn't verify request" };
            }

            var resp = new GetLoginsByNameResponse();
            var r = br as GetLoginsByNamesRequest;

            var decryptedNames = new HashSet<string>();
            foreach (string name in r.Names.Where(n => n != null))
            {
                decryptedNames.Add(name.DecryptString(aes));
            }

            var listEntries = new List<PwEntryDatabase>();
            var configOpt = GetConfigProvider();
            if (decryptedNames.Count != 0)
            {
                foreach (PwDatabase db in GetDatabases(configOpt))
                {
                    foreach (var pwEntry in db.RootGroup.GetEntries(true))
                    {
                        var title = pwEntry.Strings.ReadSafe(PwDefs.TitleField);
                        if (title != null && decryptedNames.Contains(title))
                        {
                            listEntries.Add(new PwEntryDatabase(pwEntry, db));
                        }
                    }
                }
            }

            CompleteGetLoginsResult(listEntries, configOpt, resp, r.Id, null, aes);
            return resp;
        }

        private BaseResponse GetLoginByUuidHandler(BaseRequest br, Aes aes)
        {
            if (!VerifyRequest(br, aes))
            {
                return new ErrorResponse { RequestType = RequestTypes.GET_LOGIN_BY_UUID, Error = "Couldn't verify request" };
            }

            var response = new GetLoginByUuidResponse();
            var request = br as GetLoginByUuidRequest;

            var uuid = new PwUuid(MemUtil.HexStringToByteArray(request.Uuid.DecryptString(aes)));

            var listEntries = new List<PwEntryDatabase>();
            var configOpt = GetConfigProvider();

            foreach (PwDatabase db in GetDatabases(configOpt))
            {
                var entry = db.RootGroup.FindEntry(uuid, true);
                if (entry != null)
                {
                    listEntries.Add(new PwEntryDatabase(entry, db));
                    break;
                }
            }

            CompleteGetLoginsResult(listEntries, configOpt, response, request.Id, null, aes);
            return response;
        }

        private void CompleteGetLoginsResult(IEnumerable<PwEntryDatabase> itemsList, IConfigProvider configOpt, EntriesResponse resp, string rId, string host, Aes aes)
        {
            var paired = itemsList.Select(ed => new { ed.MatchDistance, Resp = PrepareElementForResponseEntries(configOpt, ed) }).ToList();

            List<ResponseEntry> newEntries;
            if (configOpt.SortResultByUsername)
            {
                newEntries = paired.OrderBy(p => p.MatchDistance).ThenBy(p => p.Resp.Login, StringComparer.OrdinalIgnoreCase).Select(p => p.Resp).ToList();
            }
            else
            {
                newEntries = paired.OrderBy(p => p.MatchDistance).ThenBy(p => p.Resp.Name, StringComparer.OrdinalIgnoreCase).Select(p => p.Resp).ToList();
            }

            if (newEntries.Count > 0)
            {
                resp.Entries.AddRange(newEntries);

                if (configOpt.ReceiveCredentialNotification)
                {
                    var names = (from e in newEntries select e.Name).Distinct();
                    var n = string.Join("\n    ", names);

                    string recipientLabel = host == null ? rId : string.Format("{0}: {1}", rId, host);
                    ShowNotification(string.Format("'{0}' is receiving credentials for:\n    {1}", recipientLabel, n));
                }
            }

            resp.Success = true;
            resp.Id = rId;
            SetResponseVerifier(resp, aes);

            EncryptResponseEntries(newEntries, aes);

            resp.Count = resp.Entries.Count;
        }

        //http://en.wikibooks.org/wiki/Algorithm_Implementation/Strings/Levenshtein_distance#C.23
        private int LevenshteinDistance(string source, string target)
        {
            if (string.IsNullOrEmpty(source))
            {
                if (string.IsNullOrEmpty(target))
                {
                    return 0;
                }

                return target.Length;
            }
            if (string.IsNullOrEmpty(target))
            {
                return source.Length;
            }

            if (source.Length > target.Length)
            {
                var temp = target;
                target = source;
                source = temp;
            }

            var m = target.Length;
            var n = source.Length;
            var distance = new int[2, m + 1];
            // Initialize the distance 'matrix'
            for (var j = 1; j <= m; j++)
            {
                distance[0, j] = j;
            }

            var currentRow = 0;
            for (var i = 1; i <= n; ++i)
            {
                currentRow = i & 1;
                distance[currentRow, 0] = i;
                var previousRow = currentRow ^ 1;
                for (var j = 1; j <= m; j++)
                {
                    var cost = target[j - 1] == source[i - 1] ? 0 : 1;
                    distance[currentRow, j] = Math.Min(Math.Min(
                                            distance[previousRow, j] + 1,
                                            distance[currentRow, j - 1] + 1),
                                            distance[previousRow, j - 1] + cost);
                }
            }
            return distance[currentRow, m];
        }

        private ResponseEntry PrepareElementForResponseEntries(IConfigProvider configOpt, PwEntryDatabase entryDatabase)
        {
            SprContext ctx = new SprContext(entryDatabase.Entry, entryDatabase.Database, SprCompileFlags.All, false, false);

            var name = entryDatabase.Entry.Strings.ReadSafe(PwDefs.TitleField);
            var loginpass = GetUserPass(entryDatabase, ctx);
            var login = loginpass[0];
            var passwd = loginpass[1];
            var uuid = entryDatabase.Entry.Uuid.ToHexString();

            List<ResponseStringField> fields = null;
            if (configOpt.ReturnStringFields)
            {
                fields = new List<ResponseStringField>();
                foreach (var sf in entryDatabase.Entry.Strings)
                {

                    // follow references
                    var sfValue = SprEngine.Compile(entryDatabase.Entry.Strings.ReadSafe(sf.Key), ctx);
                    if (configOpt.ReturnStringFieldsWithKphOnly)
                    {
                        if (sf.Key.StartsWith("KPH: "))
                        {
                            fields.Add(new ResponseStringField(sf.Key.Substring(5), sfValue));
                        }
                    }
                    else
                    {
                        fields.Add(new ResponseStringField(sf.Key, sfValue));
                    }
                }

                if (fields.Count == 0)
                {
                    fields = null;
                }
                else
                {
                    fields = fields.OrderBy(f => f.Key, StringComparer.OrdinalIgnoreCase).ToList();
                }
            }

            return new ResponseEntry(name, login, passwd, uuid, fields, entryDatabase.Entry.ParentGroup);
        }

        private void EncryptResponseEntries(IEnumerable<ResponseEntry> entries, Aes aes)
        {
            if (entries == null)
            {
                return;
            }

            foreach (var entry in entries)
            {
                EncryptProperty(ref entry.Name, aes);
                EncryptProperty(ref entry.Login, aes);
                EncryptProperty(ref entry.Uuid, aes);
                EncryptProperty(ref entry.Password, aes);

                if (entry.StringFields != null)
                {
                    foreach (var sf in entry.StringFields)
                    {
                        EncryptProperty(ref sf.Key, aes);
                        EncryptProperty(ref sf.Value, aes);
                    }
                }

                if (entry.Group != null)
                {
                    EncryptProperty(ref entry.Group.Name, aes);
                    EncryptProperty(ref entry.Group.Uuid, aes);
                }
            }
        }

        private void EncryptProperty(ref string property, Aes aes)
        {
            property = property.EncryptString(aes);
        }

        private BaseResponse SetLoginHandler(BaseRequest br, Aes aes)
        {
            if (!VerifyRequest(br, aes))
            {
                return new ErrorResponse { RequestType = RequestTypes.SET_LOGIN, Error = "Couldn't verify request" };
            }

            var resp = new SetLoginResponse();
            var r = br as SetLoginRequest;

            string url = r.Url.DecryptString(aes);
            var urlHost = GetHost(url);
            PwUuid uuid = null;
            string username = r.Login.DecryptString(aes);
            string password = r.Password.DecryptString(aes);
            if (r.Uuid != null)
            {
                uuid = new PwUuid(MemUtil.HexStringToByteArray(r.Uuid.DecryptString(aes)));
            }

            if (uuid != null)
            {
                // modify existing entry
                resp.Success = UpdateEntry(uuid, username, password, urlHost, r.Id);
            }
            else
            {
                // create new entry
                var model = new CreateEntryModel { Username = username, Password = password, Url = url, Title = urlHost, Realm = r.Realm, SubmitUrl = r.SubmitUrl };
                resp.Success = CreateEntry(model, aes);
            }

            resp.Id = r.Id;
            SetResponseVerifier(resp, aes);
            return resp;
        }

        private BaseResponse AssociateHandler(BaseRequest br, Aes aes)
        {
            var resp = new AssociateResponse();

            var r = br as AssociateRequest;

            if (!TestRequestVerifier(r, aes, r.Key))
            {
                return resp;
            }

            // key is good, prompt user to save
            using (var f = new ConfirmAssociationForm())
            {
                var win = host.MainWindow;
                win.Invoke((MethodInvoker)delegate
                {
                    f.Activate();
                    f.Icon = win.Icon;
                    f.Key = r.Key;
                    f.KeyNameText = r.Id.Equals(Constants.UndefinedKeyPlaceholder) ? string.Empty : r.Id;
                    f.Load += delegate { f.Activate(); };
                    f.ShowDialog(win);

                    if (f.KeyId != null)
                    {
                        var entry = GetConfigEntry(true);

                        bool keyNameExists = true;
                        while (keyNameExists)
                        {
                            DialogResult keyExistsResult = DialogResult.Yes;
                            foreach (var s in entry.Strings)
                            {
                                if (s.Key == ASSOCIATE_KEY_PREFIX + f.KeyId)
                                {
                                    keyExistsResult = MessageBox.Show(
                                        win,
                                        "A shared encryption-key with the name \"" + f.KeyId + "\" already exists.\nDo you want to overwrite it?",
                                        "Overwrite existing key?",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Warning,
                                        MessageBoxDefaultButton.Button1
                                    );
                                    break;
                                }
                            }

                            if (keyExistsResult == DialogResult.No)
                            {
                                f.ShowDialog(win);
                            }
                            else
                            {
                                keyNameExists = false;
                            }
                        }

                        if (f.KeyId != null)
                        {
                            entry.Strings.Set(ASSOCIATE_KEY_PREFIX + f.KeyId, new ProtectedString(true, r.Key));
                            entry.Touch(true);
                            resp.Id = f.KeyId;
                            resp.Success = true;
                            SetResponseVerifier(resp, aes);
                            UpdateUI(null);
                        }
                    }
                });
            }

            return resp;
        }

        private BaseResponse TestAssociateHandler(BaseRequest br, Aes aes)
        {
            if (!VerifyRequest(br, aes))
            {
                // invalid request - backwards compatibility: if Id is undefined
                var error = "Couldn't verify request";
                if (br.Id.Equals(Constants.UndefinedKeyPlaceholder))
                {
                    error = "Missing key identifier (Id)";
                }
                return new ErrorResponse { RequestType = RequestTypes.TEST_ASSOCIATE, Error = error };
            }

            var r = br as TestAssociateRequest;
            var resp = new TestAssociateResponse();
            resp.Success = true;
            resp.Id = r.Id;
            SetResponseVerifier(resp, aes);
            return resp;
        }

        private BaseResponse GeneratePassword(BaseRequest br, Aes aes)
        {
            if (!VerifyRequest(br, aes))
            {
                return new ErrorResponse { RequestType = RequestTypes.GENERATE_PASSWORD, Error = "Couldn't verify request" };
            }

            var resp = new GeneratePasswordResponse();
            var r = br as GeneratePasswordRequest;

            byte[] pbEntropy = null;
            ProtectedString psNew;
            PwProfile autoProfile = Program.Config.PasswordGenerator.AutoGeneratedPasswordsProfile;
            PwGenerator.Generate(out psNew, autoProfile, pbEntropy, Program.PwGeneratorPool);

            byte[] pbNew = psNew.ReadUtf8();
            if (pbNew != null)
            {
                uint uBits = QualityEstimation.EstimatePasswordBits(pbNew);
                ResponseEntry item = new ResponseEntry(RequestTypes.GENERATE_PASSWORD, uBits.ToString(), StrUtil.Utf8.GetString(pbNew), RequestTypes.GENERATE_PASSWORD, null);
                resp.Entries.Add(item);
                resp.Success = true;
                resp.Count = 1;
                MemUtil.ZeroByteArray(pbNew);
            }

            resp.Id = r.Id;
            SetResponseVerifier(resp, aes);

            EncryptResponseEntries(resp.Entries, aes);
            return resp;
        }

        private KeePassHttpEntryConfig GetEntryConfig(PwEntry e)
        {
            var serializer = NewJsonSerializer();
            if (e.Strings.Exists(KEEPASSHTTP_NAME))
            {
                var json = e.Strings.ReadSafe(KEEPASSHTTP_NAME);
                using (var ins = new JsonTextReader(new StringReader(json)))
                {
                    return serializer.Deserialize<KeePassHttpEntryConfig>(ins);
                }
            }
            return null;
        }

        private void SetEntryConfig(PwEntry e, KeePassHttpEntryConfig c)
        {
            var serializer = NewJsonSerializer();
            var writer = new StringWriter();
            serializer.Serialize(writer, c);
            e.Strings.Set(KEEPASSHTTP_NAME, new ProtectedString(false, writer.ToString()));
            e.Touch(true);
            UpdateUI(e.ParentGroup);
        }

        private bool UpdateEntry(PwUuid uuid, string username, string password, string formHost, string requestId)
        {
            PwEntry entry = null;

            var configOpt = GetConfigProvider();
            foreach (PwDatabase db in GetDatabases(configOpt))
            {
                entry = db.RootGroup.FindEntry(uuid, true);
                if (entry != null)
                {
                    break;
                }
            }

            if (entry == null)
            {
                return false;
            }

            string[] up = GetUserPass(entry);
            var u = up[0];
            var p = up[1];

            if (u != username || p != password)
            {
                bool allowUpdate = configOpt.AlwaysAllowUpdates;

                if (!allowUpdate)
                {
                    host.MainWindow.Activate();

                    DialogResult result;
                    if (host.MainWindow.IsTrayed())
                    {
                        result = MessageBox.Show(
                            string.Format("Do you want to update the information in {0} - {1}?", formHost, u),
                            "Update Entry", MessageBoxButtons.YesNo,
                            MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    }
                    else
                    {
                        result = MessageBox.Show(
                            host.MainWindow,
                            string.Format("Do you want to update the information in {0} - {1}?", formHost, u),
                            "Update Entry", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }


                    if (result == DialogResult.Yes)
                    {
                        allowUpdate = true;
                    }
                }

                if (allowUpdate)
                {
                    PwObjectList<PwEntry> m_vHistory = entry.History.CloneDeep();
                    entry.History = m_vHistory;
                    entry.CreateBackup(null);

                    entry.Strings.Set(PwDefs.UserNameField, new ProtectedString(false, username));
                    entry.Strings.Set(PwDefs.PasswordField, new ProtectedString(true, password));
                    entry.Touch(true, false);
                    UpdateUI(entry.ParentGroup);

                    return true;
                }
            }

            return false;
        }

        private bool CreateEntry(CreateEntryModel model, Aes aes)
        {
            string realm = null;
            if (model.Realm != null)
            {
                realm = model.Realm.DecryptString(aes);
            }

            var root = host.Database.RootGroup;
            var group = root.FindCreateGroup(KEEPASSHTTP_GROUP_NAME, false);
            if (group == null)
            {
                group = new PwGroup(true, true, KEEPASSHTTP_GROUP_NAME, PwIcon.WorldComputer);
                root.AddGroup(group, true);
                UpdateUI(null);
            }
            string submithost = null;
            if (model.SubmitUrl != null)
            {
                submithost = GetHost(model.SubmitUrl.DecryptString(aes));
            }

            string baseUrl = model.Url;
            // index bigger than https:// <-- this slash
            if (baseUrl.LastIndexOf("/") > 9)
            {
                baseUrl = baseUrl.Substring(0, baseUrl.LastIndexOf("/") + 1);
            }

            PwEntry entry = new PwEntry(true, true);
            entry.Strings.Set(PwDefs.TitleField, new ProtectedString(false, model.Title));
            entry.Strings.Set(PwDefs.UserNameField, new ProtectedString(false, model.Username));
            entry.Strings.Set(PwDefs.PasswordField, new ProtectedString(true, model.Password));
            entry.Strings.Set(PwDefs.UrlField, new ProtectedString(true, baseUrl));

            if ((submithost != null && model.Title != submithost) || realm != null)
            {
                var config = new KeePassHttpEntryConfig();
                if (submithost != null)
                {
                    config.Allow.Add(submithost);
                }

                if (realm != null)
                {
                    config.Realm = realm;
                }

                var serializer = NewJsonSerializer();
                var writer = new StringWriter();
                serializer.Serialize(writer, config);
                entry.Strings.Set(KEEPASSHTTP_NAME, new ProtectedString(false, writer.ToString()));
            }

            group.AddEntry(entry, true);
            UpdateUI(group);

            return true;
        }
    }
}