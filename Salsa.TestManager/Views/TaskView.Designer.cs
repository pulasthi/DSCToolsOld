namespace Salsa.TestManager.Views
{
    partial class TaskView
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.processesPerNodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.threadsPerProcessDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.parallelismDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MaxProcessesRequired = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.nameDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.numberOfCoresDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.numberOfSocketsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.memorySizeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cpuSpeedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.requiredNodesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.requiredNodesBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn,
            this.processesPerNodeDataGridViewTextBoxColumn,
            this.threadsPerProcessDataGridViewTextBoxColumn,
            this.parallelismDataGridViewTextBoxColumn,
            this.MaxProcessesRequired});
            this.dataGridView1.DataSource = this.bindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(938, 214);
            this.dataGridView1.TabIndex = 6;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // processesPerNodeDataGridViewTextBoxColumn
            // 
            this.processesPerNodeDataGridViewTextBoxColumn.DataPropertyName = "ProcessesPerNode";
            this.processesPerNodeDataGridViewTextBoxColumn.HeaderText = "ProcessesPerNode";
            this.processesPerNodeDataGridViewTextBoxColumn.Name = "processesPerNodeDataGridViewTextBoxColumn";
            // 
            // threadsPerProcessDataGridViewTextBoxColumn
            // 
            this.threadsPerProcessDataGridViewTextBoxColumn.DataPropertyName = "ThreadsPerProcess";
            this.threadsPerProcessDataGridViewTextBoxColumn.HeaderText = "ThreadsPerProcess";
            this.threadsPerProcessDataGridViewTextBoxColumn.Name = "threadsPerProcessDataGridViewTextBoxColumn";
            // 
            // parallelismDataGridViewTextBoxColumn
            // 
            this.parallelismDataGridViewTextBoxColumn.DataPropertyName = "Parallelism";
            this.parallelismDataGridViewTextBoxColumn.HeaderText = "Parallelism";
            this.parallelismDataGridViewTextBoxColumn.Name = "parallelismDataGridViewTextBoxColumn";
            this.parallelismDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // MaxProcessesRequired
            // 
            this.MaxProcessesRequired.DataPropertyName = "MaxProcessesRequired";
            this.MaxProcessesRequired.HeaderText = "MaxProcessesRequired";
            this.MaxProcessesRequired.Name = "MaxProcessesRequired";
            this.MaxProcessesRequired.ReadOnly = true;
            // 
            // bindingSource
            // 
            this.bindingSource.DataSource = typeof(Salsa.TestManager.Models.BaseTask);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dataGridView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView2);
            this.splitContainer1.Size = new System.Drawing.Size(938, 422);
            this.splitContainer1.SplitterDistance = 214;
            this.splitContainer1.TabIndex = 7;
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AutoGenerateColumns = false;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn1,
            this.numberOfCoresDataGridViewTextBoxColumn,
            this.numberOfSocketsDataGridViewTextBoxColumn,
            this.memorySizeDataGridViewTextBoxColumn,
            this.cpuSpeedDataGridViewTextBoxColumn});
            this.dataGridView2.DataSource = this.requiredNodesBindingSource;
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView2.Location = new System.Drawing.Point(0, 0);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.Size = new System.Drawing.Size(938, 204);
            this.dataGridView2.TabIndex = 0;
            // 
            // nameDataGridViewTextBoxColumn1
            // 
            this.nameDataGridViewTextBoxColumn1.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn1.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn1.Name = "nameDataGridViewTextBoxColumn1";
            this.nameDataGridViewTextBoxColumn1.ReadOnly = true;
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
            // requiredNodesBindingSource
            // 
            this.requiredNodesBindingSource.DataMember = "RequiredNodes";
            this.requiredNodesBindingSource.DataSource = this.bindingSource;
            // 
            // TaskView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "TaskView";
            this.Size = new System.Drawing.Size(938, 422);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.requiredNodesBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn processesPerNodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn threadsPerProcessDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn parallelismDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource bindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn MaxProcessesRequired;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.BindingSource requiredNodesBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn numberOfCoresDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn numberOfSocketsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn memorySizeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cpuSpeedDataGridViewTextBoxColumn;
    }
}
