using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.WebTesting;

namespace WebAndLoadTestProject
{
    public static class Config
    {
        public static string RavenUrl = "http://localhost:8080/databases/Northwind";
        public static int MaxCustomerId = 91;
        public static int MaxOrderId = 830;

        [ThreadStatic] private static Random _random;

        public static int RandNext(int min, int max)
        {
            if(_random == null)
                _random = new Random(205);

            return _random.Next(min, max);
        }
    }

    public class SimpleLoadNoCaching : WebTest
    {
        public override IEnumerator<WebTestRequest> GetRequestEnumerator()
        {
            var next = Config.RandNext(1, Config.MaxCustomerId);


            yield return new WebTestRequest(Config.RavenUrl + "/docs/Companies/" + next)
            {
                ExpectedHttpStatusCode = 200
            };
        }
    }

    public class SimpleLoadWithCaching : WebTest
    {
        public override IEnumerator<WebTestRequest> GetRequestEnumerator()
        {
            var next = Config.RandNext(1, Config.MaxCustomerId);

            var url = Config.RavenUrl + "/docs/Companies/" + next;
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
            var next = Config.RandNext(1, Config.MaxOrderId);


            var webTestRequest = new WebTestRequest(Config.RavenUrl + "/docs/orders/" + next + "?include=Company")
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
            var next = Config.RandNext(1, Config.MaxOrderId );


            var url = Config.RavenUrl + "/docs/orders/" + next + "?include=Company";
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

    //    private readonly int _maxCustomerId = int.Parse(ConfigurationManager.AppSettings["MaxCustomerId"]);

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