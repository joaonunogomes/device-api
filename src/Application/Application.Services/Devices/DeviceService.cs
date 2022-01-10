using DeviceApi.Application.Dto;
using DeviceApi.Data.Repository.Devices;
using DeviceApi.Infrastructure.CrossCutting;
using System;
using System.Threading.Tasks;
using DomainModel = DeviceApi.Domain.Model;

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

            await this.deviceRepository.AddAsync(MappingProfile.Map<Device, DomainModel.Device>(device));

            return device;
        }

        public async Task<Device> GetAsync(Guid id)
        {
            return MappingProfile.Map<DomainModel.Device, Device>(await this.deviceRepository.GetAsync(id));
        }
    }
}
