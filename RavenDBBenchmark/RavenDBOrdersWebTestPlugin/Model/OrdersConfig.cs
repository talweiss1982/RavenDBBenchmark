using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenDBOrdersWebTestPlugin.Model
{
    public static class OrdersConfig
    {
        public static int NumberOfProducts = 1;
        public static int InitialOrdersPerCustomer = 1;
        public static int InitialOrderLinesPerOrder = 1;
        public static long CapacityOfOrderHiloKeyGenerator = 1000000;
        public static long CapacityOfCustomerHiloKeyGenerator = 1000000;
        public static string BulkInsertLogFilePath = @"C:\work\Tests\BulkInsert.txt";
    }
}
