using DeviceApi.Application.Dto;
using DeviceApi.Data.Repository.Brands;
using DeviceApi.Infrastructure.CrossCutting;
using System;
using System.Threading.Tasks;
using DomainModel = DeviceApi.Domain.Model;

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

            await this.brandRepository.AddAsync(MappingProfile.Map<Brand, DomainModel.Brand>(brand));

            return brand;
        }

        public async Task<Brand> GetAsync(Guid id)
        {
            return MappingProfile.Map<DomainModel.Brand, Brand>(await this.brandRepository.GetAsync(id));
        }
    }
}
