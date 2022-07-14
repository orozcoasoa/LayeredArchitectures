using CartingService.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CartingService.DAL
{
    public class CartingDbContext : DbContext
    {
        public CartingDbContext(DbContextOptions<CartingDbContext> options) : base(options)
        {

        }

        public DbSet<CartDAO> Carts { get; set; }
        public DbSet<CartItemDAO> CartItems { get; set; }
        public DbSet<ItemDAO> Items { get; set; }
    }
}