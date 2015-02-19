using System.Collections.Generic;
using System.IO;
using FizzWare.NBuilder;
using Microsoft.VisualStudio.TestTools.WebTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Orders;

namespace WebAndLoadTestProject
{
    public class PutCompany : WebTest
    {
        public override IEnumerator<WebTestRequest> GetRequestEnumerator()
        {
            var webTestRequest = new WebTestRequest(NorthwindUrlFactory.GenerateBulkDocsUrl())
            {
                Method = "POST",
            };


            var company = Builder<Company>.CreateNew().Build();

            webTestRequest.Body = new StringHttpBody
            {
                ContentType = "application/json; charset=utf-8",
                InsertByteOrderMark = false,
                BodyString = new JArray(new JObject
                {
                    {"Method", "PUT"},
                    {"Key", "companies/"},
                    {
                        "Metadata", new JObject
                        {
                            {"Raven-Entity-Name", "Companies"}
                        }
                    },
                    {"Document", JObject.FromObject(company)}
                }).ToString(Formatting.None)
            };

            webTestRequest.ExpectedHttpStatusCode = 200;
            yield return webTestRequest;
        }
    }
}