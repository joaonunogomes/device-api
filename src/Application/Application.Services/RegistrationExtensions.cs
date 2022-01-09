namespace DeviceApi.Application.Services
{
    using Microsoft.Extensions.DependencyInjection;

    public static class RegistrationExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            return services.AddScoped<IDeviceService, DeviceService>();
        }
    }
}