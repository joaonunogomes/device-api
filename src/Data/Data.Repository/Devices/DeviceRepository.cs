using DeviceApi.Data.Repository.Generics;
using DeviceApi.Data.Repository.Mongo;
using DeviceApi.Domain.Model;

namespace DeviceApi.Data.Repository.Devices
{
    public class DeviceRepository : BaseRepository<Device>, IDeviceRepository
    {
        public DeviceRepository(IMongoContext context) : base(context)
        {

        }
    }
}
