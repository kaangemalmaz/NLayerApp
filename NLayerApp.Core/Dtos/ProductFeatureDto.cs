namespace NLayerApp.Core.Dtos
{
    public class ProductFeatureDto
    {
        public int Id { get; set; }
        public string Color { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }

        //foreign key
        public int ProductId { get; set; }
    }
}
