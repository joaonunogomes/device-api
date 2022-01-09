namespace DeviceApi.Infrastructure.CrossCutting.Configuration
{
    public class ApplicationSettings : IApplicationSettings
    {
        public LoggingConfiguration Logging { get; set; }

        public SwaggerConfiguration Swagger { get; set; }

        public MongoSettings MongoSettings { get; set; }
    }
}