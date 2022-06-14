using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartingService.DAL
{
    public class CartingDbContext : DbContext
    {
        public CartingDbContext(DbContextOptions<CartingDbContext> options) : base(options)
        {

        }

        public DbSet<CartDAO> Carts { get; set; }
        public DbSet<ItemDAO> Items { get; set; }
    }
}
