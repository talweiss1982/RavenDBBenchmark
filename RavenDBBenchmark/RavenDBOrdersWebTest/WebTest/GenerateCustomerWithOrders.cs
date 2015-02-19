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
    public class GenerateCustomerWithOrders : RavenWebTest
    {
        protected override IEnumerable<WebTestRequest> GetRequests()
        {
            var webTestRequest = new WebTestRequest(OrdersDatabaseUrlFactory.GenerateBulkDocsUrl())
            {
                Method = "POST",
                ExpectedHttpStatusCode = 200
            };
            var customer = Customer.GenerateCustomer();
            customer.Orders = new List<String>();
            var ordersAsJObject = new JArray();
            for (int orderCount = 0; orderCount < OrdersConfig.InitialOrdersPerCustomer; orderCount++)
            {
                var order = Order.GenerateOrder();
                order.Lines = new List<ProductLine>();
                for (int orderLineCount = 0; orderLineCount < OrdersConfig.InitialOrderLinesPerOrder; orderLineCount++)
                {
                    var productLine = ProductLine.GenerateProductLine();
                    productLine.ProductId = "products/" + Utiles.NextRandom(1, OrdersConfig.NumberOfProducts);
                    order.Lines.Add(productLine);
                }
                var orderId = OrdersDatabaseFactory.GenerateNewOrderId(order);
                customer.Orders.Add(orderId);
                WebTestUtiles.AddJObjectToJArrayForPost(order, orderId, "Orders", ordersAsJObject);
            }
            var customerId = OrdersDatabaseFactory.GenerateNewCustoemrId(customer);
            WebTestUtiles.AddJObjectToJArrayForPost(customer, customerId, "Customers", ordersAsJObject);
            WebTestUtiles.GeneratePostRequestBody(webTestRequest, ordersAsJObject);
            yield return webTestRequest;
        }
    }
}
