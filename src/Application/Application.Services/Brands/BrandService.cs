using DeviceApi.Application.Dto;
using DeviceApi.Data.Repository.Brands;
using DeviceApi.Infrastructure.CrossCutting;
using System;
using System.Collections.Generic;
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
            brand.CreationDate = DateTime.UtcNow;

            await this.brandRepository.AddAsync(MappingProfile.Map<Brand, DomainModel.Brand>(brand));

            return brand;
        }

        public async Task<IEnumerable<Brand>> GetAllAsync()
        {
            var brandList = MappingProfile.Map<IEnumerable<DomainModel.Brand>, IEnumerable<Brand>>(await this.brandRepository.GetManyAsync(x => true));

            return brandList ?? new List<Brand>();
        }
    }
}
