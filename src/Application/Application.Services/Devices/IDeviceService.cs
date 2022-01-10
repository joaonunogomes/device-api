using DeviceApi.Application.Dto;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeviceApi.Application.Services.Devices
{
    public interface IDeviceService
    {
        Task<Device> CreateAsync(Device device);

        Task<Device> GetAsync(Guid id);

        Task<IEnumerable<Device>> GetAllAsync();

        Task UpdateAsync(Guid id, Device deviceToUpdate);

        Task PatchAsync(Guid id, JsonPatchDocument<Device> jsonPatchDocument);

        Task DeleteAsync(Guid id);
    }
}
