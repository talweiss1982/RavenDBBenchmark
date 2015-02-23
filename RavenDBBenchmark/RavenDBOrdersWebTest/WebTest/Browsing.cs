using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.WebTesting;
using Newtonsoft.Json;
using RavenDBOrdersWebTestPlugin.Model;

namespace RavenDBOrdersWebTest
{
    public class Browsing : RavenWebTest
    {
        protected override IEnumerable<WebTestRequest> GetRequests()
        {
            Customer custoemr = null;
            var randomCustoemrRequest = new WebTestRequest(OrdersDatabaseUrlFactory.GenerateFetchRandomCustomersQueryUrl()) {ExpectedHttpStatusCode = 200};
            randomCustoemrRequest.ValidateResponse += (sender, args) =>
            {
                custoemr = JsonConvert.DeserializeObject<Customer>(args.Response.BodyString);
            };
            yield return randomCustoemrRequest;
            var orderNumber = Utiles.NextRandom(0, custoemr.Orders.Count);
            var orderId = custoemr.Orders[orderNumber];
            Order order = null;
            var randomOrderRequest = new WebTestRequest(OrdersDatabaseUrlFactory.GenerateLoadDocUrl(orderId)) {ExpectedHttpStatusCode = 200};
            randomOrderRequest.ValidateResponse += (sender, args) =>
            {
                order = JsonConvert.DeserializeObject<Order>(args.Response.BodyString);
            };
            yield return randomOrderRequest;
            var productLineNumber = Utiles.NextRandom(0, order.Lines.Count);
            var productId = order.Lines[productLineNumber].ProductId;
            yield return new WebTestRequest(OrdersDatabaseUrlFactory.GenerateLoadDocUrl(productId)) { ExpectedHttpStatusCode = 200 };
        }
    }
}
