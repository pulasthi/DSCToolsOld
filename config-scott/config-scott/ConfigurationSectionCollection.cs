using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Salsa.Core.Configuration
{
    public class ConfigurationSectionCollection : KeyedCollection<string, ConfigurationSection>
    {
        private ConfigurationMgr _manager;

        internal ConfigurationSectionCollection(ConfigurationMgr manager)
        {
            _manager = manager;
        }

        protected override string GetKeyForItem(ConfigurationSection item)
        {
            return item.SectionName;
        }

        protected override void InsertItem(int index, ConfigurationSection item)
        {
            base.InsertItem(index, item);
            item.ManagerChanged(_manager);
        }

        protected override void RemoveItem(int index)
        {
            ConfigurationSection item = this[index];
            base.RemoveItem(index);
            item.ManagerChanged(null);
        }
    }
}
