namespace DeviceApi.Presentation.Api
{
    using Application.Services;
    using DeviceApi.Infrastructure.CrossCutting.Configuration;
    using DeviceApi.Presentation.Api.Middlewares;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.OpenApi.Models;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var appSettings = this.Configuration.Get<ApplicationSettings>();
            services
                .AddSingleton<IApplicationSettings>(appSettings);

            services
                .AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            services.AddLogging();

            ConfigureDependencies(services);

            ConfigureSwagger(services, appSettings);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            ILoggerFactory loggerFactory,
            IApplicationSettings appSettings)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (appSettings.Swagger.Enabled)
            {
                app.UseSwagger();

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", appSettings.Swagger.Title);
                });
            }

            app.UseApiExceptions();

            loggerFactory.AddFile(appSettings.Logging.FilePath);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
        {
            var builder = new ServiceCollection()
                .AddLogging()
                .AddMvc()
                .AddNewtonsoftJson()
                .Services.BuildServiceProvider();

            return builder
                .GetRequiredService<IOptions<MvcOptions>>()
                .Value
                .InputFormatters
                .OfType<NewtonsoftJsonPatchInputFormatter>()
                .First();
        }

        private static void ConfigureDependencies(IServiceCollection services)
        {
            services
                .AddApplication();
        }

        private static void ConfigureSwagger(IServiceCollection services, ApplicationSettings appSettings)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = appSettings.Swagger.Title, Version = "v1" });
                c.CustomOperationIds(apiDesc =>
                {
                    var methodName = apiDesc.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null;

                    return $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{methodName}";
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddSwaggerGenNewtonsoftSupport();
        }
    }
}