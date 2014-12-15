namespace DuplicateRemover
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
            this.pathTxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.browseBtn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pureDubCountLb = new System.Windows.Forms.Label();
            this.remBtn = new System.Windows.Forms.Button();
            this.dupBx = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.countLb = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.uniqueCountLb = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.summaryChk = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.outDirBox = new System.Windows.Forms.TextBox();
            this.dirBrowseBtn = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.reducibleUniqsCountLb = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pathTxt
            // 
            this.pathTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pathTxt.Location = new System.Drawing.Point(81, 6);
            this.pathTxt.Name = "pathTxt";
            this.pathTxt.Size = new System.Drawing.Size(776, 20);
            this.pathTxt.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Input File:";
            // 
            // browseBtn
            // 
            this.browseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.browseBtn.Location = new System.Drawing.Point(863, 4);
            this.browseBtn.Name = "browseBtn";
            this.browseBtn.Size = new System.Drawing.Size(32, 23);
            this.browseBtn.TabIndex = 2;
            this.browseBtn.Text = "...";
            this.browseBtn.UseVisualStyleBackColor = true;
            this.browseBtn.Click += new System.EventHandler(this.browseBtn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Location = new System.Drawing.Point(16, 732);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(879, 2);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Pure Duplicates:";
            // 
            // pureDubCountLb
            // 
            this.pureDubCountLb.AutoSize = true;
            this.pureDubCountLb.Location = new System.Drawing.Point(135, 87);
            this.pureDubCountLb.Name = "pureDubCountLb";
            this.pureDubCountLb.Size = new System.Drawing.Size(13, 13);
            this.pureDubCountLb.TabIndex = 5;
            this.pureDubCountLb.Text = "0";
            // 
            // remBtn
            // 
            this.remBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.remBtn.Location = new System.Drawing.Point(820, 740);
            this.remBtn.Name = "remBtn";
            this.remBtn.Size = new System.Drawing.Size(75, 23);
            this.remBtn.TabIndex = 6;
            this.remBtn.Text = "Remove";
            this.remBtn.UseVisualStyleBackColor = true;
            this.remBtn.Click += new System.EventHandler(this.remBtn_Click);
            // 
            // dupBx
            // 
            this.dupBx.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dupBx.FormattingEnabled = true;
            this.dupBx.Location = new System.Drawing.Point(15, 109);
            this.dupBx.Name = "dupBx";
            this.dupBx.Size = new System.Drawing.Size(297, 563);
            this.dupBx.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Total Sequence Count:";
            // 
            // countLb
            // 
            this.countLb.AutoSize = true;
            this.countLb.Location = new System.Drawing.Point(264, 33);
            this.countLb.Name = "countLb";
            this.countLb.Size = new System.Drawing.Size(13, 13);
            this.countLb.TabIndex = 9;
            this.countLb.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 55);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(213, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Total Uniques (including reducible uniques):";
            // 
            // uniqueCountLb
            // 
            this.uniqueCountLb.AutoSize = true;
            this.uniqueCountLb.Location = new System.Drawing.Point(264, 55);
            this.uniqueCountLb.Name = "uniqueCountLb";
            this.uniqueCountLb.Size = new System.Drawing.Size(13, 13);
            this.uniqueCountLb.TabIndex = 11;
            this.uniqueCountLb.Text = "0";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Location = new System.Drawing.Point(12, 78);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(879, 2);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            // 
            // summaryChk
            // 
            this.summaryChk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.summaryChk.AutoSize = true;
            this.summaryChk.Location = new System.Drawing.Point(15, 740);
            this.summaryChk.Name = "summaryChk";
            this.summaryChk.Size = new System.Drawing.Size(122, 17);
            this.summaryChk.TabIndex = 13;
            this.summaryChk.Text = "Create Summary File";
            this.summaryChk.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 709);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Output Dir:";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Location = new System.Drawing.Point(16, 697);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(879, 2);
            this.groupBox3.TabIndex = 15;
            this.groupBox3.TabStop = false;
            // 
            // outDirBox
            // 
            this.outDirBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.outDirBox.Location = new System.Drawing.Point(81, 706);
            this.outDirBox.Name = "outDirBox";
            this.outDirBox.Size = new System.Drawing.Size(776, 20);
            this.outDirBox.TabIndex = 16;
            // 
            // dirBrowseBtn
            // 
            this.dirBrowseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.dirBrowseBtn.Location = new System.Drawing.Point(863, 703);
            this.dirBrowseBtn.Name = "dirBrowseBtn";
            this.dirBrowseBtn.Size = new System.Drawing.Size(32, 23);
            this.dirBrowseBtn.TabIndex = 17;
            this.dirBrowseBtn.Text = "...";
            this.dirBrowseBtn.UseVisualStyleBackColor = true;
            this.dirBrowseBtn.Click += new System.EventHandler(this.dirBrowseBtn_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(183, 87);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(184, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Reducible Uniques (duplicate groups)";
            // 
            // reducibleUniqsCountLb
            // 
            this.reducibleUniqsCountLb.AutoSize = true;
            this.reducibleUniqsCountLb.Location = new System.Drawing.Point(394, 87);
            this.reducibleUniqsCountLb.Name = "reducibleUniqsCountLb";
            this.reducibleUniqsCountLb.Size = new System.Drawing.Size(13, 13);
            this.reducibleUniqsCountLb.TabIndex = 19;
            this.reducibleUniqsCountLb.Text = "0";
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(907, 774);
            this.Controls.Add(this.reducibleUniqsCountLb);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.dirBrowseBtn);
            this.Controls.Add(this.outDirBox);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.summaryChk);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.uniqueCountLb);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.countLb);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dupBx);
            this.Controls.Add(this.remBtn);
            this.Controls.Add(this.pureDubCountLb);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.browseBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pathTxt);
            this.Name = "MainFrm";
            this.Text = "Duplicate Remover";
            this.Load += new System.EventHandler(this.MainFrm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox pathTxt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button browseBtn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label pureDubCountLb;
        private System.Windows.Forms.Button remBtn;
        private System.Windows.Forms.ListBox dupBx;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label countLb;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label uniqueCountLb;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox summaryChk;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox outDirBox;
        private System.Windows.Forms.Button dirBrowseBtn;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label reducibleUniqsCountLb;
    }
}

