using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.WebTesting;

namespace WebAndLoadTestProject.WebTests
{
    public class SimpleMatchQuery : WebTest

    {
        public override IEnumerator<WebTestRequest> GetRequestEnumerator()
        {
            yield return new WebTestRequest(NorthwindUrlFactory.GenerateRandomOrderMatchCompanyQueryUrl())
                {ExpectedHttpStatusCode = 200};
        }
    }
}
