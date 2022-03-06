using System.Linq.Expressions;

namespace NLayerApp.Core.Services
{
    public interface IService<T> where T : class
    {
        //select
        //===========================================================
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAll();
        //Buradaki T entity, bool ise dönüş tipidir x>5 denirse her bir satır için 5 den büyükse true değilse false döner. yani trueları getirir.
        IQueryable<T> Where(Expression<Func<T, bool>> expression);
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);

        //insert - delete - update
        //===========================================================
        //add async var çünkü memoride uzun süren bir işlemdir.
        Task<T> AddAsync(T entity);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
        //sadece statusünü değiştiren yapılardır. Uzun süren yapılar olmadığı için asenkron metodları yoktur.
        //async in amacı uzun süren yapılar yüzünden blok yememek için kısa süren işlemler için async yoktur.
        Task UpdateAsync(T entity);
        Task RemoveAsync(T entity);
        Task RemoveRangeAsync(IEnumerable<T> entities);
    }
}
