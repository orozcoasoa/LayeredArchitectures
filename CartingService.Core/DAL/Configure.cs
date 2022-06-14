using LiteDB;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartingService.Core.DAL
{
    public static class Configure
    {
        public static void ConfigureServices(IServiceCollection services, string connectionString)
        {
            services.AddScoped<ILiteDatabase, LiteDatabase>((services) => new LiteDatabase(connectionString));
            services.AddScoped<ICartingRepository, NoSQLCartingRepository>();

            BsonMapper.Global.Entity<CartDAO>().DbRef(c => c.Items, NoSQLCartingRepository.items);
            BsonMapper.Global.Entity<ItemDAO>().DbRef(i => i.Cart, NoSQLCartingRepository.carts);
        }
    }
}
