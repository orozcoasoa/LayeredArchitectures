using CartingService.DAL.Entities;
using LiteDB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CartingService.DAL
{
    public static class Configure
    {
        public static IServiceCollection ConfigureDAL(this IServiceCollection services)
        {
            // TODO: use secrets such as key vault.
            var config = services.BuildServiceProvider().GetService<IConfiguration>();
            var connectionString = config.GetConnectionString("CartingDB");
            services.AddScoped<ILiteDatabase, LiteDatabase>((services) => new LiteDatabase(connectionString));
            services.AddScoped<ICartingRepository, NoSQLCartingRepository>();

            BsonMapper.Global.Entity<CartDAO>().DbRef(c => c.Items, NoSQLCartingRepository.cartitems);
            BsonMapper.Global.Entity<CartItemDAO>().DbRef(i => i.Cart, NoSQLCartingRepository.carts);
            BsonMapper.Global.Entity<CartItemDAO>().DbRef(i => i.Item, NoSQLCartingRepository.items);

            BsonMapper.Global.Entity<CartItemDAO>().Id(c => c.Id, true);
            return services;
        }
    }
}
