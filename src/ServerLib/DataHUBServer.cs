using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Audalia.DataHUBCommon;
using Audalia.DataHUBServer.Audit;

namespace Audalia.DataHUBServer
{
    public class DataHUBServer
    {
        public static ServiceHost _serviceHost;

        public static void Open()
        {
            DataHubLog.WriteLine("DataHUBServer.Open");

            string baseAddress = DataHubConfiguration.BaseAddress; //@"net.tcp://127.0.0.1:6789/DataHub/";
            _serviceHost = new ServiceHost(typeof(DataHUBServiceContract), new Uri(baseAddress));
            //var binding = new NetTcpBinding(SecurityMode.None)
            var binding = new NetTcpBinding(SecurityMode.Message)
            {                
                ReliableSession = new OptionalReliableSession() { Enabled = true, InactivityTimeout = TimeSpan.MaxValue },
                ReceiveTimeout = TimeSpan.MaxValue,
                MaxReceivedMessageSize = int.MaxValue,
                MaxBufferSize =  int.MaxValue,
                MaxConnections = int.MaxValue,
                ListenBacklog = int.MaxValue,
                MaxBufferPoolSize = int.MaxValue,
                SendTimeout = new TimeSpan(long.MaxValue) //new TimeSpan(0, 0, 30)
        };

            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            
            _serviceHost.AddServiceEndpoint(typeof(IDataHUBServiceContract), binding, baseAddress);
            ServiceThrottlingBehavior behavior = new ServiceThrottlingBehavior
            {
                MaxConcurrentSessions = int.MaxValue,
                MaxConcurrentCalls = int.MaxValue,
                MaxConcurrentInstances = int.MaxValue
            };
            _serviceHost.Description.Behaviors.Add(behavior);
            _serviceHost.Open();

            //Test ->
            //DataHUBReports.Go();
        }

        public static void Close()
        {
            DataHubLog.WriteLine("DataHUBServer.Close");
            _serviceHost.Close();
        }        
    }

       
}
