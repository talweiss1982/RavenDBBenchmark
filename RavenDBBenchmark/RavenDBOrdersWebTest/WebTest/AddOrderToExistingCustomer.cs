using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.WebTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RavenDBOrdersWebTestPlugin;
using RavenDBOrdersWebTestPlugin.Model;

namespace RavenDBOrdersWebTest
{
    public class AddOrderToExistingCustomer: RavenWebTest
    {
        protected override IEnumerable<WebTestRequest> GetRequests()
        {
            //Debugger.Launch();
            Customer c = null;
            string custoemrId = null;
            var fetchRandomCustomer = new WebTestRequest(OrdersDatabaseUrlFactory.GenerateFetchRandomCustomersQueryUrl())
            {
                ExpectedHttpStatusCode = 200
            };
            fetchRandomCustomer.ValidateResponse += (sender, args) =>
            {
                var response = JObject.Parse(args.Response.BodyString);
                var actualResponseObject = ((JArray) response["Results"])[0];
                c = actualResponseObject.ToObject<Customer>();
                custoemrId = actualResponseObject["@metadata"]["@id"].ToString();
            };
            yield return fetchRandomCustomer;            
            var lastOrder = c.Orders.Last();
            var rand = Utiles.NextRandom(1, 5);
            var jArray = new JArray();
            switch (rand)
            {
                case 1:
                    Order lastOrderObj = null;
                    var lastOrderRequest = new WebTestRequest(OrdersDatabaseUrlFactory.GenerateLoadDocUrl(lastOrder))
                    {
                        ExpectedHttpStatusCode = 200
                    };
                    lastOrderRequest.ValidateResponse += (sender, args) =>
                    {
                        lastOrderObj = JsonConvert.DeserializeObject<Order>(args.Response.BodyString);
                    };
                    yield return lastOrderRequest;
                    var insertOrderWithCustomerRequest = new WebTestRequest(OrdersDatabaseUrlFactory.GenerateBulkDocsUrl())
                    {
                        ExpectedHttpStatusCode = 200,
                        Method = "POST"
                    };
                    var orderId = OrdersDatabaseFactory.GenerateNewOrderId(lastOrderObj); //should i clone this object?
                    WebTestUtiles.AddJObjectToJArrayForPost(lastOrderObj, orderId, "Orders", jArray);
                    c.Orders.Add(orderId);
                    WebTestUtiles.AddJObjectToJArrayForPost(c, custoemrId, "Customers", jArray);
                    WebTestUtiles.GeneratePostRequestBody(insertOrderWithCustomerRequest,jArray);
                    yield return insertOrderWithCustomerRequest;
                    break;
                case 2:
                    var productId = "products/" + Utiles.NextRandom(1, OrdersConfig.NumberOfProducts);                    
                    lastOrderObj = null;
                    lastOrderRequest = new WebTestRequest(OrdersDatabaseUrlFactory.GenerateLoadDocUrl(lastOrder))
                    {
                        ExpectedHttpStatusCode = 200
                    };
                    lastOrderRequest.ValidateResponse += (sender, args) =>
                    {
                        lastOrderObj = JsonConvert.DeserializeObject<Order>(args.Response.BodyString);
                    };
                    yield return lastOrderRequest;
                    var newOrder = lastOrderObj.Clone();
                    var productLine = ProductLine.GenerateProductLine();
                    productLine.ProductId = productId;
                    newOrder.Lines.Add(productLine);
                    insertOrderWithCustomerRequest = new WebTestRequest(OrdersDatabaseUrlFactory.GenerateBulkDocsUrl())
                    {
                        ExpectedHttpStatusCode = 200,
                        Method = "POST"
                    };
                    orderId = OrdersDatabaseFactory.GenerateNewOrderId(newOrder); 
                    WebTestUtiles.AddJObjectToJArrayForPost(newOrder, orderId, "Orders", jArray);
                    c.Orders.Add(orderId);
                    WebTestUtiles.AddJObjectToJArrayForPost(c, custoemrId, "Customers", jArray);
                    WebTestUtiles.GeneratePostRequestBody(insertOrderWithCustomerRequest,jArray);
                    yield return insertOrderWithCustomerRequest;
                    break;
                case 3:
                    newOrder = Order.GenerateOrder();
                    newOrder.Lines = new List<ProductLine>();
                    productId = "products/" + Utiles.NextRandom(1, OrdersConfig.NumberOfProducts);     
                    productLine = ProductLine.GenerateProductLine();
                    productLine.ProductId = productId;
                    newOrder.Lines.Add(productLine);
                    insertOrderWithCustomerRequest = new WebTestRequest(OrdersDatabaseUrlFactory.GenerateBulkDocsUrl())
                    {
                        ExpectedHttpStatusCode = 200,
                        Method = "POST"
                    };
                    orderId = OrdersDatabaseFactory.GenerateNewOrderId(newOrder);
                    c.Orders.Add(orderId);
                    WebTestUtiles.AddJObjectToJArrayForPost(newOrder, orderId, "Orders",jArray);
                    WebTestUtiles.AddJObjectToJArrayForPost(c, custoemrId, "Customers", jArray);
                    WebTestUtiles.GeneratePostRequestBody(insertOrderWithCustomerRequest,jArray);
                    yield return insertOrderWithCustomerRequest;
                    break;
                case 4:
                    break;
            }

        }
    }
}
