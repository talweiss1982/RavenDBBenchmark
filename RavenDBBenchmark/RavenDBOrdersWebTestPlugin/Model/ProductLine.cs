using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FizzWare.NBuilder;

namespace RavenDBOrdersWebTestPlugin.Model
{
    public class ProductLine
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }

        public static ProductLine GenerateProductLine()
        {
            var productLine = new ProductLine();
            productLine.Quantity = Utiles.NextRandom(1, 100);
            productLine.Discount = Utiles.NextRandom(0.0, 0.5);
            return productLine;          
        }
        
    }
}
