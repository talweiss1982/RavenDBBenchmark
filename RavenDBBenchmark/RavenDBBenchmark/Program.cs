using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client.Document;
using RavenDBOrdersWebTestPlugin.Model;


namespace RavenDBBenchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var store = new DocumentStore() {Url = "http://localhost.fiddler:8080", DefaultDatabase = "OrdersDB"}.Initialize()
                )
            {
                using (var session = store.OpenSession())
                {
                    var res = session.Query<Product>().Customize(x => x.RandomOrdering()).Take(1).ToList();
                    int i = 0;
                }
            }
            //PerformanceMonitoring.MonitorDiskSpace();
            //QueryGenerator.GenerateSimpleRandomNorthwindEntityQuery("http://localhost:8080", "Northwind");

        }
    }
}
