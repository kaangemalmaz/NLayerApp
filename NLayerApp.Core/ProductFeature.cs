using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayerApp.Core
{
    public class ProductFeature
    {
        public int Id { get; set; }
        public string Color { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }

        //foreign key
        public int ProductId { get; set; }
        //navigation item
        public Product Product { get; set; }
    }
}
