using Audalia.DataHUBCommon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Audalia.DataHUBClient
{
    public delegate void DataHubMessageEventHandler(string message);
    public delegate void DataHubObjectMessageEventHandler(ObjectMessageActionType objectMessageActionType, ObjectMessageObjectType objectMessageObjectType, string objectId);
    public delegate void DataHubObjectActionMessageListEventHandler(List<ObjectActionMessage> objectActionMessageList);

    public class DataHUBClient
    {
        public static event DataHubMessageEventHandler OnActivityMessage;
        public static event DataHubObjectMessageEventHandler OnObjectMessage;
        public static event DataHubObjectActionMessageListEventHandler OnObjectActionMessageList;

        //public static void DataHubServiceCallbackContract_OnActivityMessage(string message)

        public static void InvokeActivityMessage(string message)
        {
            OnActivityMessage?.Invoke(message);
        }

        public static void InvokeObjectMessage(ObjectMessageActionType objectMessageActionType, ObjectMessageObjectType objectMessageObjectType, string objectId)
        {
            OnObjectMessage?.Invoke(objectMessageActionType, objectMessageObjectType, objectId);
        }

        public static void InvokeObjectActionMessageList(List<ObjectActionMessage> objectActionMessageList)
        {
            OnObjectActionMessageList?.Invoke(objectActionMessageList);
        }

        static DataHUBService _service = null;
        private static DataHUBService Service
        {
            get
            {
                if (_service == null)
                {                    
                    _service = new DataHUBService();
                    _service.ConnectToDataHubServer();
                    _service.Initiate();
                    _service.Register();
                }

                return _service;
            }
        }

        public static IDataHUBServiceContract ServiceContract
        {
            get 
            {
                try
                {
                    Service.Proxy.Pong();
                }
                catch
                {
                    _service = null;
                }
               
                return Service.Proxy; 
            }
        }


        /////////////

        public static void Initiate()
        {
            Service.Proxy.Initiate();
        }

        public static string Register(string processId, string applicationName, string computerName, string userName)
        {
            return Service.Proxy.Register(processId, applicationName, computerName, userName);
        }

        public static void Terminate()
        {
            Service.Proxy.Terminate();
        }

        public static void Pong()
        {
            Service.Proxy.Pong();
        }

    }
}
