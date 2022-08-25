using APIGateway.Aggregators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace APIGateway
{
    public class Program
    {
        private const string authenticationProviderKey = "AzAdKey";

        public static void Main()
        {
            new WebHostBuilder()
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config
                    .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                    .AddJsonFile("appsettings.json", true, true)
                    .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
                    .AddJsonFile("ocelot.json", false, true)
                    .AddEnvironmentVariables();
            })
            .ConfigureServices(s =>
            {
                s.AddAuthentication()
                .AddJwtBearer(authenticationProviderKey, opt =>
                {
                    opt.RequireHttpsMetadata = false;
                });

                s.AddOcelot()
                .AddCacheManager(x =>
                {
                    x.WithDictionaryHandle();
                })
                .AddSingletonDefinedAggregator<ItemDetailsAggregator>();
            })
            .ConfigureLogging((hostingContext, logging) =>
            {
                //add your logging
                logging.AddConsole();
            })
            .UseIISIntegration()
            .Configure(app =>
            {
                app.UseAuthentication();
                app.UseOcelot().Wait();
            })
            .Build()
            .Run();
        }
    }
}