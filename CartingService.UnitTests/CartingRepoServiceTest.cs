using CartingService.BLL.Setup;
using CartingService.DAL.Entities;
using Moq;

namespace CartingService.UnitTests
{
    public class CartingRepoServiceTest
    {
        private readonly IMapper _mapper;

        public CartingRepoServiceTest()
        {
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
            var repoMock = new Mock<ICartingRepository>();
            var guid = Guid.NewGuid();
            repoMock.Setup(s => s.GetCart(guid))
                .Returns(Task.FromResult(GetDefaultCart(guid)));
            var cartingService = new CartingRepoService(repoMock.Object, _mapper);
            var cart = await cartingService.GetCart(guid);
            Assert.NotNull(cart);
            Assert.Equal(2, cart.Items.Count);
        }
        [Fact]
        public async Task InitializeCart_NewId()
        {
            var newGuid = Guid.NewGuid();
            var item = new Item { Id = 2, Price = 30, Quantity = 3 };
            var repoMock = new Mock<ICartingRepository>();
            repoMock.Setup(s => s.ExistsCart(newGuid)).Returns(Task.FromResult(false));
            repoMock.Setup(s => s.AddItemToCart(newGuid, It.IsAny<CartItemDAO>()));
            repoMock.Setup(s => s.GetCart(newGuid))
                .Returns(Task.FromResult(GetEmptyCart(newGuid)));

            var cartingService = new CartingRepoService(repoMock.Object, _mapper);
            var cart = await cartingService.InitializeCart(newGuid, item);
            Assert.NotNull(cart);
            Assert.Empty(cart.Items);
        }
        [Fact]
        public async Task InitializeCart_NewIdInvalidItem()
        {
            var newGuid = Guid.NewGuid();
            var item = new Item { Id = 3, Price = 30, Quantity = 3 };
            var repoMock = new Mock<ICartingRepository>();
            repoMock.Setup(s => s.ExistsCart(newGuid)).Returns(Task.FromResult(false));
            repoMock.Setup(s => s.AddItemToCart(newGuid, It.IsAny<CartItemDAO>()));
            repoMock.Setup(s => s.GetCart(newGuid))
                .Returns(Task.FromResult(GetEmptyCart(newGuid)));

            var cartingService = new CartingRepoService(repoMock.Object, _mapper);
            var cart = await cartingService.InitializeCart(newGuid, item);
            Assert.NotNull(cart);
            Assert.Empty(cart.Items);
        }
        [Fact]
        public async Task InitializeCart_NewIdNullItem()
        {
            var newGuid = Guid.NewGuid();
            var repoMock = new Mock<ICartingRepository>();
            repoMock.Setup(s => s.ExistsCart(newGuid)).Returns(Task.FromResult(false));
            repoMock.Setup(s => s.CreateCart(newGuid))
                .Returns(Task.FromResult(GetEmptyCart(newGuid)));

            var cartingService = new CartingRepoService(repoMock.Object, _mapper);
            var cart = await cartingService.InitializeCart(newGuid, null);
            Assert.NotNull(cart);
            Assert.Empty(cart.Items);
        }
        [Fact]
        public async Task InitializeCart_ExistingId()
        {
            var newGuid = Guid.NewGuid();
            var item = new Item { Id = 1, Price = 10, Quantity = 1 };
            var repoMock = new Mock<ICartingRepository>();
            repoMock.Setup(s => s.ExistsCart(newGuid)).Returns(Task.FromResult(true));
            repoMock.Setup(s => s.AddItemToCart(newGuid, It.IsAny<CartItemDAO>()));
            repoMock.Setup(s => s.GetCart(newGuid))
                .Returns(Task.FromResult(GetCartWithItems(newGuid, 1)));

            var cartingService = new CartingRepoService(repoMock.Object, _mapper);
            var cart = await cartingService.InitializeCart(newGuid, item);
            Assert.Equal(newGuid, cart.Id);
            Assert.Single(cart.Items);
        }
        [Fact]
        public async Task AddItemToCart_ExistingCart()
        {
            var existingCartId = Guid.NewGuid();
            var item = new Item { Id = 4, Price = 30, Quantity = 3 };
            var repoMock = new Mock<ICartingRepository>();
            repoMock.Setup(s => s.GetCart(existingCartId))
                .Returns(Task.FromResult(GetCartWithItems(existingCartId, 3)));
            repoMock.Setup(s => s.AddItemToCart(existingCartId, It.IsAny<CartItemDAO>()));
            repoMock.Setup(s => s.ExistsItem(item.Id)).Returns(true);

            var cartingService = new CartingRepoService(repoMock.Object, _mapper);
            await cartingService.AddItem(existingCartId, item);
            repoMock.VerifyAll();
        }
        [Fact]
        public async Task AddItemToCart_ExistingCartExistingItem()
        {
            var existingCartId = Guid.NewGuid();
            var item = new Item { Id = 4, Price = 30, Quantity = 3 };
            var repoMock = new Mock<ICartingRepository>();
            repoMock.Setup(s => s.GetCart(existingCartId))
                .Returns(Task.FromResult(GetCartWithItems(existingCartId, 4)));
            repoMock.Setup(s => s.UpdateItemQuantity(existingCartId, It.IsAny<int>(), It.IsAny<double>()));

            var cartingService = new CartingRepoService(repoMock.Object, _mapper);
            await cartingService.AddItem(existingCartId, item);
            repoMock.VerifyAll();
        }
        [Fact]
        public async Task AddItemToCart_ExistingCartInvalidItem()
        {
            var existingCartId = Guid.NewGuid();
            var item = new Item { Id = 5, Price = 30, Quantity = 3 };
            var repoMock = new Mock<ICartingRepository>();
            repoMock.Setup(s => s.GetCart(existingCartId))
                .Returns(Task.FromResult(GetCartWithItems(existingCartId, 4)));
            repoMock.Setup(s => s.ExistsItem(item.Id)).Returns(false);

            var cartingService = new CartingRepoService(repoMock.Object, _mapper);
            await Assert.ThrowsAsync<ArgumentException>(() => cartingService.AddItem(existingCartId, item));
        }
        [Fact]
        public async Task AddItemToCart_ExistingCartNullItem()
        {
            var existingCartId = Guid.NewGuid();
            var repoMock = new Mock<ICartingRepository>();
            repoMock.Setup(s => s.GetCart(existingCartId))
                .Returns(Task.FromResult(GetCartWithItems(existingCartId, 4)));

            var cartingService = new CartingRepoService(repoMock.Object, _mapper);
            await Assert.ThrowsAsync<ArgumentException>(() => cartingService.AddItem(existingCartId, null));
        }
        [Fact]
        public async Task AddItemToCart_NewCart()
        {
            var newCartId = Guid.NewGuid();
            var item = new Item { Id = 4, Price = 30, Quantity = 3 };
            var repoMock = new Mock<ICartingRepository>();
            repoMock.Setup(s => s.GetCart(newCartId)).Returns(Task.FromResult<CartDAO>(null));
            repoMock.Setup(s => s.ExistsCart(newCartId)).Returns(Task.FromResult(false));
            repoMock.Setup(s => s.CreateCart(newCartId));
            repoMock.Setup(s => s.AddItemToCart(newCartId, It.IsAny<CartItemDAO>()));
            repoMock.Setup(s => s.ExistsItem(item.Id)).Returns(true);

            var cartingService = new CartingRepoService(repoMock.Object, _mapper);
            await cartingService.AddItem(newCartId, item);
            repoMock.VerifyAll();
        }
        [Fact]
        public async Task AddItemToCart_NewCartInvalidItem()
        {
            var newCartId = Guid.NewGuid();
            var item = new Item { Id = 4, Price = 30, Quantity = 3 };
            var repoMock = new Mock<ICartingRepository>();
            repoMock.Setup(s => s.GetCart(newCartId)).Returns(Task.FromResult<CartDAO>(null));
            repoMock.Setup(s => s.ExistsItem(item.Id)).Returns(false);

            var cartingService = new CartingRepoService(repoMock.Object, _mapper);
            await Assert.ThrowsAsync<ArgumentException>(() => cartingService.AddItem(newCartId, item));
        }
        [Fact]
        public async Task RemoveItem_ExistingItem()
        {
            var newCartId = Guid.NewGuid();
            var repoMock = new Mock<ICartingRepository>();
            repoMock.Setup(s => s.RemoveItemFromCart(newCartId, It.IsAny<int>()));

            var cartingService = new CartingRepoService(repoMock.Object, _mapper);
            await cartingService.RemoveItem(newCartId, 1);
            repoMock.VerifyAll();
        }
        [Fact]
        public async Task RemoveItem_NonExistingItem()
        {
            var newCartId = Guid.NewGuid();
            var repoMock = new Mock<ICartingRepository>();
            repoMock.Setup(s => s.RemoveItemFromCart(newCartId, It.IsAny<int>()));

            var cartingService = new CartingRepoService(repoMock.Object, _mapper);
            await cartingService.RemoveItem(newCartId, 5);
            repoMock.VerifyAll();
        }
        [Fact]
        public async Task RemoveItem_NonExistingCart()
        {
            var newCartId = Guid.NewGuid();
            var repoMock = new Mock<ICartingRepository>();
            repoMock.Setup(s => s.RemoveItemFromCart(It.IsAny<Guid>(), It.IsAny<int>()));

            var cartingService = new CartingRepoService(repoMock.Object, _mapper);
            await cartingService.RemoveItem(newCartId, 5);
            repoMock.VerifyAll();
        }
        [Fact]
        public async Task ExistsItem()
        {
            var repoMock = new Mock<ICartingRepository>();
            repoMock.Setup(s => s.ExistsItem(It.IsAny<int>()));

            var cartingService = new CartingRepoService(repoMock.Object, _mapper);
            await cartingService.ExistsItem(1);
            repoMock.VerifyAll();
        }
        [Fact]
        public void ItemUpdated()
        {
            var repoMock = new Mock<ICartingRepository>();
            repoMock.Setup(s => s.ItemUpdated(It.IsAny<ItemDAO>()));
            var itm = new MessagingService.Contracts.Item();

            var cartingService = new CartingRepoService(repoMock.Object, _mapper);
            cartingService.ItemUpdated(itm);
            repoMock.VerifyAll();
        }
        [Fact]
        public void ItemDeleted()
        {
            var repoMock = new Mock<ICartingRepository>();
            repoMock.Setup(s => s.ItemDeleted(It.IsAny<int>()));

            var cartingService = new CartingRepoService(repoMock.Object, _mapper);
            cartingService.ItemDeleted(1);
            repoMock.VerifyAll();
        }

        private CartDAO GetDefaultCart(Guid id)
        {
            return GetCartWithItems(id, 2);
        }
        private CartDAO GetEmptyCart(Guid id)
        {
            return new CartDAO()
            {
                Id = id,
                Items = new List<CartItemDAO>()
            };
        }
        private CartDAO GetCartWithItems(Guid id, int numItems)
        {
            var cart = new CartDAO() { Id = id, Items = new List<CartItemDAO>() };
            for (int i = 1; i <= numItems; i++)
                cart.Items.Add(new CartItemDAO()
                {
                    Item = new ItemDAO() { Name = "Item" + i, Id = i, Image = null, Price = i * 10 },
                    Quantity = i
                });
            return cart;
        }
    }
}
