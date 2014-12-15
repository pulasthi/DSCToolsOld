namespace PvizBuilder
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ptsTxt = new System.Windows.Forms.TextBox();
            this.clusterTxt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.nameTxt = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.buildBtn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.colorDlg = new System.Windows.Forms.ColorDialog();
            this.label4 = new System.Windows.Forms.Label();
            this.pickColorBtn = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.nonDefTxtBx = new System.Windows.Forms.TextBox();
            this.deletedPointsTxt = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.labelsAsClusterNumbersCheckBx = new System.Windows.Forms.CheckBox();
            this.allNonDefChkBx = new System.Windows.Forms.CheckBox();
            this.incOnlyNonDefChkBx = new System.Windows.Forms.CheckBox();
            this.genNumericLabelsChkBx = new System.Windows.Forms.CheckBox();
            this.writeSimplePointsChkBx = new System.Windows.Forms.CheckBox();
            this.oneToZeroChkBx = new System.Windows.Forms.CheckBox();
            this.outputLabelsEqualToNumbersChkBx = new System.Windows.Forms.CheckBox();
            this.writeLabelsChkBx = new System.Windows.Forms.CheckBox();
            this.button3 = new System.Windows.Forms.Button();
            this.pointsLabelTxt = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.clusterPrefixTxt = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.applyClusterLabelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removePointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Points File:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Cluster Labels File:";
            // 
            // ptsTxt
            // 
            this.ptsTxt.AllowDrop = true;
            this.ptsTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ptsTxt.Location = new System.Drawing.Point(113, 47);
            this.ptsTxt.Name = "ptsTxt";
            this.ptsTxt.Size = new System.Drawing.Size(617, 20);
            this.ptsTxt.TabIndex = 2;
            this.ptsTxt.TextChanged += new System.EventHandler(this.ptsTxt_TextChanged);
            this.ptsTxt.DragDrop += new System.Windows.Forms.DragEventHandler(this.ptsTxt_DragDrop);
            this.ptsTxt.DragEnter += new System.Windows.Forms.DragEventHandler(this.ptsTxt_DragEnter);
            // 
            // clusterTxt
            // 
            this.clusterTxt.AllowDrop = true;
            this.clusterTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clusterTxt.Location = new System.Drawing.Point(113, 70);
            this.clusterTxt.Name = "clusterTxt";
            this.clusterTxt.Size = new System.Drawing.Size(617, 20);
            this.clusterTxt.TabIndex = 3;
            this.clusterTxt.DragDrop += new System.Windows.Forms.DragEventHandler(this.clusterTxt_DragDrop);
            this.clusterTxt.DragEnter += new System.Windows.Forms.DragEventHandler(this.clusterTxt_DragEnter);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Name:";
            // 
            // nameTxt
            // 
            this.nameTxt.AllowDrop = true;
            this.nameTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nameTxt.Location = new System.Drawing.Point(113, 92);
            this.nameTxt.Name = "nameTxt";
            this.nameTxt.Size = new System.Drawing.Size(617, 20);
            this.nameTxt.TabIndex = 5;
            this.nameTxt.DragDrop += new System.Windows.Forms.DragEventHandler(this.nameTxt_DragDrop);
            this.nameTxt.DragEnter += new System.Windows.Forms.DragEventHandler(this.nameTxt_DragEnter);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(736, 45);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(38, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(736, 68);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(38, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "...";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // buildBtn
            // 
            this.buildBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buildBtn.Location = new System.Drawing.Point(699, 539);
            this.buildBtn.Name = "buildBtn";
            this.buildBtn.Size = new System.Drawing.Size(75, 23);
            this.buildBtn.TabIndex = 9;
            this.buildBtn.Text = "Build";
            this.buildBtn.UseVisualStyleBackColor = true;
            this.buildBtn.Click += new System.EventHandler(this.buildBtn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Location = new System.Drawing.Point(15, 510);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(759, 2);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.groupBox5);
            this.groupBox2.Location = new System.Drawing.Point(13, 173);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(759, 2);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Location = new System.Drawing.Point(0, -57);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(759, 2);
            this.groupBox5.TabIndex = 12;
            this.groupBox5.TabStop = false;
            // 
            // colorDlg
            // 
            this.colorDlg.AnyColor = true;
            this.colorDlg.Color = System.Drawing.Color.Silver;
            this.colorDlg.FullOpen = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 184);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Default Color:";
            // 
            // pickColorBtn
            // 
            this.pickColorBtn.Location = new System.Drawing.Point(89, 179);
            this.pickColorBtn.Name = "pickColorBtn";
            this.pickColorBtn.Size = new System.Drawing.Size(36, 23);
            this.pickColorBtn.TabIndex = 13;
            this.pickColorBtn.UseVisualStyleBackColor = true;
            this.pickColorBtn.Click += new System.EventHandler(this.pickColorBtn_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.nonDefTxtBx);
            this.groupBox3.Location = new System.Drawing.Point(15, 245);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(759, 232);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Non Default Cluster Labels (one per line)";
            // 
            // nonDefTxtBx
            // 
            this.nonDefTxtBx.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nonDefTxtBx.Location = new System.Drawing.Point(6, 19);
            this.nonDefTxtBx.MaxLength = 1000000;
            this.nonDefTxtBx.Multiline = true;
            this.nonDefTxtBx.Name = "nonDefTxtBx";
            this.nonDefTxtBx.Size = new System.Drawing.Size(747, 207);
            this.nonDefTxtBx.TabIndex = 0;
            // 
            // deletedPointsTxt
            // 
            this.deletedPointsTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.deletedPointsTxt.Location = new System.Drawing.Point(94, 485);
            this.deletedPointsTxt.Name = "deletedPointsTxt";
            this.deletedPointsTxt.Size = new System.Drawing.Size(678, 20);
            this.deletedPointsTxt.TabIndex = 16;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 488);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Deleted Points:";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(15, 578);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(759, 72);
            this.textBox1.TabIndex = 17;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Location = new System.Drawing.Point(15, 570);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(759, 2);
            this.groupBox4.TabIndex = 18;
            this.groupBox4.TabStop = false;
            // 
            // labelsAsClusterNumbersCheckBx
            // 
            this.labelsAsClusterNumbersCheckBx.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelsAsClusterNumbersCheckBx.AutoSize = true;
            this.labelsAsClusterNumbersCheckBx.Location = new System.Drawing.Point(15, 517);
            this.labelsAsClusterNumbersCheckBx.Name = "labelsAsClusterNumbersCheckBx";
            this.labelsAsClusterNumbersCheckBx.Size = new System.Drawing.Size(200, 17);
            this.labelsAsClusterNumbersCheckBx.TabIndex = 19;
            this.labelsAsClusterNumbersCheckBx.Text = "Use cluster labels as cluster numbers";
            this.labelsAsClusterNumbersCheckBx.UseVisualStyleBackColor = true;
            this.labelsAsClusterNumbersCheckBx.CheckedChanged += new System.EventHandler(this.labelsAsClusterNumbersCheckBx_CheckedChanged);
            // 
            // allNonDefChkBx
            // 
            this.allNonDefChkBx.AutoSize = true;
            this.allNonDefChkBx.Checked = true;
            this.allNonDefChkBx.CheckState = System.Windows.Forms.CheckState.Checked;
            this.allNonDefChkBx.Location = new System.Drawing.Point(13, 213);
            this.allNonDefChkBx.Name = "allNonDefChkBx";
            this.allNonDefChkBx.Size = new System.Drawing.Size(97, 17);
            this.allNonDefChkBx.TabIndex = 20;
            this.allNonDefChkBx.Text = "All Non Default";
            this.allNonDefChkBx.UseVisualStyleBackColor = true;
            this.allNonDefChkBx.CheckedChanged += new System.EventHandler(this.allNonDefChkBx_CheckedChanged);
            // 
            // incOnlyNonDefChkBx
            // 
            this.incOnlyNonDefChkBx.AutoSize = true;
            this.incOnlyNonDefChkBx.Location = new System.Drawing.Point(127, 213);
            this.incOnlyNonDefChkBx.Name = "incOnlyNonDefChkBx";
            this.incOnlyNonDefChkBx.Size = new System.Drawing.Size(128, 17);
            this.incOnlyNonDefChkBx.TabIndex = 21;
            this.incOnlyNonDefChkBx.Text = "Include Only Non Def";
            this.incOnlyNonDefChkBx.UseVisualStyleBackColor = true;
            // 
            // genNumericLabelsChkBx
            // 
            this.genNumericLabelsChkBx.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.genNumericLabelsChkBx.AutoSize = true;
            this.genNumericLabelsChkBx.Location = new System.Drawing.Point(252, 517);
            this.genNumericLabelsChkBx.Name = "genNumericLabelsChkBx";
            this.genNumericLabelsChkBx.Size = new System.Drawing.Size(140, 17);
            this.genNumericLabelsChkBx.TabIndex = 22;
            this.genNumericLabelsChkBx.Text = "Generate numeric labels";
            this.genNumericLabelsChkBx.UseVisualStyleBackColor = true;
            // 
            // writeSimplePointsChkBx
            // 
            this.writeSimplePointsChkBx.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.writeSimplePointsChkBx.AutoSize = true;
            this.writeSimplePointsChkBx.Checked = true;
            this.writeSimplePointsChkBx.CheckState = System.Windows.Forms.CheckState.Checked;
            this.writeSimplePointsChkBx.Location = new System.Drawing.Point(252, 543);
            this.writeSimplePointsChkBx.Name = "writeSimplePointsChkBx";
            this.writeSimplePointsChkBx.Size = new System.Drawing.Size(114, 17);
            this.writeSimplePointsChkBx.TabIndex = 23;
            this.writeSimplePointsChkBx.Text = "Write simple points";
            this.writeSimplePointsChkBx.UseVisualStyleBackColor = true;
            // 
            // oneToZeroChkBx
            // 
            this.oneToZeroChkBx.AutoSize = true;
            this.oneToZeroChkBx.Location = new System.Drawing.Point(261, 213);
            this.oneToZeroChkBx.Name = "oneToZeroChkBx";
            this.oneToZeroChkBx.Size = new System.Drawing.Size(201, 17);
            this.oneToZeroChkBx.TabIndex = 24;
            this.oneToZeroChkBx.Text = "Convert input labels 1 base to 0 base";
            this.oneToZeroChkBx.UseVisualStyleBackColor = true;
            // 
            // outputLabelsEqualToNumbersChkBx
            // 
            this.outputLabelsEqualToNumbersChkBx.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.outputLabelsEqualToNumbersChkBx.AutoSize = true;
            this.outputLabelsEqualToNumbersChkBx.Location = new System.Drawing.Point(15, 543);
            this.outputLabelsEqualToNumbersChkBx.Name = "outputLabelsEqualToNumbersChkBx";
            this.outputLabelsEqualToNumbersChkBx.Size = new System.Drawing.Size(174, 17);
            this.outputLabelsEqualToNumbersChkBx.TabIndex = 26;
            this.outputLabelsEqualToNumbersChkBx.Text = "Output labels = cluster numbers";
            this.outputLabelsEqualToNumbersChkBx.UseVisualStyleBackColor = true;
            // 
            // writeLabelsChkBx
            // 
            this.writeLabelsChkBx.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.writeLabelsChkBx.AutoSize = true;
            this.writeLabelsChkBx.Checked = true;
            this.writeLabelsChkBx.CheckState = System.Windows.Forms.CheckState.Checked;
            this.writeLabelsChkBx.Location = new System.Drawing.Point(427, 517);
            this.writeLabelsChkBx.Name = "writeLabelsChkBx";
            this.writeLabelsChkBx.Size = new System.Drawing.Size(81, 17);
            this.writeLabelsChkBx.TabIndex = 27;
            this.writeLabelsChkBx.Text = "Write labels";
            this.writeLabelsChkBx.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.Location = new System.Drawing.Point(736, 124);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(38, 23);
            this.button3.TabIndex = 30;
            this.button3.Text = "...";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // pointsLabelTxt
            // 
            this.pointsLabelTxt.AllowDrop = true;
            this.pointsLabelTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pointsLabelTxt.Location = new System.Drawing.Point(113, 126);
            this.pointsLabelTxt.Name = "pointsLabelTxt";
            this.pointsLabelTxt.Size = new System.Drawing.Size(617, 20);
            this.pointsLabelTxt.TabIndex = 29;
            this.pointsLabelTxt.DragDrop += new System.Windows.Forms.DragEventHandler(this.pointsLabelTxt_DragDrop);
            this.pointsLabelTxt.DragEnter += new System.Windows.Forms.DragEventHandler(this.pointsLabelTxt_DragEnter);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 129);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 13);
            this.label6.TabIndex = 28;
            this.label6.Text = "Points Label File:";
            // 
            // clusterPrefixTxt
            // 
            this.clusterPrefixTxt.AllowDrop = true;
            this.clusterPrefixTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clusterPrefixTxt.Location = new System.Drawing.Point(113, 148);
            this.clusterPrefixTxt.Name = "clusterPrefixTxt";
            this.clusterPrefixTxt.Size = new System.Drawing.Size(617, 20);
            this.clusterPrefixTxt.TabIndex = 32;
            this.clusterPrefixTxt.Text = "C_";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 151);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 13);
            this.label7.TabIndex = 31;
            this.label7.Text = "Cluster Prefix:";
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox6.Controls.Add(this.groupBox7);
            this.groupBox6.Location = new System.Drawing.Point(13, 118);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(759, 2);
            this.groupBox6.TabIndex = 33;
            this.groupBox6.TabStop = false;
            // 
            // groupBox7
            // 
            this.groupBox7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox7.Location = new System.Drawing.Point(0, -57);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(759, 2);
            this.groupBox7.TabIndex = 12;
            this.groupBox7.TabStop = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(784, 24);
            this.menuStrip1.TabIndex = 34;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.applyClusterLabelsToolStripMenuItem,
            this.removePointsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // applyClusterLabelsToolStripMenuItem
            // 
            this.applyClusterLabelsToolStripMenuItem.Name = "applyClusterLabelsToolStripMenuItem";
            this.applyClusterLabelsToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.applyClusterLabelsToolStripMenuItem.Text = "Apply Points Labels";
            this.applyClusterLabelsToolStripMenuItem.Click += new System.EventHandler(this.applyClusterLabelsToolStripMenuItem_Click);
            // 
            // removePointsToolStripMenuItem
            // 
            this.removePointsToolStripMenuItem.Name = "removePointsToolStripMenuItem";
            this.removePointsToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.removePointsToolStripMenuItem.Text = "Remove Points";
            this.removePointsToolStripMenuItem.Click += new System.EventHandler(this.removePointsToolStripMenuItem_Click);
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 662);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.clusterPrefixTxt);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.pointsLabelTxt);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.writeLabelsChkBx);
            this.Controls.Add(this.oneToZeroChkBx);
            this.Controls.Add(this.labelsAsClusterNumbersCheckBx);
            this.Controls.Add(this.outputLabelsEqualToNumbersChkBx);
            this.Controls.Add(this.writeSimplePointsChkBx);
            this.Controls.Add(this.genNumericLabelsChkBx);
            this.Controls.Add(this.incOnlyNonDefChkBx);
            this.Controls.Add(this.allNonDefChkBx);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.deletedPointsTxt);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.pickColorBtn);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buildBtn);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.nameTxt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.clusterTxt);
            this.Controls.Add(this.ptsTxt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainFrm";
            this.Text = "PvizBuilder";
            this.Load += new System.EventHandler(this.MainFrm_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ptsTxt;
        private System.Windows.Forms.TextBox clusterTxt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox nameTxt;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button buildBtn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ColorDialog colorDlg;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button pickColorBtn;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox nonDefTxtBx;
        private System.Windows.Forms.TextBox deletedPointsTxt;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox labelsAsClusterNumbersCheckBx;
        private System.Windows.Forms.CheckBox allNonDefChkBx;
        private System.Windows.Forms.CheckBox incOnlyNonDefChkBx;
        private System.Windows.Forms.CheckBox genNumericLabelsChkBx;
        private System.Windows.Forms.CheckBox writeSimplePointsChkBx;
        private System.Windows.Forms.CheckBox oneToZeroChkBx;
        private System.Windows.Forms.CheckBox outputLabelsEqualToNumbersChkBx;
        private System.Windows.Forms.CheckBox writeLabelsChkBx;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox pointsLabelTxt;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox clusterPrefixTxt;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem applyClusterLabelsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removePointsToolStripMenuItem;
    }
}

