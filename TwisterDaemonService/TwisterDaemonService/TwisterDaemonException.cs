using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwisterDaemonService
{
    public class TwisterDaemonException : Exception
    {
        public TwisterDaemonException(string message):base(message){}
    }
}
