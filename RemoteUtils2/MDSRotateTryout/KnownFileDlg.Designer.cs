namespace MDSTryout
{
    partial class KnownFileDlg
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
            this.rightLineGrpBx = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.relativePathTxt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.refKeyTxt = new System.Windows.Forms.TextBox();
            this.targetDirGroupBx = new System.Windows.Forms.GroupBox();
            this.descriptionTxt = new System.Windows.Forms.TextBox();
            this.addBtn = new System.Windows.Forms.Button();
            this.targetDirTxt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.headNodeCombo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.targetDirGroupBx.SuspendLayout();
            this.SuspendLayout();
            // 
            // rightLineGrpBx
            // 
            this.rightLineGrpBx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rightLineGrpBx.Location = new System.Drawing.Point(15, 59);
            this.rightLineGrpBx.Name = "rightLineGrpBx";
            this.rightLineGrpBx.Size = new System.Drawing.Size(603, 2);
            this.rightLineGrpBx.TabIndex = 25;
            this.rightLineGrpBx.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 26;
            this.label3.Text = "Relative Path:";
            // 
            // relativePathTxt
            // 
            this.relativePathTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.relativePathTxt.Location = new System.Drawing.Point(105, 65);
            this.relativePathTxt.Name = "relativePathTxt";
            this.relativePathTxt.Size = new System.Drawing.Size(513, 20);
            this.relativePathTxt.TabIndex = 27;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 13);
            this.label4.TabIndex = 28;
            this.label4.Text = "Reference Key";
            // 
            // refKeyTxt
            // 
            this.refKeyTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.refKeyTxt.Location = new System.Drawing.Point(105, 91);
            this.refKeyTxt.Name = "refKeyTxt";
            this.refKeyTxt.Size = new System.Drawing.Size(513, 20);
            this.refKeyTxt.TabIndex = 29;
            // 
            // targetDirGroupBx
            // 
            this.targetDirGroupBx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.targetDirGroupBx.Controls.Add(this.descriptionTxt);
            this.targetDirGroupBx.Location = new System.Drawing.Point(18, 117);
            this.targetDirGroupBx.Name = "targetDirGroupBx";
            this.targetDirGroupBx.Size = new System.Drawing.Size(600, 100);
            this.targetDirGroupBx.TabIndex = 30;
            this.targetDirGroupBx.TabStop = false;
            this.targetDirGroupBx.Text = "Description [optional]";
            // 
            // descriptionTxt
            // 
            this.descriptionTxt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.descriptionTxt.Location = new System.Drawing.Point(6, 19);
            this.descriptionTxt.Multiline = true;
            this.descriptionTxt.Name = "descriptionTxt";
            this.descriptionTxt.Size = new System.Drawing.Size(588, 75);
            this.descriptionTxt.TabIndex = 0;
            // 
            // addBtn
            // 
            this.addBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.addBtn.Location = new System.Drawing.Point(543, 230);
            this.addBtn.Name = "addBtn";
            this.addBtn.Size = new System.Drawing.Size(75, 23);
            this.addBtn.TabIndex = 31;
            this.addBtn.Text = "Add";
            this.addBtn.UseVisualStyleBackColor = true;
            this.addBtn.Click += new System.EventHandler(this.addBtn_Click);
            // 
            // targetDirTxt
            // 
            this.targetDirTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.targetDirTxt.Location = new System.Drawing.Point(105, 33);
            this.targetDirTxt.Name = "targetDirTxt";
            this.targetDirTxt.Size = new System.Drawing.Size(513, 20);
            this.targetDirTxt.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Target Directory:";
            // 
            // headNodeCombo
            // 
            this.headNodeCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.headNodeCombo.FormattingEnabled = true;
            this.headNodeCombo.Location = new System.Drawing.Point(105, 6);
            this.headNodeCombo.Name = "headNodeCombo";
            this.headNodeCombo.Size = new System.Drawing.Size(513, 21);
            this.headNodeCombo.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Head Node:";
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(24, 239);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(0, 13);
            this.statusLabel.TabIndex = 32;
            // 
            // KnownFileDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(630, 265);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.addBtn);
            this.Controls.Add(this.targetDirGroupBx);
            this.Controls.Add(this.refKeyTxt);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.relativePathTxt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.rightLineGrpBx);
            this.Controls.Add(this.targetDirTxt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.headNodeCombo);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "KnownFileDlg";
            this.Text = "Add Known File";
            this.targetDirGroupBx.ResumeLayout(false);
            this.targetDirGroupBx.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox rightLineGrpBx;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox relativePathTxt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox refKeyTxt;
        private System.Windows.Forms.GroupBox targetDirGroupBx;
        private System.Windows.Forms.TextBox descriptionTxt;
        private System.Windows.Forms.Button addBtn;
        private System.Windows.Forms.TextBox targetDirTxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox headNodeCombo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label statusLabel;


    }
}