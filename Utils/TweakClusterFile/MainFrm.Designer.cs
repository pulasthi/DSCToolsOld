namespace TweakClusterFile
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.clusterTxt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buildBtn = new System.Windows.Forms.Button();
            this.tweaksTxtBx = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.inputOneBaseRadioBtn = new System.Windows.Forms.RadioButton();
            this.inputZeroBaseRadioBtn = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.outputOneBaseRadioBtn = new System.Windows.Forms.RadioButton();
            this.outputZeroBaseRadioBtn = new System.Windows.Forms.RadioButton();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.tweaksOneBaseRadioBtn = new System.Windows.Forms.RadioButton();
            this.tweaksZeroBaseRadioBtn = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Location = new System.Drawing.Point(15, 323);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(700, 2);
            this.groupBox2.TabIndex = 30;
            this.groupBox2.TabStop = false;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(677, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(38, 23);
            this.button2.TabIndex = 28;
            this.button2.Text = "...";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // clusterTxt
            // 
            this.clusterTxt.AllowDrop = true;
            this.clusterTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clusterTxt.Location = new System.Drawing.Point(113, 6);
            this.clusterTxt.Name = "clusterTxt";
            this.clusterTxt.Size = new System.Drawing.Size(558, 20);
            this.clusterTxt.TabIndex = 27;
            this.clusterTxt.DragDrop += new System.Windows.Forms.DragEventHandler(this.clusterTxt_DragDrop);
            this.clusterTxt.DragEnter += new System.Windows.Forms.DragEventHandler(this.clusterTxt_DragEnter);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 26;
            this.label2.Text = "Cluster Labels File:";
            // 
            // buildBtn
            // 
            this.buildBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buildBtn.Location = new System.Drawing.Point(640, 332);
            this.buildBtn.Name = "buildBtn";
            this.buildBtn.Size = new System.Drawing.Size(75, 23);
            this.buildBtn.TabIndex = 29;
            this.buildBtn.Text = "Build";
            this.buildBtn.UseVisualStyleBackColor = true;
            this.buildBtn.Click += new System.EventHandler(this.buildBtn_Click);
            // 
            // tweaksTxtBx
            // 
            this.tweaksTxtBx.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tweaksTxtBx.Location = new System.Drawing.Point(6, 19);
            this.tweaksTxtBx.MaxLength = 1000000;
            this.tweaksTxtBx.Multiline = true;
            this.tweaksTxtBx.Name = "tweaksTxtBx";
            this.tweaksTxtBx.Size = new System.Drawing.Size(688, 202);
            this.tweaksTxtBx.TabIndex = 25;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.tweaksTxtBx);
            this.groupBox1.Location = new System.Drawing.Point(15, 43);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(700, 227);
            this.groupBox1.TabIndex = 32;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tweaks";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.inputOneBaseRadioBtn);
            this.groupBox3.Controls.Add(this.inputZeroBaseRadioBtn);
            this.groupBox3.Location = new System.Drawing.Point(15, 277);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(82, 40);
            this.groupBox3.TabIndex = 33;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Input base";
            // 
            // inputOneBaseRadioBtn
            // 
            this.inputOneBaseRadioBtn.AutoSize = true;
            this.inputOneBaseRadioBtn.Checked = true;
            this.inputOneBaseRadioBtn.Location = new System.Drawing.Point(46, 19);
            this.inputOneBaseRadioBtn.Name = "inputOneBaseRadioBtn";
            this.inputOneBaseRadioBtn.Size = new System.Drawing.Size(31, 17);
            this.inputOneBaseRadioBtn.TabIndex = 1;
            this.inputOneBaseRadioBtn.TabStop = true;
            this.inputOneBaseRadioBtn.Text = "1";
            this.inputOneBaseRadioBtn.UseVisualStyleBackColor = true;
            // 
            // inputZeroBaseRadioBtn
            // 
            this.inputZeroBaseRadioBtn.AutoSize = true;
            this.inputZeroBaseRadioBtn.Location = new System.Drawing.Point(7, 20);
            this.inputZeroBaseRadioBtn.Name = "inputZeroBaseRadioBtn";
            this.inputZeroBaseRadioBtn.Size = new System.Drawing.Size(31, 17);
            this.inputZeroBaseRadioBtn.TabIndex = 0;
            this.inputZeroBaseRadioBtn.Text = "0";
            this.inputZeroBaseRadioBtn.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.outputOneBaseRadioBtn);
            this.groupBox4.Controls.Add(this.outputZeroBaseRadioBtn);
            this.groupBox4.Location = new System.Drawing.Point(113, 277);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(82, 40);
            this.groupBox4.TabIndex = 34;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Output base";
            // 
            // outputOneBaseRadioBtn
            // 
            this.outputOneBaseRadioBtn.AutoSize = true;
            this.outputOneBaseRadioBtn.Location = new System.Drawing.Point(46, 19);
            this.outputOneBaseRadioBtn.Name = "outputOneBaseRadioBtn";
            this.outputOneBaseRadioBtn.Size = new System.Drawing.Size(31, 17);
            this.outputOneBaseRadioBtn.TabIndex = 1;
            this.outputOneBaseRadioBtn.Text = "1";
            this.outputOneBaseRadioBtn.UseVisualStyleBackColor = true;
            // 
            // outputZeroBaseRadioBtn
            // 
            this.outputZeroBaseRadioBtn.AutoSize = true;
            this.outputZeroBaseRadioBtn.Checked = true;
            this.outputZeroBaseRadioBtn.Location = new System.Drawing.Point(7, 20);
            this.outputZeroBaseRadioBtn.Name = "outputZeroBaseRadioBtn";
            this.outputZeroBaseRadioBtn.Size = new System.Drawing.Size(31, 17);
            this.outputZeroBaseRadioBtn.TabIndex = 0;
            this.outputZeroBaseRadioBtn.TabStop = true;
            this.outputZeroBaseRadioBtn.Text = "0";
            this.outputZeroBaseRadioBtn.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.tweaksOneBaseRadioBtn);
            this.groupBox5.Controls.Add(this.tweaksZeroBaseRadioBtn);
            this.groupBox5.Location = new System.Drawing.Point(214, 277);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(85, 40);
            this.groupBox5.TabIndex = 35;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Tweaks base";
            // 
            // tweaksOneBaseRadioBtn
            // 
            this.tweaksOneBaseRadioBtn.AutoSize = true;
            this.tweaksOneBaseRadioBtn.Location = new System.Drawing.Point(46, 19);
            this.tweaksOneBaseRadioBtn.Name = "tweaksOneBaseRadioBtn";
            this.tweaksOneBaseRadioBtn.Size = new System.Drawing.Size(31, 17);
            this.tweaksOneBaseRadioBtn.TabIndex = 1;
            this.tweaksOneBaseRadioBtn.Text = "1";
            this.tweaksOneBaseRadioBtn.UseVisualStyleBackColor = true;
            // 
            // tweaksZeroBaseRadioBtn
            // 
            this.tweaksZeroBaseRadioBtn.AutoSize = true;
            this.tweaksZeroBaseRadioBtn.Checked = true;
            this.tweaksZeroBaseRadioBtn.Location = new System.Drawing.Point(7, 20);
            this.tweaksZeroBaseRadioBtn.Name = "tweaksZeroBaseRadioBtn";
            this.tweaksZeroBaseRadioBtn.Size = new System.Drawing.Size(31, 17);
            this.tweaksZeroBaseRadioBtn.TabIndex = 0;
            this.tweaksZeroBaseRadioBtn.TabStop = true;
            this.tweaksZeroBaseRadioBtn.Text = "0";
            this.tweaksZeroBaseRadioBtn.UseVisualStyleBackColor = true;
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(718, 362);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.clusterTxt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buildBtn);
            this.Name = "MainFrm";
            this.Text = "Tweak Cluster File";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox clusterTxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buildBtn;
        private System.Windows.Forms.TextBox tweaksTxtBx;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton inputOneBaseRadioBtn;
        private System.Windows.Forms.RadioButton inputZeroBaseRadioBtn;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton outputOneBaseRadioBtn;
        private System.Windows.Forms.RadioButton outputZeroBaseRadioBtn;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RadioButton tweaksOneBaseRadioBtn;
        private System.Windows.Forms.RadioButton tweaksZeroBaseRadioBtn;
    }
}

