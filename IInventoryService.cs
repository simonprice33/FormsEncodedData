using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace FormsEncodedData
{
    public class Product
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public double Quantity { get; set; }
    }

    [ServiceContract]
    public interface IInventoryService
    {
        [WebInvoke]
        void InsertProduct(Product product);
        [WebGet]
        List<Product> GetAllProducts();
    }
}