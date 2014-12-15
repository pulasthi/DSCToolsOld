using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BackgroundWorkerTester
{
    class State
    {
        private string _txt;

        public State(string txt)
        {
            _txt = txt;
        }

        public string Txt
        {
            get { return _txt; }
            set { _txt = value; }
        }
    }
}
