namespace MDSTryout
{
    partial class ConfigDlg
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
            this.cancelBtn = new System.Windows.Forms.Button();
            this.setDefBtn = new System.Windows.Forms.Button();
            this.dirBrowseBtn = new System.Windows.Forms.Button();
            this.dirTxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.okBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cancelBtn
            // 
            this.cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new System.Drawing.Point(350, 47);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 23;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // setDefBtn
            // 
            this.setDefBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.setDefBtn.Location = new System.Drawing.Point(443, 47);
            this.setDefBtn.Name = "setDefBtn";
            this.setDefBtn.Size = new System.Drawing.Size(75, 23);
            this.setDefBtn.TabIndex = 22;
            this.setDefBtn.Text = "Set Default";
            this.setDefBtn.UseVisualStyleBackColor = true;
            this.setDefBtn.Click += new System.EventHandler(this.setDefBtn_Click);
            // 
            // dirBrowseBtn
            // 
            this.dirBrowseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dirBrowseBtn.Location = new System.Drawing.Point(486, 10);
            this.dirBrowseBtn.Name = "dirBrowseBtn";
            this.dirBrowseBtn.Size = new System.Drawing.Size(32, 23);
            this.dirBrowseBtn.TabIndex = 21;
            this.dirBrowseBtn.Text = "...";
            this.dirBrowseBtn.UseVisualStyleBackColor = true;
            this.dirBrowseBtn.Click += new System.EventHandler(this.dirBrowseBtn_Click);
            // 
            // dirTxt
            // 
            this.dirTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dirTxt.Location = new System.Drawing.Point(70, 12);
            this.dirTxt.Name = "dirTxt";
            this.dirTxt.Size = new System.Drawing.Size(410, 20);
            this.dirTxt.TabIndex = 20;
            this.dirTxt.TextChanged += new System.EventHandler(this.dirTxt_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Directory:";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Location = new System.Drawing.Point(4, 39);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(514, 2);
            this.groupBox3.TabIndex = 24;
            this.groupBox3.TabStop = false;
            // 
            // okBtn
            // 
            this.okBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okBtn.Location = new System.Drawing.Point(269, 47);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(75, 23);
            this.okBtn.TabIndex = 25;
            this.okBtn.Text = "OK";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // ConfigDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 77);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.setDefBtn);
            this.Controls.Add(this.dirBrowseBtn);
            this.Controls.Add(this.dirTxt);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "ConfigDlg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Config Dialog";
            this.Load += new System.EventHandler(this.ConfigDlg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Button setDefBtn;
        private System.Windows.Forms.Button dirBrowseBtn;
        private System.Windows.Forms.TextBox dirTxt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button okBtn;
    }
}