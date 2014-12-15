namespace PetqLight
{
    partial class MainFrm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components;

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
            this.pathBrowseBtn = new System.Windows.Forms.Button();
            this.directoryTxt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.nameTxt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.inFileTxt = new System.Windows.Forms.TextBox();
            this.fileBrowseBtn = new System.Windows.Forms.Button();
            this.genSubBtn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.targetTxt = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.headNodeCombo = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.inverseLink = new System.Windows.Forms.LinkLabel();
            this.selectAllLink = new System.Windows.Forms.LinkLabel();
            this.clearLink = new System.Windows.Forms.LinkLabel();
            this.nodeBox = new System.Windows.Forms.ListBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.localRBtn = new System.Windows.Forms.RadioButton();
            this.remoteRBtn = new System.Windows.Forms.RadioButton();
            this.label8 = new System.Windows.Forms.Label();
            this.processBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.saGroup = new System.Windows.Forms.GroupBox();
            this.swmsRBtn = new System.Windows.Forms.RadioButton();
            this.swgRBtn = new System.Windows.Forms.RadioButton();
            this.nwRBtn = new System.Windows.Forms.RadioButton();
            this.saChkBx = new System.Windows.Forms.CheckBox();
            this.pwcChkBx = new System.Windows.Forms.CheckBox();
            this.mdsChkBx = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.mdsConfLink = new System.Windows.Forms.LinkLabel();
            this.pwcConfLink = new System.Windows.Forms.LinkLabel();
            this.saConfLink = new System.Windows.Forms.LinkLabel();
            this.groupBox3.SuspendLayout();
            this.saGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // pathBrowseBtn
            // 
            this.pathBrowseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pathBrowseBtn.Location = new System.Drawing.Point(708, 32);
            this.pathBrowseBtn.Name = "pathBrowseBtn";
            this.pathBrowseBtn.Size = new System.Drawing.Size(30, 23);
            this.pathBrowseBtn.TabIndex = 9;
            this.pathBrowseBtn.Text = "...";
            this.pathBrowseBtn.UseVisualStyleBackColor = true;
            this.pathBrowseBtn.Click += new System.EventHandler(this.pathBrowseBtn_Click);
            // 
            // directoryTxt
            // 
            this.directoryTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.directoryTxt.Location = new System.Drawing.Point(132, 35);
            this.directoryTxt.Name = "directoryTxt";
            this.directoryTxt.Size = new System.Drawing.Size(570, 20);
            this.directoryTxt.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Project Directory:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Project Name:";
            // 
            // nameTxt
            // 
            this.nameTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.nameTxt.Location = new System.Drawing.Point(132, 9);
            this.nameTxt.Name = "nameTxt";
            this.nameTxt.Size = new System.Drawing.Size(570, 20);
            this.nameTxt.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "InputFile:";
            // 
            // inFileTxt
            // 
            this.inFileTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.inFileTxt.Location = new System.Drawing.Point(132, 61);
            this.inFileTxt.Name = "inFileTxt";
            this.inFileTxt.Size = new System.Drawing.Size(570, 20);
            this.inFileTxt.TabIndex = 11;
            // 
            // fileBrowseBtn
            // 
            this.fileBrowseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.fileBrowseBtn.Location = new System.Drawing.Point(708, 59);
            this.fileBrowseBtn.Name = "fileBrowseBtn";
            this.fileBrowseBtn.Size = new System.Drawing.Size(30, 23);
            this.fileBrowseBtn.TabIndex = 12;
            this.fileBrowseBtn.Text = "...";
            this.fileBrowseBtn.UseVisualStyleBackColor = true;
            this.fileBrowseBtn.Click += new System.EventHandler(this.fileBrowseBtn_Click);
            // 
            // genSubBtn
            // 
            this.genSubBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.genSubBtn.Location = new System.Drawing.Point(663, 746);
            this.genSubBtn.Name = "genSubBtn";
            this.genSubBtn.Size = new System.Drawing.Size(75, 23);
            this.genSubBtn.TabIndex = 13;
            this.genSubBtn.Text = "Generate";
            this.genSubBtn.UseVisualStyleBackColor = true;
            this.genSubBtn.Click += new System.EventHandler(this.genBtn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Location = new System.Drawing.Point(10, 738);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(728, 2);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 711);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Target Directory";
            // 
            // targetTxt
            // 
            this.targetTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.targetTxt.Location = new System.Drawing.Point(132, 708);
            this.targetTxt.Name = "targetTxt";
            this.targetTxt.Size = new System.Drawing.Size(568, 20);
            this.targetTxt.TabIndex = 16;
            this.targetTxt.Text = "C:\\salsa\\evaluations";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Location = new System.Drawing.Point(10, 88);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(728, 2);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            // 
            // headNodeCombo
            // 
            this.headNodeCombo.FormattingEnabled = true;
            this.headNodeCombo.Location = new System.Drawing.Point(132, 247);
            this.headNodeCombo.Name = "headNodeCombo";
            this.headNodeCombo.Size = new System.Drawing.Size(261, 21);
            this.headNodeCombo.TabIndex = 18;
            this.headNodeCombo.SelectedIndexChanged += new System.EventHandler(this.headNodeCombo_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 250);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "Select Head Node:";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox3.Controls.Add(this.inverseLink);
            this.groupBox3.Controls.Add(this.selectAllLink);
            this.groupBox3.Controls.Add(this.clearLink);
            this.groupBox3.Controls.Add(this.nodeBox);
            this.groupBox3.Location = new System.Drawing.Point(132, 274);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(259, 428);
            this.groupBox3.TabIndex = 20;
            this.groupBox3.TabStop = false;
            // 
            // inverseLink
            // 
            this.inverseLink.AutoSize = true;
            this.inverseLink.Location = new System.Drawing.Point(63, 15);
            this.inverseLink.Name = "inverseLink";
            this.inverseLink.Size = new System.Drawing.Size(42, 13);
            this.inverseLink.TabIndex = 11;
            this.inverseLink.TabStop = true;
            this.inverseLink.Text = "Inverse";
            this.inverseLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.inverseLink_LinkClicked);
            // 
            // selectAllLink
            // 
            this.selectAllLink.AutoSize = true;
            this.selectAllLink.Location = new System.Drawing.Point(6, 15);
            this.selectAllLink.Name = "selectAllLink";
            this.selectAllLink.Size = new System.Drawing.Size(51, 13);
            this.selectAllLink.TabIndex = 10;
            this.selectAllLink.TabStop = true;
            this.selectAllLink.Text = "Select All";
            this.selectAllLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.selectAllLink_LinkClicked);
            // 
            // clearLink
            // 
            this.clearLink.AutoSize = true;
            this.clearLink.Location = new System.Drawing.Point(111, 15);
            this.clearLink.Name = "clearLink";
            this.clearLink.Size = new System.Drawing.Size(31, 13);
            this.clearLink.TabIndex = 9;
            this.clearLink.TabStop = true;
            this.clearLink.Text = "Clear";
            this.clearLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.clearLink_LinkClicked);
            // 
            // nodeBox
            // 
            this.nodeBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.nodeBox.DisplayMember = "Name";
            this.nodeBox.FormattingEnabled = true;
            this.nodeBox.Location = new System.Drawing.Point(6, 37);
            this.nodeBox.Name = "nodeBox";
            this.nodeBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.nodeBox.Size = new System.Drawing.Size(247, 368);
            this.nodeBox.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 289);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(119, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "Select Compute Nodes:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 223);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(84, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "Execution Type:";
            // 
            // localRBtn
            // 
            this.localRBtn.AutoSize = true;
            this.localRBtn.Location = new System.Drawing.Point(132, 221);
            this.localRBtn.Name = "localRBtn";
            this.localRBtn.Size = new System.Drawing.Size(51, 17);
            this.localRBtn.TabIndex = 23;
            this.localRBtn.TabStop = true;
            this.localRBtn.Text = "Local";
            this.localRBtn.UseVisualStyleBackColor = true;
            this.localRBtn.CheckedChanged += new System.EventHandler(this.localRBtn_CheckedChanged);
            // 
            // remoteRBtn
            // 
            this.remoteRBtn.AutoSize = true;
            this.remoteRBtn.Location = new System.Drawing.Point(189, 221);
            this.remoteRBtn.Name = "remoteRBtn";
            this.remoteRBtn.Size = new System.Drawing.Size(62, 17);
            this.remoteRBtn.TabIndex = 24;
            this.remoteRBtn.TabStop = true;
            this.remoteRBtn.Text = "Remote";
            this.remoteRBtn.UseVisualStyleBackColor = true;
            this.remoteRBtn.CheckedChanged += new System.EventHandler(this.remoteRBtn_CheckedChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(452, 250);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(111, 13);
            this.label8.TabIndex = 25;
            this.label8.Text = "Number of Processes:";
            // 
            // processBox
            // 
            this.processBox.Location = new System.Drawing.Point(569, 247);
            this.processBox.Name = "processBox";
            this.processBox.Size = new System.Drawing.Size(74, 20);
            this.processBox.TabIndex = 26;
            this.processBox.Text = "1";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 103);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 13);
            this.label9.TabIndex = 27;
            this.label9.Text = "Applicatons:";
            // 
            // saGroup
            // 
            this.saGroup.Controls.Add(this.swmsRBtn);
            this.saGroup.Controls.Add(this.swgRBtn);
            this.saGroup.Controls.Add(this.nwRBtn);
            this.saGroup.Location = new System.Drawing.Point(143, 118);
            this.saGroup.Name = "saGroup";
            this.saGroup.Size = new System.Drawing.Size(364, 36);
            this.saGroup.TabIndex = 28;
            this.saGroup.TabStop = false;
            // 
            // swmsRBtn
            // 
            this.swmsRBtn.AutoSize = true;
            this.swmsRBtn.Location = new System.Drawing.Point(237, 11);
            this.swmsRBtn.Name = "swmsRBtn";
            this.swmsRBtn.Size = new System.Drawing.Size(125, 17);
            this.swmsRBtn.TabIndex = 3;
            this.swmsRBtn.TabStop = true;
            this.swmsRBtn.Text = "SmithWaterman (MS)";
            this.swmsRBtn.UseVisualStyleBackColor = true;
            this.swmsRBtn.CheckedChanged += new System.EventHandler(this.swmsRBtn_CheckedChanged);
            // 
            // swgRBtn
            // 
            this.swgRBtn.AutoSize = true;
            this.swgRBtn.Location = new System.Drawing.Point(131, 11);
            this.swgRBtn.Name = "swgRBtn";
            this.swgRBtn.Size = new System.Drawing.Size(100, 17);
            this.swgRBtn.TabIndex = 2;
            this.swgRBtn.TabStop = true;
            this.swgRBtn.Text = "SmithWaterman";
            this.swgRBtn.UseVisualStyleBackColor = true;
            this.swgRBtn.CheckedChanged += new System.EventHandler(this.swgRBtn_CheckedChanged);
            // 
            // nwRBtn
            // 
            this.nwRBtn.AutoSize = true;
            this.nwRBtn.Location = new System.Drawing.Point(6, 11);
            this.nwRBtn.Name = "nwRBtn";
            this.nwRBtn.Size = new System.Drawing.Size(119, 17);
            this.nwRBtn.TabIndex = 1;
            this.nwRBtn.TabStop = true;
            this.nwRBtn.Text = "NeedlemanWunsch";
            this.nwRBtn.UseVisualStyleBackColor = true;
            this.nwRBtn.CheckedChanged += new System.EventHandler(this.nwRBtn_CheckedChanged);
            // 
            // saChkBx
            // 
            this.saChkBx.AutoSize = true;
            this.saChkBx.Location = new System.Drawing.Point(132, 102);
            this.saChkBx.Name = "saChkBx";
            this.saChkBx.Size = new System.Drawing.Size(124, 17);
            this.saChkBx.TabIndex = 0;
            this.saChkBx.Text = "Sequence Alignment";
            this.saChkBx.UseVisualStyleBackColor = true;
            this.saChkBx.CheckedChanged += new System.EventHandler(this.saChkBx_CheckedChanged);
            // 
            // pwcChkBx
            // 
            this.pwcChkBx.AutoSize = true;
            this.pwcChkBx.Location = new System.Drawing.Point(132, 160);
            this.pwcChkBx.Name = "pwcChkBx";
            this.pwcChkBx.Size = new System.Drawing.Size(114, 17);
            this.pwcChkBx.TabIndex = 29;
            this.pwcChkBx.Text = "Pairwise Clustering";
            this.pwcChkBx.UseVisualStyleBackColor = true;
            this.pwcChkBx.CheckedChanged += new System.EventHandler(this.pwcChkBx_CheckedChanged);
            // 
            // mdsChkBx
            // 
            this.mdsChkBx.AutoSize = true;
            this.mdsChkBx.Location = new System.Drawing.Point(132, 184);
            this.mdsChkBx.Name = "mdsChkBx";
            this.mdsChkBx.Size = new System.Drawing.Size(140, 17);
            this.mdsChkBx.TabIndex = 30;
            this.mdsChkBx.Text = "MultiDimensionalScaling";
            this.mdsChkBx.UseVisualStyleBackColor = true;
            this.mdsChkBx.CheckedChanged += new System.EventHandler(this.mdsChkBx_CheckedChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Location = new System.Drawing.Point(10, 207);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(728, 2);
            this.groupBox5.TabIndex = 31;
            this.groupBox5.TabStop = false;
            // 
            // mdsConfLink
            // 
            this.mdsConfLink.AutoSize = true;
            this.mdsConfLink.Location = new System.Drawing.Point(271, 185);
            this.mdsConfLink.Name = "mdsConfLink";
            this.mdsConfLink.Size = new System.Drawing.Size(52, 13);
            this.mdsConfLink.TabIndex = 33;
            this.mdsConfLink.TabStop = true;
            this.mdsConfLink.Text = "Configure";
            this.mdsConfLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.mdsConfLink_LinkClicked);
            // 
            // pwcConfLink
            // 
            this.pwcConfLink.AutoSize = true;
            this.pwcConfLink.Location = new System.Drawing.Point(243, 161);
            this.pwcConfLink.Name = "pwcConfLink";
            this.pwcConfLink.Size = new System.Drawing.Size(52, 13);
            this.pwcConfLink.TabIndex = 34;
            this.pwcConfLink.TabStop = true;
            this.pwcConfLink.Text = "Configure";
            this.pwcConfLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.pwcConfLink_LinkClicked);
            // 
            // saConfLink
            // 
            this.saConfLink.AutoSize = true;
            this.saConfLink.Location = new System.Drawing.Point(511, 133);
            this.saConfLink.Name = "saConfLink";
            this.saConfLink.Size = new System.Drawing.Size(52, 13);
            this.saConfLink.TabIndex = 35;
            this.saConfLink.TabStop = true;
            this.saConfLink.Text = "Configure";
            this.saConfLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.saConfLink_LinkClicked);
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(750, 774);
            this.Controls.Add(this.saConfLink);
            this.Controls.Add(this.pwcConfLink);
            this.Controls.Add(this.mdsConfLink);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.mdsChkBx);
            this.Controls.Add(this.pwcChkBx);
            this.Controls.Add(this.saGroup);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.saChkBx);
            this.Controls.Add(this.processBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.remoteRBtn);
            this.Controls.Add(this.localRBtn);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.headNodeCombo);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.targetTxt);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.genSubBtn);
            this.Controls.Add(this.fileBrowseBtn);
            this.Controls.Add(this.inFileTxt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pathBrowseBtn);
            this.Controls.Add(this.directoryTxt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nameTxt);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PetqLight";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.saGroup.ResumeLayout(false);
            this.saGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button pathBrowseBtn;
        private System.Windows.Forms.TextBox directoryTxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox nameTxt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox inFileTxt;
        private System.Windows.Forms.Button fileBrowseBtn;
        private System.Windows.Forms.Button genSubBtn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox targetTxt;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox headNodeCombo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.LinkLabel inverseLink;
        private System.Windows.Forms.LinkLabel selectAllLink;
        private System.Windows.Forms.LinkLabel clearLink;
        private System.Windows.Forms.ListBox nodeBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RadioButton localRBtn;
        private System.Windows.Forms.RadioButton remoteRBtn;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox processBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox saGroup;
        private System.Windows.Forms.RadioButton swmsRBtn;
        private System.Windows.Forms.RadioButton swgRBtn;
        private System.Windows.Forms.RadioButton nwRBtn;
        private System.Windows.Forms.CheckBox saChkBx;
        private System.Windows.Forms.CheckBox pwcChkBx;
        private System.Windows.Forms.CheckBox mdsChkBx;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.LinkLabel mdsConfLink;
        private System.Windows.Forms.LinkLabel pwcConfLink;
        private System.Windows.Forms.LinkLabel saConfLink;
    }
}

