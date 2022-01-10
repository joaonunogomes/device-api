using DeviceApi.Application.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeviceApi.Application.Services.Brands
{
    public interface IBrandService
    {
        Task<Brand> CreateAsync(Brand brand);

        Task<IEnumerable<Brand>> GetAllAsync();
    }
}
