using Microsoft.AspNetCore.Mvc;
using NLayerApp.Core.Services;

namespace NLayerApp.API.Controllers
{
    //baseden gelsin.
    //[Route("api/[controller]")]
    //[ApiController]
    public class CategoriesController : CustomBaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        //her ikiside çalışıyor aslında buna bir bak.
        //[HttpGet("[action]")]
        [HttpGet("[action]/{categoryId}")]
        public async Task<IActionResult> GetCategoryByIdWithProducts(int categoryId)
        {
            return CreateActionResult(await _categoryService.GetCategoryByIdWithProductsAsync(categoryId));
        }
    }
}
