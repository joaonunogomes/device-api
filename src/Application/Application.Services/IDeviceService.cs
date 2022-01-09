using DeviceApi.Application.Dto;
using System;
using System.Threading.Tasks;

namespace DeviceApi.Application.Services
{
    public interface IDeviceService
    {
        Task<Device> CreateAsync(Device rover);

        Task<Device> GetAsync(Guid id);
    }
}
