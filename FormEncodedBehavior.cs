using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace FormsEncodedData
{
    public class FormEncodedBehavior : IEndpointBehavior
    {
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            foreach (var operation in endpoint.Contract.Operations)
            {
                if (operation.Messages[0].Body.Parts.Count == 1)
                {
                    var dispatchOperation = endpointDispatcher.DispatchRuntime.Operations[operation.Name];
                    var parameterType = operation.Messages[0].Body.Parts[0].Type;
                    dispatchOperation.Formatter = new FormEncodedFormatter(dispatchOperation.Formatter, parameterType);
                }
            }
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }
}