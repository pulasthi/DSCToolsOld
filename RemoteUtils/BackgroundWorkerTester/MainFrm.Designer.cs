namespace BackgroundWorkerTester
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
            this.statusTxt = new System.Windows.Forms.TextBox();
            this.startBtn = new System.Windows.Forms.Button();
            this.stopBtn = new System.Windows.Forms.Button();
            this.openJobBtn = new System.Windows.Forms.Button();
            this.jobidTxt = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // statusTxt
            // 
            this.statusTxt.Location = new System.Drawing.Point(21, 91);
            this.statusTxt.Multiline = true;
            this.statusTxt.Name = "statusTxt";
            this.statusTxt.Size = new System.Drawing.Size(230, 159);
            this.statusTxt.TabIndex = 0;
            // 
            // startBtn
            // 
            this.startBtn.Location = new System.Drawing.Point(21, 28);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(75, 23);
            this.startBtn.TabIndex = 1;
            this.startBtn.Text = "Start";
            this.startBtn.UseVisualStyleBackColor = true;
            this.startBtn.Click += new System.EventHandler(this.startBtn_Click);
            // 
            // stopBtn
            // 
            this.stopBtn.Location = new System.Drawing.Point(102, 28);
            this.stopBtn.Name = "stopBtn";
            this.stopBtn.Size = new System.Drawing.Size(75, 23);
            this.stopBtn.TabIndex = 2;
            this.stopBtn.Text = "Stop";
            this.stopBtn.UseVisualStyleBackColor = true;
            this.stopBtn.Click += new System.EventHandler(this.stopBtn_Click);
            // 
            // openJobBtn
            // 
            this.openJobBtn.Location = new System.Drawing.Point(380, 28);
            this.openJobBtn.Name = "openJobBtn";
            this.openJobBtn.Size = new System.Drawing.Size(75, 23);
            this.openJobBtn.TabIndex = 3;
            this.openJobBtn.Text = "OpenJob";
            this.openJobBtn.UseVisualStyleBackColor = true;
            this.openJobBtn.Click += new System.EventHandler(this.openJobBtn_Click);
            // 
            // jobidTxt
            // 
            this.jobidTxt.Location = new System.Drawing.Point(461, 30);
            this.jobidTxt.Name = "jobidTxt";
            this.jobidTxt.Size = new System.Drawing.Size(100, 20);
            this.jobidTxt.TabIndex = 4;
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(679, 345);
            this.Controls.Add(this.jobidTxt);
            this.Controls.Add(this.openJobBtn);
            this.Controls.Add(this.stopBtn);
            this.Controls.Add(this.startBtn);
            this.Controls.Add(this.statusTxt);
            this.Name = "MainFrm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainFrm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox statusTxt;
        private System.Windows.Forms.Button startBtn;
        private System.Windows.Forms.Button stopBtn;
        private System.Windows.Forms.Button openJobBtn;
        private System.Windows.Forms.TextBox jobidTxt;
    }
}

