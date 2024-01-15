using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace Audalia.DataHUBServer
{
    public class GlobalErrorHandler : IErrorHandler
    {
        bool IErrorHandler.HandleError(Exception error)
        {
            return true;
        }

        void IErrorHandler.ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            DataHubLog.WriteLine($"GlobalErrorHandler: {error.ToString()}");
        }
    }

    public class GlobalErrorBehaviorAttribute : Attribute, IServiceBehavior
    {
        private readonly Type errorHandlerType;

        public GlobalErrorBehaviorAttribute(Type errorHandlerType)
        {
            this.errorHandlerType = errorHandlerType;
        }

        void IServiceBehavior.AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
            //
        }

        void IServiceBehavior.ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            IErrorHandler errorHandler = (IErrorHandler)Activator.CreateInstance(errorHandlerType);
            foreach (var channelDispatcher in serviceHostBase.ChannelDispatchers)
            {
                ((ChannelDispatcher)channelDispatcher).ErrorHandlers.Add(errorHandler);
            }
        }

        void IServiceBehavior.Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            //
        }
    }
}
