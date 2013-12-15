using AngularSignalR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace AngularSignalR.Service
{
    // see: http://msdn.microsoft.com/en-us/library/dd203052.aspx
    // Getting PUT AND DELETE to work in IIS Express see: http://www.iis.net/learn/extensions/introduction-to-iis-express/iis-express-faq
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IAPIService
    {
        [WebGet(UriTemplate = "ping", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        String Ping();
        
        [WebGet(UriTemplate = "products", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        IList<Product> GetProducts();
        
        [OperationContract]
        [WebGet(UriTemplate = "products/{ProductId}", ResponseFormat = WebMessageFormat.Json)]
        Product GetProduct(string ProductId);

        [WebInvoke(Method = "POST", UriTemplate = "products/", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [OperationContract]
        Product AddProduct(Product product);

        [WebInvoke(Method = "PUT", UriTemplate = "products/{ProductId}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [OperationContract]
        Boolean UpdateProduct(string ProductId, Product updatedProduct);

        [WebInvoke(Method = "DELETE", UriTemplate = "products/{ProductId}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [OperationContract]
        Boolean DeleteProduct(string ProductId);

        //[OperationContract]
        //[WebGet(UriTemplate = "products/store/{StoreId}")]
        //IList<Product> GetStoreProducts(int StoreId);
        //[OperationContract]
        //[WebGet(UriTemplate = "products/category{CategoryId}")]
        //IList<Product> GetCategoryProducts(int CategoryId);

        //[OperationContract]
        //[WebGet(UriTemplate = "stores/{StoreId}")]
        //Store GetStore(int StoreId);
        //[OperationContract]
        //[WebGet(UriTemplate = "categories/{CategoryId}")]
        //Store GetCategory(int CategoryId);
    }

}
