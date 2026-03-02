using KeePassLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using KeePassLib.Collections;

namespace KeePassHttp
{
    public partial class OptionsForm : Form
    {
        readonly ConfigOpt _config;
        private bool _restartRequired = false;

        public OptionsForm(ConfigOpt config)
        {
            _config = config;
            InitializeComponent();
        }

        private PwEntry GetConfigEntry(PwDatabase db)
        {
            var kphe = new KeePassHttpExt();
            var root = db.RootGroup;
            var uuid = new PwUuid(kphe.KEEPASSHTTP_UUID);
            var entry = root.FindEntry(uuid, false);
            return entry;
        }

        private void OptionsForm_Load(object sender, EventArgs e)
        {
            credNotifyCheckbox.Checked = _config.ReceiveCredentialNotification;
            credMatchingCheckbox.Checked = _config.SpecificMatchingOnly;
            unlockDatabaseCheckbox.Checked = _config.UnlockDatabaseRequest;
            credAllowAccessCheckbox.Checked = _config.AlwaysAllowAccess;
            credAllowUpdatesCheckbox.Checked = _config.AlwaysAllowUpdates;
            credSearchInAllOpenedDatabases.Checked = _config.SearchInAllOpenedDatabases;
            hideExpiredCheckbox.Checked = _config.HideExpired;
            matchSchemesCheckbox.Checked = _config.MatchSchemes;
            returnStringFieldsCheckbox.Checked = _config.ReturnStringFields;
            returnStringFieldsWithKphOnlyCheckBox.Checked = _config.ReturnStringFieldsWithKphOnly;
            SortByUsernameRadioButton.Checked = _config.SortResultByUsername;
            SortByTitleRadioButton.Checked = !_config.SortResultByUsername;

            activateHttpListenerCheckbox.Checked = _config.ActivateHttpListener;
            listenerHostHttp.Text = _config.ListenerHostHttp;
            portNumberHttp.Value = _config.ListenerPortHttp;
            groupBoxHTTP.Enabled = activateHttpListenerCheckbox.Checked;

            activateHttpsListenerCheckbox.Checked = _config.ActivateHttpsListener;
            listenerHostHttps.Text = _config.ListenerHostHttps;
            portNumberHttps.Value = _config.ListenerPortHttps;
            groupBoxHTTPS.Enabled = activateHttpsListenerCheckbox.Checked;

            checkUpdatesCheckbox.Checked = _config.CheckUpdates;
            returnStringFieldsCheckbox_CheckedChanged(null, EventArgs.Empty);

            instructionsLink.Links.Add(new LinkLabel.Link() { LinkData = "https://alan-null.github.io/keepasshttp/configuration/listener-configuration.html" });
        }

        private static readonly HashSet<string> SafeHosts = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "localhost", "127.0.0.1", "::1", "[::1]"
        };

