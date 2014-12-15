using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Xml.Serialization;
using System.IO;

namespace ConfigLib
{
    //[xmln
    public class ExConfig
    {
        //[XmlAttribute(
        public string Param1
        {
            get;
            set;
        }

        public string Param2
        {
            get;
            set;
        }

        public static void Serialize(ExConfig instance, string xmlFilePath)
        {
            Stream stream;
            XmlSerializer x = new XmlSerializer(typeof(ExConfig));

            //ExConfig result = x.Deserialize(stream) as ExConfig;
        }
    }

    public class Tester
    {
        public static void Main(string[] args)
        {
            ExConfig ec = new ExConfig();
            ec.Param1 = "hello";
            Console.WriteLine(ec.Param1);
        }
    }
}
