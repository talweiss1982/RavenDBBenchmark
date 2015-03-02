using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.WebTesting;

namespace RavenDBOrdersWebTest
{
    public class FacetQuery : RavenWebTest
    {
        protected override IEnumerable<WebTestRequest> GetRequests()
        {
            yield return new WebTestRequest(OrdersDatabaseUrlFactory.GeneratePriceFacetQuery())
            {
                ExpectedHttpStatusCode = 200,
                ReportingName = "FacetQuery_PriceFacet"
            };
            yield return new WebTestRequest(OrdersDatabaseUrlFactory.GenerateManufacturerFacetOverPriceQuery())
            {
                ExpectedHttpStatusCode = 200,
                ReportingName = "FacetQuery_ManufacturerFacet"
            };
            yield return new WebTestRequest(OrdersDatabaseUrlFactory.GenerateWeightFacetOverPriceAndManufacturerQuery())
            {
                ExpectedHttpStatusCode = 200,
                 ReportingName = "FacetQuery_WeightFacetOverPriceAndManufacturer"

            };
            yield return new WebTestRequest(OrdersDatabaseUrlFactory.GenerateColorFacetOverPriceAndManufacturerAndWeightQuery())
            {
                ExpectedHttpStatusCode = 200,
                 ReportingName = "FacetQuery_ColorFacetOverPriceAndManufacturerAndWeight"
            };
        }
    }
}
