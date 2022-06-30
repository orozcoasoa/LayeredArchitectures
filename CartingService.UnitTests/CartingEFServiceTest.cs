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
            var cartingService = new CartingEFService(_context, _mapper);
            var cart = await cartingService.GetCart(_existingCartId);
            Assert.Equal(2, cart.Items.Count);
        }
        [Fact]
        public async Task InitializeCart_NewId()
        {
            var cartingService = new CartingEFService(_context, _mapper);
            var newGuid = Guid.NewGuid();
            var cart = await cartingService.InitializeCart(newGuid, new Item { Id = 3, Name = "Item3", Price = 30, Quantity = 3 });
            Assert.Equal(newGuid, cart.Id);
            var cartWItems = await cartingService.GetCart(newGuid);
            Assert.Single(cartWItems.Items);
        }
        [Fact]
        public async Task InitializeCart_ExistingId()
        {
            var cartingService = new CartingEFService(_context, _mapper);
            var cart = await cartingService.InitializeCart(_existingCartId, new Item { Id = 3, Name = "Item3", Price = 30, Quantity = 3 });
            Assert.Equal(_existingCartId, cart.Id);
            Assert.Single(cart.Items);
        }
        [Fact]
        public async Task InitializeCart_NullItem()
        {
            var cartingService = new CartingEFService(_context, _mapper);
            var newGuid = Guid.NewGuid();
            var cart = await cartingService.InitializeCart(newGuid, null);
            Assert.Equal(newGuid, cart.Id);
            Assert.Empty(cart.Items);
        }
        [Fact]
        public async Task AddItemToCart_ExistingCart()
        {
            var cartingService = new CartingEFService(_context, _mapper);
            await cartingService.AddItem(_existingCartId, new Item { Id = 3, Name = "Item3", Price = 30, Quantity = 3 });
            var cart = await cartingService.GetCart(_existingCartId);
            Assert.Equal(3, cart.Items.Count);
        }
        [Fact]
        public async Task AddItemToCart_ExistingItem()
        {
            var cartingService = new CartingEFService(_context, _mapper);
            await cartingService.AddItem(_existingCartId, new Item { Id = 2, Name = "Item2", Price = 30, Quantity = 3 });
            var cart = await cartingService.GetCart(_existingCartId);
            Assert.Equal(2, cart.Items.Count);
            var item = cart.Items.Find(i => i.Id == 2);
            Assert.NotNull(item);
            Assert.Equal(5, item.Quantity);
        }
        [Fact]
        public async Task AddItemToCart_NewCart()
        {
            var cartingService = new CartingEFService(_context, _mapper);
            var newGuid = Guid.NewGuid();
            await cartingService.AddItem(newGuid, new Item { Id = 3, Name = "Item3", Price = 30, Quantity = 3 });
            var cart = await cartingService.GetCart(newGuid);
            Assert.Single(cart.Items);
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
            Assert.Equal(2,cart.Items.Count);
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