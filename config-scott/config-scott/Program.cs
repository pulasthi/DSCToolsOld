using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Salsa.Core.Configuration;
namespace config_scott
{
    class Program
    {
        static void Main(string[] args)
        {
            ManxcatConfigurationSection mcs = new ManxcatConfigurationSection();
            PairwiseConfigurationSection pcs = new PairwiseConfigurationSection();

            ConfigurationMgr mgr = new ConfigurationMgr();
            mgr.Sections.Add(mcs);
            mgr.Sections.Add(pcs);

            mgr.Save("conf.xml");
            //mgr = ConfigurationMgr.Load("conf.xml");
            
            
            

        }
    }
}
