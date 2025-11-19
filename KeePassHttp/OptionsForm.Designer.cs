namespace KeePassHttp
{
    partial class OptionsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.checkUpdatesCheckbox = new System.Windows.Forms.CheckBox();
            this.labelUpdates = new System.Windows.Forms.Label();
            this.labelInterface = new System.Windows.Forms.Label();
            this.labelResults = new System.Windows.Forms.Label();
            this.labelCleanup = new System.Windows.Forms.Label();
            this.labelSorting = new System.Windows.Forms.Label();
            this.SortByUsernameRadioButton = new System.Windows.Forms.RadioButton();
            this.SortByTitleRadioButton = new System.Windows.Forms.RadioButton();
            this.hideExpiredCheckbox = new System.Windows.Forms.CheckBox();
            this.matchSchemesCheckbox = new System.Windows.Forms.CheckBox();
            this.removePermissionsButton = new System.Windows.Forms.Button();
            this.unlockDatabaseCheckbox = new System.Windows.Forms.CheckBox();
            this.removeButton = new System.Windows.Forms.Button();
            this.credMatchingCheckbox = new System.Windows.Forms.CheckBox();
            this.credNotifyCheckbox = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.returnStringFieldsWithKphOnlyCheckBox = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.returnStringFieldsCheckbox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.credSearchInAllOpenedDatabases = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.credAllowUpdatesCheckbox = new System.Windows.Forms.CheckBox();
            this.credAllowAccessCheckbox = new System.Windows.Forms.CheckBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.activateHttpListenerCheckbox = new System.Windows.Forms.CheckBox();
            this.groupBoxHTTPS = new System.Windows.Forms.GroupBox();
            this.label15 = new System.Windows.Forms.Label();
            this.listenerHostHttps = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.portNumberHttps = new System.Windows.Forms.NumericUpDown();
            this.label18 = new System.Windows.Forms.Label();
            this.activateHttpsListenerCheckbox = new System.Windows.Forms.CheckBox();
            this.groupBoxHTTP = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.listenerHostHttp = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.portNumberHttp = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.instructionsLink = new System.Windows.Forms.LinkLabel();
            this.labelListenerDescription = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBoxHTTPS.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.portNumberHttps)).BeginInit();
            this.groupBoxHTTP.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.portNumberHttp)).BeginInit();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(313, 508);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(88, 28);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.okButton.Location = new System.Drawing.Point(219, 508);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(88, 28);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "&Save";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(1, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(410, 498);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.checkUpdatesCheckbox);
            this.tabPage1.Controls.Add(this.labelUpdates);
            this.tabPage1.Controls.Add(this.labelInterface);
            this.tabPage1.Controls.Add(this.labelResults);
            this.tabPage1.Controls.Add(this.labelCleanup);
            this.tabPage1.Controls.Add(this.labelSorting);
            this.tabPage1.Controls.Add(this.SortByUsernameRadioButton);
            this.tabPage1.Controls.Add(this.SortByTitleRadioButton);
            this.tabPage1.Controls.Add(this.hideExpiredCheckbox);
            this.tabPage1.Controls.Add(this.matchSchemesCheckbox);
            this.tabPage1.Controls.Add(this.removePermissionsButton);
            this.tabPage1.Controls.Add(this.unlockDatabaseCheckbox);
            this.tabPage1.Controls.Add(this.removeButton);
            this.tabPage1.Controls.Add(this.credMatchingCheckbox);
            this.tabPage1.Controls.Add(this.credNotifyCheckbox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(402, 472);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // checkUpdatesCheckbox
            // 
            this.checkUpdatesCheckbox.AutoSize = true;
            this.checkUpdatesCheckbox.Location = new System.Drawing.Point(14, 347);
            this.checkUpdatesCheckbox.Name = "checkUpdatesCheckbox";
            this.checkUpdatesCheckbox.Size = new System.Drawing.Size(163, 17);
            this.checkUpdatesCheckbox.TabIndex = 27;
            this.checkUpdatesCheckbox.Text = "Check KeePassHttp updates";
            this.checkUpdatesCheckbox.UseVisualStyleBackColor = true;
            // 
            // labelUpdates
            // 
            this.labelUpdates.AutoSize = true;
            this.labelUpdates.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUpdates.Location = new System.Drawing.Point(7, 328);
            this.labelUpdates.Name = "labelUpdates";
            this.labelUpdates.Size = new System.Drawing.Size(58, 13);
            this.labelUpdates.TabIndex = 26;
            this.labelUpdates.Text = "Updates:";
            // 
            // labelInterface
            // 
            this.labelInterface.AutoSize = true;
            this.labelInterface.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInterface.Location = new System.Drawing.Point(7, 6);
            this.labelInterface.Name = "labelInterface";
            this.labelInterface.Size = new System.Drawing.Size(62, 13);
            this.labelInterface.TabIndex = 25;
            this.labelInterface.Text = "Interface:";
            // 
            // labelResults
            // 
            this.labelResults.AutoSize = true;
            this.labelResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelResults.Location = new System.Drawing.Point(7, 66);
            this.labelResults.Name = "labelResults";
            this.labelResults.Size = new System.Drawing.Size(53, 13);
            this.labelResults.TabIndex = 24;
            this.labelResults.Text = "Results:";
            // 
            // labelCleanup
            // 
            this.labelCleanup.AutoSize = true;
            this.labelCleanup.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCleanup.Location = new System.Drawing.Point(7, 236);
            this.labelCleanup.Name = "labelCleanup";
            this.labelCleanup.Size = new System.Drawing.Size(57, 13);
            this.labelCleanup.TabIndex = 22;
            this.labelCleanup.Text = "Cleanup:";
            // 
            // labelSorting
            // 
            this.labelSorting.AutoSize = true;
            this.labelSorting.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSorting.Location = new System.Drawing.Point(7, 170);
            this.labelSorting.Name = "labelSorting";
            this.labelSorting.Size = new System.Drawing.Size(51, 13);
            this.labelSorting.TabIndex = 21;
            this.labelSorting.Text = "Sorting:";
            // 
            // SortByUsernameRadioButton
            // 
            this.SortByUsernameRadioButton.AutoSize = true;
            this.SortByUsernameRadioButton.Location = new System.Drawing.Point(14, 188);
            this.SortByUsernameRadioButton.Name = "SortByUsernameRadioButton";
            this.SortByUsernameRadioButton.Size = new System.Drawing.Size(171, 17);
            this.SortByUsernameRadioButton.TabIndex = 19;
            this.SortByUsernameRadioButton.TabStop = true;
            this.SortByUsernameRadioButton.Text = "Sort found entries by &username";
            this.SortByUsernameRadioButton.UseVisualStyleBackColor = true;
            // 
            // SortByTitleRadioButton
            // 
            this.SortByTitleRadioButton.AutoSize = true;
            this.SortByTitleRadioButton.Location = new System.Drawing.Point(14, 211);
            this.SortByTitleRadioButton.Name = "SortByTitleRadioButton";
            this.SortByTitleRadioButton.Size = new System.Drawing.Size(141, 17);
            this.SortByTitleRadioButton.TabIndex = 18;
            this.SortByTitleRadioButton.TabStop = true;
            this.SortByTitleRadioButton.Text = "Sort found entries by &title";
            this.SortByTitleRadioButton.UseVisualStyleBackColor = true;
            // 
            // hideExpiredCheckbox
            // 
            this.hideExpiredCheckbox.AutoSize = true;
            this.hideExpiredCheckbox.Location = new System.Drawing.Point(14, 117);
            this.hideExpiredCheckbox.Name = "hideExpiredCheckbox";
            this.hideExpiredCheckbox.Size = new System.Drawing.Size(152, 17);
            this.hideExpiredCheckbox.TabIndex = 17;
            this.hideExpiredCheckbox.Text = "Don\'t return e&xpired entries";
            this.hideExpiredCheckbox.UseVisualStyleBackColor = true;
            // 
            // matchSchemesCheckbox
            // 
            this.matchSchemesCheckbox.AutoSize = true;
            this.matchSchemesCheckbox.Location = new System.Drawing.Point(14, 137);
            this.matchSchemesCheckbox.Name = "matchSchemesCheckbox";
            this.matchSchemesCheckbox.Size = new System.Drawing.Size(375, 30);
            this.matchSchemesCheckbox.TabIndex = 17;
            this.matchSchemesCheckbox.Text = "&Match URL schemes\r\nonly entries with the same scheme (http://, https://, ftp://," +
    " ...) are returned";
            this.matchSchemesCheckbox.UseVisualStyleBackColor = true;
            // 
            // removePermissionsButton
            // 
            this.removePermissionsButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.removePermissionsButton.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.removePermissionsButton.Location = new System.Drawing.Point(14, 290);
            this.removePermissionsButton.Name = "removePermissionsButton";
            this.removePermissionsButton.Size = new System.Drawing.Size(372, 28);
            this.removePermissionsButton.TabIndex = 16;
            this.removePermissionsButton.Text = "Remo&ve all stored permissions from entries in active database";
            this.removePermissionsButton.UseVisualStyleBackColor = true;
            this.removePermissionsButton.Click += new System.EventHandler(this.removePermissionsButton_Click);
            // 
            // unlockDatabaseCheckbox
            // 
            this.unlockDatabaseCheckbox.AutoSize = true;
            this.unlockDatabaseCheckbox.Location = new System.Drawing.Point(14, 44);
            this.unlockDatabaseCheckbox.Name = "unlockDatabaseCheckbox";
            this.unlockDatabaseCheckbox.Size = new System.Drawing.Size(256, 17);
            this.unlockDatabaseCheckbox.TabIndex = 15;
            this.unlockDatabaseCheckbox.Text = "Re&quest for unlocking the database if it is locked";
            this.unlockDatabaseCheckbox.UseVisualStyleBackColor = true;
            // 
            // removeButton
            // 
            this.removeButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.removeButton.Location = new System.Drawing.Point(14, 256);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(372, 28);
            this.removeButton.TabIndex = 11;
            this.removeButton.Text = "R&emove all shared encryption-keys from active database";
            this.removeButton.UseVisualStyleBackColor = true;
            this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
            // 
            // credMatchingCheckbox
            // 
            this.credMatchingCheckbox.AutoSize = true;
            this.credMatchingCheckbox.Location = new System.Drawing.Point(14, 84);
            this.credMatchingCheckbox.Name = "credMatchingCheckbox";
            this.credMatchingCheckbox.Size = new System.Drawing.Size(238, 30);
            this.credMatchingCheckbox.TabIndex = 9;
            this.credMatchingCheckbox.Text = "&Return only best matching entries for an URL\r\ninstead of all entries for the who" +
    "le domain";
            this.credMatchingCheckbox.UseVisualStyleBackColor = true;
            // 
            // credNotifyCheckbox
            // 
            this.credNotifyCheckbox.AutoSize = true;
            this.credNotifyCheckbox.Location = new System.Drawing.Point(14, 22);
            this.credNotifyCheckbox.Name = "credNotifyCheckbox";
            this.credNotifyCheckbox.Size = new System.Drawing.Size(267, 17);
            this.credNotifyCheckbox.TabIndex = 8;
            this.credNotifyCheckbox.Text = "Sh&ow a notification when credentials are requested";
            this.credNotifyCheckbox.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.returnStringFieldsWithKphOnlyCheckBox);
            this.tabPage2.Controls.Add(this.label10);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.returnStringFieldsCheckbox);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.credSearchInAllOpenedDatabases);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.credAllowUpdatesCheckbox);
            this.tabPage2.Controls.Add(this.credAllowAccessCheckbox);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(402, 472);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Advanced";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // returnStringFieldsWithKphOnlyCheckBox
            // 
            this.returnStringFieldsWithKphOnlyCheckBox.AutoSize = true;
            this.returnStringFieldsWithKphOnlyCheckBox.Location = new System.Drawing.Point(55, 215);
            this.returnStringFieldsWithKphOnlyCheckBox.Name = "returnStringFieldsWithKphOnlyCheckBox";
            this.returnStringFieldsWithKphOnlyCheckBox.Size = new System.Drawing.Size(300, 30);
            this.returnStringFieldsWithKphOnlyCheckBox.TabIndex = 31;
            this.returnStringFieldsWithKphOnlyCheckBox.Text = "Only return advanced string fields which start with \"KPH: \"\r\n(Mind the space afte" +
    "r KPH:)";
            this.returnStringFieldsWithKphOnlyCheckBox.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(7, 283);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(375, 26);
            this.label10.TabIndex = 23;
            this.label10.Text = "Only change the host to bind to if you want to give access to other computers.\r\nU" +
    "se \'*\' to bind it to all your IP addresses (potentially dangerous!)\r\n";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(52, 248);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(277, 26);
            this.label4.TabIndex = 22;
            this.label4.Text = "Automatic creates or updates are not supported\r\nfor string fields!";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(52, 156);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(289, 52);
            this.label3.TabIndex = 21;
            this.label3.Text = "If there are more fields needed than username + password,\r\nnormal \"String Fields\"" +
    " are used, which can be defined in the\r\n\"Advanced\" tab of an entry.\r\nString fiel" +
    "ds are returned in alphabetical order.";
            // 
            // returnStringFieldsCheckbox
            // 
            this.returnStringFieldsCheckbox.AutoSize = true;
            this.returnStringFieldsCheckbox.Location = new System.Drawing.Point(7, 136);
            this.returnStringFieldsCheckbox.Name = "returnStringFieldsCheckbox";
            this.returnStringFieldsCheckbox.Size = new System.Drawing.Size(186, 17);
            this.returnStringFieldsCheckbox.TabIndex = 20;
            this.returnStringFieldsCheckbox.Text = "&Return also advanced string fields";
            this.returnStringFieldsCheckbox.UseVisualStyleBackColor = true;
            this.returnStringFieldsCheckbox.CheckedChanged += new System.EventHandler(this.returnStringFieldsCheckbox_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(52, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(299, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Only the selected database has to be connected with a client!";
            // 
            // credSearchInAllOpenedDatabases
            // 
            this.credSearchInAllOpenedDatabases.AutoSize = true;
            this.credSearchInAllOpenedDatabases.Location = new System.Drawing.Point(7, 88);
            this.credSearchInAllOpenedDatabases.Name = "credSearchInAllOpenedDatabases";
            this.credSearchInAllOpenedDatabases.Size = new System.Drawing.Size(270, 17);
            this.credSearchInAllOpenedDatabases.TabIndex = 18;
            this.credSearchInAllOpenedDatabases.Text = "Searc&h in all opened databases for matching entries";
            this.credSearchInAllOpenedDatabases.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(4, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(391, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Activate the following options only, if you know what you are doing!";
            // 
            // credAllowUpdatesCheckbox
            // 
            this.credAllowUpdatesCheckbox.AutoSize = true;
            this.credAllowUpdatesCheckbox.Location = new System.Drawing.Point(6, 56);
            this.credAllowUpdatesCheckbox.Name = "credAllowUpdatesCheckbox";
            this.credAllowUpdatesCheckbox.Size = new System.Drawing.Size(164, 17);
            this.credAllowUpdatesCheckbox.TabIndex = 16;
            this.credAllowUpdatesCheckbox.Text = "Always allow &updating entries";
            this.credAllowUpdatesCheckbox.UseVisualStyleBackColor = true;
            // 
            // credAllowAccessCheckbox
            // 
            this.credAllowAccessCheckbox.AutoSize = true;
            this.credAllowAccessCheckbox.Location = new System.Drawing.Point(6, 33);
            this.credAllowAccessCheckbox.Name = "credAllowAccessCheckbox";
            this.credAllowAccessCheckbox.Size = new System.Drawing.Size(169, 17);
            this.credAllowAccessCheckbox.TabIndex = 15;
            this.credAllowAccessCheckbox.Text = "Always allow &access to entries";
            this.credAllowAccessCheckbox.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.activateHttpListenerCheckbox);
            this.tabPage3.Controls.Add(this.groupBoxHTTPS);
            this.tabPage3.Controls.Add(this.activateHttpsListenerCheckbox);
            this.tabPage3.Controls.Add(this.groupBoxHTTP);
            this.tabPage3.Controls.Add(this.instructionsLink);
            this.tabPage3.Controls.Add(this.labelListenerDescription);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(402, 472);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Listener Configuration";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // activateHttpListenerCheckbox
            // 
            this.activateHttpListenerCheckbox.AutoSize = true;
            this.activateHttpListenerCheckbox.Location = new System.Drawing.Point(7, 84);
            this.activateHttpListenerCheckbox.Margin = new System.Windows.Forms.Padding(2);
            this.activateHttpListenerCheckbox.Name = "activateHttpListenerCheckbox";
            this.activateHttpListenerCheckbox.Size = new System.Drawing.Size(137, 17);
            this.activateHttpListenerCheckbox.TabIndex = 52;
            this.activateHttpListenerCheckbox.Text = "Activate HTTP Listener";
            this.activateHttpListenerCheckbox.UseVisualStyleBackColor = true;
            this.activateHttpListenerCheckbox.CheckedChanged += new System.EventHandler(this.activateHttpListenerCheckbox_CheckedChanged);
            // 
            // groupBoxHTTPS
            // 
            this.groupBoxHTTPS.Controls.Add(this.label15);
            this.groupBoxHTTPS.Controls.Add(this.listenerHostHttps);
            this.groupBoxHTTPS.Controls.Add(this.label16);
            this.groupBoxHTTPS.Controls.Add(this.label17);
            this.groupBoxHTTPS.Controls.Add(this.portNumberHttps);
            this.groupBoxHTTPS.Controls.Add(this.label18);
            this.groupBoxHTTPS.Location = new System.Drawing.Point(7, 221);
            this.groupBoxHTTPS.Margin = new System.Windows.Forms.Padding(2);
            this.groupBoxHTTPS.Name = "groupBoxHTTPS";
            this.groupBoxHTTPS.Padding = new System.Windows.Forms.Padding(2);
            this.groupBoxHTTPS.Size = new System.Drawing.Size(387, 77);
            this.groupBoxHTTPS.TabIndex = 51;
            this.groupBoxHTTPS.TabStop = false;
            this.groupBoxHTTPS.Text = "HTTPS Listener";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(218, 22);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(89, 13);
            this.label15.TabIndex = 48;
            this.label15.Text = "Default: localhost";
            // 
            // listenerHostHttps
            // 
            this.listenerHostHttps.Location = new System.Drawing.Point(44, 21);
            this.listenerHostHttps.Margin = new System.Windows.Forms.Padding(2);
            this.listenerHostHttps.Name = "listenerHostHttps";
            this.listenerHostHttps.Size = new System.Drawing.Size(164, 20);
            this.listenerHostHttps.TabIndex = 47;
            this.listenerHostHttps.Text = "localhost";
            this.listenerHostHttps.TextChanged += new System.EventHandler(this.hostName_TextChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(8, 22);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(32, 13);
            this.label16.TabIndex = 46;
            this.label16.Text = "Host:";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(218, 47);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(77, 13);
            this.label17.TabIndex = 45;
            this.label17.Text = "Default: 19456";
            // 
            // portNumberHttps
            // 
            this.portNumberHttps.Location = new System.Drawing.Point(44, 48);
            this.portNumberHttps.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.portNumberHttps.Minimum = new decimal(new int[] {
            1025,
            0,
            0,
            0});
            this.portNumberHttps.Name = "portNumberHttps";
            this.portNumberHttps.Size = new System.Drawing.Size(60, 20);
            this.portNumberHttps.TabIndex = 44;
            this.portNumberHttps.Value = new decimal(new int[] {
            19456,
            0,
            0,
            0});
            this.portNumberHttps.ValueChanged += new System.EventHandler(this.portNumber_ValueChanged);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(10, 49);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(29, 13);
            this.label18.TabIndex = 43;
            this.label18.Text = "Port:";
            // 
            // activateHttpsListenerCheckbox
            // 
            this.activateHttpsListenerCheckbox.AutoSize = true;
            this.activateHttpsListenerCheckbox.Location = new System.Drawing.Point(7, 203);
            this.activateHttpsListenerCheckbox.Margin = new System.Windows.Forms.Padding(2);
            this.activateHttpsListenerCheckbox.Name = "activateHttpsListenerCheckbox";
            this.activateHttpsListenerCheckbox.Size = new System.Drawing.Size(144, 17);
            this.activateHttpsListenerCheckbox.TabIndex = 50;
            this.activateHttpsListenerCheckbox.Text = "Activate HTTPS Listener";
            this.activateHttpsListenerCheckbox.UseVisualStyleBackColor = true;
            this.activateHttpsListenerCheckbox.CheckedChanged += new System.EventHandler(this.activateHttpsListenerCheckbox_CheckedChanged);
            // 
            // groupBoxHTTP
            // 
            this.groupBoxHTTP.Controls.Add(this.label11);
            this.groupBoxHTTP.Controls.Add(this.listenerHostHttp);
            this.groupBoxHTTP.Controls.Add(this.label12);
            this.groupBoxHTTP.Controls.Add(this.label13);
            this.groupBoxHTTP.Controls.Add(this.portNumberHttp);
            this.groupBoxHTTP.Controls.Add(this.label14);
            this.groupBoxHTTP.Location = new System.Drawing.Point(7, 105);
            this.groupBoxHTTP.Margin = new System.Windows.Forms.Padding(2);
            this.groupBoxHTTP.Name = "groupBoxHTTP";
            this.groupBoxHTTP.Padding = new System.Windows.Forms.Padding(2);
            this.groupBoxHTTP.Size = new System.Drawing.Size(387, 81);
            this.groupBoxHTTP.TabIndex = 49;
            this.groupBoxHTTP.TabStop = false;
            this.groupBoxHTTP.Text = "HTTP Listener";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(220, 24);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(89, 13);
            this.label11.TabIndex = 48;
            this.label11.Text = "Default: localhost";
            // 
            // listenerHostHttp
            // 
            this.listenerHostHttp.Location = new System.Drawing.Point(46, 24);
            this.listenerHostHttp.Margin = new System.Windows.Forms.Padding(2);
            this.listenerHostHttp.Name = "listenerHostHttp";
            this.listenerHostHttp.Size = new System.Drawing.Size(164, 20);
            this.listenerHostHttp.TabIndex = 47;
            this.listenerHostHttp.Text = "localhost";
            this.listenerHostHttp.TextChanged += new System.EventHandler(this.hostName_TextChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(10, 24);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(32, 13);
            this.label12.TabIndex = 46;
            this.label12.Text = "Host:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(220, 49);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(77, 13);
            this.label13.TabIndex = 45;
            this.label13.Text = "Default: 19455";
            // 
            // portNumberHttp
            // 
            this.portNumberHttp.Location = new System.Drawing.Point(46, 50);
            this.portNumberHttp.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.portNumberHttp.Minimum = new decimal(new int[] {
            1025,
            0,
            0,
            0});
            this.portNumberHttp.Name = "portNumberHttp";
            this.portNumberHttp.Size = new System.Drawing.Size(60, 20);
            this.portNumberHttp.TabIndex = 44;
            this.portNumberHttp.Value = new decimal(new int[] {
            19455,
            0,
            0,
            0});
            this.portNumberHttp.ValueChanged += new System.EventHandler(this.portNumber_ValueChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(13, 51);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(29, 13);
            this.label14.TabIndex = 43;
            this.label14.Text = "Port:";
            // 
            // instructionsLink
            // 
            this.instructionsLink.AutoSize = true;
            this.instructionsLink.Location = new System.Drawing.Point(4, 51);
            this.instructionsLink.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.instructionsLink.Name = "instructionsLink";
            this.instructionsLink.Size = new System.Drawing.Size(110, 13);
            this.instructionsLink.TabIndex = 48;
            this.instructionsLink.TabStop = true;
            this.instructionsLink.Text = "Read the instructions!";
            this.instructionsLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.instructionsLink_LinkClicked);
            // 
            // labelListenerDescription
            // 
            this.labelListenerDescription.AutoSize = true;
            this.labelListenerDescription.Location = new System.Drawing.Point(4, 7);
            this.labelListenerDescription.Name = "labelListenerDescription";
            this.labelListenerDescription.Size = new System.Drawing.Size(362, 39);
            this.labelListenerDescription.TabIndex = 37;
            this.labelListenerDescription.Text = resources.GetString("labelListenerDescription.Text");
            // 
            // OptionsForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(411, 545);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "KeePassHttp Options";
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.groupBoxHTTPS.ResumeLayout(false);
            this.groupBoxHTTPS.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.portNumberHttps)).EndInit();
            this.groupBoxHTTP.ResumeLayout(false);
            this.groupBoxHTTP.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.portNumberHttp)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.CheckBox hideExpiredCheckbox;
        private System.Windows.Forms.CheckBox matchSchemesCheckbox;
        private System.Windows.Forms.Button removePermissionsButton;
        private System.Windows.Forms.CheckBox unlockDatabaseCheckbox;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.CheckBox credMatchingCheckbox;
        private System.Windows.Forms.CheckBox credNotifyCheckbox;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox credSearchInAllOpenedDatabases;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox credAllowUpdatesCheckbox;
        private System.Windows.Forms.CheckBox credAllowAccessCheckbox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox returnStringFieldsCheckbox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton SortByUsernameRadioButton;
        private System.Windows.Forms.RadioButton SortByTitleRadioButton;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox returnStringFieldsWithKphOnlyCheckBox;
        private System.Windows.Forms.Label labelSorting;
        private System.Windows.Forms.Label labelCleanup;
        private System.Windows.Forms.Label labelResults;
        private System.Windows.Forms.Label labelInterface;
        private System.Windows.Forms.Label labelUpdates;
        private System.Windows.Forms.CheckBox checkUpdatesCheckbox;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.CheckBox activateHttpListenerCheckbox;
        private System.Windows.Forms.GroupBox groupBoxHTTPS;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox listenerHostHttps;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.NumericUpDown portNumberHttps;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.CheckBox activateHttpsListenerCheckbox;
        private System.Windows.Forms.GroupBox groupBoxHTTP;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox listenerHostHttp;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown portNumberHttp;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.LinkLabel instructionsLink;
        private System.Windows.Forms.Label labelListenerDescription;
    }
}