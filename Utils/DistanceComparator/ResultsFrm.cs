using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DistanceComparator
{
    public partial class ResultsFrm : Form
    {
       
        public ResultsFrm(RowData[] rows, string txt)
        {
            InitializeComponent();
            dgrid.DataSource = rows;
            dgrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
        }

        public ResultsFrm(NegativeRowData[] rows, string txt)
        {
            InitializeComponent();
            dgrid.DataSource = rows;
            dgrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            txtTxt.Text = txt;
        }

        private void ResultsFrm_Load(object sender, EventArgs e)
        {
            
        }
    }
}
