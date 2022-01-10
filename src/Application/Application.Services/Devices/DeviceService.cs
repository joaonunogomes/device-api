using DeviceApi.Application.Dto;
using DeviceApi.Data.Repository.Devices;
using DeviceApi.Infrastructure.CrossCutting;
using DeviceApi.Infrastructure.CrossCutting.Exceptions;
using DeviceApi.Infrastructure.CrossCutting.Extensions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var mappedDevice = MappingProfile.Map<Device, DomainModel.Device>(device);

            mappedDevice.Id = Guid.NewGuid();
            mappedDevice.CreationDate = DateTime.UtcNow;

            await this.deviceRepository.AddAsync(mappedDevice);

            return MappingProfile.Map<DomainModel.Device, Device>(mappedDevice);
        }

        public async Task<Device> GetAsync(Guid id)
        {
            return MappingProfile.Map<DomainModel.Device, Device>(await this.deviceRepository.GetAsync(id));
        }

        public async Task<IEnumerable<Device>> GetAllAsync(DeviceFilters filters)
        {
            IEnumerable<Device> deviceList;

            if (filters.BrandId == Guid.Empty)
            {
                deviceList = MappingProfile.Map<IEnumerable<DomainModel.Device>, IEnumerable<Device>>(await this.deviceRepository.GetManyAsync(x => true));
            }
            else
            {
                deviceList = MappingProfile.Map<IEnumerable<DomainModel.Device>, IEnumerable<Device>>(await this.deviceRepository.GetManyAsync(x => x.BrandId == filters.BrandId));
            }

            return deviceList ?? new List<Device>();
        }

        public async Task UpdateAsync(Guid id, Device deviceToUpdate)
        {
            if (deviceToUpdate == null)
            {
                var errorMessage = "Device can't be null";
                this.logger.Error(errorMessage);
                throw new ApiErrorException(errorMessage);
            }

            var device = await TryGetDevice(id);

            device.Name = deviceToUpdate.Name;
            device.BrandId = deviceToUpdate.BrandId;

            await this.deviceRepository.UpdateAsync(x => x.Id == id, device);
        }

        public async Task PatchAsync(Guid id, JsonPatchDocument<Device> jsonPatchDocument)
        {
            if (jsonPatchDocument == null)
            {
                var errorMessage = "Json patch document can't be null";
                this.logger.Error(errorMessage);
                throw new ApiErrorException(errorMessage);
            }

            var device = await TryGetDevice(id);

            var mappedJsonPatchDocument = MappingProfile.Map<JsonPatchDocument<Device>, JsonPatchDocument<DomainModel.Device>>(jsonPatchDocument);
            mappedJsonPatchDocument.ApplyTo(device);

            await this.deviceRepository.UpdateAsync(x => x.Id == id, device);
        }

        public async Task DeleteAsync(Guid id)
        {
            await TryGetDevice(id);

            await this.deviceRepository.DeleteAsync(id);
        }

        private async Task<DomainModel.Device> TryGetDevice(Guid id)
        {
            var device = await this.deviceRepository.GetAsync(id);

            if (device == null)
            {
                this.logger.Error("Device not found", new { DeviceId = id });
                throw new NotFoundException();
            }

            return device;
        }
    }
}
