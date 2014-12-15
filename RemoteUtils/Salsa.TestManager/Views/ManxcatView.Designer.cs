namespace Salsa.TestManager.Views
{
    partial class ManxcatView
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
            this.txtControlPath = new System.Windows.Forms.TextBox();
            this.bindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.txtJobName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtApplicationPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // txtControlPath
            // 
            this.txtControlPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtControlPath.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "ControlFilePath", true));
            this.txtControlPath.Location = new System.Drawing.Point(134, 63);
            this.txtControlPath.Name = "txtControlPath";
            this.txtControlPath.Size = new System.Drawing.Size(681, 20);
            this.txtControlPath.TabIndex = 27;
            // 
            // bindingSource
            // 
            this.bindingSource.DataSource = typeof(Salsa.TestManager.Models.ManxcatJob);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 26;
            this.label2.Text = "Control File Path:";
            // 
            // txtJobName
            // 
            this.txtJobName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "Name", true));
            this.txtJobName.Location = new System.Drawing.Point(134, 11);
            this.txtJobName.Name = "txtJobName";
            this.txtJobName.Size = new System.Drawing.Size(393, 20);
            this.txtJobName.TabIndex = 23;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "Job Name:";
            // 
            // txtApplicationPath
            // 
            this.txtApplicationPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtApplicationPath.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "ExecutableFilePath", true));
            this.txtApplicationPath.Location = new System.Drawing.Point(134, 37);
            this.txtApplicationPath.Name = "txtApplicationPath";
            this.txtApplicationPath.Size = new System.Drawing.Size(681, 20);
            this.txtApplicationPath.TabIndex = 25;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "Executable File Path:";
            // 
            // ManxcatView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtControlPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtJobName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtApplicationPath);
            this.Controls.Add(this.label1);
            this.Name = "ManxcatView";
            this.Padding = new System.Windows.Forms.Padding(8);
            this.Size = new System.Drawing.Size(826, 96);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtControlPath;
        private System.Windows.Forms.BindingSource bindingSource;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtJobName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtApplicationPath;
        private System.Windows.Forms.Label label1;
    }
}
