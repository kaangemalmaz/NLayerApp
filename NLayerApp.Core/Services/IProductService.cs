﻿using NLayerApp.Core.Dtos;
using NLayerApp.Core.Models;

namespace NLayerApp.Core.Services
{
    public interface IProductService : IService<Product>
    {
        Task<CustomResponseDto<List<ProductWithCategoryDto>>> GetProductsWithCategory();
    }
}
