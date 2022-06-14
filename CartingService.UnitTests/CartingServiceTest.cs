using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;

namespace CartingService.UnitTests
{
    public class CartingServiceTest
    {
        private static IMapper _mapper;
        private CartingDbContext _context;
        private Guid _existingCartId;

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
                        new ItemDAO { Name = "Item1", Id = 1, Price=10, Quantity=1, Image=null },
                        new ItemDAO { Name = "Item2", Id = 2, Price=20, Quantity = 2, Image=null}
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
            var cartingService = new Mock<BLL.CartingService>(_context, _mapper);
            var items = await cartingService.Object.GetCartItemsAsync(_existingCartId);
            Assert.Equal<int>(2, items.Count);
        }
        [Fact]
        public async Task InitializeCart_NewId()
        {
            var cartingService = new Mock<BLL.CartingService>(_context, _mapper);
            var newGuid = Guid.NewGuid();
            var cart = await cartingService.Object.InitializeCartAsync(newGuid, new Item { Id = 3, Name = "Item3", Price = 30, Quantity = 3 });
            Assert.Equal(newGuid, cart.Id);
        }
        [Fact]
        public async Task InitializeCart_ExistingId()
        {
            var cartingService = new Mock<BLL.CartingService>(_context, _mapper);
            var cart = await cartingService.Object.InitializeCartAsync(_existingCartId, new Item { Id = 3, Name = "Item3", Price = 30, Quantity = 3 });
            Assert.Equal(_existingCartId, cart.Id);
            Assert.Single(cart.Items);

        }
        [Fact]
        public async Task InitializeCart_NullItem()
        {
            var cartingService = new Mock<BLL.CartingService>(_context, _mapper);
            var newGuid = Guid.NewGuid();
            var cart = await cartingService.Object.InitializeCartAsync(newGuid, null);
            Assert.Equal(newGuid, cart.Id);
            Assert.Empty(cart.Items);
        }
    }
}