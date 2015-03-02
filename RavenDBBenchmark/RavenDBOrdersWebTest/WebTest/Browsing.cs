using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.WebTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RavenDBOrdersWebTestPlugin.Model;

namespace RavenDBOrdersWebTest
{
    public class Browsing : RavenWebTest
    {
        protected override IEnumerable<WebTestRequest> GetRequests()
        {
            Customer custoemr = null;
            var randomCustoemrRequest = new WebTestRequest(OrdersDatabaseUrlFactory.GenerateFetchRandomCustomersQueryUrl())
            {
                ExpectedHttpStatusCode = 200,
                ReportingName = "Browsing_randomCustoemrRequest"
            };
            randomCustoemrRequest.ValidateResponse += (sender, args) =>
            {
                var response = JObject.Parse(args.Response.BodyString);
                var actualResponseObject = ((JArray)response["Results"])[0];
                custoemr = actualResponseObject.ToObject<Customer>();
            };
            yield return randomCustoemrRequest;
            var orderNumber = Utiles.NextRandom(0, custoemr.Orders.Count);
            var orderId = custoemr.Orders[orderNumber];
            Order order = null;
            var randomOrderRequest = new WebTestRequest(OrdersDatabaseUrlFactory.GenerateLoadDocUrl(orderId))
            {
                ExpectedHttpStatusCode = 200,
                ReportingName = "Browsing_randomOrderRequest"
            };
            randomOrderRequest.ValidateResponse += (sender, args) =>
            {
                order = JsonConvert.DeserializeObject<Order>(args.Response.BodyString);
            };
            yield return randomOrderRequest;
            var productLineNumber = Utiles.NextRandom(0, order.Lines.Count);
            var productId = order.Lines[productLineNumber].ProductId;
            yield return new WebTestRequest(OrdersDatabaseUrlFactory.GenerateLoadDocUrl(productId))
            {
                ExpectedHttpStatusCode = 200,
                ReportingName = "Browsing_LoadProduct"
            };
        }
    }
}
