using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.WebTesting;

namespace WebAndLoadTestProject
{
    public class SimpleLoadNoCaching : WebTest
    {
        public override IEnumerator<WebTestRequest> GetRequestEnumerator()
        {
            yield return new WebTestRequest(NorthwindUrlFactory.GenerateRandomCompanyDocUrl())
            {
                ExpectedHttpStatusCode = 200
            };
        }
    }

    public class SimpleLoadWithCaching : WebTest
    {
        public override IEnumerator<WebTestRequest> GetRequestEnumerator()
        {
            var url = NorthwindUrlFactory.GenerateRandomCompanyDocUrl();
            string etag = null;
            var webTestRequest = new WebTestRequest(url)
            {
                ExpectedHttpStatusCode = 200
            };
            webTestRequest.ValidateResponse += (sender, args) =>
            {
                etag = args.Response.Headers["Etag"];
            };
            yield return webTestRequest;
            yield return new WebTestRequest(url)
            {
                ExpectedHttpStatusCode = 304,
                Headers =
                {
                    {"If-None-Match", etag}
                }
            };
        }
    }
    public class SimpleLoadwithIncludeNoCaching : WebTest
    {
        public override IEnumerator<WebTestRequest> GetRequestEnumerator()
        {
            var webTestRequest = new WebTestRequest(NorthwindUrlFactory.GenerateRandomOrderDocIncludeCompanyUrl())
            {
                ExpectedHttpStatusCode =  200
            };
            yield return webTestRequest;
            
        }
    }


    public class SimpleLoadwithIncludeWithCaching : WebTest
    {
        public override IEnumerator<WebTestRequest> GetRequestEnumerator()
        {
            var url = NorthwindUrlFactory.GenerateRandomOrderDocIncludeCompanyUrl();
            var webTestRequest = new WebTestRequest(url)
            {
                ExpectedHttpStatusCode = 200
            };
            string etag = null;
            webTestRequest.ValidateResponse += (sender, args) =>
            {
                etag = args.Response.Headers["Etag"];
            };
            yield return webTestRequest;

            yield return new WebTestRequest(url)
            {
                ExpectedHttpStatusCode = 304,
                Headers =
                {
                    {"If-None-Match", etag}
                }
            };

        }
    }

    //public class SimpleLoadWithCache : WebTest
    //{
    //    private readonly string _baseUrl = ConfigurationManager.AppSettings["RavenUrl"];

    //    private readonly int _maxCustomerId = int.Parse(ConfigurationManager.AppSettings["MaxCompanyId"]);

    //    private readonly Random _rand = new Random(206);

    //    public override IEnumerator<WebTestRequest> GetRequestEnumerator()
    //    {
    //        for (int i = 0; i < 15 * 1000; i++)
    //        {
    //            var next = _rand.Next(1, _maxCustomerId + 10);

    //            var webTestRequest = new WebTestRequest(_baseUrl + "/docs/Companies/" + next)
    //            {
    //                ExpectedHttpStatusCode = next > _maxCustomerId ? 404 : 200
    //            };
    //            yield return webTestRequest;
    //        }
    //    }
    //}
}