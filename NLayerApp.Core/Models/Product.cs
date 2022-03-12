namespace NLayerApp.Core.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }

        //foreign Key
        public int CategoryId { get; set; }

        //navigation item
        public Category Category { get; set; }
        public ProductFeature ProductFeature { get; set; }
    }
}
