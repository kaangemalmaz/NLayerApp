namespace NLayerApp.Core.Dtos
{
    public class ProductWithCategoryDto : ProductDto
    {
        //public ProductDto ProductDto { get; set; }


        //buraya category yazmak zorundasın çünkü automapper map ederken diğer türlü mapleyemiyor unutma!
        public CategoryDto Category { get; set; }
    }
}
