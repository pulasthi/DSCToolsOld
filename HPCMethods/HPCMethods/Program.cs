using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace HPCMethods
{
    class Program
    {
        const string RequestUsernameMsg = "Login as <domain\\username>: ";
        const string RequestPwdMsg = "Password: ";
        const string PressKeyToExitMsg = "Press any key to exit.";
        static void Main(string[] args)
        {
            Console.Write(RequestUsernameMsg);
            var username = Console.ReadLine();
            Console.Write(RequestPwdMsg);
            var pwd = GetPassword();

            /* Running Scheduler Command Test */
            /*Console.WriteLine("\n***Running Scheduler Command Test***\n");
            var sct = new SchedulerCommandTest();
            sct.Run(username, pwd);*/

            /* Running Scheduler Job Test */
            Console.WriteLine("\n***Running Scheduler Job Test***\n");
            var t = new SchedulerJobTest();
            t.Run(username, pwd);
            
            Console.WriteLine(PressKeyToExitMsg);
            Console.Read();
        }

        public static SecureString GetPassword()
        {
            var pwd = new SecureString();
            while (true)
            {
                ConsoleKeyInfo i = Console.ReadKey(true);
                if (i.Key == ConsoleKey.Enter)
                {
                    break;
                }
                if (i.Key == ConsoleKey.Backspace)
                {
                    if (pwd.Length > 0)
                    {
                        pwd.RemoveAt(pwd.Length - 1);
                        Console.Write("\b \b");
                    }

                }
                else if (((int) i.Key) >= 65 && ((int) i.Key <= 90)) 
                {
                    pwd.AppendChar(i.KeyChar);
                    Console.Write("*");
                }
            }
            pwd.MakeReadOnly();
            return pwd;
        }

        

    }
}
