using NLayerApp.Core.UnitOfWorks;

namespace NLayerApp.Repository.UnitofWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _appDbContext;

        public UnitOfWork(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void Commit()
        {
            _appDbContext.SaveChanges();
        }

        public async Task CommitAsync()
        {
            //result = asenkron metodu sekrona çevirir.

            await _appDbContext.SaveChangesAsync();
        }
    }
}
