namespace UtilsManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFrm));
            this.dupRemBtn = new System.Windows.Forms.Button();
            this.randSamplerBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // dupRemBtn
            // 
            this.dupRemBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dupRemBtn.Location = new System.Drawing.Point(12, 12);
            this.dupRemBtn.Name = "dupRemBtn";
            this.dupRemBtn.Size = new System.Drawing.Size(391, 37);
            this.dupRemBtn.TabIndex = 0;
            this.dupRemBtn.Text = "Duplicate Remover";
            this.dupRemBtn.UseVisualStyleBackColor = true;
            this.dupRemBtn.Click += new System.EventHandler(this.dupRemBtn_Click);
            // 
            // randSamplerBtn
            // 
            this.randSamplerBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.randSamplerBtn.Location = new System.Drawing.Point(12, 55);
            this.randSamplerBtn.Name = "randSamplerBtn";
            this.randSamplerBtn.Size = new System.Drawing.Size(391, 37);
            this.randSamplerBtn.TabIndex = 1;
            this.randSamplerBtn.Text = "Random Sampler";
            this.randSamplerBtn.UseVisualStyleBackColor = true;
            this.randSamplerBtn.Click += new System.EventHandler(this.randSamplerBtn_Click);
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 107);
            this.Controls.Add(this.randSamplerBtn);
            this.Controls.Add(this.dupRemBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainFrm";
            this.Text = "Utils Manager";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button dupRemBtn;
        private System.Windows.Forms.Button randSamplerBtn;

    }
}

