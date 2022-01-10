namespace DeviceApi.Data.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IBaseRepository<TEntity> : IDisposable where TEntity : class
    {
        Task AddAsync(TEntity obj);

        Task<TEntity> GetAsync(Guid id);

        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> query);

        Task<IEnumerable<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> query);
        
        Task DeleteAsync(Guid id);

        Task UpdateAsync(Expression<Func<TEntity, bool>> query, TEntity obj);
    }
}
