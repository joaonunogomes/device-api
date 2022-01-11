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
                .AddSingleton<IMongoContext>(provider =>
                    new MongoContext(
                        settings.MongoConnectionString,
                        settings.MongoDataBase))
                .AddSingleton<IUnitOfWork, UnitOfWork>()
                .AddTransient<IDeviceRepository, DeviceRepository>()
                .AddTransient<IBrandRepository, BrandRepository>();
        }
    }
}