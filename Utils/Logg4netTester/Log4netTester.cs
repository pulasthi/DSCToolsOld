using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Appender;
using log4net.Config;

namespace Logg4netTester
{
    class Log4NetTester
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof (Log4NetTester));
        static void Main(string[] args)
        {
           /* var fa = new FileAppender
                         {File = @"C:\Sali\pti\sub\salsa\Saliya\c#\Utils\Logg4netTester\bin\x64\Release\log2.txt"};
            fa.ActivateOptions();*/
            XmlConfigurator.Configure();
            var h =
                (log4net.Repository.Hierarchy.Hierarchy) LogManager.GetRepository();
            foreach (var a in h.Root.Appenders)
            {
                var fa = a as FileAppender;
                if (fa != null)
                {
                    // Programmatically set this to the desired location here
                    const string logFileLocation = "log22.txt";

                    // Uncomment the lines below if you want to retain the base file name
                    // and change the folder name...
                    //FileInfo fileInfo = new FileInfo(fa.File);
                    //logFileLocation = string.Format(@"C:\MySpecialFolder\{0}", fileInfo.Name);

                    fa.File = logFileLocation;
                    fa.ActivateOptions();
                    break;
                }
            }
            Logger.Debug("This is a debug message");
            Logger.Info("This is an info message");
            Logger.Error("This is an error message");
            
            Console.WriteLine("Done.");
            Console.Read();
        }
    }
}
