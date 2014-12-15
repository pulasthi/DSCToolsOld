using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Collections;


namespace XMLTester
{
    class MySD
    {
        static void Main(string[] args)
        {
            Things t = new Things();
            //t.whatever = new object [] {"str", 12, new Bever("rover")};
            //t.StringsAndInts = new object[] { "str", 12};
            t.Bevers = new Bever[] { new Bever("grumpy"), new Bever("clumsy") };
            t.Pets = new ArrayList();
            t.Pets.Add("wow");
            t.Pets.Add(new Bever("funky"));

            // can't serialize hashtable';
            t.HT.Add("keyBever1", new Bever("HTBever1"));
            t.HT.Add("keyBever2", new Bever("HTBever2"));
            t.HT.Add("keyBever3", new Bever("HTBever3"));
            t.HT.Add("keyBeve4", new Bever("HTBever4"));

            t.HT.Add("keyJaguar1", new Bever("HTJaguar1"));
            t.HT.Add("keyJaguar2", new Bever("HTJaguar2"));

            XmlSerializer s = new XmlSerializer(typeof(Things));
            FileStream fs = new FileStream("simple.xml", FileMode.Create);
            s.Serialize(fs, t);
            fs.Close();


        }
    }

    public class Bever
    {
        [XmlAttribute (AttributeName="BeverName")]
        public string Name
        {
            get;
            set;
        }
        public Bever()
        { }

        public Bever(string name)
        {
            Name = name;
        }
    }

    public class Jaguar
    {
        [XmlElement (ElementName = "JaguarName")]
        public string Name {get; set;}
        public Jaguar(string name)
        {
            Name = name;
        }

        public Jaguar()
        { }
    }

    [XmlRoot (ElementName="THINGS")]
    public class Things
    {
        //[XmlElement(DataType = typeof(string)),
        //XmlElement(DataType = typeof(int))]
        //[XmlElement (ElementName = "BEVER"),
        [XmlArrayAttribute(ElementName="ArrayOfBevers"), 
        XmlArrayItem (ElementName="CoolBever", Type=(typeof(Bever)))]
        public Bever[] Bevers;

        [XmlArrayAttribute(ElementName="ListOfPets"),
        XmlArrayItem (ElementName="ChubbyBever", Type=typeof(Bever)),
        XmlArrayItem(ElementName = "JustString", Type = typeof(string))]

        // hmm, how to give this type[] ?
        //Type [] types =new Type[]{typeof(Bever), typeof(Jaguar)};
        
        public ArrayList Pets;

        [XmlIgnore]
        public Hashtable HT = new Hashtable();


    }



}
