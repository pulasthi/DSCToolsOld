namespace Editor_tests
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
            this.tabs = new System.Windows.Forms.TabControl();
            this.tab1 = new System.Windows.Forms.TabPage();
            this.tab2 = new System.Windows.Forms.TabPage();
            this.t1NextBtn = new System.Windows.Forms.Button();
            this.t2NextBtn = new System.Windows.Forms.Button();
            this.t2BackBtn = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tabs.SuspendLayout();
            this.tab1.SuspendLayout();
            this.tab2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabs
            // 
            this.tabs.Controls.Add(this.tab1);
            this.tabs.Controls.Add(this.tab2);
            this.tabs.Location = new System.Drawing.Point(-1, 338);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(292, 313);
            this.tabs.TabIndex = 0;
            // 
            // tab1
            // 
            this.tab1.Controls.Add(this.t1NextBtn);
            this.tab1.Location = new System.Drawing.Point(4, 22);
            this.tab1.Name = "tab1";
            this.tab1.Padding = new System.Windows.Forms.Padding(3);
            this.tab1.Size = new System.Drawing.Size(284, 287);
            this.tab1.TabIndex = 0;
            this.tab1.Text = "tabPage1";
            this.tab1.UseVisualStyleBackColor = true;
            // 
            // tab2
            // 
            this.tab2.Controls.Add(this.t2BackBtn);
            this.tab2.Controls.Add(this.t2NextBtn);
            this.tab2.Location = new System.Drawing.Point(4, 22);
            this.tab2.Name = "tab2";
            this.tab2.Padding = new System.Windows.Forms.Padding(3);
            this.tab2.Size = new System.Drawing.Size(943, 625);
            this.tab2.TabIndex = 1;
            this.tab2.Text = "tabPage2";
            this.tab2.UseVisualStyleBackColor = true;
            // 
            // t1NextBtn
            // 
            this.t1NextBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.t1NextBtn.Location = new System.Drawing.Point(200, 253);
            this.t1NextBtn.Name = "t1NextBtn";
            this.t1NextBtn.Size = new System.Drawing.Size(75, 23);
            this.t1NextBtn.TabIndex = 0;
            this.t1NextBtn.Text = "Next";
            this.t1NextBtn.UseVisualStyleBackColor = true;
            this.t1NextBtn.Click += new System.EventHandler(this.t1NextBtn_Click);
            // 
            // t2NextBtn
            // 
            this.t2NextBtn.Location = new System.Drawing.Point(863, 591);
            this.t2NextBtn.Name = "t2NextBtn";
            this.t2NextBtn.Size = new System.Drawing.Size(75, 23);
            this.t2NextBtn.TabIndex = 1;
            this.t2NextBtn.Text = "Next";
            this.t2NextBtn.UseVisualStyleBackColor = true;
            // 
            // t2BackBtn
            // 
            this.t2BackBtn.Location = new System.Drawing.Point(782, 591);
            this.t2BackBtn.Name = "t2BackBtn";
            this.t2BackBtn.Size = new System.Drawing.Size(75, 23);
            this.t2BackBtn.TabIndex = 2;
            this.t2BackBtn.Text = "Back";
            this.t2BackBtn.UseVisualStyleBackColor = true;
            this.t2BackBtn.Click += new System.EventHandler(this.t2BackBtn_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(279, 181);
            this.panel1.TabIndex = 1;
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(308, 41);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(633, 595);
            this.treeView1.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(866, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Browse";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(308, 15);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(552, 20);
            this.textBox1.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(953, 648);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tabs);
            this.Name = "Form1";
            this.Text = ":)";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabs.ResumeLayout(false);
            this.tab1.ResumeLayout(false);
            this.tab2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage tab1;
        private System.Windows.Forms.Button t1NextBtn;
        private System.Windows.Forms.TabPage tab2;
        private System.Windows.Forms.Button t2BackBtn;
        private System.Windows.Forms.Button t2NextBtn;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;

    }
}

