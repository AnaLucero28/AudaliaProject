using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Audalia.DataHUBClient
{
    public class DataHubLog
    {
        private static object lockObj = new object();

        public static void WriteLine(string header, string message = "")        
        {
            lock (lockObj)
            {

                /*
                string header = "";
                if (OperationContext.Current != null)
                {
                    var session = DataHUBServiceContract.GetSession(OperationContext.Current.SessionId);
                    header = Environment.NewLine + @"SessionId: " + OperationContext.Current.SessionId;
                    header += Environment.NewLine + @"UserName: " + session.UserName + @" - ComputerName: " + session.ComputerName + @" - ApplicationName: " + session.ApplicationName + @" - ProcessId: " + session.ProcessId;
                    header += Environment.NewLine + @"RemoteEndpoint: " + session.RemoteEndpoint.Address + ":" + session.RemoteEndpoint.Port;
                }
                */

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
                    Console.WriteLine(message);
                    Console.ResetColor();
                }
                Console.WriteLine();

                //Console.WriteLine(new System.Diagnostics.StackTrace().ToString());

                //////////////////////

                DataHubLogWriter.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + header);                 
                DataHubLogWriter.WriteLine(message);
                DataHubLogWriter.WriteLine("");
            }

        }
    }

    public class DataHubLogWriter
    {
        public static void WriteLine(string logMessage)
        {            
            try
            {
                var exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string pId = Process.GetCurrentProcess().Id.ToString();
                string applicationName =
                    System.IO.Path.GetFileNameWithoutExtension(System.AppDomain.CurrentDomain.FriendlyName);
                string computerName = Environment.MachineName;
                string userName = Environment.UserName;
                string logFileName = exePath + "\\" + DateTime.Now.ToString("yyyyMMdd") + $"_{applicationName}_{pId}_{computerName}_{userName}.txt";                
                using (StreamWriter streamWriter = File.AppendText(logFileName))
                {
                    try
                    {                        
                        streamWriter.WriteLine(logMessage);                        
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }


        private string m_exePath = string.Empty;
        public DataHubLogWriter(string logMessage)
        {
            LogWrite(logMessage);
        }

        public void LogWrite(string logMessage)
        {
            m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try
            {
                using (StreamWriter w = File.AppendText(m_exePath + "\\" + "DataHubLog.txt"))
                {
                    Log(logMessage, w);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void Log(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("\r\nLog Entry : ");
                txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                txtWriter.WriteLine("  :");
                txtWriter.WriteLine("  :{0}", logMessage);
                txtWriter.WriteLine("-------------------------------");
            }
            catch (Exception ex)
            {
            }
        }
    }

}
