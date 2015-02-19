using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Raven.Abstractions.Indexing;
using Raven.Client.Document;
using Raven.Client.Extensions;
using Raven.Client.Indexes;
using Raven.Json.Linq;
using Orders;

namespace RavenDBBenchmark
{
    public class DatabaseFactory
    {
        public static DocumentStore GenerateTestDatabase(string databaseName = null, string serverUrl = null)
        {
            var documentStore = new DocumentStore()
            {
                DefaultDatabase = databaseName ?? "Northwind",
                Url = serverUrl ?? "http://localhost:8080"
            };
            GenerateNorthwindSampleDatabase(databaseName, serverUrl, documentStore);
            return documentStore;
        }

        private static void GenerateNorthwindSampleDatabase(string databaseName, string serverUrl, DocumentStore documentStore)
        {
            documentStore.Initialize();
            documentStore.DatabaseCommands.GlobalAdmin.DeleteDatabase(databaseName ?? "Northwind", hardDelete: true);
            documentStore.DatabaseCommands.GlobalAdmin.EnsureDatabaseExists(databaseName ?? "Northwind");
            using (var session = documentStore.OpenSession())
            {
                session.Store(new RavenJObject());
                session.SaveChanges();
                var res = session.Query<object>().Customize(x => x.WaitForNonStaleResults()).FirstOrDefault();
                session.Delete(res);
                session.SaveChanges();
            }
            var request =
                HttpWebRequest.Create(string.Format("{0}/databases/{1}/studio-tasks/createSampleData",
                    serverUrl ?? "http://localhost:8080", databaseName ?? "Northwind"));
            request.Method = "POST";
            request.ContentLength = 0;
            var response = (HttpWebResponse) request.GetResponse();
            GenerateSpatialDataAndIndex(documentStore);
            new OrdersByCompanyAndOrderedAtIndex().Execute(documentStore);
        }

        private static void GenerateSpatialDataAndIndex(DocumentStore documentStore)
        {
            var bulkInsert = documentStore.BulkInsert();
            
            for (int i = 0; i < 90; i++)
            {
                bulkInsert.Store(new Store() { Latitude = i, Longitude = i, Name = string.Format("store{0}", i) }, string.Format("stores/{0}", i));
            }
            bulkInsert.Dispose();
            new StoreSpatialIndex().Execute(documentStore);            
        }

        public class OrdersByCompanyAndOrderedAtIndex : AbstractIndexCreationTask<Order>
        {
            public OrdersByCompanyAndOrderedAtIndex()
            {
                Map = orders => from order in orders
                    select new
                    {
                        Company = order.Company,
                        OrderedAt = order.OrderedAt
                    };
            }
        }
    }
}
