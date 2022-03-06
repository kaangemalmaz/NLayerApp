﻿using AutoMapper;
using NLayerApp.Core.Dtos;
using NLayerApp.Core.Models;

namespace NLayerApp.Service.Mappings.AutoMapper
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<ProductFeature, ProductFeatureDto>().ReverseMap();
            //product gelirse onur productdto ya dönüştür demektir. reverse map demek tam tersini de yapabilirsin demektir.
        }
    }
}