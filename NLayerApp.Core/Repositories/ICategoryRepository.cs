using NLayerApp.Core.Models;
using NLayerApp.Core.Repository;

namespace NLayerApp.Core.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<Category> GetCategoryByIdWithProductsAsync(int categoryId);
    }
}
