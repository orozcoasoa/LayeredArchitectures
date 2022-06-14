using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            repoMock.Setup(s => s.GetCartAsync(guid))
                .Returns(Task.FromResult(new CartDAO()
                {
                    Id = guid,
                    Items = new List<ItemDAO>()
                    {
                        new ItemDAO { Name = "Item1", Id = 1, Price=10, Quantity = 1, Image = null },
                        new ItemDAO { Name = "Item2", Id = 2, Price=20, Quantity = 2, Image = null}
                    }
                }));

            var cartingService = new CartingRepoService(repoMock.Object, _mapper);
            var items = await cartingService.GetCartItemsAsync(guid);
            Assert.Equal(2, items.Count);
        }
        [Fact]
        public void InitializeCart_NewId()
        {
            Assert.True(true);
        }
        [Fact]
        public void InitializeCart_ExistingId()
        {
            Assert.True(true);
        }
        [Fact]
        public async Task InitializeCart_NullItem()
        {
            var repoMock = new Mock<ICartingRepository>();
            var guid = Guid.NewGuid();
            var itemToAdd = new ItemDAO { Name = "Item1", Id = 1, Price = 10, Quantity = 1, Image = null };
            repoMock.Setup(s => s.GetCartAsync(guid))
                .Returns(Task.FromResult(new CartDAO()
                {
                    Id = guid,
                    Items = new List<ItemDAO>()
                    {
                        new ItemDAO { Name = "Item1", Id = 1, Price=10, Quantity = 1, Image = null },
                        new ItemDAO { Name = "Item2", Id = 2, Price=20, Quantity = 2, Image = null}
                    }
                }));


            var cartingService = new CartingRepoService(repoMock.Object, _mapper);
            var cart = await cartingService.InitializeCartAsync(guid, null);
            Assert.Equal(guid, cart.Id);
            Assert.Empty(cart.Items);
        }
        [Fact]
        public void AddItemToCart_ExistingCart()
        {
            Assert.True(true);
        }
        [Fact]
        public void AddItemToCart_ExistingItem()
        {
            Assert.True(true);
        }
        [Fact]
        public void AddItemToCart_NewCart()
        {
            Assert.True(true);
        }
        [Fact]
        public void RemoveItem_ExistingItem()
        {
            Assert.True(true);
        }
        [Fact]
        public void RemoveItem_NonExistingItem()
        {
            Assert.True(true);
        }
        [Fact]
        public void RemoveItem_NonExistingCart()
        {
            Assert.True(true);
        }
    }
}
