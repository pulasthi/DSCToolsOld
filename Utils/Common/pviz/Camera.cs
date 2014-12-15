using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Common.pviz
{
    [Serializable]
    [XmlType("camera")]
    public class Camera
    {
        public Camera()
        {
            FocusMode = (int) FocusModes.Auto;
            Focus = new Focus(0.0f, 0.0f, 0.0f);
        }

        public Camera(FocusModes focusMode, Focus focus)
        {
            FocusMode = (int)focusMode;
            Focus = focus;
        }

        [XmlElement("focusmode")]
        public int FocusMode { get; set; }

        [XmlElement("focus")]
        public Focus Focus { get; set; }
    }

    public enum FocusModes
    {
        // Number assignment is redundant for this, but keeping it for clarity
        Auto = 0,
        Origin = 1,
        Custom = 2

    }

    
}
