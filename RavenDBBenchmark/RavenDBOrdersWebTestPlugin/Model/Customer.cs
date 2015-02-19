using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FizzWare.NBuilder;

namespace RavenDBOrdersWebTestPlugin.Model
{
    public class Customer
    {
        public string Name { get; set; }
        public Contact Contact { get; set; }
        public String Phone { get; set; }
        public String AddressInfo { get; set; }
        /// <summary>
        /// holds ids of orders
        /// </summary>
        public List<string> Orders { get; set; } 
        public static Customer GenerateCustomer()
        {
            var customer = builder.Value.Build();
            customer.Contact = Model.Contact.GenerateContact();
            return customer;
        }
        private static readonly ThreadLocal<ISingleObjectBuilder<Customer>> builder = new ThreadLocal<ISingleObjectBuilder<Customer>>(Builder<Customer>.CreateNew);
    }
}
