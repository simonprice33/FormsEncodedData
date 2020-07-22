using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Description;
using System.ServiceModel.Web;

namespace FormsEncodedData
{
    public class InventoryServiceFactory : ServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            return new InventoryServiceHost(serviceType, baseAddresses);
        }

        class InventoryServiceHost : ServiceHost
        {
            public InventoryServiceHost(Type serviceType, Uri[] baseAddresses)
                : base(serviceType, baseAddresses)
            {
            }

            protected override void InitializeRuntime()
            {
                var endpoint = this.AddServiceEndpoint(typeof(IInventoryService), new WebHttpBinding(), "");
                endpoint.Behaviors.Add(new WebHttpBehavior { DefaultOutgoingResponseFormat = WebMessageFormat.Json });
                endpoint.Behaviors.Add(new FormEncodedBehavior());

                base.InitializeRuntime();
            }
        }
    }

    public class InventoryService : IInventoryService
    {
        static List<Product> AllProducts = new List<Product>();

        public void InsertProduct(Product product)
        {
            AllProducts.Add(product);
        }

        public List<Product> GetAllProducts()
        {
            WebOperationContext.Current.OutgoingResponse.Headers[HttpResponseHeader.CacheControl] = "no-cache";
            return AllProducts;
        }
    }
}
