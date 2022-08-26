using CartingService.BLL.Setup;
using CartingService.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CartingService.UnitTests
{
    public class CartingEFServiceTest
    {
        private static IMapper _mapper;
        private readonly CartingDbContext _context;
        private readonly Guid _existingCartId;

        public CartingEFServiceTest()
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
                    Items = new List<CartItemDAO>()
                    {
                        new CartItemDAO { Item = new ItemDAO() { Name = "Item1", Id = 1, Image = null, Price=10 },
                                        Quantity = 1},
                        new CartItemDAO { Item = new ItemDAO() { Name = "Item2", Id = 2, Image = null, Price=20 },
                                        Quantity = 2}
                    }
                }
               );

            _context.SaveChanges();
            _context.Items.Add(new ItemDAO() { Name = "Item4", Id = 4, Image = null, Price = 40 });
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
            var cartingService = new CartingEFService(_context, _mapper);
            var cart = await cartingService.GetCart(_existingCartId);
            Assert.Equal(2, cart.Items.Count);
        }
        [Fact]
        public async Task InitializeCart_NewId()
        {
            var cartingService = new CartingEFService(_context, _mapper);
            var newGuid = Guid.NewGuid();
            var item = new Item { Id = 2, Price = 30, Quantity = 3 };
            var cart = await cartingService.InitializeCart(newGuid, item);
            Assert.Equal(newGuid, cart.Id);
            Assert.Single(cart.Items);
            var itemInCart = cart.Items.Find(i => i.Id == item.Id);
            Assert.NotNull(itemInCart);
            Assert.Equal("Item2", itemInCart.Name);
            Assert.Null(itemInCart.Image);
            Assert.NotEqual(item.Price, itemInCart.Price);
            Assert.Equal(item.Quantity, itemInCart.Quantity);
        }
        [Fact]
        public async Task InitializeCart_NewIdInvalidItem()
        {
            var cartingService = new CartingEFService(_context, _mapper);
            var newGuid = Guid.NewGuid();
            var item = new Item { Id = 3, Price = 30, Quantity = 3 };
            var cart = await cartingService.InitializeCart(newGuid, item);
            Assert.Equal(newGuid, cart.Id);
            Assert.Empty(cart.Items);
        }
        [Fact]
        public async Task InitializeCart_NewIdNullItem()
        {
            var cartingService = new CartingEFService(_context, _mapper);
            var newGuid = Guid.NewGuid();
            var cart = await cartingService.InitializeCart(newGuid, null);
            Assert.Equal(newGuid, cart.Id);
            Assert.Empty(cart.Items);
        }
        [Fact]
        public async Task InitializeCart_ExistingId()
        {
            var cartingService = new CartingEFService(_context, _mapper);
            var item = new Item { Id = 2, Price = 30, Quantity = 3 };
            var cart = await cartingService.InitializeCart(_existingCartId, item);
            Assert.Equal(_existingCartId, cart.Id);
            Assert.Single(cart.Items);
            var itemInCart = cart.Items.Find(i => i.Id == item.Id);
            Assert.NotNull(itemInCart);
            Assert.Equal("Item2", itemInCart.Name);
            Assert.Null(itemInCart.Image);
            Assert.NotEqual(item.Price, itemInCart.Price);
            Assert.Equal(item.Quantity, itemInCart.Quantity);
        }
        [Fact]
        public async Task AddItemToCart_ExistingCart()
        {
            var cartingService = new CartingEFService(_context, _mapper);
            var newItem = new Item { Id = 4, Price = 30, Quantity = 3 };
            await cartingService.AddItem(_existingCartId, newItem);
            var cart = await cartingService.GetCart(_existingCartId);
            Assert.Equal(_existingCartId, cart.Id);
            Assert.Equal(3, cart.Items.Count);
            var item = cart.Items.Find(i => i.Id == 4);
            Assert.NotNull(item);
            Assert.Equal("Item4", item.Name);
            Assert.Null(item.Image);
            Assert.NotEqual(newItem.Price, item.Price);
            Assert.Equal(newItem.Quantity, item.Quantity);
        }
        [Fact]
        public async Task AddItemToCart_ExistingCartExistingItem()
        {
            var cartingService = new CartingEFService(_context, _mapper);
            var newItem = new Item { Id = 2, Price = 30, Quantity = 3 };
            await cartingService.AddItem(_existingCartId, newItem);
            var cart = await cartingService.GetCart(_existingCartId);
            Assert.Equal(_existingCartId, cart.Id);
            Assert.Equal(2, cart.Items.Count);
            var item = cart.Items.Find(i => i.Id == 2);
            Assert.NotNull(item);
            Assert.Equal(5, item.Quantity);
        }
        [Fact]
        public async Task AddItemToCart_ExistingCartInvalidItem()
        {
            var cartingService = new CartingEFService(_context, _mapper);
            var newItem = new Item { Id = 3, Price = 30, Quantity = 3 };
            await Assert.ThrowsAsync<ArgumentException>(() => cartingService.AddItem(_existingCartId, newItem));
            var cart = await cartingService.GetCart(_existingCartId);
            Assert.Equal(_existingCartId, cart.Id);
            Assert.Equal(2, cart.Items.Count);
        }
        [Fact]
        public async Task AddItemToCart_ExistingCartNullItem()
        {
            var cartingService = new CartingEFService(_context, _mapper);
            await Assert.ThrowsAsync<ArgumentException>(() => cartingService.AddItem(_existingCartId, null));
            var cart = await cartingService.GetCart(_existingCartId);
            Assert.Equal(_existingCartId, cart.Id);
            Assert.Equal(2, cart.Items.Count);
        }
        [Fact]
        public async Task AddItemToCart_NewCart()
        {
            var cartingService = new CartingEFService(_context, _mapper);
            var newGuid = Guid.NewGuid();
            var newItem = new Item { Id = 4, Price = 30, Quantity = 3 };
            await cartingService.AddItem(newGuid, newItem);
            var cart = await cartingService.GetCart(newGuid);
            Assert.Equal(newGuid, cart.Id);
            Assert.Single(cart.Items);
            var item = cart.Items[0];
            Assert.NotNull(item);
            Assert.Equal("Item4", item.Name);
            Assert.Null(item.Image);
            Assert.NotEqual(newItem.Price, item.Price);
            Assert.Equal(newItem.Quantity, item.Quantity);
        }
        [Fact]
        public async Task AddItemToCart_NewCartInvalidItem()
        {
            var cartingService = new CartingEFService(_context, _mapper);
            var newGuid = Guid.NewGuid();
            var newItem = new Item { Id = 3, Price = 30, Quantity = 3 };
            await Assert.ThrowsAsync<ArgumentException>(() => cartingService.AddItem(_existingCartId, newItem));
            var cart = await cartingService.GetCart(newGuid);
            Assert.Null(cart);
        }
        [Fact]
        public async Task RemoveItem_ExistingItem()
        {
            var cartingService = new CartingEFService(_context, _mapper);
            await cartingService.RemoveItem(_existingCartId, 1);
            var cart = await cartingService.GetCart(_existingCartId);
            Assert.Single(cart.Items);
        }
        [Fact]
        public async Task RemoveItem_NonExistingItem()
        {
            var cartingService = new CartingEFService(_context, _mapper);
            await cartingService.RemoveItem(_existingCartId, 3);
            var cart = await cartingService.GetCart(_existingCartId);
            Assert.Equal(2, cart.Items.Count);
        }
        [Fact]
        public async Task RemoveItem_NonExistingCart()
        {
            var cartingService = new CartingEFService(_context, _mapper);
            var newGuid = Guid.NewGuid();
            await cartingService.RemoveItem(newGuid, 1);
            var cart = await cartingService.GetCart(newGuid);
            Assert.Null(cart);
        }

    }
}
