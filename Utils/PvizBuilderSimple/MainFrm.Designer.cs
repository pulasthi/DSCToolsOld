namespace PvizBuilderSimple
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
            this.components = new System.ComponentModel.Container();
            this.clusterBrwsBtn = new System.Windows.Forms.Button();
            this.ptsBrwsBtn = new System.Windows.Forms.Button();
            this.clusterTxt = new System.Windows.Forms.TextBox();
            this.ptsTxt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.nameTxt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.autoCheckBx = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.buildBtn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.saveBtn = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.plotDoneLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // clusterBrwsBtn
            // 
            this.clusterBrwsBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.clusterBrwsBtn.Location = new System.Drawing.Point(548, 27);
            this.clusterBrwsBtn.Name = "clusterBrwsBtn";
            this.clusterBrwsBtn.Size = new System.Drawing.Size(38, 23);
            this.clusterBrwsBtn.TabIndex = 15;
            this.clusterBrwsBtn.Text = "...";
            this.clusterBrwsBtn.UseVisualStyleBackColor = true;
            this.clusterBrwsBtn.Click += new System.EventHandler(this.clusterBrwsBtn_Click);
            // 
            // ptsBrwsBtn
            // 
            this.ptsBrwsBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ptsBrwsBtn.Location = new System.Drawing.Point(548, 4);
            this.ptsBrwsBtn.Name = "ptsBrwsBtn";
            this.ptsBrwsBtn.Size = new System.Drawing.Size(38, 23);
            this.ptsBrwsBtn.TabIndex = 14;
            this.ptsBrwsBtn.Text = "...";
            this.ptsBrwsBtn.UseVisualStyleBackColor = true;
            this.ptsBrwsBtn.Click += new System.EventHandler(this.ptsBrwsBtn_Click);
            // 
            // clusterTxt
            // 
            this.clusterTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.clusterTxt.Location = new System.Drawing.Point(113, 29);
            this.clusterTxt.Name = "clusterTxt";
            this.clusterTxt.Size = new System.Drawing.Size(429, 20);
            this.clusterTxt.TabIndex = 11;
            // 
            // ptsTxt
            // 
            this.ptsTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ptsTxt.Location = new System.Drawing.Point(113, 6);
            this.ptsTxt.Name = "ptsTxt";
            this.ptsTxt.Size = new System.Drawing.Size(429, 20);
            this.ptsTxt.TabIndex = 10;
            this.ptsTxt.TextChanged += new System.EventHandler(this.ptsTxt_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Cluster Labels File:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Points File:";
            // 
            // nameTxt
            // 
            this.nameTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.nameTxt.Location = new System.Drawing.Point(113, 52);
            this.nameTxt.Name = "nameTxt";
            this.nameTxt.Size = new System.Drawing.Size(429, 20);
            this.nameTxt.TabIndex = 17;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Name:";
            // 
            // autoCheckBx
            // 
            this.autoCheckBx.AutoSize = true;
            this.autoCheckBx.Checked = true;
            this.autoCheckBx.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoCheckBx.Location = new System.Drawing.Point(548, 55);
            this.autoCheckBx.Name = "autoCheckBx";
            this.autoCheckBx.Size = new System.Drawing.Size(48, 17);
            this.autoCheckBx.TabIndex = 18;
            this.autoCheckBx.Text = "Auto";
            this.autoCheckBx.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(15, 131);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(571, 36);
            this.textBox1.TabIndex = 20;
            this.textBox1.Text = "Note. Points file and cluster file should have same number of points in the same " +
                "order and the point numbers should be identical in them.";
            // 
            // buildBtn
            // 
            this.buildBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buildBtn.Location = new System.Drawing.Point(444, 89);
            this.buildBtn.Name = "buildBtn";
            this.buildBtn.Size = new System.Drawing.Size(142, 23);
            this.buildBtn.TabIndex = 19;
            this.buildBtn.Text = "Save and Show";
            this.buildBtn.UseVisualStyleBackColor = true;
            this.buildBtn.Click += new System.EventHandler(this.buildBtn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Location = new System.Drawing.Point(15, 81);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(571, 2);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            // 
            // saveBtn
            // 
            this.saveBtn.Location = new System.Drawing.Point(363, 89);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(75, 23);
            this.saveBtn.TabIndex = 22;
            this.saveBtn.Text = "Save";
            this.saveBtn.UseVisualStyleBackColor = true;
            this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 3000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // plotDoneLabel
            // 
            this.plotDoneLabel.AutoSize = true;
            this.plotDoneLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.plotDoneLabel.ForeColor = System.Drawing.Color.Teal;
            this.plotDoneLabel.Location = new System.Drawing.Point(242, 89);
            this.plotDoneLabel.Name = "plotDoneLabel";
            this.plotDoneLabel.Size = new System.Drawing.Size(114, 20);
            this.plotDoneLabel.TabIndex = 23;
            this.plotDoneLabel.Text = "Plot Created!";
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(596, 175);
            this.Controls.Add(this.plotDoneLabel);
            this.Controls.Add(this.saveBtn);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.buildBtn);
            this.Controls.Add(this.autoCheckBx);
            this.Controls.Add(this.nameTxt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.clusterBrwsBtn);
            this.Controls.Add(this.ptsBrwsBtn);
            this.Controls.Add(this.clusterTxt);
            this.Controls.Add(this.ptsTxt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "MainFrm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainFrm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button clusterBrwsBtn;
        private System.Windows.Forms.Button ptsBrwsBtn;
        private System.Windows.Forms.TextBox clusterTxt;
        private System.Windows.Forms.TextBox ptsTxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox nameTxt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox autoCheckBx;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button buildBtn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button saveBtn;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label plotDoneLabel;
    }
}

