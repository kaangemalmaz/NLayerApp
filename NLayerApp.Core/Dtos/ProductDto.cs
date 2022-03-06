namespace NLayerApp.Core.Dtos
{
    public class ProductDto : BaseDto
    {
        public string Name { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        
        //foreign Key
        public int CategoryId { get; set; }
    }
}
