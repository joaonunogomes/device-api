using DeviceApi.Application.Dto;
using System;
using System.Threading.Tasks;

namespace DeviceApi.Application.Services.Brands
{
    public interface IBrandService
    {
        Task<Brand> CreateAsync(Brand brand);

        Task<Brand> GetAsync(Guid id);
    }
}
