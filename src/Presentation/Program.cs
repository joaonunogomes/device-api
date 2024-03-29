namespace DeviceApi.Presentation.Api
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;


    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel(hostBuilder =>
                {
                    hostBuilder.ListenAnyIP(9199);
                })
                .UseStartup<Startup>();
    }
}