        /// <summary>
        /// Validates a listener host value. Returns true if safe or user-confirmed.
        /// </summary>
        private bool ValidateListenerHost(string host, string listenerLabel)
        {
            if (string.IsNullOrWhiteSpace(host))
            {
                return true; // will use default
            }

            host = host.Trim();

            if (SafeHosts.Contains(host))
            {
                return true;
            }

            string warning;
            if (host == "*" || host == "+")
            {
                warning = string.Format(
                    "WARNING: Setting the {0} host to '{1}' will expose your KeePass credentials " +
                    "to ALL network interfaces, making them accessible from other computers.\n\n" +
                    "This is a serious security risk.\n\n" +
                    "Are you sure you want to continue?",
                    listenerLabel, host);
            }
            else
            {
                warning = string.Format(
                    "WARNING: Setting the {0} host to '{1}' (non-localhost) may expose your " +
                    "KeePass credentials to other computers on the network.\n\n" +
                    "Are you sure you want to continue?",
                    listenerLabel, host);
            }

            var result = MessageBox.Show(
                this, warning, "Security Warning",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            return result == DialogResult.Yes;
        }

        private bool ValidateHttpListenerHostChange()
        {
            bool listenerDisabled = !activateHttpListenerCheckbox.Checked;
            if (listenerDisabled)
            {
                return true;
            }

            var newHost = listenerHostHttp.Text;
            if (newHost == _config.ListenerHostHttp)
            {
                return true;
            }

            if (ValidateListenerHost(newHost, "HTTP"))
            {
                return true;
            }

            return false;
        }

        private bool ValidateHttpsListenerHostChange()
        {
            bool listenerDisabled = !activateHttpsListenerCheckbox.Checked;
            if (listenerDisabled)
            {
                return true;
            }

            var newHost = listenerHostHttps.Text;
            if (newHost == _config.ListenerHostHttps)
            {
                return true;
            }

            if (ValidateListenerHost(newHost, "HTTPS"))
            {
                return true;
            }

            return false;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (!ValidateHttpListenerHostChange())
            {
                listenerHostHttp.Text = Constants.Host.DEFAULT_HOST;
                DialogResult = DialogResult.None;
                return;
            }

            if (!ValidateHttpsListenerHostChange())
            {
                listenerHostHttps.Text = Constants.Host.DEFAULT_HOST;
                DialogResult = DialogResult.None;
                return;
            }

            _config.ReceiveCredentialNotification = credNotifyCheckbox.Checked;
            _config.SpecificMatchingOnly = credMatchingCheckbox.Checked;
            _config.UnlockDatabaseRequest = unlockDatabaseCheckbox.Checked;
            _config.AlwaysAllowAccess = credAllowAccessCheckbox.Checked;
            _config.AlwaysAllowUpdates = credAllowUpdatesCheckbox.Checked;
            _config.SearchInAllOpenedDatabases = credSearchInAllOpenedDatabases.Checked;
            _config.HideExpired = hideExpiredCheckbox.Checked;
            _config.MatchSchemes = matchSchemesCheckbox.Checked;
            _config.ReturnStringFields = returnStringFieldsCheckbox.Checked;
            _config.ReturnStringFieldsWithKphOnly = returnStringFieldsWithKphOnlyCheckBox.Checked;
            _config.SortResultByUsername = SortByUsernameRadioButton.Checked;

            _config.ActivateHttpListener = activateHttpListenerCheckbox.Checked;
            _config.ListenerPortHttp = (int)portNumberHttp.Value;
            _config.ListenerHostHttp = listenerHostHttp.Text;

            _config.ActivateHttpsListener = activateHttpsListenerCheckbox.Checked;
            _config.ListenerPortHttps = (int)portNumberHttps.Value;
            _config.ListenerHostHttps = listenerHostHttps.Text;

            _config.CheckUpdates = checkUpdatesCheckbox.Checked;
            if (_restartRequired)
            {
                MessageBox.Show(
                    "You have successfully changed the port number and/or the host name.\nA restart of KeePass is required!\n\nPlease restart KeePass now.",
                    "Restart required!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }

            if (!_config.ActivateHttpListener && !_config.ActivateHttpsListener)
            {
                MessageBox.Show(
                    "You have no listener configured, so the endpoints won't be available. You should either enable an HTTP or HTTPS listener.",
                    "Configuration Warning!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            if (KeePass.Program.MainForm.DocumentManager.ActiveDatabase.IsOpen)
            {
                PwDatabase db = KeePass.Program.MainForm.DocumentManager.ActiveDatabase;
                var entry = GetConfigEntry(db);
                if (entry != null)
                {
                    List<string> deleteKeys = new List<string>();

                    foreach (var s in entry.Strings)
                    {
                        if (s.Key.IndexOf(KeePassHttpExt.ASSOCIATE_KEY_PREFIX) == 0)
                        {
                            deleteKeys.Add(s.Key);
                        }
                    }


                    if (deleteKeys.Count > 0)
                    {
                        PwObjectList<PwEntry> m_vHistory = entry.History.CloneDeep();
                        entry.History = m_vHistory;
                        entry.CreateBackup(null);

                        foreach (var key in deleteKeys)
                        {
                            entry.Strings.Remove(key);
                        }

                        entry.Touch(true);
                        KeePass.Program.MainForm.UpdateUI(false, null, true, db.RootGroup, true, null, true);
                        MessageBox.Show(
                            string.Format("Successfully removed {0} encryption-key{1} from KeePassHttp Settings.", deleteKeys.Count.ToString(), deleteKeys.Count == 1 ? "" : "s"),
                            string.Format("Removed {0} key{1} from database", deleteKeys.Count.ToString(), deleteKeys.Count == 1 ? "" : "s"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                    }
                    else
                    {
                        MessageBox.Show(
                            "No shared encryption-keys found in KeePassHttp Settings.", "No keys found",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                    }
                }
                else
                {
                    MessageBox.Show("The active database does not contain an entry of KeePassHttp Settings.", "KeePassHttp Settings not available!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("The active database is locked!\nPlease unlock the selected database or choose another one which is unlocked.", "Database locked!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void removePermissionsButton_Click(object sender, EventArgs e)
        {
            if (KeePass.Program.MainForm.DocumentManager.ActiveDatabase.IsOpen)
            {
                PwDatabase db = KeePass.Program.MainForm.DocumentManager.ActiveDatabase;

                uint counter = 0;
                var entries = db.RootGroup.GetEntries(true);

                if (entries.Count() > 999)
                {
                    MessageBox.Show(
                        string.Format("{0} entries detected!\nSearching and removing permissions could take some while.\n\nWe will inform you when the process has been finished.", entries.Count().ToString()),
                        string.Format("{0} entries detected", entries.Count().ToString()),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }

                foreach (var entry in entries)
                {
                    foreach (var str in entry.Strings)
                    {
                        if (str.Key == KeePassHttpExt.KEEPASSHTTP_NAME)
                        {
                            PwObjectList<PwEntry> m_vHistory = entry.History.CloneDeep();
                            entry.History = m_vHistory;
                            entry.CreateBackup(null);

                            entry.Strings.Remove(str.Key);

                            entry.Touch(true);

                            counter += 1;

                            break;
                        }
                    }
                }

                if (counter > 0)
                {
                    KeePass.Program.MainForm.UpdateUI(false, null, true, db.RootGroup, true, null, true);
                    MessageBox.Show(
                        string.Format("Successfully removed permissions from {0} entr{1}.", counter.ToString(), counter == 1 ? "y" : "ies"),
                        string.Format("Removed permissions from {0} entr{1}", counter.ToString(), counter == 1 ? "y" : "ies"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                else
                {
                    MessageBox.Show(
                        "The active database does not contain an entry with permissions.",
                        "No entry with permissions found!",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            else
            {
                MessageBox.Show("The active database is locked!\nPlease unlock the selected database or choose another one which is unlocked.", "Database locked!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void portNumber_ValueChanged(object sender, EventArgs e)
        {
            SetRestartRequired();
        }

        private void hostName_TextChanged(object sender, EventArgs e)
        {
            SetRestartRequired();
        }

        private void SetRestartRequired()
        {
            _restartRequired =
                _config.ActivateHttpListener != activateHttpListenerCheckbox.Checked ||
                _config.ListenerPortHttp != portNumberHttp.Value ||
                _config.ListenerHostHttp != listenerHostHttp.Text ||
                _config.ActivateHttpsListener != activateHttpsListenerCheckbox.Checked ||
                _config.ListenerPortHttps != portNumberHttps.Value ||
                _config.ListenerHostHttps != listenerHostHttps.Text;
        }

        private void returnStringFieldsCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            returnStringFieldsWithKphOnlyCheckBox.Enabled = returnStringFieldsCheckbox.Checked;
        }

        private void activateHttpsListenerCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            bool enabled = activateHttpsListenerCheckbox.Checked;
            groupBoxHTTPS.Enabled = enabled;
            listenerHostHttps.Enabled = enabled;
            portNumberHttps.Enabled = enabled;
            SetRestartRequired();
        }

        private void activateHttpListenerCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            bool enabled = activateHttpListenerCheckbox.Checked;
            groupBoxHTTP.Enabled = enabled;
            listenerHostHttp.Enabled = enabled;
            portNumberHttp.Enabled = enabled;
            SetRestartRequired();
        }

        private void instructionsLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData as string);
        }
    }
}
