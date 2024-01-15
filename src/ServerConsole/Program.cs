using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Audalia.DataHUBServer;
using System.Windows.Forms;

namespace ServerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException +=
                (sender, aargs) => HandleUnhandledException(aargs.ExceptionObject as Exception);
            Application.ThreadException +=
                (sender, aargs) => HandleUnhandledException(aargs.Exception);

            //

            DataHUBServer.Open();
            Console.ReadLine();
            DataHUBServer.Close();
        }

        static object lockObj = new object();

        static void DataHubServer_OnMessage(string message)
        {
            lock (lockObj)
            {
                Console.WriteLine();
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " ");
                Console.ResetColor();

                Console.Write(" " + message);
                Console.ResetColor();
                Console.WriteLine();
            }
        }

        static void HandleUnhandledException(Exception e)
        {
            Console.WriteLine(e.ToString());            
        }
    }
}
