using Microsoft.EntityFrameworkCore;
using NLayerApp.Core.Repository;
using System.Linq.Expressions;

namespace NLayerApp.Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        //özel bir sorgu yazmak istersen diye burayı protected yapıyorum.
        //readonly olunca ya tanımlandığı noktada yada constructor da değer atılabilir demektir. Başka yerde set edilemez. !!
        protected readonly AppDbContext _appDbContext;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _dbSet = _appDbContext.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);  
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.AnyAsync(expression);
        }

        public IQueryable<T> GetAll()
        {
            //burada asnotracking demek efcore çekmiş olduğu datayı memory alıp tek tek durumlarını izlemesin diyedir. Önemli!
            //10bin kaydı alır tek tek izlerse performansı sıkıntıya sokar.
            return _dbSet.AsNoTracking().AsQueryable();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void Remove(T entity)
        {

            //burada direk olarak silme yok aslında. burada efcore aslında silinecek datanın statini silinecek olarak işaretliyor. savechanges gelene kadar bekliyor.
            //yüklü bir işlem olmadığı için remove a gerek yok.
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression);
        }
    }
}
