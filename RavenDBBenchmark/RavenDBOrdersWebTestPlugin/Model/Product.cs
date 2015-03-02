using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using System.Drawing;
namespace RavenDBOrdersWebTestPlugin.Model
{
    public class Product
    {
        public String Name { get; set; }
        public decimal Price { get; set; }
        public List<String> Tags { get; set; }
        public decimal Weight { get; set; }
        public String Manufacturer { get; set; }
        public String Color { get; set; }
        public static Product GenerateProduct(string productName = null)
        {
            var p = new Product();
            p.Name = productName;
            p.Price = Utiles.NextRandom(0.0, 1000.0);
            p.Tags = new List<string>();
            var numOfTags = Utiles.NextRandom(0, 5);
            for (int tagCount = 0; tagCount < numOfTags; tagCount++)
            {
                var tagNum = Utiles.NextRandom(1, 100);
                p.Tags.Add("Tag"+tagNum);
            }
            p.Weight = Utiles.NextRandom(0.001, 10.0);
            p.Manufacturer = "Manufacturer" + Utiles.NextRandom(1, 100);
            p.Color = ColorNames.Value[Utiles.NextRandom(0, ColorNames.Value.Keys.Count)];
            return p;
        }


        public static readonly ThreadLocal<Dictionary<int,string>> ColorNames = new ThreadLocal<Dictionary<int, string>>(
            () =>
            {
                Dictionary<int,string> colorDictionary  = new Dictionary<int, string>();
                var colors = (KnownColor[])Enum.GetValues(typeof(KnownColor));
                for (int i = 0; i < colors.Length; i++)
                {
                    colorDictionary[i] = colors[i].ToString();
                }
                return colorDictionary;
            });
    }
}
