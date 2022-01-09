using DeviceApi.Application.Dto;
using System;
using System.Threading.Tasks;

namespace DeviceApi.Application.Services.Devices
{
    public interface IDeviceService
    {
        Task<Device> CreateAsync(Device device);

        Task<Device> GetAsync(Guid id);
    }
}
