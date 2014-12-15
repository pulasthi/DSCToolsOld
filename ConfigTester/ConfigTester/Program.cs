using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Salsa.Core.Configuration;

namespace ConfigTester
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigurationMgr mgr = new ConfigurationMgr();
            mgr.SaveAs(@"C:\Users\sekanaya\Desktop\config.xml");
        }
    }
}
