namespace Salsa.TestManager
{
    partial class dlgViewComputeNodes
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.reachableDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.numberOfCoresDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.numberOfSocketsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.memorySizeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cpuSpeedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn,
            this.stateDataGridViewTextBoxColumn,
            this.reachableDataGridViewCheckBoxColumn,
            this.numberOfCoresDataGridViewTextBoxColumn,
            this.numberOfSocketsDataGridViewTextBoxColumn,
            this.memorySizeDataGridViewTextBoxColumn,
            this.cpuSpeedDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.bindingSource1;
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(711, 283);
            this.dataGridView1.TabIndex = 0;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // stateDataGridViewTextBoxColumn
            // 
            this.stateDataGridViewTextBoxColumn.DataPropertyName = "State";
            this.stateDataGridViewTextBoxColumn.HeaderText = "State";
            this.stateDataGridViewTextBoxColumn.Name = "stateDataGridViewTextBoxColumn";
            this.stateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // reachableDataGridViewCheckBoxColumn
            // 
            this.reachableDataGridViewCheckBoxColumn.DataPropertyName = "Reachable";
            this.reachableDataGridViewCheckBoxColumn.HeaderText = "Reachable";
            this.reachableDataGridViewCheckBoxColumn.Name = "reachableDataGridViewCheckBoxColumn";
            this.reachableDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // numberOfCoresDataGridViewTextBoxColumn
            // 
            this.numberOfCoresDataGridViewTextBoxColumn.DataPropertyName = "NumberOfCores";
            this.numberOfCoresDataGridViewTextBoxColumn.HeaderText = "NumberOfCores";
            this.numberOfCoresDataGridViewTextBoxColumn.Name = "numberOfCoresDataGridViewTextBoxColumn";
            this.numberOfCoresDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // numberOfSocketsDataGridViewTextBoxColumn
            // 
            this.numberOfSocketsDataGridViewTextBoxColumn.DataPropertyName = "NumberOfSockets";
            this.numberOfSocketsDataGridViewTextBoxColumn.HeaderText = "NumberOfSockets";
            this.numberOfSocketsDataGridViewTextBoxColumn.Name = "numberOfSocketsDataGridViewTextBoxColumn";
            this.numberOfSocketsDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // memorySizeDataGridViewTextBoxColumn
            // 
            this.memorySizeDataGridViewTextBoxColumn.DataPropertyName = "MemorySize";
            this.memorySizeDataGridViewTextBoxColumn.HeaderText = "MemorySize";
            this.memorySizeDataGridViewTextBoxColumn.Name = "memorySizeDataGridViewTextBoxColumn";
            this.memorySizeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // cpuSpeedDataGridViewTextBoxColumn
            // 
            this.cpuSpeedDataGridViewTextBoxColumn.DataPropertyName = "CpuSpeed";
            this.cpuSpeedDataGridViewTextBoxColumn.HeaderText = "CpuSpeed";
            this.cpuSpeedDataGridViewTextBoxColumn.Name = "cpuSpeedDataGridViewTextBoxColumn";
            this.cpuSpeedDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataSource = typeof(Microsoft.Hpc.Scheduler.ISchedulerNode);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(648, 309);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // dlgViewComputeNodes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(735, 344);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.dataGridView1);
            this.Name = "dlgViewComputeNodes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Compute Node Information";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn stateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn reachableDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn numberOfCoresDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn numberOfSocketsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn memorySizeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cpuSpeedDataGridViewTextBoxColumn;
    }
}