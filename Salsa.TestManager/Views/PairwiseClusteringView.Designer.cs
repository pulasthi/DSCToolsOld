namespace Salsa.TestManager.Views
{
    partial class PairwiseClusteringView
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
            this.components = new System.ComponentModel.Container();
            this.txtJobName = new System.Windows.Forms.TextBox();
            this.bindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.txtApplicationPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtControlPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // txtJobName
            // 
            this.txtJobName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "Name", true));
            this.txtJobName.Location = new System.Drawing.Point(124, 11);
            this.txtJobName.Name = "txtJobName";
            this.txtJobName.Size = new System.Drawing.Size(393, 20);
            this.txtJobName.TabIndex = 16;
            // 
            // bindingSource
            // 
            this.bindingSource.DataSource = typeof(Salsa.TestManager.Models.PairwiseClusteringJob);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Job Name:";
            // 
            // txtApplicationPath
            // 
            this.txtApplicationPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtApplicationPath.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "ExecutableFilePath", true));
            this.txtApplicationPath.Location = new System.Drawing.Point(124, 35);
            this.txtApplicationPath.Name = "txtApplicationPath";
            this.txtApplicationPath.Size = new System.Drawing.Size(394, 20);
            this.txtApplicationPath.TabIndex = 18;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Executable File Path:";
            // 
            // txtControlPath
            // 
            this.txtControlPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtControlPath.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "ControlFilePath", true));
            this.txtControlPath.Location = new System.Drawing.Point(124, 61);
            this.txtControlPath.Name = "txtControlPath";
            this.txtControlPath.Size = new System.Drawing.Size(394, 20);
            this.txtControlPath.TabIndex = 21;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "Control File Path:";
            // 
            // PairwiseClusteringView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtControlPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtJobName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtApplicationPath);
            this.Controls.Add(this.label1);
            this.Name = "PairwiseClusteringView";
            this.Padding = new System.Windows.Forms.Padding(8);
            this.Size = new System.Drawing.Size(529, 93);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtJobName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtApplicationPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtControlPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.BindingSource bindingSource;
    }
}
