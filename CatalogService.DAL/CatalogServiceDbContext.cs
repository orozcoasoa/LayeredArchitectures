using Microsoft.EntityFrameworkCore;

namespace CatalogService.DAL
{
    public class CatalogServiceDbContext : DbContext
    {
        public CatalogServiceDbContext(DbContextOptions<CatalogServiceDbContext> options) : base(options)
        {
            base.Database.EnsureCreated();
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Item> Items { get; set; }
    }
}
