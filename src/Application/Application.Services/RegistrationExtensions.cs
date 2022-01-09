namespace DeviceApi.Application.Services
{
    using DeviceApi.Application.Services.Brands;
    using DeviceApi.Application.Services.Devices;
    using Microsoft.Extensions.DependencyInjection;

    public static class RegistrationExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            return services.AddScoped<IDeviceService, DeviceService>()
                           .AddScoped<IBrandService, BrandService>();
        }
    }
}