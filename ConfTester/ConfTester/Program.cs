using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Salsa.Core.Configuration;
using Salsa.Core.Configuration.Sections;
using System.Xml.Serialization;
using System.IO;
using System.Collections.ObjectModel;
namespace ConfTester
{
    class Program
    {
        public static string NAME
        {
            get
            {
                return "john";
            }
        }

        static void Main(string[] args)
        {
            ConfigurationMgr mgr = new ConfigurationMgr();
            ManxcatSection manxcat = new ManxcatSection();
            SmithWatermanSection sw = new SmithWatermanSection();
            PairwiseSection pw = new PairwiseSection();

            mgr.Sections.Add(manxcat);
            //manxcat.Comments.Add("a comment");
            //manxcat.Comments.Add("another comment");
            mgr.Sections.Add(pw);
            mgr.Sections.Add(sw);

            mgr.SaveAs("conf.xml");
            mgr = ConfigurationMgr.LoadConfiguration("conf.xml");
            SectionCollection sections = mgr.Sections;
            manxcat = mgr.Sections["Manxcat"] as ManxcatSection;
            Console.WriteLine(manxcat.ReducedVectorOutputFileName);
            Console.WriteLine(manxcat.Comment);
            //foreach (var section in sections)
            //{
            //    Console.WriteLine(section.Name);
                
            //}
           


            
            //Dog rover = new Dog();
            //rover.Name = "Rover";
            //Dog tweeter = new Dog();
            //tweeter.Name = "Tweeter";
            //Cat rose = new Cat();
            //rose.Name = "Rose";

            //Zoo z = new Zoo();
            //z.Animals.Add(rover);
            //z.Animals.Add(tweeter);
            //z.Animals.Add(rose);

            //z.SaveAs("zoo.xml");
            
            
        }
    }

    public interface Animal 
    {
        string Name { get; set; }
    }

    public class Dog : Animal
    {
        private string _name;
        public string Name {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
    }

    public class Cat : Animal
    {
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
    }



    public class Zoo
    {
        //public AnimalCollection Animals;
        public List<Animal> Animals;

        public Zoo()
        {
            //Animals = new AnimalCollection();
            Animals = new List<Animal>();
            
        }

        private XmlAttributeOverrides GetOverrides()
        {
            XmlAttributes attribs = new XmlAttributes();
            attribs.XmlArray = new XmlArrayAttribute("ANIMALS");
            foreach (var animal in Animals)
            {
                attribs.XmlArrayItems.Add(new XmlArrayItemAttribute(animal.GetType()));                
            }

            XmlAttributeOverrides overrides = new XmlAttributeOverrides();
            overrides.Add(typeof(Zoo), "Animals", attribs);
            return overrides;
        }

        private Type[] GetTypes()
        {
            List<Type> types = new List<Type>();
            foreach (var animal in Animals)
            {
                types.Add(animal.GetType());
            }
            return types.ToArray();
        }

        public void SaveAs(string fileName)
        {
            StreamWriter writer = new StreamWriter(fileName);
            XmlSerializer s = new XmlSerializer(typeof(Zoo));
            s.Serialize(writer, this);
            writer.Close();
        }

    }

    public class AnimalCollection : KeyedCollection<string, Animal>
    {
        public AnimalCollection() : base() { }
        protected override string GetKeyForItem(Animal item)
        {
            return item.Name;
        }
    }

}
