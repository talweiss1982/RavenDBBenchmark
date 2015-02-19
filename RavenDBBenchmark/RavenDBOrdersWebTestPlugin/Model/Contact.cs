using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FizzWare.NBuilder;

namespace RavenDBOrdersWebTestPlugin.Model
{
    public class Contact
    {
        public string Name { get; set; }
        public string Title { get; set; }

        public static Contact GenerateContact()
        {
            return builder.Value.Build();
        }
        private static readonly ThreadLocal<ISingleObjectBuilder<Contact>> builder = new ThreadLocal<ISingleObjectBuilder<Contact>>(Builder<Contact>.CreateNew);
    }
}
