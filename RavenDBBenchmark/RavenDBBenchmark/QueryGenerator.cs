using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RavenDBBenchmark
{
    public class QueryGenerator
    {
        //http://localhost:8080/databases/Northwind/indexes/Raven/DocumentsByEntityName?query=__document_id%3Aorders%2F238&start=0&pageSize=22 
        public static Uri GenerateSimpleRandomNorthwindEntityQuery(string serverUrl, string databaseName)
        {
            var collectionId = IntToIds[random.Next(IdsToSize.Count)];
            var sb = new StringBuilder();
            sb.Append(String.Format("{0}/databases/{1}/indexes/Raven/DocumentsByEntityName?", serverUrl, databaseName));
            sb.Append(HttpUtility.UrlEncode(String.Format("query=__document_id:{0}/{1}&start=0&pageSize=22",collectionId, random.Next(IdsToSize[collectionId] ))));
            var uriStr = sb.ToString();
            return new Uri(uriStr);
        }

        private static int seed = 0;
        private static Random random = seed==0?new Random():new Random(seed);
        private static Dictionary<string,int> IdsToSize = new Dictionary<string,int>()
        {
            { "categories",9  }, {  "companies",92 }, { "employees",10 }, { "orders",831 }, {"products",78}, {"regions",5}, {"shippers",4}, {"suppliers",30}
        };
        private static Dictionary<int,string> IntToIds = new Dictionary<int, string>()
        {
            { 0,"categories"}, {  1,"companies"}, { 2,"employees"}, { 3,"orders"}, {4,"products"}, {5,"regions"}, {6,"shippers"}, {7,"suppliers"}
        };
    }
}
