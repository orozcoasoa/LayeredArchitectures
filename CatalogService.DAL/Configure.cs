using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogService.DAL
{
    public static class Configure
    {
        public static IServiceCollection ConfigureDAL(this IServiceCollection services)
        {
            var config = services.BuildServiceProvider().GetService<IConfiguration>();
            var connectionString = config.GetConnectionString("CatalogDB");
            services.AddDbContext<CatalogServiceDbContext>(opt => opt.UseSqlite(connectionString));
            return services;
        }
    }
}
