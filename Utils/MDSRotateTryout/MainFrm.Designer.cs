namespace MDSTryout
{
    partial class MainFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFrm));
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.newToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.openToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.appDirGrpBx = new System.Windows.Forms.GroupBox();
            this.copyMDSAppDirLink = new System.Windows.Forms.LinkLabel();
            this.mdsAppDirTxt = new System.Windows.Forms.TextBox();
            this.changeAppDirBtn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.resetConfigBtn = new System.Windows.Forms.Button();
            this.loadConfigBtn = new System.Windows.Forms.Button();
            this.pGrid = new System.Windows.Forms.PropertyGrid();
            this.runBtn = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.autoIncrChkBx = new System.Windows.Forms.CheckBox();
            this.processTxt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.processBar = new System.Windows.Forms.TrackBar();
            this.copyDirLink = new System.Windows.Forms.LinkLabel();
            this.baseDirTxt = new System.Windows.Forms.TextBox();
            this.changeBaseDirBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.nameTxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.clusTx = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.clusBrowseBtn = new System.Windows.Forms.Button();
            this.outTxt = new System.Windows.Forms.TextBox();
            this.mainSplit = new System.Windows.Forms.SplitContainer();
            this.runIncBtn = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.outSplit = new System.Windows.Forms.SplitContainer();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.errTxt = new System.Windows.Forms.TextBox();
            this.showRefPlotBtn = new System.Windows.Forms.Button();
            this.rebuildBtn = new System.Windows.Forms.Button();
            this.showInPvizBtn = new System.Windows.Forms.Button();
            this.rightLineGrpBx = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.threadTxt = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.appDirGrpBx.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.processBar)).BeginInit();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainSplit)).BeginInit();
            this.mainSplit.Panel1.SuspendLayout();
            this.mainSplit.Panel2.SuspendLayout();
            this.mainSplit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.outSplit)).BeginInit();
            this.outSplit.Panel1.SuspendLayout();
            this.outSplit.Panel2.SuspendLayout();
            this.outSplit.SuspendLayout();
            this.SuspendLayout();
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripSeparator,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripMenuItem.Image")));
            this.newToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.newToolStripMenuItem.Text = "&New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
            this.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(143, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1276, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Location = new System.Drawing.Point(0, 50);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1278, 2);
            this.groupBox3.TabIndex = 16;
            this.groupBox3.TabStop = false;
            // 
            // newToolStripButton
            // 
            this.newToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.newToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripButton.Image")));
            this.newToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolStripButton.Name = "newToolStripButton";
            this.newToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.newToolStripButton.Text = "&New";
            this.newToolStripButton.Click += new System.EventHandler(this.newToolStripButton_Click);
            // 
            // openToolStripButton
            // 
            this.openToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripButton.Image")));
            this.openToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripButton.Name = "openToolStripButton";
            this.openToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.openToolStripButton.Text = "&Open";
            this.openToolStripButton.Click += new System.EventHandler(this.openToolStripButton_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripButton,
            this.openToolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1276, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // appDirGrpBx
            // 
            this.appDirGrpBx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.appDirGrpBx.Controls.Add(this.copyMDSAppDirLink);
            this.appDirGrpBx.Controls.Add(this.mdsAppDirTxt);
            this.appDirGrpBx.Controls.Add(this.changeAppDirBtn);
            this.appDirGrpBx.Location = new System.Drawing.Point(3, 6);
            this.appDirGrpBx.Name = "appDirGrpBx";
            this.appDirGrpBx.Size = new System.Drawing.Size(555, 58);
            this.appDirGrpBx.TabIndex = 17;
            this.appDirGrpBx.TabStop = false;
            this.appDirGrpBx.Text = "MDS App Directory";
            // 
            // copyMDSAppDirLink
            // 
            this.copyMDSAppDirLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.copyMDSAppDirLink.AutoSize = true;
            this.copyMDSAppDirLink.LinkArea = new System.Windows.Forms.LinkArea(1, 5);
            this.copyMDSAppDirLink.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.copyMDSAppDirLink.Location = new System.Drawing.Point(430, 26);
            this.copyMDSAppDirLink.Name = "copyMDSAppDirLink";
            this.copyMDSAppDirLink.Size = new System.Drawing.Size(38, 17);
            this.copyMDSAppDirLink.TabIndex = 3;
            this.copyMDSAppDirLink.TabStop = true;
            this.copyMDSAppDirLink.Text = "(Copy)";
            this.copyMDSAppDirLink.UseCompatibleTextRendering = true;
            this.copyMDSAppDirLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.copyMDSAppDirLink_LinkClicked);
            // 
            // mdsAppDirTxt
            // 
            this.mdsAppDirTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mdsAppDirTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.mdsAppDirTxt.Location = new System.Drawing.Point(24, 26);
            this.mdsAppDirTxt.Name = "mdsAppDirTxt";
            this.mdsAppDirTxt.ReadOnly = true;
            this.mdsAppDirTxt.Size = new System.Drawing.Size(380, 13);
            this.mdsAppDirTxt.TabIndex = 2;
            this.mdsAppDirTxt.Text = "For New Runs This Should Be The Default Path to Find MDS Applications";
            // 
            // changeAppDirBtn
            // 
            this.changeAppDirBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.changeAppDirBtn.Location = new System.Drawing.Point(474, 21);
            this.changeAppDirBtn.Name = "changeAppDirBtn";
            this.changeAppDirBtn.Size = new System.Drawing.Size(75, 23);
            this.changeAppDirBtn.TabIndex = 1;
            this.changeAppDirBtn.Text = "Change...";
            this.changeAppDirBtn.UseVisualStyleBackColor = true;
            this.changeAppDirBtn.Click += new System.EventHandler(this.changeAppDirBtn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.resetConfigBtn);
            this.groupBox1.Controls.Add(this.loadConfigBtn);
            this.groupBox1.Controls.Add(this.pGrid);
            this.groupBox1.Location = new System.Drawing.Point(3, 279);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(555, 391);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "MDS Configuration";
            // 
            // resetConfigBtn
            // 
            this.resetConfigBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.resetConfigBtn.Location = new System.Drawing.Point(383, 19);
            this.resetConfigBtn.Name = "resetConfigBtn";
            this.resetConfigBtn.Size = new System.Drawing.Size(75, 23);
            this.resetConfigBtn.TabIndex = 2;
            this.resetConfigBtn.Text = "Reset";
            this.resetConfigBtn.UseVisualStyleBackColor = true;
            this.resetConfigBtn.Click += new System.EventHandler(this.resetConfigBtn_Click);
            // 
            // loadConfigBtn
            // 
            this.loadConfigBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.loadConfigBtn.Location = new System.Drawing.Point(460, 19);
            this.loadConfigBtn.Name = "loadConfigBtn";
            this.loadConfigBtn.Size = new System.Drawing.Size(89, 23);
            this.loadConfigBtn.TabIndex = 1;
            this.loadConfigBtn.Text = "Load Existing...";
            this.loadConfigBtn.UseVisualStyleBackColor = true;
            this.loadConfigBtn.Click += new System.EventHandler(this.loadConfigBtn_Click);
            // 
            // pGrid
            // 
            this.pGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pGrid.Location = new System.Drawing.Point(6, 19);
            this.pGrid.Name = "pGrid";
            this.pGrid.Size = new System.Drawing.Size(543, 366);
            this.pGrid.TabIndex = 0;
            this.pGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pGrid_PropertyValueChanged);
            // 
            // runBtn
            // 
            this.runBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.runBtn.Location = new System.Drawing.Point(483, 687);
            this.runBtn.Name = "runBtn";
            this.runBtn.Size = new System.Drawing.Size(75, 23);
            this.runBtn.TabIndex = 20;
            this.runBtn.Text = "Run";
            this.runBtn.UseVisualStyleBackColor = true;
            this.runBtn.Click += new System.EventHandler(this.runBtn_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.threadTxt);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.autoIncrChkBx);
            this.groupBox4.Controls.Add(this.processTxt);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.processBar);
            this.groupBox4.Controls.Add(this.copyDirLink);
            this.groupBox4.Controls.Add(this.baseDirTxt);
            this.groupBox4.Controls.Add(this.changeBaseDirBtn);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.nameTxt);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Location = new System.Drawing.Point(3, 70);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(555, 139);
            this.groupBox4.TabIndex = 21;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "MDS Run Info";
            // 
            // autoIncrChkBx
            // 
            this.autoIncrChkBx.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.autoIncrChkBx.AutoSize = true;
            this.autoIncrChkBx.Location = new System.Drawing.Point(451, 25);
            this.autoIncrChkBx.Name = "autoIncrChkBx";
            this.autoIncrChkBx.Size = new System.Drawing.Size(98, 17);
            this.autoIncrChkBx.TabIndex = 28;
            this.autoIncrChkBx.Text = "Auto Increment";
            this.autoIncrChkBx.UseVisualStyleBackColor = true;
            this.autoIncrChkBx.CheckedChanged += new System.EventHandler(this.autoIncrChkBx_CheckedChanged);
            // 
            // processTxt
            // 
            this.processTxt.BackColor = System.Drawing.SystemColors.Window;
            this.processTxt.Enabled = false;
            this.processTxt.Location = new System.Drawing.Point(345, 87);
            this.processTxt.Name = "processTxt";
            this.processTxt.Size = new System.Drawing.Size(31, 20);
            this.processTxt.TabIndex = 27;
            this.processTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 26;
            this.label3.Text = "Processes:";
            // 
            // processBar
            // 
            this.processBar.Location = new System.Drawing.Point(94, 87);
            this.processBar.Maximum = 20;
            this.processBar.Minimum = 1;
            this.processBar.Name = "processBar";
            this.processBar.Size = new System.Drawing.Size(245, 45);
            this.processBar.TabIndex = 25;
            this.processBar.Value = 1;
            this.processBar.Scroll += new System.EventHandler(this.processBar_Scroll);
            // 
            // copyDirLink
            // 
            this.copyDirLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.copyDirLink.AutoSize = true;
            this.copyDirLink.LinkArea = new System.Windows.Forms.LinkArea(1, 5);
            this.copyDirLink.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.copyDirLink.Location = new System.Drawing.Point(430, 57);
            this.copyDirLink.Name = "copyDirLink";
            this.copyDirLink.Size = new System.Drawing.Size(38, 17);
            this.copyDirLink.TabIndex = 24;
            this.copyDirLink.TabStop = true;
            this.copyDirLink.Text = "(Copy)";
            this.copyDirLink.UseCompatibleTextRendering = true;
            this.copyDirLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.copyDirLink_LinkClicked);
            // 
            // baseDirTxt
            // 
            this.baseDirTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.baseDirTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseDirTxt.Location = new System.Drawing.Point(94, 57);
            this.baseDirTxt.Name = "baseDirTxt";
            this.baseDirTxt.ReadOnly = true;
            this.baseDirTxt.Size = new System.Drawing.Size(330, 13);
            this.baseDirTxt.TabIndex = 23;
            this.baseDirTxt.Text = "For New Runs This Should Be The Default Project Directory";
            // 
            // changeBaseDirBtn
            // 
            this.changeBaseDirBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.changeBaseDirBtn.Location = new System.Drawing.Point(474, 52);
            this.changeBaseDirBtn.Name = "changeBaseDirBtn";
            this.changeBaseDirBtn.Size = new System.Drawing.Size(75, 23);
            this.changeBaseDirBtn.TabIndex = 22;
            this.changeBaseDirBtn.Text = "Change...";
            this.changeBaseDirBtn.UseVisualStyleBackColor = true;
            this.changeBaseDirBtn.Click += new System.EventHandler(this.changeBaseDirBtn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Base Dir:";
            // 
            // nameTxt
            // 
            this.nameTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.nameTxt.Location = new System.Drawing.Point(94, 23);
            this.nameTxt.Name = "nameTxt";
            this.nameTxt.Size = new System.Drawing.Size(351, 20);
            this.nameTxt.TabIndex = 1;
            this.nameTxt.TextChanged += new System.EventHandler(this.nameTxt_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.clusTx);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.clusBrowseBtn);
            this.groupBox5.Location = new System.Drawing.Point(3, 215);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(555, 58);
            this.groupBox5.TabIndex = 22;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Point Clusters (Optional)";
            // 
            // clusTx
            // 
            this.clusTx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.clusTx.Location = new System.Drawing.Point(94, 24);
            this.clusTx.Name = "clusTx";
            this.clusTx.Size = new System.Drawing.Size(374, 20);
            this.clusTx.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Cluster File:";
            // 
            // clusBrowseBtn
            // 
            this.clusBrowseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.clusBrowseBtn.Location = new System.Drawing.Point(474, 22);
            this.clusBrowseBtn.Name = "clusBrowseBtn";
            this.clusBrowseBtn.Size = new System.Drawing.Size(75, 23);
            this.clusBrowseBtn.TabIndex = 1;
            this.clusBrowseBtn.Text = "Browse...";
            this.clusBrowseBtn.UseVisualStyleBackColor = true;
            this.clusBrowseBtn.Click += new System.EventHandler(this.clusBrowseBtn_Click);
            // 
            // outTxt
            // 
            this.outTxt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.outTxt.Location = new System.Drawing.Point(3, 29);
            this.outTxt.Multiline = true;
            this.outTxt.Name = "outTxt";
            this.outTxt.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.outTxt.Size = new System.Drawing.Size(678, 367);
            this.outTxt.TabIndex = 23;
            // 
            // mainSplit
            // 
            this.mainSplit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mainSplit.BackColor = System.Drawing.SystemColors.Control;
            this.mainSplit.Location = new System.Drawing.Point(0, 52);
            this.mainSplit.Name = "mainSplit";
            // 
            // mainSplit.Panel1
            // 
            this.mainSplit.Panel1.Controls.Add(this.runIncBtn);
            this.mainSplit.Panel1.Controls.Add(this.groupBox2);
            this.mainSplit.Panel1.Controls.Add(this.appDirGrpBx);
            this.mainSplit.Panel1.Controls.Add(this.runBtn);
            this.mainSplit.Panel1.Controls.Add(this.groupBox5);
            this.mainSplit.Panel1.Controls.Add(this.groupBox1);
            this.mainSplit.Panel1.Controls.Add(this.groupBox4);
            // 
            // mainSplit.Panel2
            // 
            this.mainSplit.Panel2.Controls.Add(this.outSplit);
            this.mainSplit.Panel2.Controls.Add(this.showRefPlotBtn);
            this.mainSplit.Panel2.Controls.Add(this.rebuildBtn);
            this.mainSplit.Panel2.Controls.Add(this.showInPvizBtn);
            this.mainSplit.Panel2.Controls.Add(this.rightLineGrpBx);
            this.mainSplit.Size = new System.Drawing.Size(1274, 725);
            this.mainSplit.SplitterDistance = 564;
            this.mainSplit.SplitterWidth = 6;
            this.mainSplit.TabIndex = 24;
            // 
            // runIncBtn
            // 
            this.runIncBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.runIncBtn.Location = new System.Drawing.Point(357, 687);
            this.runIncBtn.Name = "runIncBtn";
            this.runIncBtn.Size = new System.Drawing.Size(120, 23);
            this.runIncBtn.TabIndex = 25;
            this.runIncBtn.Text = "Run Incremented";
            this.runIncBtn.UseVisualStyleBackColor = true;
            this.runIncBtn.Click += new System.EventHandler(this.runIncBtn_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Location = new System.Drawing.Point(7, 679);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(556, 2);
            this.groupBox2.TabIndex = 23;
            this.groupBox2.TabStop = false;
            // 
            // outSplit
            // 
            this.outSplit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.outSplit.Location = new System.Drawing.Point(3, 3);
            this.outSplit.Name = "outSplit";
            this.outSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // outSplit.Panel1
            // 
            this.outSplit.Panel1.Controls.Add(this.label6);
            this.outSplit.Panel1.Controls.Add(this.outTxt);
            // 
            // outSplit.Panel2
            // 
            this.outSplit.Panel2.Controls.Add(this.label5);
            this.outSplit.Panel2.Controls.Add(this.errTxt);
            this.outSplit.Size = new System.Drawing.Size(690, 670);
            this.outSplit.SplitterDistance = 399;
            this.outSplit.SplitterWidth = 10;
            this.outSplit.TabIndex = 29;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(0, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 13);
            this.label6.TabIndex = 30;
            this.label6.Text = "Out Stream:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(0, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 29;
            this.label5.Text = "Error Stream:";
            // 
            // errTxt
            // 
            this.errTxt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.errTxt.Location = new System.Drawing.Point(3, 29);
            this.errTxt.Multiline = true;
            this.errTxt.Name = "errTxt";
            this.errTxt.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.errTxt.Size = new System.Drawing.Size(684, 205);
            this.errTxt.TabIndex = 28;
            // 
            // showRefPlotBtn
            // 
            this.showRefPlotBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.showRefPlotBtn.Location = new System.Drawing.Point(309, 687);
            this.showRefPlotBtn.Name = "showRefPlotBtn";
            this.showRefPlotBtn.Size = new System.Drawing.Size(140, 23);
            this.showRefPlotBtn.TabIndex = 27;
            this.showRefPlotBtn.Text = "Show Reference Plot";
            this.showRefPlotBtn.UseVisualStyleBackColor = true;
            this.showRefPlotBtn.Click += new System.EventHandler(this.showRefBtn_Click);
            // 
            // rebuildBtn
            // 
            this.rebuildBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.rebuildBtn.Location = new System.Drawing.Point(478, 687);
            this.rebuildBtn.Name = "rebuildBtn";
            this.rebuildBtn.Size = new System.Drawing.Size(100, 23);
            this.rebuildBtn.TabIndex = 26;
            this.rebuildBtn.Text = "Rebuild Plot";
            this.rebuildBtn.UseVisualStyleBackColor = true;
            this.rebuildBtn.Click += new System.EventHandler(this.rebuildBtn_Click);
            // 
            // showInPvizBtn
            // 
            this.showInPvizBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.showInPvizBtn.Location = new System.Drawing.Point(584, 687);
            this.showInPvizBtn.Name = "showInPvizBtn";
            this.showInPvizBtn.Size = new System.Drawing.Size(100, 23);
            this.showInPvizBtn.TabIndex = 25;
            this.showInPvizBtn.Text = "Show in Pviz";
            this.showInPvizBtn.UseVisualStyleBackColor = true;
            this.showInPvizBtn.Click += new System.EventHandler(this.showInPvizBtn_Click);
            // 
            // rightLineGrpBx
            // 
            this.rightLineGrpBx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rightLineGrpBx.Location = new System.Drawing.Point(7, 679);
            this.rightLineGrpBx.Name = "rightLineGrpBx";
            this.rightLineGrpBx.Size = new System.Drawing.Size(684, 2);
            this.rightLineGrpBx.TabIndex = 24;
            this.rightLineGrpBx.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(21, 119);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(46, 13);
            this.label7.TabIndex = 29;
            this.label7.Text = "Threads";
            // 
            // threadTxt
            // 
            this.threadTxt.Location = new System.Drawing.Point(94, 113);
            this.threadTxt.Name = "threadTxt";
            this.threadTxt.Size = new System.Drawing.Size(46, 20);
            this.threadTxt.TabIndex = 30;
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1276, 774);
            this.Controls.Add(this.mainSplit);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(300, 38);
            this.Name = "MainFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MDS Try Out";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFrm_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.appDirGrpBx.ResumeLayout(false);
            this.appDirGrpBx.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.processBar)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.mainSplit.Panel1.ResumeLayout(false);
            this.mainSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainSplit)).EndInit();
            this.mainSplit.ResumeLayout(false);
            this.outSplit.Panel1.ResumeLayout(false);
            this.outSplit.Panel1.PerformLayout();
            this.outSplit.Panel2.ResumeLayout(false);
            this.outSplit.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.outSplit)).EndInit();
            this.outSplit.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ToolStripButton newToolStripButton;
        private System.Windows.Forms.ToolStripButton openToolStripButton;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.GroupBox appDirGrpBx;
        private System.Windows.Forms.Button changeAppDirBtn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button resetConfigBtn;
        private System.Windows.Forms.Button loadConfigBtn;
        private System.Windows.Forms.PropertyGrid pGrid;
        private System.Windows.Forms.Button runBtn;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox nameTxt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button changeBaseDirBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox mdsAppDirTxt;
        private System.Windows.Forms.TextBox baseDirTxt;
        private System.Windows.Forms.LinkLabel copyMDSAppDirLink;
        private System.Windows.Forms.LinkLabel copyDirLink;
        private System.Windows.Forms.TrackBar processBar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox processTxt;
        private System.Windows.Forms.CheckBox autoIncrChkBx;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox clusTx;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button clusBrowseBtn;
        private System.Windows.Forms.TextBox outTxt;
        private System.Windows.Forms.SplitContainer mainSplit;
        private System.Windows.Forms.GroupBox rightLineGrpBx;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button showInPvizBtn;
        private System.Windows.Forms.Button rebuildBtn;
        private System.Windows.Forms.Button runIncBtn;
        private System.Windows.Forms.Button showRefPlotBtn;
        private System.Windows.Forms.TextBox errTxt;
        private System.Windows.Forms.SplitContainer outSplit;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox threadTxt;
        private System.Windows.Forms.Label label7;
    }
}

