using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Salsa.TestManager.Views
{
    public partial class PairwiseClusteringView : UserControl
    {
        public PairwiseClusteringView()
        {
            InitializeComponent();
        }

        public object DataSource
        {
            get
            {
                return bindingSource.DataSource;
            }
            set
            {
                bindingSource.DataSource = value;
            }
        }

    }
}
