namespace DeviceApi.Infrastructure.CrossCutting.Configuration
{
    public interface IApplicationSettings
    {
        LoggingConfiguration Logging { get; set; }

        SwaggerConfiguration Swagger { get; set; }
    }
}