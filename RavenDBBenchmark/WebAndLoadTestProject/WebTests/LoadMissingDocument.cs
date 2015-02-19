using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.WebTesting;

namespace WebAndLoadTestProject
{
    public class LoadMissingDocument: WebTest
    {
        public override IEnumerator<WebTestRequest> GetRequestEnumerator()
        {
            var url = NorthwindUrlFactory.GenerateMissingOrderDocUrl();
            yield return new WebTestRequest(url)
            {
                ExpectedHttpStatusCode = 404
            };
        }
    }
}
