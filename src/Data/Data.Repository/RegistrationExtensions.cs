using DeviceApi.Data.Repository.Brands;
using DeviceApi.Data.Repository.Devices;
using DeviceApi.Data.Repository.Mongo;
using DeviceApi.Infrastructure.CrossCutting.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeviceApi.Data.Repository
{
    public static class RegistrationExtensions
    {
        public static IServiceCollection AddRepository(this IServiceCollection services, MongoSettings settings)
        {
            return services
                .AddTransient<IMongoContext>(provider =>
                    new MongoContext(
                        settings.MongoConnectionString,
                        settings.MongoDataBase))
                .AddTransient<IUnitOfWork, UnitOfWork>()
                .AddScoped<IDeviceRepository, DeviceRepository>()
                .AddTransient<IBrandRepository, BrandRepository>(); ;
        }
    }
}