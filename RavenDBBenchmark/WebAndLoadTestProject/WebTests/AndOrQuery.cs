using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.WebTesting;

namespace WebAndLoadTestProject.WebTests
{
    public class AndOrQuery: WebTest
    {
        public override IEnumerator<WebTestRequest> GetRequestEnumerator()
        {
            yield return new WebTestRequest(NorthwindUrlFactory.GenerateRandomAndOrQueryUrl(true)) {ExpectedHttpStatusCode = 200};
            yield return new WebTestRequest(NorthwindUrlFactory.GenerateRandomAndOrQueryUrl(false)) { ExpectedHttpStatusCode = 200 };
        }
    }
}
