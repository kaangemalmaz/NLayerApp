using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NLayerApp.Core.Dtos;
using NLayerApp.Core.Models;
using NLayerApp.Core.Services;

namespace NLayerApp.API.Controllers
{
    //base classında var artık buraya koymaya gerek yok.
    //[Route("api/[controller]")]
    //[ApiController]
    public class Products : CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly IService<Product> _productService;

        public Products(IMapper mapper, IService<Product> productService)
        {
            _mapper = mapper;
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAll();
            var productsDtos = _mapper.Map<List<ProductDto>>(products.ToList());
            //return Ok(CustomResponseDto<List<ProductDto>>.Success(200, productsDtos));
            return CreateActionResult(CustomResponseDto<List<ProductDto>>.Success(200,productsDtos));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            var productDto = _mapper.Map<ProductDto>(product);
            return CreateActionResult(CustomResponseDto<ProductDto>.Success(200, productDto));
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductDto productDto)
        {
            await _productService.AddAsync(_mapper.Map<Product>(productDto));
            return CreateActionResult(CustomResponseDto<ProductDto>.Success(201, productDto));
        }

        [HttpPut]
        public async Task<IActionResult> Update(ProductDto productDto)
        {
            await _productService.UpdateAsync(_mapper.Map<Product>(productDto));
            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            await _productService.RemoveAsync(product);
            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }


    }
}
