using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.LoadTesting;
using Raven.Abstractions.Indexing;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Json.Linq;
using RavenDBOrdersWebTestPlugin.Model;

namespace RavenDBOrdersWebTestPlugin
{
    public class OrdersDatabaseFactory : ILoadTestPlugin
    {
        public void Initialize(LoadTest loadTest)
        {
            GenerateTestDatabase(loadTest.Name);
        }
        public static void GenerateTestDatabase(String loadTestName)
        {
            _documentStore = new DocumentStore()
            {
                DefaultDatabase = DatabaseName,
                Url = ServerUrl
            }.Initialize();
            if (ShouldGenerateNewDBForTestSet.Contains(loadTestName))
                GenerateOrdersDBSampleDatabase();
        }

        private static void GenerateOrdersDBSampleDatabase()
        {
            _documentStore.DatabaseCommands.GlobalAdmin.DeleteDatabase(DatabaseName, hardDelete: true);
            _documentStore.DatabaseCommands.GlobalAdmin.EnsureDatabaseExists(DatabaseName);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            using (var bulkInsert = _documentStore.BulkInsert())
            {
                for (int productCount = 0; productCount < OrdersConfig.NumberOfProducts; productCount++)
                {
                    bulkInsert.Store(Product.GenerateProduct("Product" + productCount));
                }
            }
            sw.Stop();
            var writer = File.CreateText(OrdersConfig.BulkInsertLogFilePath);
            writer.WriteLine(String.Format("It took {0}(ms) to insert {1} products into the database.",sw.ElapsedMilliseconds,OrdersConfig.NumberOfProducts));
            writer.Close();
            new ProductIndexByPriceAndWeightAndManufacturerAndColor().Execute(_documentStore);
            while (_documentStore.DatabaseCommands.GetStatistics().StaleIndexes.Count() != 0)
            {
                Thread.Sleep(100);
            }
        }
        private static HashSet<String> ShouldGenerateNewDBForTestSet = new HashSet<string>() { "WriteInitialDatabase", "GenerateDatabase" };
        private static IDocumentStore _documentStore = null;
        private static DocumentConvention _convention = new DocumentConvention();
        private static HiLoKeyGenerator _ordersHiloGenerator = new HiLoKeyGenerator("orders", OrdersConfig.CapacityOfOrderHiloKeyGenerator);
        private static HiLoKeyGenerator _customersHiloGenerator = new HiLoKeyGenerator("customers", OrdersConfig.CapacityOfCustomerHiloKeyGenerator);
        public static readonly string DatabaseName = "OrdersDB";
        public static readonly string ServerUrl = "http://localhost:8080";

        public static string GenerateNewOrderId(object entity)
        {
            return _ordersHiloGenerator.GenerateDocumentKey(_documentStore.DatabaseCommands, _convention, entity);
        }
        public static string GenerateNewCustoemrId(object entity)
        {
            return _customersHiloGenerator.GenerateDocumentKey(_documentStore.DatabaseCommands, _convention, entity);
        }
    }

    public class ProductIndexByPriceAndWeightAndManufacturerAndColor : AbstractIndexCreationTask<Product>
    {
        public ProductIndexByPriceAndWeightAndManufacturerAndColor()
        {
            Map = products => from product in products
                select new
                {
                    product.Price,
                    product.Weight,
                    product.Manufacturer,
                    product.Color
                };
            Sort(x=>x.Price,SortOptions.Double);
            Sort(x => x.Weight, SortOptions.Double);
        }
    }
    
}
