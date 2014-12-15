using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;


namespace XMLTester
{
    //// Class definition.
    //public class MyCustomClass
    //{
    //    // Class members:
    //    // Property.
    //    private int Number { get; set; }

    //    // Method.
    //    public int Multiply(int num)
    //    {
    //        return num * Number;
    //    }

    //    // Instance Constructor.
    //    public MyCustomClass()
    //    {
    //        Number = 0;
    //    }
    //}
    //// Another class definition. This one contains
    //// the Main method, the entry point for the program.
    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        // Create an object of type MyCustomClass.
    //        MyCustomClass myClass = new MyCustomClass();

    //        // Set the value of a public property.


    //        // Call a public method.
    //        int result = myClass.Multiply(4);
    //        Console.WriteLine("result" + result);
    //        Console.Read();
    //    }
    //}

    public class Person
    {
        private String name;
        public String Name
        {
            get
            {
                return name;
            }
            set
            {
                Console.WriteLine("asigning");
                Name = value;
            }
        }
    }
    public class MyInt
    {
        public int x;
    }
    public class Hello
    {
        static void Method(string s)
        {
            s = "changed";
        }

        static void ChangeX(ref int x)
        {
            x = 10;
        }

        static void RefArr(ref int[] arr)
        {
            // Create the array on demand:
            if (arr == null)
            {
                arr = new int[10];
            }
            // Fill the array:
            arr[0] = 1111;
            arr[4] = 5555;

        }
        static void MMain()
        {
            Person person = new Person();
            person.Name = "wow";
            Console.WriteLine(person.Name);
            //string str = "original";
            //Method(str);
            //int x = 0 ;
            //ChangeX(ref x);
            //Console.WriteLine(x);

            // Initialize the array:
            int[] theArray = null;

            // Pass the array using ref:
            RefArr(ref theArray);

            // Display the updated array:
            System.Console.WriteLine("Array elements are:");
            for (int i = 0; i < theArray.Length; i++)
            {
                System.Console.Write(theArray[i] + " ");
            }

            // Keep the console window open in debug mode.

            System.Console.WriteLine("Press any key to exit.");
            System.Console.ReadKey();
        }
        //public static void Main(string [] args)
        //{
        //    foreach (var arg in args)
        //    {
        //        Console.WriteLine(arg);
        //        Console.WriteLine(arg.GetType());
        //    }

        //    int[,,,] numbers = new int[2, 2, 2, 3] { { { { 1, 2, 3 }, { 4, 5, 6 } }, { { 7, 8, 9 }, { 10, 11, 12 } } }, { { { 21, 22, 23 }, { 24, 25, 26 } }, { { 27, 28, 29 }, { 210, 211, 212 } } } };
        //    Console.WriteLine(numbers[1,1,1,1]);
        //    Console.Read();

        //}
    }

    public class Parent
    {
        private String name;
        public virtual String Name
        {
            set
            {
                name = value;
            }
            get
            {
                return name;
            }
        }
    }

    public class Child : Parent
    {
        private String cname;
        public override string Name
        {
            get
            {
                return cname;
            }
            set
            {
                cname = value;
            }
        }
    }



    public class Tester
    {
        public static void Main(string[] args)
        {
            //Child C = new Child();
            //C.Name = "Little John";
            //Console.WriteLine(C.Name);
            //Console.Read();
            TestSuit ts = new TestSuit();
            //string xml = ts.ReadFile("ManxcatControl.txt");
            //ts.WriteToFile("ManxcatControl.xml", xml);
            //string xml = ts.ReadFile("PairwiseClusteringControl.txt");
            //ts.WriteToFile("PairwiseClusteringControl.xml", xml);
            //ts.PlayDOM("Config.xml");
            //ts.PlayDOM2();
            //ts.SerializeConfig();
            //ConfigList cl = ts.DeserializeConfig();
            //Console.WriteLine(cl.GlobalConfig.BaseResultDirectoryName);
            //cl.SayHello();
            AnmialTester at = new AnmialTester();
            //at.TestAnimals();
            at.SerializeAnimals();
        }



    }

    public interface IAnimal
    {
        string Sound
        {
            get;
            set;
        }

        void DoSound();
    }

    public class Dog : IAnimal
    {
        public string Sound
        {
            get;
            set;
        }
        //private string sound;
        //public string Sound
        //{
        //    get 
        //    {
        //        return sound;
        //    }
        //    set
        //    {
        //        sound = value;
        //    }
        //}

        public void DoSound()
        {
            Console.WriteLine("Wow! I am barking ... " + Sound);
        }
    }

    public class Cat : IAnimal
    {
        public string Sound
        {
            get;
            set;
        }
        //private string sound;
        //public string Sound
        //{
        //    get 
        //    {
        //        return sound;
        //    }
        //    set
        //    {
        //        sound = value;
        //    }
        //}

        public void DoSound()
        {
            Console.WriteLine("Wow! I am meowing ... " + Sound);
        }
    }

    public class AnmialTester
    {
        public void TestAnimals()
        {
            IAnimal dog = new Dog();
            dog.Sound = "bow wow booo";
            dog.DoSound();

            IAnimal cat = new Cat();
            cat.Sound = "meaaaaaw";
            cat.DoSound();
        }

