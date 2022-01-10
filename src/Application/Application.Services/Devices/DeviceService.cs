using DeviceApi.Application.Dto;
using DeviceApi.Data.Repository.Devices;
using DeviceApi.Infrastructure.CrossCutting;
using DeviceApi.Infrastructure.CrossCutting.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using DomainModel = DeviceApi.Domain.Model;

namespace DeviceApi.Application.Services.Devices
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository deviceRepository;
        private readonly ILogger<DeviceService> logger;

        public DeviceService(IDeviceRepository deviceRepository, ILogger<DeviceService> logger)
        {
            this.deviceRepository = deviceRepository;
            this.logger = logger;
        }

        public async Task<Device> CreateAsync(Device device)
        {
            device.Id = Guid.NewGuid();

            await this.deviceRepository.AddAsync(MappingProfile.Map<Device, DomainModel.Device>(device));

            return device;
        }

        public async Task<Device> GetAsync(Guid id)
        {
            return MappingProfile.Map<DomainModel.Device, Device>(await this.deviceRepository.GetAsync(id));
        }

        public async Task UpdateAsync(Guid id, Device deviceToUpdate)
        {
            if (deviceToUpdate == null)
            {
                this.logger.Error("Device to update is null");
                throw new Exception("Bad request");
            }

            deviceToUpdate.Id = id;

            await this.deviceRepository.UpdateAsync(x => x.Id == id, MappingProfile.Map<Device, DomainModel.Device>(deviceToUpdate));
        }
    }
}
