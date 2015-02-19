using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.WebTesting;

namespace WebAndLoadTestProject.WebTests
{
    public class DynamicFacetQuery: WebTest
    {
        public override IEnumerator<WebTestRequest> GetRequestEnumerator()
        {
            yield return new WebTestRequest(NorthwindUrlFactory.GenerateDynamicFacetQueryUrl()) { ExpectedHttpStatusCode = 200 };
        }
    }

    public abstract class RavenWebTest : WebTest
    {
        private static readonly ConcurrentDictionary<string, string> _etags = new ConcurrentDictionary<string, string>(); 

        public override IEnumerator<WebTestRequest> GetRequestEnumerator()
        {
            foreach (var req in GetRequests())
            {
                if (req.Method == "GET")
                {
                    string value;
                    if (_etags.TryGetValue(req.Url, out value))
                    {
                        req.Headers.Add("If-None-Modified", value);
                    }
                    req.ValidateResponse += (sender, args) =>
                    {
                        if (args.Response.StatusCode == HttpStatusCode.OK)
                        {
                            var addValue = args.Response.Headers["ETag"];
                            _etags.AddOrUpdate(req.Url, addValue, (s, s1) => addValue);
                        }
                    };
                }

                yield return req;
            }
        }

        protected abstract IEnumerable<WebTestRequest> GetRequests();
    }
}
