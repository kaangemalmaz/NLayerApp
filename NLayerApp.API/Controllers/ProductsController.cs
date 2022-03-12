using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NLayerApp.API.Filters;
using NLayerApp.Core.Dtos;
using NLayerApp.Core.Models;
using NLayerApp.Core.Services;

namespace NLayerApp.API.Controllers
{
    //base classında var artık buraya koymaya gerek yok.
    //[Route("api/[controller]")]
    //[ApiController]
    public class ProductsController : CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly IProductService _productService;

        public ProductsController(IMapper mapper, IProductService productService)
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
            return CreateActionResult(CustomResponseDto<List<ProductDto>>.Success(200, productsDtos));
        }

        //burası direk IAsyncFilter i kullandığı için böyle ctor da impl. aldığı için çok kritik.
        //burada daha hiç actiona girmeden gider. unutma! service de yazılan business a kadar gider geri döner.
        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        //urldeki idyi alır.
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
            var product = await _productService.AddAsync(_mapper.Map<Product>(productDto));
            var productDtoReturn = _mapper.Map<ProductDto>(product);
            return CreateActionResult(CustomResponseDto<ProductDto>.Success(201, productDtoReturn));
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

        //YUKARDAKI ILE BUNUN FARKI YUKARDAKILER GENERICDEN GELIYOR 
        //ASAGIDAKI OZELLESMIS YAPI OLDUĞU ICIN HER TURLU DONUS SAGLANABILIR.

        [HttpGet("ProductWithCategory")]
        public async Task<IActionResult> ProductWithCategory()
        {
            return CreateActionResult(await _productService.GetProductsWithCategory());
        }


    }
}
