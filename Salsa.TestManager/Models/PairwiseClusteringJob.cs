using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Salsa.TestManager.Models
{
    public class PairwiseClusteringJob : BaseJob
    {
        private string _controlFilePath = string.Empty;

        internal PairwiseClusteringJob()
        {
        }

        public string ControlFilePath
        {
            get
            {
                return _controlFilePath;
            }
            set
            {
                if (_controlFilePath != value)
                {
                    _controlFilePath = value;
                    OnPropertyChanged("ControlFilePath");
                }
            }
        }
    }
}
