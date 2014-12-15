using System.Xml.Serialization;

namespace Salsa.Core.Configuration
{
    public abstract class ConfigurationSection
    {
        protected ConfigurationSection()
        {
        }
       
        [XmlAttribute()]
        public abstract string SectionName
        {
            get;
        }

        internal protected virtual void ManagerChanged(ConfigurationMgr mgr)
        {
        }
    }
}
