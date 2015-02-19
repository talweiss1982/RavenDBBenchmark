using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RavenDBOrdersWebTestPlugin;
using System.Web;
using RavenDBOrdersWebTestPlugin.Model;
namespace RavenDBOrdersWebTest
{
    public static class OrdersDatabaseUrlFactory
    {
        public static string DatabaseUrl = OrdersDatabaseFactory.ServerUrl + "/databases/" + OrdersDatabaseFactory.DatabaseName;
        [ThreadStatic]
        private static Random _random;

        public static string GenerateBulkDocsUrl()
        {
            return DatabaseUrl + "/bulk_docs";
        }

        public static string GenerateFetchRandomCustomersQueryUrl()
        {
            return DatabaseUrl + "/indexes/Raven/DocumentsByEntityName?query=Tag%3ACustomers&start=0&pageSize=1&sort=__random%3" + Guid.NewGuid().ToString();

        }

        public static string GenerateLoadCustoemrUrl(int id)
        {
            return DatabaseUrl + "/docs/Customers/" + id;
        }
        public static string GenerateLoadOrderUrl(string id)
        {
            return DatabaseUrl + "/docs/" + id;
        }

        public static string GeneratePriceFacetQuery()
        {
             //http://localhost:8080/databases/OrdersDB/indexes/dynamic/Products?query=Price_Range:[Dx0 TO Dx10]           
            //Need to generate index for facet
            return DatabaseUrl + "/facets/ProductIndexByPriceAndWeightAndManufacturerAndColor?query=facetStart=0&facetPageSize=&facets="
                + HttpUtility.UrlEncode("[{\"Mode\":1,\"Name\":\"Price_Range\",\"Ranges\":[\"[Dx0.0 TO Dx250.0]\",\"[Dx250.0 TO Dx500.0]\",\"[Dx500.0 TO Dx750.0]\",\"[Dx750.0 TO Dx1000.0]\"]}]");
        }

        public static string GenerateManufacturerFacetOverPriceQuery()
        {
            var priceRangeQuery = GeneratePriceRangeQuery();
            return DatabaseUrl + "/facets/ProductIndexByPriceAndWeightAndManufacturerAndColor?query=" + priceRangeQuery + "&facetStart=0&facetPageSize=&facets=" +
                HttpUtility.UrlEncode("[{\"Mode\":0,\"Name\":\"Manufacturer\"}]");
        }

        private static string GeneratePriceRangeQuery()
        {
            string priceRangeQuery = "Price_Range:";
            switch (Utiles.NextRandom(1, 5))
            {
                case 1:
                    priceRangeQuery += "[Dx0.0 TO Dx250.0]";
                    break;
                case 2:
                    priceRangeQuery += "[Dx250.0 TO Dx500.0]";
                    break;
                case 3:
                    priceRangeQuery += "[Dx500.0 TO Dx750.0]";
                    break;
                case 4:
                    priceRangeQuery += "[Dx750.0 TO Dx1000.0]";
                    break;
            }
            return priceRangeQuery;
        }

        public static string GenerateWeightFacetOverPriceAndManufacturerQuery()
        {
            var priceRangeQuery = GeneratePriceRangeQuery();
            string manufacturerQuery = "Manufacturer:Manufacturer"+Utiles.NextRandom(1,100);
            return DatabaseUrl + "/facets/ProductIndexByPriceAndWeightAndManufacturerAndColor?query=" + priceRangeQuery + " AND " + manufacturerQuery + "&facetStart=0&facetPageSize=&facets=" +
                HttpUtility.UrlEncode("[{\"Mode\":1,\"Name\":\"Weight_Range\",\"Ranges\":[\"[Dx0.0 TO Dx2.5]\",\"[Dx2.5 TO Dx5.0]\",\"[Dx5.0 TO Dx7.5]\",\"[Dx7.5 TO Dx10.0]\"]}]");
        }

        public static string GenerateColorFacetOverPriceAndManufacturerAndWeightQuery()
        {
            var priceRangeQuery = GeneratePriceRangeQuery();
            string manufacturerQuery = "Manufacturer:Manufacturer" + Utiles.NextRandom(1, 100);
            var weightRangeQuery = GenerateWeightRangeQuery();
            var colorQuery = "Color:" + Product.ColorNames.Value[Utiles.NextRandom(0,Product.ColorNames.Value.Count)];
            return DatabaseUrl + "/facets/ProductIndexByPriceAndWeightAndManufacturerAndColor?query=" + priceRangeQuery + " AND " + manufacturerQuery + " AND " + weightRangeQuery + "&facetStart=0&facetPageSize=&facets=" +
                HttpUtility.UrlEncode("[{\"Mode\":0,\"Name\":\"Color\"}]");
        }

        private static string GenerateWeightRangeQuery()
        {
            string weightRangeQuery = "Weight_Range:";
            switch (Utiles.NextRandom(1, 5))
            {
                case 1:
                    weightRangeQuery += "[Dx0.0 TO Dx2.5]";
                    break;
                case 2:
                    weightRangeQuery += "[Dx2.5 TO Dx5.0]";
                    break;
                case 3:
                    weightRangeQuery += "[Dx5.0 TO Dx7.5]";
                    break;
                case 4:
                    weightRangeQuery += "[Dx7.5 TO Dx10.0]";
                    break;
            }
            return weightRangeQuery;
        }
    }
}
