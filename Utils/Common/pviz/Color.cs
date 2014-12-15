using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Common.pviz
{
    [Serializable]
    [XmlType("color")]
    public class Color
    {
        public Color(){}
        public Color(System.Drawing.Color c)
        {
            R = c.R.ToString(CultureInfo.InvariantCulture);
            G = c.G.ToString(CultureInfo.InvariantCulture);
            B = c.B.ToString(CultureInfo.InvariantCulture);
            A = c.A.ToString(CultureInfo.InvariantCulture);
        }
        public Color(string r, string g, string b, string a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }
        [XmlAttribute("r")]
        public string R { get; set; }
        [XmlAttribute("g")]
        public string G { get; set; }
        [XmlAttribute("b")]
        public string B { get; set; }
        [XmlAttribute("a")]
        public string A { get; set; }

        public override string ToString()
        {
            return R + "." + G + "." + B + "." + A;
        }

        public string ToStringWithoutAlpha()
        {
            return R + "." + G + "." + B;
        }

        public string ToColorCode()
        {
            int code = (int.Parse(R)*65536)+(int.Parse(G)*256)+int.Parse(B);
            return code.ToString("X6");
        }

        public static Color FromString(string color)
        {
            string[] arr = color.Split('.');
            return new Color(arr[0], arr[1], arr[2], arr[3]);
        }
    }
}
