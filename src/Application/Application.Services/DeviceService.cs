using DeviceApi.Application.Dto;
using DeviceApi.Data.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeviceApi.Application.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository deviceRepository;

        public DeviceService(IDeviceRepository deviceRepository)
        {
            this.deviceRepository = deviceRepository;
        }

        public async Task<Device> CreateAsync(Device rover)
        {
            rover.Id = Guid.NewGuid();

            await this.deviceRepository.AddAsync(rover);

            return rover;
        }

        public Task<Device> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
