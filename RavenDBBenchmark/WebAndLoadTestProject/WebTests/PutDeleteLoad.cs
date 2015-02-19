using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using Microsoft.VisualStudio.TestTools.WebTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebAndLoadTestProject.Model;

namespace WebAndLoadTestProject
{

    public class PutDeleteLoad:WebTest
    {
        private static int personId;
        public override IEnumerator<WebTestRequest> GetRequestEnumerator()
        {
            var preson = Builder<Person>.CreateNew().Build();
            var webTestRequestPut = new WebTestRequest(NorthwindUrlFactory.GenerateBulkDocsUrl())
            {
                Method = "POST",
                ExpectedHttpStatusCode = 200
            };
            var id = Interlocked.Increment(ref personId);
            webTestRequestPut.Body = new StringHttpBody
            {
                ContentType = "application/json; charset=utf-8",
                InsertByteOrderMark = false,
                BodyString = new JArray(new JObject
                {
                    {"Method", "PUT"},
                    {"Key", string.Format("persons/{0}",id)},
                    {
                        "Metadata", new JObject
                        {
                            {"Raven-Entity-Name", "Persons"}
                        }
                    },
                    {"Document", JObject.FromObject(preson)}
                }).ToString(Formatting.None)
            };
            yield return webTestRequestPut;
            var url = NorthwindUrlFactory.GeneratePersonDocUrl(id);
            var webTestRequestDelete = new WebTestRequest(url)
            {
                ExpectedHttpStatusCode = 204,
                Method = "DELETE"
            };
            yield return webTestRequestDelete;
            var webTestRequestGet = new WebTestRequest(url)
            {
                ExpectedHttpStatusCode = 404
            };
            yield return webTestRequestGet;
        }        
    }
}
