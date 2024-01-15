using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Audalia.DataHUBCommon
{
    public enum ObjectMessageObjectType { Project, Employee, Budget, ProjectEmployeeAssoc, ProjectEmployeeAlloc };
    public enum ObjectMessageActionType { Post, Put, Delete };

    [ServiceContract]
    public interface IDataHubServiceCallbackContract
    {
        [OperationContract(IsOneWay = true)]
        void Ping();

        [OperationContract(IsOneWay = true)]
        void ActivityMessage(string message);

        [OperationContract(IsOneWay = true)]
        void ObjectMessage(ObjectMessageActionType objectMessageActionType, ObjectMessageObjectType objectMessageObjectType, string objectId);

        [OperationContract(IsOneWay = true)]
        void ObjectActionMessageList(List<ObjectActionMessage> objectActionMessageList);
    }
}
