using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.LoadTesting;
using RavenDBBenchmark;

namespace NorthwindLoadTestPlugin
{
    public class NorthwindLoadTestPlugin : ILoadTestPlugin
    {
        public void Initialize(LoadTest loadTest)
        {
            DatabaseFactory.GenerateTestDatabase();
        }
    }
}
