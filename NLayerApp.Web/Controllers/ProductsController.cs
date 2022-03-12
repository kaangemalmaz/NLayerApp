using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NLayerApp.Core.Dtos;
using NLayerApp.Core.Models;
using NLayerApp.Core.Services;
using NLayerApp.Web.Filters;

namespace NLayerApp.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, ICategoryService categoryService, IMapper mapper)
        {
            _productService = productService;
            _categoryService = categoryService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            return View((await _productService.GetProductsWithCategory()).Data);
        }

        [HttpGet]
        public async Task<IActionResult> Save()
        {
            var categories = await _categoryService.GetAll();
            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories.ToList());
            ViewBag.Categories = new SelectList(categoriesDto, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetAll();
                var categoriesDto = _mapper.Map<List<CategoryDto>>(categories.ToList());
                ViewBag.Categories = new SelectList(categoriesDto, "Id", "Name");
                return View();
            }

            await _productService.AddAsync(_mapper.Map<Product>(productDto));
            return RedirectToAction(nameof(Index));
        }


        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            //if (product == null)
            //    return RedirectToAction("Error", "Home");

            var categories = await _categoryService.GetAll();
            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories.ToList());
            ViewBag.Categories = new SelectList(categoriesDto, "Id", "Name", product.CategoryId);

            return View(_mapper.Map<ProductDto>(product));
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetAll();
                var categoriesDto = _mapper.Map<List<CategoryDto>>(categories.ToList());
                ViewBag.Categories = new SelectList(categoriesDto, "Id", "Name", productDto.CategoryId);
                return View(productDto);
            }

            await _productService.UpdateAsync(_mapper.Map<Product>(productDto));
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
                return RedirectToAction("Error", "Home");

            await _productService.RemoveAsync(product);
            return RedirectToAction(nameof(Index));
        }
    }
}
