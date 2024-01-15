using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Audalia.DataHUBClient;

namespace ClientConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            DataHUBClient.OnActivityMessage += DataHub_OnActivityMessage;
            DataHUBClient.OnObjectMessage += DataHUBClient_OnObjectMessage;
            DataHUBClient.Initiate();
            Console.ReadKey();            
        }

        private static void DataHUBClient_OnObjectMessage(Audalia.DataHUBCommon.ObjectMessageActionType objectMessageActionType, Audalia.DataHUBCommon.ObjectMessageObjectType objectMessageObjectType, string objectId)
        {
            DataHubLog.WriteLine("ObjectMessage: ", objectMessageActionType.ToString() + " - " + objectMessageObjectType.ToString() + " - " + objectId);
            //WriteLine("ObjectMessage", objectMessageActionType.ToString() + " - " + objectMessageObjectType.ToString() + " - " + objectId);
        } 

        static void DataHub_OnActivityMessage(string message)
        {
            DataHubLog.WriteLine("ActivityMessage: ", message);
        }

        /*
        static object lockObj = new object();
        static void WriteLine(string header, string message = "")
        {
            lock (lockObj)
            {
                Console.ResetColor();
                Console.WriteLine();

                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(" " + header);
                Console.ResetColor();

                if (message != "")
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(" " + message);
                    Console.ResetColor();
                }

            }

        }
        */
    }
}
