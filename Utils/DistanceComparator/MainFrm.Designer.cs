namespace DistanceComparator
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
            this.label1 = new System.Windows.Forms.Label();
            this.distTxt = new System.Windows.Forms.TextBox();
            this.distBrowseBtn = new System.Windows.Forms.Button();
            this.pointsBrowseBtn = new System.Windows.Forms.Button();
            this.pointsTxt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.showPointsBtn = new System.Windows.Forms.Button();
            this.rangeTxt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.colCountTxt = new System.Windows.Forms.TextBox();
            this.dataTypeCombo = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.validateBtn = new System.Windows.Forms.Button();
            this.cmpCheck = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Distance File:";
            // 
            // distTxt
            // 
            this.distTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.distTxt.Location = new System.Drawing.Point(79, 6);
            this.distTxt.Name = "distTxt";
            this.distTxt.Size = new System.Drawing.Size(426, 20);
            this.distTxt.TabIndex = 1;
            // 
            // distBrowseBtn
            // 
            this.distBrowseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.distBrowseBtn.Location = new System.Drawing.Point(511, 4);
            this.distBrowseBtn.Name = "distBrowseBtn";
            this.distBrowseBtn.Size = new System.Drawing.Size(31, 23);
            this.distBrowseBtn.TabIndex = 2;
            this.distBrowseBtn.Text = "...";
            this.distBrowseBtn.UseVisualStyleBackColor = true;
            this.distBrowseBtn.Click += new System.EventHandler(this.distBrowseBtn_Click);
            // 
            // pointsBrowseBtn
            // 
            this.pointsBrowseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pointsBrowseBtn.Location = new System.Drawing.Point(511, 144);
            this.pointsBrowseBtn.Name = "pointsBrowseBtn";
            this.pointsBrowseBtn.Size = new System.Drawing.Size(31, 23);
            this.pointsBrowseBtn.TabIndex = 5;
            this.pointsBrowseBtn.Text = "...";
            this.pointsBrowseBtn.UseVisualStyleBackColor = true;
            this.pointsBrowseBtn.Click += new System.EventHandler(this.pointsBrowseBtn_Click);
            // 
            // pointsTxt
            // 
            this.pointsTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pointsTxt.Location = new System.Drawing.Point(79, 146);
            this.pointsTxt.Name = "pointsTxt";
            this.pointsTxt.Size = new System.Drawing.Size(426, 20);
            this.pointsTxt.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 149);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Points File:";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Location = new System.Drawing.Point(-1, 90);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(549, 2);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            // 
            // showPointsBtn
            // 
            this.showPointsBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.showPointsBtn.Location = new System.Drawing.Point(453, 182);
            this.showPointsBtn.Name = "showPointsBtn";
            this.showPointsBtn.Size = new System.Drawing.Size(90, 23);
            this.showPointsBtn.TabIndex = 12;
            this.showPointsBtn.Text = "Show Points";
            this.showPointsBtn.UseVisualStyleBackColor = true;
            this.showPointsBtn.Click += new System.EventHandler(this.showPointsBtn_Click);
            // 
            // rangeTxt
            // 
            this.rangeTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rangeTxt.Location = new System.Drawing.Point(79, 101);
            this.rangeTxt.Name = "rangeTxt";
            this.rangeTxt.Size = new System.Drawing.Size(120, 20);
            this.rangeTxt.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Point Range:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(2, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Column Count:";
            // 
            // colCountTxt
            // 
            this.colCountTxt.Location = new System.Drawing.Point(79, 33);
            this.colCountTxt.Name = "colCountTxt";
            this.colCountTxt.Size = new System.Drawing.Size(121, 20);
            this.colCountTxt.TabIndex = 16;
            // 
            // dataTypeCombo
            // 
            this.dataTypeCombo.FormattingEnabled = true;
            this.dataTypeCombo.Items.AddRange(new object[] {
            "INT16",
            "UINT16",
            "DOUBLE"});
            this.dataTypeCombo.Location = new System.Drawing.Point(79, 60);
            this.dataTypeCombo.Name = "dataTypeCombo";
            this.dataTypeCombo.Size = new System.Drawing.Size(121, 21);
            this.dataTypeCombo.TabIndex = 17;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(2, 63);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "DataType:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Location = new System.Drawing.Point(-1, 174);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(549, 2);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            // 
            // validateBtn
            // 
            this.validateBtn.Location = new System.Drawing.Point(453, 58);
            this.validateBtn.Name = "validateBtn";
            this.validateBtn.Size = new System.Drawing.Size(90, 23);
            this.validateBtn.TabIndex = 20;
            this.validateBtn.Text = "Validate Matrix";
            this.validateBtn.UseVisualStyleBackColor = true;
            this.validateBtn.Click += new System.EventHandler(this.validateBtn_Click);
            // 
            // cmpCheck
            // 
            this.cmpCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmpCheck.AutoSize = true;
            this.cmpCheck.Location = new System.Drawing.Point(5, 127);
            this.cmpCheck.Name = "cmpCheck";
            this.cmpCheck.Size = new System.Drawing.Size(90, 17);
            this.cmpCheck.TabIndex = 21;
            this.cmpCheck.Text = "Compare with";
            this.cmpCheck.UseVisualStyleBackColor = true;
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 212);
            this.Controls.Add(this.cmpCheck);
            this.Controls.Add(this.validateBtn);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dataTypeCombo);
            this.Controls.Add(this.colCountTxt);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.rangeTxt);
            this.Controls.Add(this.showPointsBtn);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.pointsBrowseBtn);
            this.Controls.Add(this.pointsTxt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.distBrowseBtn);
            this.Controls.Add(this.distTxt);
            this.Controls.Add(this.label1);
            this.Name = "MainFrm";
            this.Text = "Distance Manipulator";
            this.Load += new System.EventHandler(this.MainFrm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox distTxt;
        private System.Windows.Forms.Button distBrowseBtn;
        private System.Windows.Forms.Button pointsBrowseBtn;
        private System.Windows.Forms.TextBox pointsTxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button showPointsBtn;
        private System.Windows.Forms.TextBox rangeTxt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox colCountTxt;
        private System.Windows.Forms.ComboBox dataTypeCombo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button validateBtn;
        private System.Windows.Forms.CheckBox cmpCheck;
    }
}

