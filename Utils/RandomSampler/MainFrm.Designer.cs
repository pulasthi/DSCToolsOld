namespace RandomSampler
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
            this.inBrowseBtn = new System.Windows.Forms.Button();
            this.inPathTxt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.sizeBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.outPathBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.genBtn = new System.Windows.Forms.Button();
            this.outBrowseBtn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lenGtEqTxt = new System.Windows.Forms.TextBox();
            this.lenLtEqTxt = new System.Windows.Forms.TextBox();
            this.excludeTxt = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "FASTA Input:";
            // 
            // inBrowseBtn
            // 
            this.inBrowseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.inBrowseBtn.Location = new System.Drawing.Point(516, 3);
            this.inBrowseBtn.Name = "inBrowseBtn";
            this.inBrowseBtn.Size = new System.Drawing.Size(32, 23);
            this.inBrowseBtn.TabIndex = 4;
            this.inBrowseBtn.Text = "...";
            this.inBrowseBtn.UseVisualStyleBackColor = true;
            this.inBrowseBtn.Click += new System.EventHandler(this.BrowseBtnClick);
            // 
            // inPathTxt
            // 
            this.inPathTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inPathTxt.Location = new System.Drawing.Point(108, 6);
            this.inPathTxt.Name = "inPathTxt";
            this.inPathTxt.Size = new System.Drawing.Size(402, 20);
            this.inPathTxt.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Sample Sizes:";
            // 
            // sizeBox
            // 
            this.sizeBox.Location = new System.Drawing.Point(108, 36);
            this.sizeBox.Name = "sizeBox";
            this.sizeBox.Size = new System.Drawing.Size(402, 20);
            this.sizeBox.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Output Directory:";
            // 
            // outPathBox
            // 
            this.outPathBox.Location = new System.Drawing.Point(108, 68);
            this.outPathBox.Name = "outPathBox";
            this.outPathBox.Size = new System.Drawing.Size(402, 20);
            this.outPathBox.TabIndex = 8;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Location = new System.Drawing.Point(15, 186);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(533, 2);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            // 
            // genBtn
            // 
            this.genBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.genBtn.Location = new System.Drawing.Point(473, 194);
            this.genBtn.Name = "genBtn";
            this.genBtn.Size = new System.Drawing.Size(75, 23);
            this.genBtn.TabIndex = 14;
            this.genBtn.Text = "Generate";
            this.genBtn.UseVisualStyleBackColor = true;
            this.genBtn.Click += new System.EventHandler(this.GenBtnClick);
            // 
            // outBrowseBtn
            // 
            this.outBrowseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.outBrowseBtn.Location = new System.Drawing.Point(516, 66);
            this.outBrowseBtn.Name = "outBrowseBtn";
            this.outBrowseBtn.Size = new System.Drawing.Size(32, 23);
            this.outBrowseBtn.TabIndex = 15;
            this.outBrowseBtn.Text = "...";
            this.outBrowseBtn.UseVisualStyleBackColor = true;
            this.outBrowseBtn.Click += new System.EventHandler(this.OutBrowseBtnClick);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Length >=";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 125);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Length <=";
            // 
            // lenGtEqTxt
            // 
            this.lenGtEqTxt.Location = new System.Drawing.Point(108, 97);
            this.lenGtEqTxt.Name = "lenGtEqTxt";
            this.lenGtEqTxt.Size = new System.Drawing.Size(100, 20);
            this.lenGtEqTxt.TabIndex = 18;
            this.lenGtEqTxt.Text = "500";
            // 
            // lenLtEqTxt
            // 
            this.lenLtEqTxt.Location = new System.Drawing.Point(108, 122);
            this.lenLtEqTxt.Name = "lenLtEqTxt";
            this.lenLtEqTxt.Size = new System.Drawing.Size(100, 20);
            this.lenLtEqTxt.TabIndex = 19;
            this.lenLtEqTxt.Text = "5000";
            // 
            // excludeTxt
            // 
            this.excludeTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.excludeTxt.Location = new System.Drawing.Point(108, 147);
            this.excludeTxt.Name = "excludeTxt";
            this.excludeTxt.Size = new System.Drawing.Size(402, 20);
            this.excludeTxt.TabIndex = 21;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 150);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(84, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "Exclude seqs in:";
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 229);
            this.Controls.Add(this.excludeTxt);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lenLtEqTxt);
            this.Controls.Add(this.lenGtEqTxt);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.outBrowseBtn);
            this.Controls.Add(this.genBtn);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.outPathBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.sizeBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.inBrowseBtn);
            this.Controls.Add(this.inPathTxt);
            this.Controls.Add(this.label1);
            this.Name = "MainFrm";
            this.Text = "RandomSampler";
            this.Load += new System.EventHandler(this.MainFrmLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button inBrowseBtn;
        private System.Windows.Forms.TextBox inPathTxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox sizeBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox outPathBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button genBtn;
        private System.Windows.Forms.Button outBrowseBtn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox lenGtEqTxt;
        private System.Windows.Forms.TextBox lenLtEqTxt;
        private System.Windows.Forms.TextBox excludeTxt;
        private System.Windows.Forms.Label label6;
    }
}