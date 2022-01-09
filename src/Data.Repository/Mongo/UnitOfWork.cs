
namespace DeviceApi.Data.Repository.Mongo
{
    using System;
    using System.Threading.Tasks;

    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMongoContext _context;

        public UnitOfWork(IMongoContext context)
        {
            _context = context;
        }

        public async Task<bool> CommitAsync()
        {
            var changeAmount = await _context.SaveChangesAsync();

            return changeAmount > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}