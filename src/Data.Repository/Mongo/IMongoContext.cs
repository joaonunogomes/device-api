using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace DeviceApi.Data.Repository.Mongo
{

    public interface IMongoContext : IDisposable
    {
        void AddCommand(Func<Task> func);

        Task<int> SaveChangesAsync();

        IMongoCollection<T> GetCollection<T>(string name);
    }
}