        public void SerializeAnimals()
        {
            IAnimal dog = new Dog();
            dog.Sound = "bow wow booo";
            dog.DoSound();

            IAnimal cat = new Cat();
            cat.Sound = "meaaaaaw";
            cat.DoSound();

            XmlSerializer serializer = new XmlSerializer(typeof(IAnimal));
            FileStream fs = new FileStream("animals.xml", FileMode.Create);
            serializer.Serialize(fs, dog);
            //serializer.Serialize(fs, cat);
            fs.Close();
        }
    }
           
        

        
        
        
         
        



    public class ConfigList
    {
        public GlobalConfig GlobalConfig
        {
            get;
            set;
        }

        public void SayHello()
        {
            Console.WriteLine("wow I just learnt to say Hello");
        }
    }

    public class GlobalConfig
    {
        public string DataFileName
        {
            get;
            set;
        }

        public int DataPoints
        {
            get;
            set;
        }

        public int ProcessingOption
        {
            get;
            set;
        }

        public string BaseResultDirectoryName
        {
            get;
            set;
        }

        public string RunSetLabel
        {
            get;
            set;
        }

        public string ControlDirectoryName
        {
            get;
            set;
        }

    }
    public class TestSuit
    {
        public ConfigList DeserializeConfig()
        {
            ConfigList cl;
            XmlSerializer serializer = 
                new XmlSerializer(typeof(ConfigList));
            // To read the file, create a FileStream.
            FileStream fs = 
                new FileStream("test.xml", FileMode.Open);
            // Call the Deserialize method and cast to the object type.
            cl = (ConfigList)serializer.Deserialize(fs);
            return cl;
        }
        public void SerializeConfig()
        {
            ConfigList cl = new ConfigList();

            GlobalConfig gc = new GlobalConfig();
            gc.DataFileName = @"C:\Salsa\Evaluations\Alu35339\data.txt";
            gc.DataPoints = 35339;
            gc.ProcessingOption = 0;
            gc.BaseResultDirectoryName = @"\\madrid-headnode\c$\Salsa\Evaluations\Alu35339\Results";
            gc.RunSetLabel = @"Alu35339";
            gc.ControlDirectoryName = @"\\madrid-headnode\c$\Salsa\Evaluations\Alu35339";
            cl.GlobalConfig = gc;

            XmlSerializer serializer = new XmlSerializer(typeof(ConfigList));
            StreamWriter writer = new StreamWriter("test.xml");
            serializer.Serialize(writer, cl);
            writer.Close();
        }


        public void PlayDOM2()
        {
            XmlDocument xmlDom = new XmlDocument();
            xmlDom.AppendChild(xmlDom.CreateElement("", "books", ""));
            XmlElement xmlRoot = xmlDom.DocumentElement;
            XmlElement xmlBook;
            XmlElement xmlTitle, xmlAuthor, xmlPrice;
            XmlText xmlText;

            xmlBook = xmlDom.CreateElement("", "A", "");
            xmlBook.SetAttribute("property", "", "a");

            xmlTitle = xmlDom.CreateElement("", "B", "");
            xmlText = xmlDom.CreateTextNode("text");
            xmlTitle.AppendChild(xmlText);
            xmlBook.AppendChild(xmlTitle);

            xmlRoot.AppendChild(xmlBook);

            xmlAuthor = xmlDom.CreateElement("", "C", "");
            xmlText = xmlDom.CreateTextNode("textg");
            xmlAuthor.AppendChild(xmlText);
            xmlBook.AppendChild(xmlAuthor);

            xmlPrice = xmlDom.CreateElement("", "D", "");
            xmlText = xmlDom.CreateTextNode("99999");
            xmlPrice.AppendChild(xmlText);
            xmlBook.AppendChild(xmlPrice);

            xmlRoot.AppendChild(xmlBook);

            Console.WriteLine(xmlDom.InnerXml);

            xmlDom.Save("books.xml");

            XmlDocument xmlDom2 = new XmlDocument();
            xmlDom2.Load("books.xml");
            Console.WriteLine(xmlDom2.InnerXml);
        }

        public void PlayDOM(String fileName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);
            XmlElement root = doc.DocumentElement;
            //Console.WriteLine(root.OuterXml + "\n\n");
            XmlNodeList list = root.GetElementsByTagName("component", root.NamespaceURI);
            foreach (XmlNode node in list)
            {
                Console.WriteLine(node.OuterXml + "\n\n");
                Console.WriteLine(node.InnerText + "\n\n");
            }
            Console.WriteLine();
            Console.WriteLine(doc.FirstChild.Name);
        }

        public void WriteToFile(String fileName, String text)
        {
            StreamWriter file = new StreamWriter(fileName);
            file.WriteLine(text);
            file.Close();
        }

        public string ReadFile(String fileName)
        {
            StreamReader file = new StreamReader(fileName);
            string line;
            int index;
            string name;
            string value;
            string xml = "<component name=\"MDS\" xmlns=\"http://pervasivetechnologylabs.iu.edu/components/MDS\">\n" +
                "<params>\n";
            while ((line = file.ReadLine()) != null)
            {
                index = line.IndexOf(":");
                name = line.Substring(0, index);
                value = line.Substring(index + 1);
                xml += ("<" + name + ">" + value + "</" + name + ">\n");
            }
            xml += "</params>\n</component>";
            file.Close();
            Console.WriteLine(xml);
            return xml;

        }

        public void SplitString()
        {
            string s = @"wot\a\facinating\world$oh$really\do$you$think\so";
            s = s.ToUpper();
            string[] arr = s.Split(new char[] { '\\', '$' });
            foreach (string str in arr)
            {
                Console.WriteLine(str);
            }
        }

    }

}
