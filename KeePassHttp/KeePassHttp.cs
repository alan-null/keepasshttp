using KeePass.App.Configuration;
using KeePass.Plugins;
using KeePass.UI;
using KeePass.Util.Spr;
using KeePassHttp.Abstraction;
using KeePassHttp.Configuration;
using KeePassHttp.Model.Request;
using KeePassHttp.Model.Response;
using KeePassLib;
using KeePassLib.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace KeePassHttp
{
    public enum CMode { ENCRYPT, DECRYPT }
    public sealed partial class KeePassHttpExt : Plugin
    {
        /// <summary>
        /// an arbitrarily generated uuid for the keepasshttp root entry
        /// </summary>
        public readonly byte[] KEEPASSHTTP_UUID = {
                0x34, 0x69, 0x7a, 0x40, 0x8a, 0x5b, 0x41, 0xc0,
                0x9f, 0x36, 0x89, 0x7d, 0x62, 0x3e, 0xcb, 0x31
                                                };

        private const int DEFAULT_NOTIFICATION_TIME = 5000;
        public const string KEEPASSHTTP_NAME = "KeePassHttp Settings";
        private const string KEEPASSHTTP_GROUP_NAME = "KeePassHttp Passwords";
        public const string ASSOCIATE_KEY_PREFIX = "AES Key: ";
        private IPluginHost host;
        private HttpListener listener;
        public const int DEFAULT_PORT = 19455;
        public const string DEFAULT_HOST = "localhost";
        /// <summary>
        /// TODO make configurable
        /// </summary>
        private const string HTTP_SCHEME = "http://";
        //private const string HTTPS_PREFIX = "https://localhost:";
        //private int HTTPS_PORT = DEFAULT_PORT + 1;
        private Thread httpThread;
        private volatile bool stopped = false;

        internal static Func<AceCustomConfig, IConfigProvider> ConfigProviderFactory = CreateDefaultFactory();

        /// <summary>
        /// Provider selection logic.
        /// Test Mode:
        ///   If KeePass is launched with the command line argument "--kph-ev-config"
        ///   OR environment variable KPH_EV_CONFIG is set to 1/true,
        ///   an EnvironmentConfigProvider is returned. This allows the automated Pester tests
        ///   (KeePassHttp.Tests.ps1) to supply configuration without modifying the database/UI.
        /// Normal Mode:
        ///   Falls back to DefaultConfigProvider for interactive usage.
        /// </summary>
        private static Func<AceCustomConfig, IConfigProvider> CreateDefaultFactory()
        {
            try
            {
                var args = Environment.GetCommandLineArgs();
                foreach (var a in args)
                {
                    if (a.Equals("--kph-ev-config", StringComparison.OrdinalIgnoreCase))
                    {
                        return cfg => new EnvironmentConfigProvider(cfg);
                    }
                }

                var ev = Environment.GetEnvironmentVariable("KPH_EV_CONFIG");
                if (!string.IsNullOrEmpty(ev) && (ev == "1" || ev.Equals("true", StringComparison.OrdinalIgnoreCase)))
                {
                    return cfg => new EnvironmentConfigProvider(cfg);
                }
            }
            catch { }

            return cfg => new DefaultConfigProvider(cfg);
        }

        private IConfigProvider GetConfigProvider()
        {
            return ConfigProviderFactory(host.CustomConfig);
        }

        private string _updateUrl = "https://raw.githubusercontent.com/alan-null/keepasshttp/refs/heads/master/latest-version.txt";

        public override string UpdateUrl { get { return _updateUrl; } }

        private SearchParameters MakeSearchParameters()
        {
            var p = new SearchParameters
            {
                SearchInTitles = true,
                SearchMode = PwSearchMode.Regular,
                SearchInGroupNames = false,
                SearchInNotes = false,
                SearchInOther = false,
                SearchInPasswords = false,
                SearchInTags = false,
                SearchInUrls = true,
                SearchInUserNames = false,
                SearchInUuids = false
            };
            return p;
        }

        private string CryptoTransform(string input, bool base64in, bool base64out, Aes cipher, CMode mode)
        {
            byte[] bytes;
            if (base64in)
            {
                bytes = Decode64(input);
            }
            else
            {
                bytes = Encoding.UTF8.GetBytes(input);
            }

            using (var c = mode == CMode.ENCRYPT ? cipher.CreateEncryptor() : cipher.CreateDecryptor())
            {
                var buf = c.TransformFinalBlock(bytes, 0, bytes.Length);
                return base64out ? Encode64(buf) : Encoding.UTF8.GetString(buf);
            }
        }

        private PwEntry GetConfigEntry(bool create)
        {
            var root = host.Database.RootGroup;
            var uuid = new PwUuid(KEEPASSHTTP_UUID);
            var entry = root.FindEntry(uuid, false);
            if (entry == null && create)
            {
                entry = new PwEntry(false, true)
                {
                    Uuid = uuid
                };
                entry.Strings.Set(PwDefs.TitleField, new ProtectedString(false, KEEPASSHTTP_NAME));
                root.AddEntry(entry, true);
                UpdateUI(null);
            }
            return entry;
        }

        private int GetNotificationTime()
        {
            var time = DEFAULT_NOTIFICATION_TIME;
            var entry = GetConfigEntry(false);
            if (entry != null)
            {
                var s = entry.Strings.ReadSafe("Prompt Timeout");
                if (s != null && s.Trim() != "")
                {
                    try
                    {
                        time = int.Parse(s) * 1000;
                    }
                    catch { }
                }
            }

            return time;
        }

        private void ShowNotification(string text)
        {
            ShowNotification(text, null, null);
        }

        private void ShowNotification(string text, EventHandler onclick)
        {
            ShowNotification(text, onclick, null);
        }

        private void ShowNotification(string text, EventHandler onclick, EventHandler onclose)
        {
            MethodInvoker m = delegate
            {
                var notify = host.MainWindow.MainNotifyIcon;
                if (notify == null)
                {
                    return;
                }

                EventHandler clicked = null;
                EventHandler closed = null;

                clicked = delegate
                {
                    notify.BalloonTipClicked -= clicked;
                    notify.BalloonTipClosed -= closed;
                    if (onclick != null)
                    {
                        onclick(notify, null);
                    }
                };
                closed = delegate
                {
                    notify.BalloonTipClicked -= clicked;
                    notify.BalloonTipClosed -= closed;
                    if (onclose != null)
                    {
                        onclose(notify, null);
                    }
                };

                //notify.BalloonTipIcon = ToolTipIcon.Info;
                notify.BalloonTipTitle = "KeePassHttp";
                notify.BalloonTipText = text;
                notify.ShowBalloonTip(GetNotificationTime());
                // need to add listeners after showing, or closed is sent right away
                notify.BalloonTipClosed += closed;
                notify.BalloonTipClicked += clicked;
            };
            if (host.MainWindow.InvokeRequired)
            {
                host.MainWindow.Invoke(m);
            }
            else
            {
                m.Invoke();
            }
        }

        public override bool Initialize(IPluginHost host)
        {
            var httpSupported = HttpListener.IsSupported;
            this.host = host;

            var optionsMenu = new ToolStripMenuItem("KeePassHttp Options...");
            optionsMenu.Click += OnOptions_Click;
            optionsMenu.Image = Properties.Resources.earth_lock;
            //optionsMenu.Image = global::KeePass.Properties.Resources.B16x16_File_Close;
            this.host.MainWindow.ToolsMenu.DropDownItems.Add(optionsMenu);

            if (httpSupported)
            {
                try
                {
                    listener = new HttpListener();

                    var configOpt = GetConfigProvider();

                    string httpEndpoint = HTTP_SCHEME + configOpt.ListenerHost + ":" + configOpt.ListenerPort.ToString() + "/";
                    listener.Prefixes.Add(httpEndpoint);
                    //listener.Prefixes.Add(HTTPS_PREFIX + HTTPS_PORT + "/");
                    listener.Start();

                    if (!configOpt.CheckUpdates)
                    {
                        // disable update check by pointing to local url which will return current version
                        _updateUrl = httpEndpoint + "version?format=keepass";
                    }

                    httpThread = new Thread(new ThreadStart(Run));
                    httpThread.Start();
                }
                catch (HttpListenerException e)
                {
                    MessageBox.Show(host.MainWindow,
                        "Unable to start HttpListener!\nDo you really have only one installation of KeePassHttp in your KeePass-directory?\n\n" + e,
                        "Unable to start HttpListener",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
            else
            {
                MessageBox.Show(host.MainWindow, "The .NET HttpListener is not supported on your OS",
                        ".NET HttpListener not supported",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
            }
            return httpSupported;
        }

        void OnOptions_Click(object sender, EventArgs e)
        {
            var form = new OptionsForm(new ConfigOpt(host.CustomConfig));
            UIUtil.ShowDialogAndDestroy(form);
        }

        private void Run()
        {
            while (!stopped)
            {
                try
                {
                    var r = listener.BeginGetContext(new AsyncCallback(RequestHandler), listener);
                    r.AsyncWaitHandle.WaitOne();
                    r.AsyncWaitHandle.Close();
                }
                catch (ThreadInterruptedException) { }
                catch (HttpListenerException e)
                {
                    MessageBox.Show(host.MainWindow, "Unable to process request!\n\n" + e,
                        "Unable to process request",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
        }

        private JsonSerializer NewJsonSerializer()
        {
            var settings = new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            };

            return JsonSerializer.Create(settings);
        }

        private BaseResponse ProcessRequest(BaseRequest r, HttpListenerResponse resp)
        {
            BaseResponse response = null;

            using (var aes = new AesManaged())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                try
                {
                    switch (r.RequestType)
                    {
                        case RequestTypes.TEST_ASSOCIATE:
                            response = TestAssociateHandler(r, aes);
                            break;
                        case RequestTypes.ASSOCIATE:
                            response = AssociateHandler(r, aes);
                            break;
                        case RequestTypes.GET_LOGINS:
                            response = GetLoginsHandler(r, aes);
                            break;
                        case RequestTypes.GET_LOGINS_COUNT:
                            response = GetLoginsCountHandler(r, aes);
                            break;
                        case RequestTypes.GET_LOGINS_BY_NAMES:
                            response = GetLoginsByNamesHandler(r, aes);
                            break;
                        case RequestTypes.GET_LOGIN_BY_UUID:
                            response = GetLoginByUuidHandler(r, aes);
                            break;
                        case RequestTypes.GET_ALL_LOGINS:
                            response = GetAllLoginsHandler(r, aes);
                            break;
                        case RequestTypes.SET_LOGIN:
                            response = SetLoginHandler(r, aes);
                            break;
                        case RequestTypes.GENERATE_PASSWORD:
                            response = GeneratePassword(r, aes);
                            break;
                        default:
                            response = new ErrorResponse("Unknown command: " + r.RequestType) { RequestType = r.RequestType };
                            resp.StatusCode = (int)HttpStatusCode.BadRequest;
                            break;
                    }
                }
                catch (Exception e)
                {
                    ShowNotification("***BUG*** " + e, (s, evt) => MessageBox.Show(host.MainWindow, e + ""));
                    response = new ErrorResponse(e + "") { RequestType = r.RequestType };
                    resp.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            }

            return response;
        }

        private void RequestHandler(IAsyncResult r)
        {
            try
            {
                _RequestHandler(r);
            }
            catch (Exception e)
            {
                MessageBox.Show(host.MainWindow, "RequestHandler failed: " + e);
            }
        }

        private void _RequestHandler(IAsyncResult r)
        {
            if (stopped)
            {
                return;
            }

            var l = (HttpListener)r.AsyncState;
            var ctx = l.EndGetContext(r);
            var req = ctx.Request;
            var resp = ctx.Response;

            resp.StatusCode = (int)HttpStatusCode.OK;

            if (TryHandlePathRequest(req, resp))
            {
                // Endpoint handled; no further processing.
                return;
            }

            var serializer = NewJsonSerializer();
            BaseRequest request = null;
            BaseResponse response = null;

            using (var ins = new JsonTextReader(new StreamReader(req.InputStream)))
            {
                try
                {
                    var jo = JObject.Load(ins);
                    request = BaseRequest.Factory(jo, serializer);
                    if (request != null)
                    {
                        var errors = Validation.RequestValidator.Validate(request);
                        if (errors.Count > 0)
                        {
                            resp.StatusCode = (int)HttpStatusCode.BadRequest;
                            response = new ErrorResponse("Invalid request: " + string.Join("; ", errors)) { RequestType = request.RequestType };
                        }
                    }
                }
                catch (JsonReaderException e)
                {
                    resp.StatusCode = (int)HttpStatusCode.BadRequest;
                    response = new ErrorResponse("Malformed JSON: " + e.Message);
                }
                catch (JsonSerializationException e)
                {
                    resp.StatusCode = (int)HttpStatusCode.BadRequest;
                    response = new ErrorResponse("Invalid JSON: " + e.Message);
                }
                catch (Exception e)
                {
                    resp.StatusCode = (int)HttpStatusCode.BadRequest;
                    response = new ErrorResponse("Error parsing request: " + e.Message);
                }
            }

            var db = host.Database;
            var configOpt = GetConfigProvider();

            if (response == null && request != null && (configOpt.UnlockDatabaseRequest || request.TriggerUnlock == "true") && !db.IsOpen)
            {
                host.MainWindow.Invoke((MethodInvoker)delegate { host.MainWindow.EnsureVisibleForegroundWindow(true, true); });

                // UnlockDialog not already opened
                bool bNoDialogOpened = (GlobalWindowManager.WindowCount == 0);
                if (!db.IsOpen && bNoDialogOpened)
                {
                    host.MainWindow.Invoke((MethodInvoker)delegate
                    {
                        host.MainWindow.OpenDatabase(host.MainWindow.DocumentManager.ActiveDocument.LockedIoc, null, false);
                    });
                }
            }

            if (response == null)
            {
                if (request != null && db.IsOpen)
                {
                    response = ProcessRequest(request, resp);
                }
                else
                {
                    resp.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                    response = new ErrorResponse("Database locked") { RequestType = request.RequestType };
                }
            }

            SerializeAndSendResponse(request, response, resp, serializer);
        }

        private bool TryHandlePathRequest(HttpListenerRequest req, HttpListenerResponse resp)
        {
            if (!string.Equals(req.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            var path = req.Url.AbsolutePath.TrimEnd('/').ToLowerInvariant();

            switch (path)
            {
                case "/version":
                    var format = req.QueryString["format"];
                    string payload;
                    if (string.Equals(format, "keepass", StringComparison.OrdinalIgnoreCase))
                    {
                        payload = ":\nKeePassHttp:" + GetVersion() + "\n:";
                    }
                    else
                    {
                        payload = GetVersion();
                    }
                    WritePlainText(resp, payload);
                    return true;
                default:
                    return false;
            }
        }

        private void WritePlainText(HttpListenerResponse resp, string content)
        {
            resp.ContentType = "text/plain; charset=utf-8";
            var buffer = Encoding.UTF8.GetBytes(content);
            resp.ContentLength64 = buffer.Length;
            resp.OutputStream.Write(buffer, 0, buffer.Length);
            resp.OutputStream.Close();
            resp.Close();
        }

        private void SerializeAndSendResponse(BaseRequest req, BaseResponse response, HttpListenerResponse resp, JsonSerializer serializer)
        {
            if (response == null)
            {
                resp.StatusCode = (int)HttpStatusCode.InternalServerError;
                response = new ErrorResponse("Internal error");
            }

            // common metadata
            if (host.Database != null && host.Database.RootGroup != null)
            {
                string hash = host.Database.RootGroup.Uuid.ToHexString() + host.Database.RecycleBinUuid.ToHexString();
                response.Hash = GetSHA1(hash);
            }

            if (req != null && string.IsNullOrEmpty(response.RequestType))
            {
                response.RequestType = req.RequestType;
            }

            if (string.IsNullOrEmpty(response.Version))
            {
                response.Version = GetVersion();
            }

            resp.ContentType = "application/json";
            var writer = new StringWriter();
            serializer.Serialize(writer, response);
            var buffer = Encoding.UTF8.GetBytes(writer.ToString());
            resp.ContentLength64 = buffer.Length;
            resp.OutputStream.Write(buffer, 0, buffer.Length);
            resp.OutputStream.Close();
            resp.Close();
        }

        private static string GetVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fvi.ProductVersion;
        }

        public override void Terminate()
        {
            stopped = true;
            listener.Stop();
            listener.Close();
            httpThread.Interrupt();
        }

        private void UpdateUI(PwGroup group)
        {
            var win = host.MainWindow;
            if (group == null)
            {
                group = host.Database.RootGroup;
            }

            var f = (MethodInvoker)delegate
            {
                win.UpdateUI(false, null, true, group, true, null, true);
            };
            if (win.InvokeRequired)
            {
                win.Invoke(f);
            }
            else
            {
                f.Invoke();
            }
        }

        internal string[] GetUserPass(PwEntry entry)
        {
            return GetUserPass(new PwEntryDatabase(entry, host.Database));
        }

        internal string[] GetUserPass(PwEntryDatabase entryDatabase)
        {
            // follow references
            SprContext ctx = new SprContext(entryDatabase.Entry, entryDatabase.Database,
                    SprCompileFlags.All, false, false);

            return GetUserPass(entryDatabase, ctx);
        }

        internal string[] GetUserPass(PwEntryDatabase entryDatabase, SprContext ctx)
        {
            // follow references
            string user = SprEngine.Compile(
                    entryDatabase.Entry.Strings.ReadSafe(PwDefs.UserNameField), ctx);
            string pass = SprEngine.Compile(
                    entryDatabase.Entry.Strings.ReadSafe(PwDefs.PasswordField), ctx);
            var f = (MethodInvoker)delegate
            {
                // apparently, SprEngine.Compile might modify the database
                host.MainWindow.UpdateUI(false, null, false, null, false, null, false);
            };
            if (host.MainWindow.InvokeRequired)
            {
                host.MainWindow.Invoke(f);
            }
            else
            {
                f.Invoke();
            }

            return new string[] { user, pass };
        }

        /// <summary>
        /// Liefert den SHA1 Hash
        /// </summary>
        /// <param name="input">Eingabestring</param>
        /// <returns>SHA1 Hash der Eingabestrings</returns>
        private string GetSHA1(string input)
        {
            //Umwandlung des Eingastring in den SHA1 Hash
            System.Security.Cryptography.SHA1 sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            byte[] textToHash = Encoding.Default.GetBytes(input);
            byte[] result = sha1.ComputeHash(textToHash);

            //SHA1 Hash in String konvertieren
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in result)
            {
                s.Append(b.ToString("x2").ToLower());
            }

            return s.ToString();
        }
    }
}
