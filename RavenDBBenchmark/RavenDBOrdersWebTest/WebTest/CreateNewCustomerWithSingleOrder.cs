using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.WebTesting;
using Newtonsoft.Json.Linq;
using RavenDBOrdersWebTestPlugin;
using RavenDBOrdersWebTestPlugin.Model;

namespace RavenDBOrdersWebTest
{
    public class CreateNewCustomerWithSingleOrder: RavenWebTest
    {
        protected override IEnumerable<WebTestRequest> GetRequests()
        {
            Customer customer = Customer.GenerateCustomer();
            Order order = Order.GenerateOrder();
            customer.Orders = new List<string>();
            order.Lines = new List<ProductLine>();
            var orderAndCustomer = new JArray();
            for (int i = 0; i < 3; i++)
            {
                var productId = "products/" + Utiles.NextRandom(1, OrdersConfig.NumberOfProducts);
                var productLine = ProductLine.GenerateProductLine();
                productLine.ProductId = productId;
                order.Lines.Add(productLine);
            }
            var orderId = OrdersDatabaseFactory.GenerateNewOrderId(order);
            customer.Orders.Add(orderId);
            var insertCustomerWithOrder = new WebTestRequest(OrdersDatabaseUrlFactory.GenerateBulkDocsUrl())
            {
                ExpectedHttpStatusCode = 200,
                Method = "POST",
                ReportingName = "CreateNewCustomerWithSingleOrder_insertCustomerWithOrder"
            };
            WebTestUtiles.AddJObjectToJArrayForPost(order, orderId, "Orders", orderAndCustomer);
            var customerId = OrdersDatabaseFactory.GenerateNewCustoemrId(customer);
            WebTestUtiles.AddJObjectToJArrayForPost(customer, customerId, "Customers", orderAndCustomer);
            WebTestUtiles.GeneratePostRequestBody(insertCustomerWithOrder, orderAndCustomer);
            yield return insertCustomerWithOrder;
        }
    }
}
