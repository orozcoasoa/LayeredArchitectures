using AutoMapper;
using CartingService.Core.BLL;
using CartingService.Core.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CartingService.UnitTests
{
    public class CartingServiceTest
    {
        private static IMapper _mapper;
        private readonly CartingDbContext _context;
        private readonly Guid _existingCartId;

        public CartingServiceTest()
        {
            var contextOptions = new DbContextOptionsBuilder<CartingDbContext>()
                                .UseInMemoryDatabase("CartingServiceTest")
                                //.ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                                .Options;
            _context = new CartingDbContext(contextOptions);

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _existingCartId = Guid.NewGuid();
            _context.AddRange(
                new CartDAO()
                {
                    Id = _existingCartId,
                    Items = new List<ItemDAO>()
                    {
                        new ItemDAO { Name = "Item1", Id = 1, Price=10, Quantity = 1, Image = null },
                        new ItemDAO { Name = "Item2", Id = 2, Price=20, Quantity = 2, Image = null}
                    }
                }
               );

            _context.SaveChanges();

            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc => mc.AddProfile(new CartingProfile()));
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }
        [Fact]
        public async Task GetAllItemsForCart()
        {
            var cartingService = new Core.BLL.CartingService(_context, _mapper);
            var items = await cartingService.GetCartItemsAsync(_existingCartId);
            Assert.Equal(2, items.Count);
        }
        [Fact]
        public async Task InitializeCart_NewId()
        {
            var cartingService = new Core.BLL.CartingService(_context, _mapper);
            var newGuid = Guid.NewGuid();
            var cart = await cartingService.InitializeCartAsync(newGuid, new Item { Id = 3, Name = "Item3", Price = 30, Quantity = 3 });
            Assert.Equal(newGuid, cart.Id);
            var items = await cartingService.GetCartItemsAsync(newGuid);
            Assert.Single(items);
        }
        [Fact]
        public async Task InitializeCart_ExistingId()
        {
            var cartingService = new Core.BLL.CartingService(_context, _mapper);
            var cart = await cartingService.InitializeCartAsync(_existingCartId, new Item { Id = 3, Name = "Item3", Price = 30, Quantity = 3 });
            Assert.Equal(_existingCartId, cart.Id);
            Assert.Single(cart.Items);
        }
        [Fact]
        public async Task InitializeCart_NullItem()
        {
            var cartingService = new Core.BLL.CartingService(_context, _mapper);
            var newGuid = Guid.NewGuid();
            var cart = await cartingService.InitializeCartAsync(newGuid, null);
            Assert.Equal(newGuid, cart.Id);
            Assert.Empty(cart.Items);
        }
        [Fact]
        public async Task AddItemToCart_ExistingCart()
        {
            var cartingService = new Core.BLL.CartingService(_context, _mapper);
            await cartingService.AddItemAsync(_existingCartId, new Item { Id = 3, Name = "Item3", Price = 30, Quantity = 3 });
            var items = await cartingService.GetCartItemsAsync(_existingCartId);
            Assert.Equal(3, items.Count);
        }
        [Fact]
        public async Task AddItemToCart_ExistingItem()
        {
            var cartingService = new Core.BLL.CartingService(_context, _mapper);
            await cartingService.AddItemAsync(_existingCartId, new Item { Id = 2, Name = "Item2", Price = 30, Quantity = 3 });
            var items = await cartingService.GetCartItemsAsync(_existingCartId);
            Assert.Equal(2, items.Count);
            var item = items.ToList().Find(i => i.Id == 2);
            Assert.NotNull(item);
            Assert.Equal(5, item.Quantity);
        }
        [Fact]
        public async Task AddItemToCart_NewCart()
        {
            var cartingService = new Core.BLL.CartingService(_context, _mapper);
            var newGuid = Guid.NewGuid();
            await cartingService.AddItemAsync(newGuid, new Item { Id = 3, Name = "Item3", Price = 30, Quantity = 3 });
            var items = await cartingService.GetCartItemsAsync(newGuid);
            Assert.Single(items);
        }
        [Fact]
        public async Task RemoveItem_ExistingItem()
        {
            var cartingService = new Core.BLL.CartingService(_context, _mapper);
            await cartingService.RemoveItemAsync(_existingCartId, 1);
            var items = await cartingService.GetCartItemsAsync(_existingCartId);
            Assert.Single(items);
        }
        [Fact]
        public async Task RemoveItem_NonExistingItem()
        {
            var cartingService = new Core.BLL.CartingService(_context, _mapper);
            await cartingService.RemoveItemAsync(_existingCartId, 3);
            var items = await cartingService.GetCartItemsAsync(_existingCartId);
            Assert.Equal(2,items.Count);
        }
        [Fact]
        public async Task RemoveItem_NonExistingCart()
        {
            var cartingService = new Core.BLL.CartingService(_context, _mapper);
            var newGuid = Guid.NewGuid();
            await cartingService.RemoveItemAsync(newGuid, 1);
            var items = await cartingService.GetCartItemsAsync(newGuid);
            Assert.Empty(items);
        }

    }
}