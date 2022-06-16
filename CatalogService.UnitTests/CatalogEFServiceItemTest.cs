using AutoMapper;
using CatalogService.Core.BLL;
using CatalogService.Core.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.UnitTests
{
    public class CatalogEFServiceItemTest
    {
        private readonly CatalogServiceDbContext _context;
        private readonly IMapper _mapper;

        public CatalogEFServiceItemTest()
        {
            var contextOptions = new DbContextOptionsBuilder<CatalogServiceDbContext>()
                                .UseInMemoryDatabase("CatalogServiceTest")
                                .Options;
            _context = new CatalogServiceDbContext(contextOptions);

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _context.AddRange(
                    new Core.DAL.Category()
                    {
                        Id = 1,
                        Name = "Cleaning",
                        Image = "",
                    }
               );
            _context.AddRange(
                new Core.DAL.Item()
                {
                    Id = 1,
                    Name = "Broom1",
                    Description = "Awesome broom.",
                    Image = "",
                    CategoryId = 1,
                    Price = 10,
                    Amount =1
                },
                new Core.DAL.Item()
                {
                    Id = 2,
                    Name = "Mop1",
                    Description = "Awesome mop.",
                    Image = "",
                    CategoryId = 1,
                    Price = 20,
                    Amount = 1
                }
           );

            _context.SaveChanges();

            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc => mc.AddProfile(new CatalogProfile()));
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }

        [Fact]
        public async Task GetAllItems()
        {
            var service = new CatalogEFService(_context, _mapper);
            var items = await service.GetAllItemsAsync();
            Assert.Equal(2, items.Count);
        }
        [Fact]
        public async Task GetItemsByCategory()
        {
            var service = new CatalogEFService(_context, _mapper);
            var items = await service.GetItemsAsync(1);
            Assert.Equal(2, items.Count);
        }
        [Fact]
        public async Task GetItemsByCategory_InvalidId()
        {
            var service = new CatalogEFService(_context, _mapper);
            var items = await service.GetItemsAsync(5);
            Assert.Empty(items);
        }
        [Fact]
        public async Task GetItemsByPriceRange()
        {
            var service = new CatalogEFService(_context, _mapper);
            var items = await service.GetItemsAsync(10,15);
            Assert.Single(items);
        }
        [Fact]
        public async Task GetItem()
        {
            var service = new CatalogEFService(_context, _mapper);
            var item = await service.GetItemAsync(1);
            Assert.NotNull(item);
            Assert.Equal(1, item.Id);
            Assert.True(item.Name.Length > 0);
            Assert.True(item.Description.Length > 0);
            Assert.NotNull(item.Category);
        }
        [Fact]
        public async Task GetItem_InvalidId()
        {
            var service = new CatalogEFService(_context, _mapper);
            var item = await service.GetItemAsync(10);
            Assert.Null(item);
        }
        [Fact]
        public async Task GetItemByName()
        {
            var service = new CatalogEFService(_context, _mapper);
            var item = await service.GetItemAsync("Broom1");
            Assert.NotNull(item);
            Assert.Equal(1, item.Id);
            Assert.True(item.Name.Length > 0);
            Assert.True(item.Description.Length > 0);
            Assert.NotNull(item.Category);
        }
        [Fact]
        public async Task GetItemByName_Invalid()
        {
            var service = new CatalogEFService(_context, _mapper);
            var item = await service.GetItemAsync("Broo");
            Assert.Null(item);
        }
        [Fact]
        public async Task DeleteItem()
        {
            var service = new CatalogEFService(_context, _mapper);
            await service.DeleteItemAsync(1);
            var numItems = await _context.Items.CountAsync();
            Assert.Equal(1, numItems);
        }
        [Fact]
        public async Task DeleteItem_InexistentId()
        {
            var service = new CatalogEFService(_context, _mapper);
            await service.DeleteItemAsync(10);
            var numItems = await _context.Items.CountAsync();
            Assert.Equal(2, numItems);
        }
        [Fact]
        public async Task AddItem()
        {
            var service = new CatalogEFService(_context, _mapper);
            var newItem = new Core.BLL.Item()
            {
                Id = 5,
                Name = "Rag1",
                Description = "Awesome rag.",
                Image = "",
                Category = new Core.BLL.Category() { Id = 1},
                Price = 5,
                Amount = 1
            };
            var addedItem = await service.AddItemAsync(newItem);
            Assert.NotNull(addedItem);
            Assert.NotEqual(newItem.Id, addedItem.Id);
            Assert.True(addedItem.Id > 0);
            Assert.Equal(newItem.Name, addedItem.Name);
            Assert.NotNull(addedItem.Category);

            var numItems = await _context.Items.CountAsync();
            Assert.Equal(3, numItems);
        }
        [Fact]
        public async Task AddItem_InvalidName()
        {
            var service = new CatalogEFService(_context, _mapper);
            var newItem = new Core.BLL.Item()
            {
                Id = 5,
                Name = "Broom1",
                Description = "Awesome rag",
                Image = "",
                Category = new Core.BLL.Category() { Id = 1 },
                Price = 5,
                Amount = 1
            };
            await Assert.ThrowsAsync<ArgumentException>(() => service.AddItemAsync(newItem));
        }
        [Fact]
        public async Task AddItem_InvalidPrice()
        {
            var service = new CatalogEFService(_context, _mapper);
            var newItem = new Core.BLL.Item()
            {
                Id = 5,
                Name = "Rag1",
                Description = "Awesome rag",
                Image = "",
                Category = new Core.BLL.Category() { Id = 1 },
                Price = -5,
                Amount = 1
            };
            await Assert.ThrowsAsync<ArgumentException>(() => service.AddItemAsync(newItem));
        }
        [Fact]
        public async Task AddItem_InvalidAmount()
        {
            var service = new CatalogEFService(_context, _mapper);
            var newItem = new Core.BLL.Item()
            {
                Id = 5,
                Name = "Rag1",
                Description = "Awesome rag",
                Image = "",
                Category = new Core.BLL.Category() { Id = 1 },
                Price = 5,
                Amount = -1
            };
            await Assert.ThrowsAsync<ArgumentException>(() => service.AddItemAsync(newItem));
        }
        [Fact]
        public async Task AddItem_InvalidCategory()
        {
            var service = new CatalogEFService(_context, _mapper);
            var newItem = new Core.BLL.Item()
            {
                Id = 5,
                Name = "Rag1",
                Description = "Awesome rag",
                Image = "",
                Price = 5,
                Amount = 1
            };
            await Assert.ThrowsAsync<ArgumentException>(() => service.AddItemAsync(newItem));
        }
        [Fact]
        public async Task UpdateItem()
        {
            var service = new CatalogEFService(_context, _mapper);
            var item = new Core.BLL.Item()
            {
                Id = 1,
                Name = "Rag1",
                Description = "Awesome rag.",
                Image = "",
                Category = new Core.BLL.Category() { Id = 1 },
                Price = 5,
                Amount = 1
            };
            await service.UpdateItemAsync(item);
            var updatedItem = await _context.Items.FindAsync(1);
            Assert.NotNull(updatedItem);
            Assert.Equal(item.Name, updatedItem.Name);
        }
        [Fact]
        public async Task UpdateItem_InvalidId()
        {
            var service = new CatalogEFService(_context, _mapper);
            var item = new Core.BLL.Item()
            {
                Id = 5,
                Name = "Rag1",
                Description = "Awesome rag.",
                Image = "",
                Category = new Core.BLL.Category() { Id = 1 },
                Price = 5,
                Amount = 1
            };
            await Assert.ThrowsAsync<KeyNotFoundException>(() => service.UpdateItemAsync(item));
        }
        [Fact]
        public async Task UpdateItem_InvalidPrice()
        {
            var service = new CatalogEFService(_context, _mapper);
            var item = new Core.BLL.Item()
            {
                Id = 1,
                Name = "Rag1",
                Description = "Awesome rag",
                Image = "",
                Category = new Core.BLL.Category() { Id = 1 },
                Price = -5,
                Amount = 1
            };
            await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateItemAsync(item));
        }
        [Fact]
        public async Task UpdateItem_InvalidAmount()
        {
            var service = new CatalogEFService(_context, _mapper);
            var item = new Core.BLL.Item()
            {
                Id = 1,
                Name = "Rag1",
                Description = "Awesome rag",
                Image = "",
                Category = new Core.BLL.Category() { Id = 1 },
                Price = 5,
                Amount = -1
            };
            await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateItemAsync(item));
        }
        [Fact]
        public async Task UpdateItem_InvalidCategory()
        {
            var service = new CatalogEFService(_context, _mapper);
            var item = new Core.BLL.Item()
            {
                Id = 1,
                Name = "Rag1",
                Description = "Awesome rag",
                Image = "",
                Price = 5,
                Amount = 1
            };
            await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateItemAsync(item));
        }
    }
}
