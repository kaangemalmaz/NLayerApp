using Microsoft.EntityFrameworkCore;
using NLayerApp.Core.Models;
using NLayerApp.Core.Repositories;

namespace NLayerApp.Repository.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public async Task<Category> GetCategoryByIdWithProductsAsync(int categoryId)
        {
            //single or default o idden db de 1 tane olmasını bekler birden fazla dönerse hata döner.
            //first or default dbde birden fazla olsa bile ilk bulduğunu döner.
            return await _appDbContext.Categories.Include(c => c.Products).Where(p => p.Id == categoryId).SingleOrDefaultAsync();
        }
    }
}
