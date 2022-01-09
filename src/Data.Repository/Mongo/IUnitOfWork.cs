using System;
using System.Threading.Tasks;

namespace DeviceApi.Data.Repository.Mongo
{

    public interface IUnitOfWork : IDisposable
    {
        Task<bool> CommitAsync();
    }
}