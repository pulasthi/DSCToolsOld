using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Salsa.Core.Configuration
{
    public abstract class Section
    {
        /* The name will identifiy the configuraiton
         * section uniquely. This is a read-only property */        
        public abstract string Name { get; }

    }
}
