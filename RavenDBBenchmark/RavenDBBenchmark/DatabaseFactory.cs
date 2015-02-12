using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Raven.Client.Document;
using Raven.Json.Linq;

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
        }
    }
}
