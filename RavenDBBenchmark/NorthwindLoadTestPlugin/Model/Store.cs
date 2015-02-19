
using System.Drawing;
using System.Linq;
using Raven.Client.Indexes;

namespace Orders
{
    public class Store
    {
        public string Name { get; set; }

        public int Latitude { get; set; }

        public int Longitude { get; set; }

        public Point SpatialData{get {return new Point(Latitude,Longitude);}}

    }

    public class StoreSpatialIndex : AbstractIndexCreationTask<Store>
    {
        public StoreSpatialIndex()
        {
            Map = stores => from s in stores
                select new
                {
                    Name = s.Name,
                    SpatialData = s.SpatialData
                };
            Spatial(x => x.SpatialData, options => options.Geography.Default());
        }
    }
}
