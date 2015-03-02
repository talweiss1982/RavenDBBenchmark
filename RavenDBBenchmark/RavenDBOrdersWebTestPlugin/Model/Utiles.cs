using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenDBOrdersWebTestPlugin.Model
{
    public static class Utiles
    {
        private static Random _rand = new Random();

        public static int NextRandom(int from, int to)
        {
            return _rand.Next(from, to);
        }

        public static decimal NextRandom(decimal from, decimal to)
        {
            return (decimal)_rand.NextDouble()*(to - from) + from;
        }
        public static decimal NextRandom(double from, double to)
        {
            return (decimal)(_rand.NextDouble() * (to - from) + from);
        }
    }
}
