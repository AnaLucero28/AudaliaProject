using Audalia.DataHUBCommon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Audalia.DataHUBClient
{
    public class DataHUBService
    {
        private static IDataHUBServiceContract _proxy;
        private static ChannelFactory<IDataHUBServiceContract> _factory;
        private static IDataHubServiceCallbackContract _callback;

        public IDataHUBServiceContract Proxy => _proxy;
        public IDataHubServiceCallbackContract Callback => _callback;

        private static System.Timers.Timer _pingTimer = null;

        public void ConnectToDataHubServer()
        {
            AutoResetEvent autoEvent = new AutoResetEvent(false);
            ThreadPool.QueueUserWorkItem(delegate
            {
                string baseAddress = DataHubConfiguration.BaseAddress; //@"net.tcp://127.0.0.1:6789/DataHub/";
                //var binding = new NetTcpBinding(SecurityMode.None);
                var binding = new NetTcpBinding(SecurityMode.Message);
                binding.MaxReceivedMessageSize = int.MaxValue;

                binding.ReliableSession = new OptionalReliableSession() { Enabled = true, InactivityTimeout = TimeSpan.MaxValue };
                binding.ReceiveTimeout = TimeSpan.MaxValue;

                _callback = new DataHubServiceCallback();
                _factory = new DuplexChannelFactory<IDataHUBServiceContract>(_callback, binding,
                    new EndpointAddress(baseAddress));
                _proxy = _factory.CreateChannel();
                (_proxy as IContextChannel).OperationTimeout = new TimeSpan(long.MaxValue); //(0, 0, 30);
                autoEvent.Set();
            }, autoEvent);

            autoEvent.WaitOne();
        }
        public void Initiate()
        {
            _proxy.Initiate();
        }

        public string Register()
        {
            string pId = Process.GetCurrentProcess().Id.ToString();
            string applicationName =
                System.IO.Path.GetFileNameWithoutExtension(System.AppDomain.CurrentDomain.FriendlyName);
            string computerName = Environment.MachineName;
            string userName = Environment.UserName;

            if (_pingTimer == null)
            {
                _pingTimer = new System.Timers.Timer();
                _pingTimer.Interval = 10000;
                _pingTimer.Elapsed += _pingTimer_Elapsed;
                _pingTimer.Start();
            }

            DataHUBClient.InvokeActivityMessage($"Client: Registering {pId}/{applicationName}/{computerName}/{userName}");
            return _proxy.Register(pId, applicationName, computerName, userName);
        }

        private static void _pingTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            DataHUBUtilities.Synchronize<bool>(delegate
            {
                _pingTimer.Stop();

                try
                {
                    _proxy.Pong();
                }
                finally
                {
                    _pingTimer.Start();
                }

                return true;
            });
        }

        public void DisconnectFromDataHubServer()
        {
            try
            {
                if (_proxy != null)
                {
                    _proxy.Terminate();

                    (_proxy as IClientChannel).Close();
                    _factory.Close();
                    _proxy = null;
                }
            }
            catch
            {
                _proxy = null;
            }
        }

        public T CallFunc<T>(Func<IDataHUBServiceContract, T> func)
        {
            T result = default(T);

            for (int i = 0; i < 5; i++)
            {
                try
                {
                    result = func(_proxy);
                    break;
                }
                catch (Exception e)
                {
                    //DataHub.DataHubServiceCallbackContract_OnActivityMessage(e.ToString());
                    DataHUBClient.InvokeActivityMessage(e.ToString());
                    DisconnectFromDataHubServer();
                    ConnectToDataHubServer();
                }
            }

            return result;
        }

    }
}
