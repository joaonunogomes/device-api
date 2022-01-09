using DeviceApi.Data.Repository.Mongo;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DeviceApi.Data.Repository.Generics
{

    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected readonly IMongoContext dbContext;
        protected readonly IMongoCollection<TEntity> collection;

        protected BaseRepository(IMongoContext context)
        {
            dbContext = context;
            collection = dbContext.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public virtual async Task AddAsync(TEntity obj)
        {
            await this.collection.InsertOneAsync(obj);

            return;
        }

        public virtual async Task<TEntity> GetAsync(Guid id)
        {
            var data = await this.collection.FindAsync(Builders<TEntity>.Filter.Eq("_id", id));
            return data.FirstOrDefault();
        }

        public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> query)
        {
            return (await this.collection.FindAsync(query))?.ToList()?.FirstOrDefault();
        }

        public virtual async Task<IEnumerable<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> query)
        {
            return (await this.collection.FindAsync(query))?.ToList();
        }

        public virtual async Task DeleteOneAsync(Expression<Func<TEntity, bool>> query)
        {
            await this.collection.DeleteOneAsync(query, default);
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            await this.collection.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", id));
        }

        public virtual async Task UpdateAsync(Expression<Func<TEntity, bool>> query, TEntity obj)
        {
            await this.collection.ReplaceOneAsync(query, obj);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
