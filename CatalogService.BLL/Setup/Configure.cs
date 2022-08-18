using CatalogService.BLL.GraphQL;
using MessagingService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System.Reflection;

namespace CatalogService.BLL.Setup
{
    public static class Configure
    {
        public static IServiceCollection ConfigureBLL(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<ICatalogService, CatalogEFService>();
            services.AddSingleton(s =>
            {
                var configuration = s.GetService<IConfiguration>();
                var rabbitMQSettings = configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
                var conn = new ConnectionFactory() { HostName = rabbitMQSettings.HostName };
                if (!(string.IsNullOrEmpty(rabbitMQSettings.User) || string.IsNullOrEmpty(rabbitMQSettings.Password)))
                {
                    conn.UserName = rabbitMQSettings.User;
                    conn.Password = rabbitMQSettings.Password;
                }
                return conn.CreateConnection();
            });
            services.AddSingleton<IMQClient, RabbitMQClient>();
            services.AddGraphQLServer()
                .AddType<CategoryType>()
                .AddType<ItemType>()
                .AddQueryType<CatalogQuery>();

            return services;
        }
    }
}
