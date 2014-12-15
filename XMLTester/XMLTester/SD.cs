using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace XMLTester
{
    public class Orchestra
    {
        public Instrument[] Instruments;
    }

    public class Instrument
    {
        public string Name;
    }

    public class Brass : Instrument
    {
    }

    public class Iron : Instrument
    {
    }

    public class Run
    {
        public static void Main()
        {
            Run test = new Run();
            test.SerializeObject("Override.xml");
            //test.SimpleSerialize("Simple.xml");
            //Orchestra band = test.DeserializeObject("Override.xml");
            //test.SerializeObject("reserialized.xml", band);
            Console.WriteLine("Done");

        }
        private XmlAttributeOverrides GetOverrides()
        {
            /* Each overridden field, property, or type requires 
            an XmlAttributes object. */
            XmlAttributes attrs = new XmlAttributes();

            XmlArrayAttribute arrayAttrib = new XmlArrayAttribute("MusicalInstruments");
            XmlArrayItemAttributes arrayItemAttribs = new XmlArrayItemAttributes();
            arrayItemAttribs.Add(new XmlArrayItemAttribute("BRASS", typeof(Brass)));
            arrayItemAttribs.Add(new XmlArrayItemAttribute("IRON", typeof(Iron)));

            attrs.XmlArray = arrayAttrib;
            foreach (XmlArrayItemAttribute arrayItem in arrayItemAttribs)
            {
                attrs.XmlArrayItems.Add(arrayItem);
            }

            // Create the XmlAttributeOverrides object.
            XmlAttributeOverrides attrOverrides = new XmlAttributeOverrides();

            /* Add the type of the class that contains the overridden 
            member and the XmlAttributes to override it with to the 
            XmlAttributeOverrides object. */
            attrOverrides.Add(typeof(Orchestra), "Instruments", attrs);

            return attrOverrides;
        }

        public void SerializeObject(string filename, Orchestra o)
        {
            //XmlSerializer s =
            //new XmlSerializer(typeof(Orchestra), GetOverrides());

            Type [] types = new Type[2];
            types[0] = typeof (Brass);
            types[1] = typeof (Iron);
            XmlSerializer s = new XmlSerializer(typeof(Orchestra), types);
            // Writing the file requires a TextWriter.
            TextWriter writer = new StreamWriter(filename);
            // Serialize the object.
            s.Serialize(writer, o);
            writer.Close();
 
        }

        public void SerializeObject(string filename)
        {
            /* Each overridden field, property, or type requires 
            an XmlAttributes object. */
            //XmlAttributes attrs = new XmlAttributes();

            //XmlArrayAttribute arrayAttrib = new XmlArrayAttribute("MusicalInstruments");
            //XmlArrayItemAttributes arrayItemAttribs = new XmlArrayItemAttributes();
            //arrayItemAttribs.Add(new XmlArrayItemAttribute("BRASS", typeof(Brass)));
            //arrayItemAttribs.Add(new XmlArrayItemAttribute("IRON", typeof(Iron)));

            //attrs.XmlArray = arrayAttrib;
            //foreach (XmlArrayItemAttribute arrayItem in arrayItemAttribs)
            //{
            //    attrs.XmlArrayItems.Add(arrayItem);
            //}
            
            
            ///* Create an XmlElementAttribute to override the 
            //field that returns Instrument objects. The overridden field
            //returns Brass objects instead. */
            //XmlElementAttribute attr = new XmlElementAttribute();
            //attr.ElementName = "Brass";
            //attr.Type = typeof(Brass);
            

            //// Add the element to the collection of elements.
            //attrs.XmlElements.Add(attr);

            //attr = new XmlElementAttribute();
            //attr.ElementName = "Iron";
            //attr.Type = typeof(Iron);

            //attrs.XmlElements.Add(attr);

            // Create the XmlAttributeOverrides object.
            //XmlAttributeOverrides attrOverrides = new XmlAttributeOverrides();

            /* Add the type of the class that contains the overridden 
            member and the XmlAttributes to override it with to the 
            XmlAttributeOverrides object. */
            //attrOverrides.Add(typeof(Orchestra), "Instruments", attrs);

            // Create the XmlSerializer using the XmlAttributeOverrides.
            //XmlSerializer s =
            //new XmlSerializer(typeof(Orchestra), GetOverrides());

            // Writing the file requires a TextWriter.
            //TextWriter writer = new StreamWriter(filename);

            // Create the object that will be serialized.
            Orchestra band = new Orchestra();

            // Create an object of the derived type.
            Brass i = new Brass();
            i.Name = "Trumpet";
            Iron ir = new Iron();
            ir.Name = "IronInstr";
            Instrument[] myInstruments = { i, ir };
            band.Instruments = myInstruments;

            SerializeObject(filename, band);
            // Serialize the object.
            //s.Serialize(writer, band);
            //writer.Close();
        }

        public Orchestra DeserializeObject(string filename)
        {
            XmlSerializer s =
                new XmlSerializer(typeof(Orchestra), GetOverrides());
            s.UnknownNode += new XmlNodeEventHandler(serializer_UnknownNode);

            FileStream fs = new FileStream(filename, FileMode.Open);
            Orchestra band = (Orchestra)s.Deserialize(fs);
            Console.WriteLine("Instruments");

            /* The difference between deserializing the overridden 
            XML document and serializing it is this: To read the derived 
            object values, you must declare an object of the derived type 
            (Brass), and cast the Instrument instance to it. */
            //Brass b;
            foreach (Instrument i in band.Instruments)
            {
                Console.WriteLine((i.GetType()) + "\t" + i.Name);
                //b = (Brass)i;
                //Console.WriteLine(
                //b.Name + "\n" +
                ////b.IsValved);
            }
            Console.Read();
            return band;
        }

        public void serializer_UnknownNode(object sender, XmlNodeEventArgs e)
        {
            XmlNode n = e.ObjectBeingDeserialized as XmlNode;
            

            Console.WriteLine("Unknown Node:" + e.Name + "\t" + e.Text);
        }
    }
}
