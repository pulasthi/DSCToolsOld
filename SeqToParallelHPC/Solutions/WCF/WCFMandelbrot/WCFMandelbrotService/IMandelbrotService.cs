using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WCFMandelbrotService
{

  [ServiceContract]
  public interface IMandelbrotService
  {
    [OperationContract]
    int[] GenerateMandelbrotRow(double yp, double y, double x, double size, int pixels);
  }

}//namespace
