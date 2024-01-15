using Audalia.DataHUBCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Audalia.DataHUBServer
{
    [DataContract(Name = "DataHubSessionBase", Namespace = "Audalia.DataHubClient")]
    public class DataHubSession : DataHubSessionBase
    {
        public OperationContext OperationContext { get; set; }
        public RemoteEndpointMessageProperty RemoteEndpoint { get; set; }
    }
}
