namespace NLayerApp.Core
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }

        //navigation item
        public ICollection<Product> Products { get; set; }
    }
}
