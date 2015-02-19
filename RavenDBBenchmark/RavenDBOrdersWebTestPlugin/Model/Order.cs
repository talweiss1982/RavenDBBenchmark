using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FizzWare.NBuilder;

namespace RavenDBOrdersWebTestPlugin.Model
{
    public class Order
    {
        public DateTime OrderAt { get; set; }
        public List<ProductLine> Lines { get; set; }
        public static Order GenerateOrder()
        {
            return builder.Value.Build();
        }

        public Order Clone()
        {
            return new Order() {OrderAt = this.OrderAt,Lines = new List<ProductLine>(this.Lines)};
        }
        private static readonly ThreadLocal<ISingleObjectBuilder<Order>> builder = new ThreadLocal<ISingleObjectBuilder<Order>>(Builder<Order>.CreateNew);
    }
}
