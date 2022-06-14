﻿using Microsoft.EntityFrameworkCore;

namespace CartingService.Core.DAL
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