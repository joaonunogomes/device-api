namespace DeviceApi.Infrastructure.CrossCutting.Configuration
{
    using Microsoft.Extensions.Logging;

    public class LoggingConfiguration
    {
        public LogLevel LogLevel { get; set; }

        public string FilePath { get; set; }
    }
}