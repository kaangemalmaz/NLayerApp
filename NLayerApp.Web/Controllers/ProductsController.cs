using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NLayerApp.Core.Dtos;
using NLayerApp.Core.Models;
using NLayerApp.Web.Filters;
using NLayerApp.Web.Services;

namespace NLayerApp.Web.Controllers
{
    public class ProductsController : Controller
    {

        private readonly ProductApiService _productApiService;
        private readonly CategoryApiService _categoryApiService;

        public ProductsController(ProductApiService productApiService, CategoryApiService categoryApiService)
        {
            _productApiService = productApiService;
            _categoryApiService = categoryApiService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _productApiService.GetProductsWithCategory());
        }

        [HttpGet]
        public async Task<IActionResult> Save()
        {
            var categories = await _categoryApiService.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryApiService.GetAllAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                return View();
            }

            await _productApiService.Save(productDto);
            return RedirectToAction(nameof(Index));
        }


        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _productApiService.GetById(id);
            //if (product == null)
            //    return RedirectToAction("Error", "Home");

            var categories = await _categoryApiService.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryApiService.GetAllAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name", productDto.CategoryId);
                return View(productDto);
            }

            await _productApiService.Update(productDto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productApiService.GetById(id);
            if (product == null)
                return RedirectToAction("Error", "Home");

            await _productApiService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
