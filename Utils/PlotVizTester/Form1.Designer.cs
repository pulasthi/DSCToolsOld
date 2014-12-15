namespace PlotVizTester
{
    partial class Form1
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.txtCoordianteFile = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnBrowseCoordiateFile = new System.Windows.Forms.Button();
            this.txtClusterFile = new System.Windows.Forms.TextBox();
            this.btnBrowseClusterFile = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtIndexFile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBrowseIndexFile = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(452, 103);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 21;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(371, 103);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 20;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtCoordianteFile
            // 
            this.txtCoordianteFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCoordianteFile.Location = new System.Drawing.Point(87, 72);
            this.txtCoordianteFile.Name = "txtCoordianteFile";
            this.txtCoordianteFile.Size = new System.Drawing.Size(401, 20);
            this.txtCoordianteFile.TabIndex = 18;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Coo&rdiante File:";
            // 
            // btnBrowseCoordiateFile
            // 
            this.btnBrowseCoordiateFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseCoordiateFile.Location = new System.Drawing.Point(495, 70);
            this.btnBrowseCoordiateFile.Name = "btnBrowseCoordiateFile";
            this.btnBrowseCoordiateFile.Size = new System.Drawing.Size(31, 23);
            this.btnBrowseCoordiateFile.TabIndex = 19;
            this.btnBrowseCoordiateFile.Text = "...";
            this.btnBrowseCoordiateFile.UseVisualStyleBackColor = true;
            this.btnBrowseCoordiateFile.Click +=new System.EventHandler(btnBrowseCoordiateFile_Click);
            // 
            // txtClusterFile
            // 
            this.txtClusterFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtClusterFile.Location = new System.Drawing.Point(87, 43);
            this.txtClusterFile.Name = "txtClusterFile";
            this.txtClusterFile.Size = new System.Drawing.Size(401, 20);
            this.txtClusterFile.TabIndex = 15;
            // 
            // btnBrowseClusterFile
            // 
            this.btnBrowseClusterFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseClusterFile.Location = new System.Drawing.Point(495, 40);
            this.btnBrowseClusterFile.Name = "btnBrowseClusterFile";
            this.btnBrowseClusterFile.Size = new System.Drawing.Size(31, 23);
            this.btnBrowseClusterFile.TabIndex = 16;
            this.btnBrowseClusterFile.Text = "...";
            this.btnBrowseClusterFile.UseVisualStyleBackColor = true;
            this.btnBrowseClusterFile.Click+=new System.EventHandler(btnBrowseClusterFile_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "&Index File:";
            // 
            // txtIndexFile
            // 
            this.txtIndexFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtIndexFile.Location = new System.Drawing.Point(87, 12);
            this.txtIndexFile.Name = "txtIndexFile";
            this.txtIndexFile.Size = new System.Drawing.Size(401, 20);
            this.txtIndexFile.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "C&luster File:";
            // 
            // btnBrowseIndexFile
            // 
            this.btnBrowseIndexFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseIndexFile.Location = new System.Drawing.Point(495, 9);
            this.btnBrowseIndexFile.Name = "btnBrowseIndexFile";
            this.btnBrowseIndexFile.Size = new System.Drawing.Size(31, 23);
            this.btnBrowseIndexFile.TabIndex = 13;
            this.btnBrowseIndexFile.Text = "...";
            this.btnBrowseIndexFile.UseVisualStyleBackColor = true;
            this.btnBrowseIndexFile.Click += new System.EventHandler(this.btnBrowseIndexFile_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(543, 182);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtCoordianteFile);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnBrowseCoordiateFile);
            this.Controls.Add(this.txtClusterFile);
            this.Controls.Add(this.btnBrowseClusterFile);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtIndexFile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnBrowseIndexFile);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox txtCoordianteFile;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnBrowseCoordiateFile;
        private System.Windows.Forms.TextBox txtClusterFile;
        private System.Windows.Forms.Button btnBrowseClusterFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtIndexFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBrowseIndexFile;

    }
}

