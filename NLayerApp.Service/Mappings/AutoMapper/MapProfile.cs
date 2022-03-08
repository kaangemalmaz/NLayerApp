using AutoMapper;
using NLayerApp.Core.Dtos;
using NLayerApp.Core.Models;

namespace NLayerApp.Service.Mappings.AutoMapper
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, ProductWithCategoryDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CategoryWithProductsDto>().ReverseMap();
            CreateMap<ProductFeature, ProductFeatureDto>().ReverseMap();
            //product gelirse onur productdto ya dönüştür demektir. reverse map demek tam tersini de yapabilirsin demektir.
        }
    }
}
