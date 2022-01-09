using DeviceApi.Application.Dto;
using DeviceApi.Data.Repository.Brands;
using System;
using System.Threading.Tasks;

namespace DeviceApi.Application.Services.Brands
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository brandRepository;

        public BrandService(IBrandRepository brandRepository)
        {
            this.brandRepository = brandRepository;
        }

        public async Task<Brand> CreateAsync(Brand brand)
        {
            brand.Id = Guid.NewGuid();

            await this.brandRepository.AddAsync(brand);

            return brand;
        }

        public Task<Brand> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
