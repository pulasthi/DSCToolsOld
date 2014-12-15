namespace Salsa.TestManager.Views
{
    partial class SmithwatermanView
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
            this.label2 = new System.Windows.Forms.Label();
            this.txtTimingFilePath = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.txtFastaFilePath = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkUseRange = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.nudOpen = new System.Windows.Forms.NumericUpDown();
            this.nudExtend = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.txtOutputFolder = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOpen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudExtend)).BeginInit();
            this.SuspendLayout();
            // 
            // txtJobName
            // 
            this.txtJobName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "Name", true));
            this.txtJobName.Location = new System.Drawing.Point(134, 11);
            this.txtJobName.Name = "txtJobName";
            this.txtJobName.Size = new System.Drawing.Size(238, 20);
            this.txtJobName.TabIndex = 1;
            // 
            // bindingSource
            // 
            this.bindingSource.DataSource = typeof(Salsa.TestManager.Models.SmithwatermanJob);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Job Name:";
            // 
            // txtApplicationPath
            // 
            this.txtApplicationPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtApplicationPath.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "ExecutableFilePath", true));
            this.txtApplicationPath.Location = new System.Drawing.Point(134, 40);
            this.txtApplicationPath.Name = "txtApplicationPath";
            this.txtApplicationPath.Size = new System.Drawing.Size(462, 20);
            this.txtApplicationPath.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Executable File Path:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Timing File Path:";
            // 
            // txtTimingFilePath
            // 
            this.txtTimingFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTimingFilePath.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "TimingFilePath", true));
            this.txtTimingFilePath.Location = new System.Drawing.Point(134, 67);
            this.txtTimingFilePath.Name = "txtTimingFilePath";
            this.txtTimingFilePath.Size = new System.Drawing.Size(462, 20);
            this.txtTimingFilePath.TabIndex = 5;
            // 
            // textBox2
            // 
            this.textBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "StartIndex", true));
            this.textBox2.Location = new System.Drawing.Point(390, 151);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 9;
            // 
            // textBox3
            // 
            this.textBox3.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "EndIndex", true));
            this.textBox3.Location = new System.Drawing.Point(496, 151);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 20);
            this.textBox3.TabIndex = 10;
            // 
            // txtFastaFilePath
            // 
            this.txtFastaFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFastaFilePath.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "FastaFilePath", true));
            this.txtFastaFilePath.Location = new System.Drawing.Point(134, 94);
            this.txtFastaFilePath.Name = "txtFastaFilePath";
            this.txtFastaFilePath.Size = new System.Drawing.Size(462, 20);
            this.txtFastaFilePath.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 97);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "FASTA File Path:";
            // 
            // chkUseRange
            // 
            this.chkUseRange.AutoSize = true;
            this.chkUseRange.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkUseRange.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.bindingSource, "UseRange", true));
            this.chkUseRange.Location = new System.Drawing.Point(303, 153);
            this.chkUseRange.Name = "chkUseRange";
            this.chkUseRange.Size = new System.Drawing.Size(80, 17);
            this.chkUseRange.TabIndex = 8;
            this.chkUseRange.Text = "Use Range";
            this.chkUseRange.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 154);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Gap Open Penalty:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 180);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(117, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Gap Extension Penalty:";
            // 
            // nudOpen
            // 
            this.nudOpen.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.bindingSource, "GapOpenPenalty", true));
            this.nudOpen.Location = new System.Drawing.Point(134, 152);
            this.nudOpen.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudOpen.Name = "nudOpen";
            this.nudOpen.Size = new System.Drawing.Size(120, 20);
            this.nudOpen.TabIndex = 13;
            this.nudOpen.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudExtend
            // 
            this.nudExtend.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.bindingSource, "GapExtensionPenalty", true));
            this.nudExtend.Location = new System.Drawing.Point(134, 178);
            this.nudExtend.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudExtend.Name = "nudExtend";
            this.nudExtend.Size = new System.Drawing.Size(120, 20);
            this.nudExtend.TabIndex = 14;
            this.nudExtend.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 124);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(99, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Output Folder Path:";
            // 
            // txtOutputFolder
            // 
            this.txtOutputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutputFolder.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "OutputFolderPath", true));
            this.txtOutputFolder.Location = new System.Drawing.Point(134, 121);
            this.txtOutputFolder.Name = "txtOutputFolder";
            this.txtOutputFolder.Size = new System.Drawing.Size(462, 20);
            this.txtOutputFolder.TabIndex = 16;
            // 
            // SmithwatermanView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtOutputFolder);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.nudExtend);
            this.Controls.Add(this.nudOpen);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.chkUseRange);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtFastaFilePath);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.txtTimingFilePath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtJobName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtApplicationPath);
            this.Controls.Add(this.label1);
            this.Name = "SmithwatermanView";
            this.Padding = new System.Windows.Forms.Padding(8);
            this.Size = new System.Drawing.Size(607, 212);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOpen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudExtend)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtJobName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtApplicationPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.BindingSource bindingSource;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTimingFilePath;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox txtFastaFilePath;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkUseRange;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown nudOpen;
        private System.Windows.Forms.NumericUpDown nudExtend;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtOutputFolder;
    }
}
