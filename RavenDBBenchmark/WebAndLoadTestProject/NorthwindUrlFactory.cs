using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace WebAndLoadTestProject
{
    public static class NorthwindUrlFactory
    {
        public static string RavenUrl = "http://localhost:8080/databases/Northwind";
        public static int MaxCompanyId = 91;
        public static int MaxOrderId = 830;

        [ThreadStatic]
        private static Random _random;

        public static int RandNext(int min, int max)
        {
            if (_random == null)
                _random = new Random(205);

            return _random.Next(min, max);
        }

        public static string GenerateRandomCompanyDocUrl()
        {
            var next = RandNext(1, MaxCompanyId);
            return RavenUrl + "/docs/Companies/" + next;
        }

        public static string GenerateRandomOrderDocDocUrl()
        {
            string orderId;
            return GenerateRandomOrderDocDocUrl(out orderId);
        }

        public static string GenerateRandomOrderDocDocUrl(out string orderId)
        {
            var next = RandNext(1, MaxOrderId);
            orderId = next.ToString();
            return RavenUrl + "/docs/Orders/" + next;
        }

        public static string GenerateRandomOrderDocIncludeCompanyUrl()
        {
            var next = RandNext(1, MaxOrderId);
            return RavenUrl + "/docs/orders/" + next + "?include=Company";
        }

        public static string GenerateBulkDocsUrl()
        {
            return RavenUrl + "/bulk_docs";
        }

        public static string GenerateMissingOrderDocUrl()
        {
            var next = RandNext(MaxOrderId + 1, MaxOrderId + 100);
            return RavenUrl + "/docs/Orders/" + next;
        }

        public static string GeneratePersonDocUrl(int personId)
        {
            return RavenUrl + "/docs/Persons/" + personId;
        }

        public static string GenerateRandomOrderMatchCompanyQueryUrl()
        {
            var companyQuery = GenerateCompanyQueryUrl();
            return RavenUrl + "/indexes/dynamic/Orders?query=" + companyQuery;
        }

        private static string GenerateCompanyQueryUrl()
        {
            var companyId = RandNext(1, MaxCompanyId);
            var companyQuery = "Company:companies/" + companyId;
            return companyQuery;
        }

        private static int maxDays = 628;
        private static DateTime minOrderDate = DateTime.Parse("1996-07-26T00:00:00.0000000");
        //HomeState:[Illinois TO Maryland]
        public static string GenerateRandomOrderOrderedAtQueryUrl()
        {
            var dateTimeRange = GenerateDateTimeRangeQueryUrl();
            return RavenUrl + "/indexes/dynamic/Orders?query=" + dateTimeRange;
        }

        private static string GenerateDateTimeRangeQueryUrl()
        {
            var firstOffset = RandNext(0, maxDays);
            var secondOffset = RandNext(0, maxDays - firstOffset) + firstOffset;
            var culture = new CultureInfo("he-IL");
            var dateTimeRange =
                HttpUtility.UrlEncode("OrderedAt:[" + minOrderDate.AddDays(firstOffset).ToString("O", culture) + " TO " +
                                      minOrderDate.AddDays(secondOffset).ToString("O", culture) + "]");
            return dateTimeRange;
        }

        public static string GenerateRandomAndOrQueryUrl(bool isAndQuery)
        {
            return RavenUrl + "/indexes/dynamic/Orders?query=" + GenerateDateTimeRangeQueryUrl() + 
                (isAndQuery?" AND ":" OR ") + GenerateCompanyQueryUrl();
        }

        public static string GenerateStoreSpatialIndexQueryUrl()
        {
            var storeId = RandNext(0, 90);
            return RavenUrl +
                   "/indexes/StoreSpatialIndex?&queryShape="+HttpUtility.UrlEncode(string.Format("Circle({0}.000000 {0}.000000 d=10.000000)&spatialRelation=Within&spatialField=SpatialData&distErrPrc=0.025&pageSize=128)",storeId));
        }

        public static string GenerateDynamicFacetQueryUrl()
        {

            return RavenUrl + "/facets/OrdersByCompanyAndOrderedAtIndex?query=" + GenerateDateTimeRangeQueryUrl()+
                "&facetStart=0&facetPageSize=&facets=" + HttpUtility.UrlEncode("[{\"Mode\":1,\"Name\":\"OrderedAt\",\"Ranges\":[\"[1996-07-04T00:00:00.0000000 TO 1996-11-04T00:00:00.0000000]\",\"[1996-11-04T00:00:00.0000000 TO 1997-04-04T00:00:00.0000000]\",\"[1997-04-04T00:00:00.0000000 TO 1997-11-04T00:00:00.0000000]\",\"[1997-11-04T00:00:00.000000 TO 1998-07-04T00:00:00.0000000]\"]}]");
        }
    }
}
