namespace MDSTryout
{
    partial class PlotVizBuilderDlg
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
            this.coordinatesFileTxt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnBrowseCoordiateFile = new System.Windows.Forms.Button();
            this.clusterFileTxt = new System.Windows.Forms.TextBox();
            this.btnBrowseClusterFile = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.indexFileTxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBrowseIndexFile = new System.Windows.Forms.Button();
            this.topLineGrpBx = new System.Windows.Forms.GroupBox();
            this.bottomLineGrpBx = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.outFileTxt = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(566, 141);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 32;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(485, 141);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 31;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // coordinatesFileTxt
            // 
            this.coordinatesFileTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.coordinatesFileTxt.Location = new System.Drawing.Point(98, 66);
            this.coordinatesFileTxt.Name = "coordinatesFileTxt";
            this.coordinatesFileTxt.Size = new System.Drawing.Size(506, 20);
            this.coordinatesFileTxt.TabIndex = 29;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 28;
            this.label3.Text = "Coordiante File:";
            // 
            // btnBrowseCoordiateFile
            // 
            this.btnBrowseCoordiateFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseCoordiateFile.Location = new System.Drawing.Point(610, 64);
            this.btnBrowseCoordiateFile.Name = "btnBrowseCoordiateFile";
            this.btnBrowseCoordiateFile.Size = new System.Drawing.Size(31, 23);
            this.btnBrowseCoordiateFile.TabIndex = 30;
            this.btnBrowseCoordiateFile.Text = "...";
            this.btnBrowseCoordiateFile.UseVisualStyleBackColor = true;
            this.btnBrowseCoordiateFile.Click += new System.EventHandler(this.btnBrowseCoordiateFile_Click);
            // 
            // clusterFileTxt
            // 
            this.clusterFileTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.clusterFileTxt.Location = new System.Drawing.Point(98, 37);
            this.clusterFileTxt.Name = "clusterFileTxt";
            this.clusterFileTxt.Size = new System.Drawing.Size(506, 20);
            this.clusterFileTxt.TabIndex = 26;
            // 
            // btnBrowseClusterFile
            // 
            this.btnBrowseClusterFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseClusterFile.Location = new System.Drawing.Point(610, 34);
            this.btnBrowseClusterFile.Name = "btnBrowseClusterFile";
            this.btnBrowseClusterFile.Size = new System.Drawing.Size(31, 23);
            this.btnBrowseClusterFile.TabIndex = 27;
            this.btnBrowseClusterFile.Text = "...";
            this.btnBrowseClusterFile.UseVisualStyleBackColor = true;
            this.btnBrowseClusterFile.Click += new System.EventHandler(this.btnBrowseClusterFile_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Index File:";
            // 
            // indexFileTxt
            // 
            this.indexFileTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.indexFileTxt.Location = new System.Drawing.Point(98, 6);
            this.indexFileTxt.Name = "indexFileTxt";
            this.indexFileTxt.Size = new System.Drawing.Size(506, 20);
            this.indexFileTxt.TabIndex = 23;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Cluster File:";
            // 
            // btnBrowseIndexFile
            // 
            this.btnBrowseIndexFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseIndexFile.Location = new System.Drawing.Point(610, 3);
            this.btnBrowseIndexFile.Name = "btnBrowseIndexFile";
            this.btnBrowseIndexFile.Size = new System.Drawing.Size(31, 23);
            this.btnBrowseIndexFile.TabIndex = 24;
            this.btnBrowseIndexFile.Text = "...";
            this.btnBrowseIndexFile.UseVisualStyleBackColor = true;
            this.btnBrowseIndexFile.Click += new System.EventHandler(this.btnBrowseIndexFile_Click);
            // 
            // topLineGrpBx
            // 
            this.topLineGrpBx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.topLineGrpBx.Location = new System.Drawing.Point(-8, 93);
            this.topLineGrpBx.Name = "topLineGrpBx";
            this.topLineGrpBx.Size = new System.Drawing.Size(662, 2);
            this.topLineGrpBx.TabIndex = 33;
            this.topLineGrpBx.TabStop = false;
            // 
            // bottomLineGrpBx
            // 
            this.bottomLineGrpBx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.bottomLineGrpBx.Location = new System.Drawing.Point(-3, 133);
            this.bottomLineGrpBx.Name = "bottomLineGrpBx";
            this.bottomLineGrpBx.Size = new System.Drawing.Size(661, 2);
            this.bottomLineGrpBx.TabIndex = 34;
            this.bottomLineGrpBx.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 106);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 35;
            this.label4.Text = "Output File:";
            // 
            // outFileTxt
            // 
            this.outFileTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.outFileTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.outFileTxt.Location = new System.Drawing.Point(98, 106);
            this.outFileTxt.Name = "outFileTxt";
            this.outFileTxt.ReadOnly = true;
            this.outFileTxt.Size = new System.Drawing.Size(506, 13);
            this.outFileTxt.TabIndex = 36;
            this.outFileTxt.Text = "ReadOnly path to the plot of the most recent run";
            // 
            // PlotVizBuilderDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(653, 174);
            this.Controls.Add(this.outFileTxt);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.bottomLineGrpBx);
            this.Controls.Add(this.topLineGrpBx);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.coordinatesFileTxt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnBrowseCoordiateFile);
            this.Controls.Add(this.clusterFileTxt);
            this.Controls.Add(this.btnBrowseClusterFile);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.indexFileTxt);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnBrowseIndexFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "PlotVizBuilderDlg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PlotVizBuilderDlg";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox coordinatesFileTxt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnBrowseCoordiateFile;
        private System.Windows.Forms.TextBox clusterFileTxt;
        private System.Windows.Forms.Button btnBrowseClusterFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox indexFileTxt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBrowseIndexFile;
        private System.Windows.Forms.GroupBox topLineGrpBx;
        private System.Windows.Forms.GroupBox bottomLineGrpBx;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox outFileTxt;
    }
}