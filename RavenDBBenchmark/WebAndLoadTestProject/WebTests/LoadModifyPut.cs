﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.WebTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Orders;

namespace WebAndLoadTestProject
{
    public class LoadModifyPut: WebTest
    {
        public override IEnumerator<WebTestRequest> GetRequestEnumerator()
        {
            string orderId;
            var url = NorthwindUrlFactory.GenerateRandomOrderDocDocUrl(out orderId);
            var webTestRequest = new WebTestRequest(url)
            {
                ExpectedHttpStatusCode = 200
            };
            Order order = null;
            webTestRequest.ValidateResponse += (sender, args) =>
            {
                order = Newtonsoft.Json.JsonConvert.DeserializeObject<Order>(args.Response.BodyString);
            };
            yield return webTestRequest;
            order.Freight += (decimal)0.1;
            var webTestRequestPut = new WebTestRequest(NorthwindUrlFactory.GenerateBulkDocsUrl())
            {
                Method = "POST"
            };
            webTestRequestPut.Body = new StringHttpBody
            {
                ContentType = "application/json; charset=utf-8",
                InsertByteOrderMark = false,
                BodyString = new JArray(new JObject
                {
                    {"Method", "PUT"},
                    {"Key", string.Format("orders/{0}",orderId)},
                    {
                        "Metadata", new JObject
                        {
                            {"Raven-Entity-Name", "Orders"}
                        }
                    },
                    {"Document", JObject.FromObject(order)}
                }).ToString(Formatting.None)
            };

            webTestRequestPut.ExpectedHttpStatusCode = 200;
            yield return webTestRequestPut;
        }
    }
}
