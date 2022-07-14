using AutoMapper;
using CatalogService.BLL;
using CatalogService.BLL.Entities;
using CatalogService.DAL;
using MessagingService;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CatalogService.UnitTests
{
    public class CatalogEFServiceItemTest
    {
        private readonly CatalogServiceDbContext _context;
        private readonly IMapper _mapper;

        public CatalogEFServiceItemTest()
        {
            var contextOptions = new DbContextOptionsBuilder<CatalogServiceDbContext>()
                                .UseInMemoryDatabase("CatalogServiceItemTest")
                                .Options;
            _context = new CatalogServiceDbContext(contextOptions);

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _context.AddRange(
                    new DAL.Category()
                    {
                        Id = 1,
                        Name = "Cleaning",
                        Image = "",
                    }
               );
            _context.AddRange(
                new DAL.Item()
                {
                    Id = 1,
                    Name = "Broom1",
                    Description = "Awesome broom.",
                    Image = "",
                    CategoryId = 1,
                    Price = 10,
                    Amount =1
                },
                new DAL.Item()
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
                var mappingConfig = new MapperConfiguration(mc => mc.AddProfile(new BLL.Setup.CatalogProfile()));
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }

        [Fact]
        public async Task GetAllItems()
        {
            var service = GetService();
            var items = await service.GetAllItems();
            Assert.Equal(2, items.Count);
        }
        [Fact]
        public async Task GetItemsByCategory()
        {
            var service = GetService();
            var qry = new ItemQuery() { CategoryId = 1, Page = 1, Limit = 20 };
            var items = await service.GetItems(qry);
            Assert.Equal(2, items.Count());
        }
        [Fact]
        public async Task GetItemsByCategory_InvalidId()
        {
            var service = GetService();
            var qry = new ItemQuery() { CategoryId = 5, Page = 1, Limit = 1 };
            var items = await service.GetItems(qry);
            Assert.Empty(items);
        }
        [Fact]
        public async Task GetItemsByCategory_WithPagination()
        {
            var service = GetService();
            var qry = new ItemQuery() { CategoryId = 1, Page = 1, Limit = 1 };
            var items = await service.GetItems(qry);
            Assert.Single(items);
            Assert.Equal(2, items.ItemCount);
            qry = new ItemQuery() { CategoryId = 1, Page = 2, Limit = 1 };
            items = await service.GetItems(qry);
            Assert.Single(items);
            Assert.Equal(2, items.ItemCount);
        }
        [Fact]
        public async Task GetItemsByPriceRange()
        {
            var service = GetService();
            var qry = new ItemQuery() { CategoryId = 1, Page = 1, Limit = 20, PriceMax = 15, PriceMin=5 };
            var items = await service.GetItems(qry);
            Assert.Single(items);
        }
        [Fact]
        public async Task GetItemsByPriceRange_MaxPrice()
        {
            var service = GetService();
            var qry = new ItemQuery() { CategoryId = 1, Page = 1, Limit = 20, PriceMax = 35 };
            var items = await service.GetItems(qry);
            Assert.Equal(2,items.Count());
        }
        [Fact]
        public async Task GetItemsByPriceRange_MinPrice()
        {
            var service = GetService();
            var qry = new ItemQuery() { CategoryId = 1, Page = 1, Limit = 20, PriceMin = 15 };
            var items = await service.GetItems(qry);
            Assert.Single(items);
        }
        [Fact]
        public async Task GetItem()
        {
            var service = GetService();
            var item = await service.GetItem(1);
            Assert.NotNull(item);
            Assert.Equal(1, item.Id);
            Assert.True(item.Name.Length > 0);
            Assert.True(item.Description.Length > 0);
            Assert.NotNull(item.Category);
        }
        [Fact]
        public async Task GetItem_InvalidId()
        {
            var service = GetService();
            var item = await service.GetItem(10);
            Assert.Null(item);
        }
        [Fact]
        public async Task GetItemByName()
        {
            var service = GetService();
            var item = await service.GetItem("Broom1");
            Assert.NotNull(item);
            Assert.Equal(1, item.Id);
            Assert.True(item.Name.Length > 0);
            Assert.True(item.Description.Length > 0);
            Assert.NotNull(item.Category);
        }
        [Fact]
        public async Task GetItemByName_Invalid()
        {
            var service = GetService();
            var item = await service.GetItem("Broo");
            Assert.Null(item);
        }
        [Fact]
        public async Task DeleteItem()
        {
            var service = GetService();
            await service.DeleteItem(1);
            var numItems = await _context.Items.CountAsync();
            Assert.Equal(1, numItems);
        }
        [Fact]
        public async Task DeleteItem_InexistentId()
        {
            var service = GetService();
            await service.DeleteItem(10);
            var numItems = await _context.Items.CountAsync();
            Assert.Equal(2, numItems);
        }
        [Fact]
        public async Task AddItem()
        {
            var service = GetService();
            var newItem = new ItemDTO()
            {
                Name = "Rag1",
                Description = "Awesome rag.",
                Image = "",
                CategoryId = 1,
                Price = 5,
                Amount = 1
            };
            var addedItem = await service.AddItem(newItem);
            Assert.NotNull(addedItem);
            Assert.True(addedItem.Id > 0);
            Assert.Equal(newItem.Name, addedItem.Name);
            Assert.NotNull(addedItem.Category);

            var numItems = await _context.Items.CountAsync();
            Assert.Equal(3, numItems);
        }
        [Fact]
        public async Task AddItem_InvalidName()
        {
            var service = GetService();
            var newItem = new ItemDTO()
            {
                Name = "Broom1",
                Description = "Awesome rag",
                Image = "",
                CategoryId = 1,
                Price = 5,
                Amount = 1
            };
            await Assert.ThrowsAsync<ArgumentException>(() => service.AddItem(newItem));
        }
        [Fact]
        public async Task AddItem_InvalidPrice()
        {
            var service = GetService();
            var newItem = new ItemDTO()
            {
                Name = "Rag1",
                Description = "Awesome rag",
                Image = "",
                CategoryId = 1,
                Price = -5,
                Amount = 1
            };
            await Assert.ThrowsAsync<ArgumentException>(() => service.AddItem(newItem));
        }
        [Fact]
        public async Task AddItem_InvalidAmount()
        {
            var service = GetService();
            var newItem = new ItemDTO()
            {
                Name = "Rag1",
                Description = "Awesome rag",
                Image = "",
                CategoryId = 1,
                Price = 5,
                Amount = -1
            };
            await Assert.ThrowsAsync<ArgumentException>(() => service.AddItem(newItem));
        }
        [Fact]
        public async Task AddItem_InvalidCategory()
        {
            var service = GetService();
            var newItem = new ItemDTO()
            {
                Name = "Rag1",
                Description = "Awesome rag",
                Image = "",
                Price = 5,
                Amount = 1
            };
            await Assert.ThrowsAsync<ArgumentException>(() => service.AddItem(newItem));
        }
        [Fact]
        public async Task UpdateItem()
        {
            var service = GetService();
            var item = new ItemDTO()
            {
                Name = "Rag1",
                Description = "Awesome rag.",
                Image = "",
                CategoryId = 1,
                Price = 5,
                Amount = 1
            };
            await service.UpdateItem(1, item);
            var updatedItem = await _context.Items.FindAsync(1);
            Assert.NotNull(updatedItem);
            Assert.Equal(item.Name, updatedItem.Name);
        }
        [Fact]
        public async Task UpdateItem_InvalidId()
        {
            var service = GetService();
            var item = new ItemDTO()
            {
                Name = "Rag1",
                Description = "Awesome rag.",
                Image = "",
                CategoryId = 1,
                Price = 5,
                Amount = 1
            };
            await Assert.ThrowsAsync<KeyNotFoundException>(() => service.UpdateItem(5, item));
        }
        [Fact]
        public async Task UpdateItem_InvalidPrice()
        {
            var service = GetService();
            var item = new ItemDTO()
            {
                Name = "Rag1",
                Description = "Awesome rag",
                Image = "",
                CategoryId = 1,
                Price = -5,
                Amount = 1
            };
            await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateItem(1,item));
        }
        [Fact]
        public async Task UpdateItem_InvalidAmount()
        {
            var service = GetService();
            var item = new ItemDTO()
            {
                Name = "Rag1",
                Description = "Awesome rag",
                Image = "",
                CategoryId = 1,
                Price = 5,
                Amount = -1
            };
            await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateItem(1,item));
        }
        [Fact]
        public async Task UpdateItem_InvalidCategory()
        {
            var service = GetService();
            var item = new ItemDTO()
            {
                Name = "Rag1",
                Description = "Awesome rag",
                Image = "",
                Price = 5,
                Amount = 1
            };
            await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateItem(1,item));
        }

        private ICatalogService GetService()
        {
            var mockMQ = new Mock<IMQClient>();
            mockMQ.Setup(s => s.PublishItemUpdated(It.IsAny<MessagingService.Contracts.Item>()));
            mockMQ.Setup(s => s.PublishItemDeleted(It.IsAny<int>()));
            return new CatalogEFService(_context, _mapper, mockMQ.Object);
        }
    }
}
