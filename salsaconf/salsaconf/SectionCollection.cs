using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Salsa.Core.Configuration
{
    public class SectionCollection : 
        KeyedCollection<string, Section>
    {        
        public SectionCollection() : base() { }

        protected override string GetKeyForItem(Section section)
        {
            return section.Name;
        }
    }
}
