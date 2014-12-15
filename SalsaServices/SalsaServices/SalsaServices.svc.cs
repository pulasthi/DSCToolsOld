using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SalsaServices
{
   // todo: saliya - how does this pick up SimpleService now? continue from here
    public class CalculatorService : ICalculatorService
    {
        public int Add(int x, int y)
        {
            return x + y;
        }
    }
}
