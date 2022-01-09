using DeviceApi.Application.Dto;
using DeviceApi.Data.Repository.Devices;
using System;
using System.Threading.Tasks;

namespace DeviceApi.Application.Services.Devices
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository deviceRepository;

        public DeviceService(IDeviceRepository deviceRepository)
        {
            this.deviceRepository = deviceRepository;
        }

        public async Task<Device> CreateAsync(Device device)
        {
            device.Id = Guid.NewGuid();

            await this.deviceRepository.AddAsync(device);

            return device;
        }

        public Task<Device> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
