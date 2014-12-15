namespace Test4
{
    partial class ProjectWizard
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.inverseLink = new System.Windows.Forms.LinkLabel();
            this.selectAllLink = new System.Windows.Forms.LinkLabel();
            this.clearLink = new System.Windows.Forms.LinkLabel();
            this.nodeBox = new System.Windows.Forms.ListBox();
            this.bindingSource = new System.Windows.Forms.BindingSource();
            this.label3 = new System.Windows.Forms.Label();
            this.headNodeCombo = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.appCombo = new System.Windows.Forms.ComboBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.changeBtn = new System.Windows.Forms.Button();
            this.inFileBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.remBtn = new System.Windows.Forms.Button();
            this.addBtn = new System.Windows.Forms.Button();
            this.inverseLink2 = new System.Windows.Forms.LinkLabel();
            this.selectAllLink2 = new System.Windows.Forms.LinkLabel();
            this.clearLink2 = new System.Windows.Forms.LinkLabel();
            this.binBox = new System.Windows.Forms.ListBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.changeBtn2 = new System.Windows.Forms.Button();
            this.configBox = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.btnPanel = new System.Windows.Forms.Panel();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.propGrid = new System.Windows.Forms.PropertyGrid();
            this.sectionCombo = new System.Windows.Forms.ComboBox();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Controls.Add(this.tabPage3);
            this.tabControl.Controls.Add(this.tabPage4);
            this.tabControl.Location = new System.Drawing.Point(3, 3);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(654, 706);
            this.tabControl.TabIndex = 2;
            this.tabControl.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl_Selected);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.headNodeCombo);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.appCombo);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(646, 680);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Application Settings";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.inverseLink);
            this.groupBox1.Controls.Add(this.selectAllLink);
            this.groupBox1.Controls.Add(this.clearLink);
            this.groupBox1.Controls.Add(this.nodeBox);
            this.groupBox1.Location = new System.Drawing.Point(115, 72);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(259, 593);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
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
            this.nodeBox.DataSource = this.bindingSource;
            this.nodeBox.DisplayMember = "Name";
            this.nodeBox.FormattingEnabled = true;
            this.nodeBox.Location = new System.Drawing.Point(6, 37);
            this.nodeBox.Name = "nodeBox";
            this.nodeBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.nodeBox.Size = new System.Drawing.Size(247, 550);
            this.nodeBox.TabIndex = 5;
            this.nodeBox.SelectedIndexChanged += new System.EventHandler(this.nodeBox_SelectedIndexChanged);
            // 
            // bindingSource
            // 
            this.bindingSource.DataSource = typeof(Microsoft.Hpc.Scheduler.ISchedulerNode);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Compute Nodes:";
            // 
            // headNodeCombo
            // 
            this.headNodeCombo.FormattingEnabled = true;
            this.headNodeCombo.Location = new System.Drawing.Point(115, 44);
            this.headNodeCombo.Name = "headNodeCombo";
            this.headNodeCombo.Size = new System.Drawing.Size(259, 21);
            this.headNodeCombo.TabIndex = 3;
            this.headNodeCombo.SelectedIndexChanged += new System.EventHandler(this.headNodeCombo_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Select Head Node:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select Application(s):";
            // 
            // appCombo
            // 
            this.appCombo.FormattingEnabled = true;
            this.appCombo.Items.AddRange(new object[] {
            "SWG then PWC and MDS",
            "PWC and MDS",
            "SWG",
            "PWC",
            "MDS"});
            this.appCombo.Location = new System.Drawing.Point(115, 11);
            this.appCombo.Name = "appCombo";
            this.appCombo.Size = new System.Drawing.Size(259, 21);
            this.appCombo.TabIndex = 0;
            this.appCombo.SelectedIndexChanged += new System.EventHandler(this.appCombo_SelectedIndexChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(646, 680);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Files";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 535);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(492, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Note: Files you select in this step will be copied from the original location int" +
                "o project location of the disk";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.changeBtn);
            this.groupBox3.Controls.Add(this.inFileBox);
            this.groupBox3.Location = new System.Drawing.Point(6, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(634, 50);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Input File";
            // 
            // changeBtn
            // 
            this.changeBtn.Location = new System.Drawing.Point(553, 22);
            this.changeBtn.Name = "changeBtn";
            this.changeBtn.Size = new System.Drawing.Size(75, 23);
            this.changeBtn.TabIndex = 1;
            this.changeBtn.Text = "Change ...";
            this.changeBtn.UseVisualStyleBackColor = true;
            this.changeBtn.Click += new System.EventHandler(this.changeBtn_Click);
            // 
            // inFileBox
            // 
            this.inFileBox.Enabled = false;
            this.inFileBox.Location = new System.Drawing.Point(6, 24);
            this.inFileBox.Name = "inFileBox";
            this.inFileBox.Size = new System.Drawing.Size(541, 20);
            this.inFileBox.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.remBtn);
            this.groupBox2.Controls.Add(this.addBtn);
            this.groupBox2.Controls.Add(this.inverseLink2);
            this.groupBox2.Controls.Add(this.selectAllLink2);
            this.groupBox2.Controls.Add(this.clearLink2);
            this.groupBox2.Controls.Add(this.binBox);
            this.groupBox2.Location = new System.Drawing.Point(6, 77);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(634, 440);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Runtime Binaries";
            // 
            // remBtn
            // 
            this.remBtn.Location = new System.Drawing.Point(553, 69);
            this.remBtn.Name = "remBtn";
            this.remBtn.Size = new System.Drawing.Size(75, 23);
            this.remBtn.TabIndex = 11;
            this.remBtn.Text = "Remove";
            this.remBtn.UseVisualStyleBackColor = true;
            this.remBtn.Click += new System.EventHandler(this.remBtn_Click);
            // 
            // addBtn
            // 
            this.addBtn.Location = new System.Drawing.Point(553, 40);
            this.addBtn.Name = "addBtn";
            this.addBtn.Size = new System.Drawing.Size(75, 23);
            this.addBtn.TabIndex = 10;
            this.addBtn.Text = "Add Files ...";
            this.addBtn.UseVisualStyleBackColor = true;
            this.addBtn.Click += new System.EventHandler(this.addBtn_Click);
            // 
            // inverseLink2
            // 
            this.inverseLink2.AutoSize = true;
            this.inverseLink2.Location = new System.Drawing.Point(63, 24);
            this.inverseLink2.Name = "inverseLink2";
            this.inverseLink2.Size = new System.Drawing.Size(42, 13);
            this.inverseLink2.TabIndex = 8;
            this.inverseLink2.TabStop = true;
            this.inverseLink2.Text = "Inverse";
            this.inverseLink2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.inverseLink2_LinkClicked);
            // 
            // selectAllLink2
            // 
            this.selectAllLink2.AutoSize = true;
            this.selectAllLink2.Location = new System.Drawing.Point(6, 24);
            this.selectAllLink2.Name = "selectAllLink2";
            this.selectAllLink2.Size = new System.Drawing.Size(51, 13);
            this.selectAllLink2.TabIndex = 7;
            this.selectAllLink2.TabStop = true;
            this.selectAllLink2.Text = "Select All";
            this.selectAllLink2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.selectAllLink2_LinkClicked);
            // 
            // clearLink2
            // 
            this.clearLink2.AutoSize = true;
            this.clearLink2.Location = new System.Drawing.Point(111, 24);
            this.clearLink2.Name = "clearLink2";
            this.clearLink2.Size = new System.Drawing.Size(31, 13);
            this.clearLink2.TabIndex = 6;
            this.clearLink2.TabStop = true;
            this.clearLink2.Text = "Clear";
            this.clearLink2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.clearLink2_LinkClicked);
            // 
            // binBox
            // 
            this.binBox.FormattingEnabled = true;
            this.binBox.HorizontalScrollbar = true;
            this.binBox.Location = new System.Drawing.Point(6, 40);
            this.binBox.Name = "binBox";
            this.binBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.binBox.Size = new System.Drawing.Size(541, 394);
            this.binBox.TabIndex = 4;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox6);
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Controls.Add(this.groupBox4);
            this.tabPage3.Controls.Add(this.groupBox5);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(646, 680);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Runtime Configuration";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 653);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(492, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Note: Files you select in this step will be copied from the original location int" +
                "o project location of the disk";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.changeBtn2);
            this.groupBox4.Controls.Add(this.configBox);
            this.groupBox4.Location = new System.Drawing.Point(6, 62);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(634, 50);
            this.groupBox4.TabIndex = 16;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Configuration File";
            // 
            // changeBtn2
            // 
            this.changeBtn2.Location = new System.Drawing.Point(553, 22);
            this.changeBtn2.Name = "changeBtn2";
            this.changeBtn2.Size = new System.Drawing.Size(75, 23);
            this.changeBtn2.TabIndex = 1;
            this.changeBtn2.Text = "Change ...";
            this.changeBtn2.UseVisualStyleBackColor = true;
            this.changeBtn2.Click += new System.EventHandler(this.changeBtn2_Click);
            // 
            // configBox
            // 
            this.configBox.Enabled = false;
            this.configBox.Location = new System.Drawing.Point(6, 24);
            this.configBox.Name = "configBox";
            this.configBox.Size = new System.Drawing.Size(541, 20);
            this.configBox.TabIndex = 0;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.textBox2);
            this.groupBox5.Location = new System.Drawing.Point(6, 6);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(634, 50);
            this.groupBox5.TabIndex = 15;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Target Directory";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(6, 24);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(622, 20);
            this.textBox2.TabIndex = 0;
            this.textBox2.Text = "C:\\Salsa\\Evaluations";
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(646, 680);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Summary";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // btnPanel
            // 
            this.btnPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPanel.Location = new System.Drawing.Point(491, 711);
            this.btnPanel.Name = "btnPanel";
            this.btnPanel.Size = new System.Drawing.Size(164, 30);
            this.btnPanel.TabIndex = 5;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label5);
            this.groupBox6.Controls.Add(this.propGrid);
            this.groupBox6.Controls.Add(this.sectionCombo);
            this.groupBox6.Location = new System.Drawing.Point(6, 118);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(634, 522);
            this.groupBox6.TabIndex = 18;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Runtime Properties";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 32;
            this.label5.Text = "Section:";
            // 
            // propGrid
            // 
            this.propGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.propGrid.Location = new System.Drawing.Point(6, 46);
            this.propGrid.Name = "propGrid";
            this.propGrid.Size = new System.Drawing.Size(619, 470);
            this.propGrid.TabIndex = 31;
            this.propGrid.ToolbarVisible = false;
            // 
            // sectionCombo
            // 
            this.sectionCombo.FormattingEnabled = true;
            this.sectionCombo.Location = new System.Drawing.Point(58, 19);
            this.sectionCombo.Name = "sectionCombo";
            this.sectionCombo.Size = new System.Drawing.Size(211, 21);
            this.sectionCombo.TabIndex = 30;
            this.sectionCombo.SelectedIndexChanged += new System.EventHandler(this.sectionCombo_SelectedIndexChanged);            // 
            // ProjectWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnPanel);
            this.Controls.Add(this.tabControl);
            this.Name = "ProjectWizard";
            this.Size = new System.Drawing.Size(655, 745);
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);

        }



        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ComboBox headNodeCombo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox appCombo;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Panel btnPanel;
        private System.Windows.Forms.ListBox nodeBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.BindingSource bindingSource;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.LinkLabel clearLink;
        private System.Windows.Forms.LinkLabel selectAllLink;
        private System.Windows.Forms.LinkLabel inverseLink;
        private System.Windows.Forms.ListBox binBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.LinkLabel clearLink2;
        private System.Windows.Forms.LinkLabel inverseLink2;
        private System.Windows.Forms.LinkLabel selectAllLink2;
        private System.Windows.Forms.Button addBtn;
        private System.Windows.Forms.Button remBtn;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button changeBtn;
        private System.Windows.Forms.TextBox inFileBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button changeBtn2;
        private System.Windows.Forms.TextBox configBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PropertyGrid propGrid;
        private System.Windows.Forms.ComboBox sectionCombo;

    }
}
