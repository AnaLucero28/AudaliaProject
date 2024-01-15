using Audalia.DataHUBCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Audalia.DataHUBClient
{
    public class DataHubServiceCallback : IDataHubServiceCallbackContract
    {        
        void IDataHubServiceCallbackContract.Ping()
        {
            DataHUBClient.Pong();
        }

        void IDataHubServiceCallbackContract.ActivityMessage(string message)
        {         
            DataHUBClient.InvokeActivityMessage(message);
        }

        void IDataHubServiceCallbackContract.ObjectMessage(ObjectMessageActionType objectMessageActionType, ObjectMessageObjectType objectMessageObjectType, string objectId)
        {
            DataHUBClient.InvokeObjectMessage(objectMessageActionType, objectMessageObjectType, objectId);
        }

        void IDataHubServiceCallbackContract.ObjectActionMessageList(List<ObjectActionMessage> objectActionMessageList)
        {
            DataHUBClient.InvokeObjectActionMessageList(objectActionMessageList);
        }
    }
}
