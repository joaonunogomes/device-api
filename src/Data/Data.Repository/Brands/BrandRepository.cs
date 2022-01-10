using DeviceApi.Data.Repository.Generics;
using DeviceApi.Data.Repository.Mongo;
using DeviceApi.Domain.Model;

namespace DeviceApi.Data.Repository.Brands
{
    public class BrandRepository : BaseRepository<Brand>, IBrandRepository
    {
        public BrandRepository(IMongoContext context) : base(context)
        {

        }
    }
}
