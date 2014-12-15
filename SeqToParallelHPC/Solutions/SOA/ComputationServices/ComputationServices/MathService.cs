using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ComputationServices
{

  [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
  class MathService : IMathService
  {
    public int Add(int a, int b)
    {
      return a + b;
    }
  }

}
