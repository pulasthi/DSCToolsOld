namespace Salsa.TestManager
{
    partial class dlgAddTask
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
            this.btnRemoveAllRequired = new System.Windows.Forms.Button();
            this.btnAddAllRequired = new System.Windows.Forms.Button();
            this.btnRemoveRequired = new System.Windows.Forms.Button();
            this.btnAddRequired = new System.Windows.Forms.Button();
            this.lbxRequireNodes = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lbxComputeNode = new System.Windows.Forms.ListBox();
            this.lblProccessPerNode = new System.Windows.Forms.Label();
            this.txtProcessPerNode = new System.Windows.Forms.TextBox();
            this.txtThreadsPerProcess = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnNodeInfo = new System.Windows.Forms.Button();
            this.lblPattern = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRemoveAllRequired
            // 
            this.btnRemoveAllRequired.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnRemoveAllRequired.Location = new System.Drawing.Point(8, 231);
            this.btnRemoveAllRequired.Name = "btnRemoveAllRequired";
            this.btnRemoveAllRequired.Size = new System.Drawing.Size(44, 23);
            this.btnRemoveAllRequired.TabIndex = 3;
            this.btnRemoveAllRequired.Text = "<<";
            this.btnRemoveAllRequired.UseVisualStyleBackColor = true;
            this.btnRemoveAllRequired.Click += new System.EventHandler(this.btnRemoveAllRequired_Click);
            // 
            // btnAddAllRequired
            // 
            this.btnAddAllRequired.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAddAllRequired.Location = new System.Drawing.Point(8, 23);
            this.btnAddAllRequired.Margin = new System.Windows.Forms.Padding(0);
            this.btnAddAllRequired.Name = "btnAddAllRequired";
            this.btnAddAllRequired.Size = new System.Drawing.Size(44, 23);
            this.btnAddAllRequired.TabIndex = 0;
            this.btnAddAllRequired.Text = ">>";
            this.btnAddAllRequired.UseVisualStyleBackColor = true;
            this.btnAddAllRequired.Click += new System.EventHandler(this.btnAddAllRequired_Click);
            // 
            // btnRemoveRequired
            // 
            this.btnRemoveRequired.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnRemoveRequired.Location = new System.Drawing.Point(8, 161);
            this.btnRemoveRequired.Name = "btnRemoveRequired";
            this.btnRemoveRequired.Size = new System.Drawing.Size(44, 23);
            this.btnRemoveRequired.TabIndex = 2;
            this.btnRemoveRequired.Text = "<";
            this.btnRemoveRequired.UseVisualStyleBackColor = true;
            this.btnRemoveRequired.Click += new System.EventHandler(this.btnRemoveRequired_Click);
            // 
            // btnAddRequired
            // 
            this.btnAddRequired.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAddRequired.Location = new System.Drawing.Point(8, 92);
            this.btnAddRequired.Name = "btnAddRequired";
            this.btnAddRequired.Size = new System.Drawing.Size(44, 23);
            this.btnAddRequired.TabIndex = 1;
            this.btnAddRequired.Text = ">";
            this.btnAddRequired.UseVisualStyleBackColor = true;
            this.btnAddRequired.Click += new System.EventHandler(this.btnAddRequired_Click);
            // 
            // lbxRequireNodes
            // 
            this.lbxRequireNodes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbxRequireNodes.FormattingEnabled = true;
            this.lbxRequireNodes.IntegralHeight = false;
            this.lbxRequireNodes.Location = new System.Drawing.Point(288, 3);
            this.lbxRequireNodes.Name = "lbxRequireNodes";
            this.lbxRequireNodes.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbxRequireNodes.Size = new System.Drawing.Size(219, 272);
            this.lbxRequireNodes.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Threads\\Process:";
            // 
            // lbxComputeNode
            // 
            this.lbxComputeNode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbxComputeNode.FormattingEnabled = true;
            this.lbxComputeNode.IntegralHeight = false;
            this.lbxComputeNode.Location = new System.Drawing.Point(3, 3);
            this.lbxComputeNode.Name = "lbxComputeNode";
            this.lbxComputeNode.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbxComputeNode.Size = new System.Drawing.Size(219, 272);
            this.lbxComputeNode.TabIndex = 0;
            // 
            // lblProccessPerNode
            // 
            this.lblProccessPerNode.AutoSize = true;
            this.lblProccessPerNode.Location = new System.Drawing.Point(12, 44);
            this.lblProccessPerNode.Name = "lblProccessPerNode";
            this.lblProccessPerNode.Size = new System.Drawing.Size(90, 13);
            this.lblProccessPerNode.TabIndex = 2;
            this.lblProccessPerNode.Text = "Processes\\Node:";
            // 
            // txtProcessPerNode
            // 
            this.txtProcessPerNode.Location = new System.Drawing.Point(113, 41);
            this.txtProcessPerNode.Name = "txtProcessPerNode";
            this.txtProcessPerNode.Size = new System.Drawing.Size(198, 20);
            this.txtProcessPerNode.TabIndex = 3;
            this.txtProcessPerNode.Text = "24";
            this.txtProcessPerNode.TextChanged += new System.EventHandler(this.txtProcessPerNode_TextChanged);
            // 
            // txtThreadsPerProcess
            // 
            this.txtThreadsPerProcess.Location = new System.Drawing.Point(113, 15);
            this.txtThreadsPerProcess.Name = "txtThreadsPerProcess";
            this.txtThreadsPerProcess.Size = new System.Drawing.Size(198, 20);
            this.txtThreadsPerProcess.TabIndex = 1;
            this.txtThreadsPerProcess.Text = "1";
            this.txtThreadsPerProcess.TextChanged += new System.EventHandler(this.txtThreadsPerProcess_TextChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(447, 383);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(366, 383);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbxComputeNode, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbxRequireNodes, 2, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 99);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 278F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(510, 278);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.btnAddRequired, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.btnRemoveAllRequired, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.btnRemoveRequired, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.btnAddAllRequired, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(225, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(60, 278);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Required Nodes:";
            // 
            // btnNodeInfo
            // 
            this.btnNodeInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNodeInfo.Location = new System.Drawing.Point(12, 383);
            this.btnNodeInfo.Name = "btnNodeInfo";
            this.btnNodeInfo.Size = new System.Drawing.Size(87, 23);
            this.btnNodeInfo.TabIndex = 5;
            this.btnNodeInfo.Text = "&Node Info";
            this.btnNodeInfo.UseVisualStyleBackColor = true;
            this.btnNodeInfo.Click += new System.EventHandler(this.btnNodeInfo_Click);
            // 
            // lblPattern
            // 
            this.lblPattern.AutoSize = true;
            this.lblPattern.Location = new System.Drawing.Point(339, 32);
            this.lblPattern.Name = "lblPattern";
            this.lblPattern.Size = new System.Drawing.Size(44, 13);
            this.lblPattern.TabIndex = 8;
            this.lblPattern.Text = "Pattern:";
            // 
            // dlgAddTask
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 418);
            this.Controls.Add(this.lblPattern);
            this.Controls.Add(this.btnNodeInfo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.txtProcessPerNode);
            this.Controls.Add(this.txtThreadsPerProcess);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblProccessPerNode);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Name = "dlgAddTask";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create Task";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRemoveAllRequired;
        private System.Windows.Forms.Button btnAddAllRequired;
        private System.Windows.Forms.Button btnRemoveRequired;
        private System.Windows.Forms.Button btnAddRequired;
        private System.Windows.Forms.ListBox lbxRequireNodes;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox lbxComputeNode;
        private System.Windows.Forms.Label lblProccessPerNode;
        private System.Windows.Forms.TextBox txtProcessPerNode;
        private System.Windows.Forms.TextBox txtThreadsPerProcess;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnNodeInfo;
        private System.Windows.Forms.Label lblPattern;
    }
}