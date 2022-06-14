using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartingService.UnitTests
{
    public class NoSQLCartingRepoTest
    {
        private readonly ILiteDatabase _db;
        private readonly Guid _existingCartId;
        private static int _count = 0;

        public NoSQLCartingRepoTest()
        {
            var myCount = _count++;
            _db = new LiteDatabase("test" + myCount + ".db");
            _db.DropCollection(NoSQLCartingRepository.carts);
            _db.DropCollection(NoSQLCartingRepository.items);
            BsonMapper.Global.Entity<CartDAO>().Id(c => c.Id);
            BsonMapper.Global.Entity<ItemDAO>().Id(i => i.Id);

            BsonMapper.Global.Entity<CartDAO>().DbRef(c => c.Items, NoSQLCartingRepository.items);
            BsonMapper.Global.Entity<ItemDAO>().DbRef(i => i.Cart, NoSQLCartingRepository.carts);

            _existingCartId = Guid.NewGuid();
            var item1 = new ItemDAO { Name = "Item1", Id = 1, Price = 10, Quantity = 1, Image = null };
            var item2 = new ItemDAO { Name = "Item2", Id = 2, Price = 20, Quantity = 2, Image = null };
            var cart = new CartDAO()
            {
                Id = _existingCartId,
                Items = new List<ItemDAO>()
                    {
                        item1, item2
                    }
            };
            var col = _db.GetCollection<CartDAO>(NoSQLCartingRepository.carts);
            col.Insert(cart);
            item1.Cart = cart;
            item2.Cart = cart;
            var colItems = _db.GetCollection<ItemDAO>(NoSQLCartingRepository.items);
            colItems.Insert(item1);
            colItems.Insert(item2);
        }

        [Fact]
        public async Task GetExistingCart()
        {
            var repo = new NoSQLCartingRepository(_db);
            var cart = await repo.GetCartAsync(_existingCartId);
            Assert.Equal(_existingCartId, cart.Id);
            Assert.Equal(2, cart.Items.Count);
        }
        [Fact]
        public async Task GetNonExistingCart()
        {
            var repo = new NoSQLCartingRepository(_db);
            var newGuid = Guid.NewGuid();
            var cart = await repo.GetCartAsync(newGuid);
            Assert.Null(cart);
        }
        [Fact]
        public async Task CreateCart_ExistingCart()
        {
            var repo = new NoSQLCartingRepository(_db);
            var cart = await repo.CreateCartAsync(_existingCartId);
            Assert.Equal(_existingCartId, cart.Id);
            Assert.Equal(2, cart.Items.Count);
        }
        [Fact]
        public async Task CreateCart_NewCart()
        {
            var repo = new NoSQLCartingRepository(_db);
            var newGuid = Guid.NewGuid();
            var cart = await repo.CreateCartAsync(newGuid);
            Assert.Equal(newGuid, cart.Id);
            Assert.Empty(cart.Items);
        }
        [Fact]
        public async Task AddItemToCart()
        {
            var repo = new NoSQLCartingRepository(_db);
            var item = new ItemDAO() { Id = 3, Name = "Item3", Quantity = 3, Price = 30, Image = null };
            await repo.AddItemToCartAsync(_existingCartId, item);

            var cart = await repo.GetCartAsync(_existingCartId);
            Assert.Equal(_existingCartId, cart.Id);
            Assert.Equal(3, cart.Items.Count);
        }
        [Fact]
        public async Task AddItemToCart_NonExistingCart()
        {
            var repo = new NoSQLCartingRepository(_db);
            var item = new ItemDAO() { Id = 3, Name = "Item3", Quantity = 3, Price = 30, Image = null };
            var newGuid = Guid.NewGuid();
            await repo.AddItemToCartAsync(newGuid, item);

            var cart = await repo.GetCartAsync(newGuid);
            Assert.Null(cart);
        }
        [Fact]
        public async Task AddItemToCart_AlreadyExistingItem()
        {
            var repo = new NoSQLCartingRepository(_db);
            var item = new ItemDAO { Name = "Item2", Id = 2, Price = 20, Quantity = 2, Image = null };
            await repo.AddItemToCartAsync(_existingCartId, item);

            var cart = await repo.GetCartAsync(_existingCartId);
            Assert.Equal(_existingCartId, cart.Id);
            Assert.Equal(2, cart.Items.Count);
        }
        [Fact]
        public async Task RemoveItemFromCart()
        {
            var repo = new NoSQLCartingRepository(_db);
            await repo.RemoveItemFromCartAsync(_existingCartId, 2);

            var cart = await repo.GetCartAsync(_existingCartId);
            Assert.Equal(_existingCartId, cart.Id);
            Assert.Single(cart.Items);
        }
        [Fact]
        public async Task RemoveItemFromCart_NonExistantItem()
        {
            var repo = new NoSQLCartingRepository(_db);
            await repo.RemoveItemFromCartAsync(_existingCartId, 3);

            var cart = await repo.GetCartAsync(_existingCartId);
            Assert.Equal(_existingCartId, cart.Id);
            Assert.Equal(2, cart.Items.Count);
        }
        [Fact]
        public async Task RemoveItemFromCart_NonExistantCart()
        {
            var repo = new NoSQLCartingRepository(_db);
            var newGuid = Guid.NewGuid();
            await repo.RemoveItemFromCartAsync(newGuid, 3);

            var cart = await repo.GetCartAsync(_existingCartId);
            Assert.Equal(_existingCartId, cart.Id);
            Assert.Equal(2, cart.Items.Count);
        }
        [Fact]
        public async Task UpdateItemQuantity()
        {
            var repo = new NoSQLCartingRepository(_db);
            await repo.UpdateItemQuantityAsync(_existingCartId, 1, 3);

            var cart = await repo.GetCartAsync(_existingCartId);
            Assert.Equal(_existingCartId, cart.Id);
            Assert.Equal(2, cart.Items.Count);
            Assert.Equal(4, cart.Items.First(i => i.Id == 1).Quantity);
        }
        [Fact]
        public async Task UpdateItemQuantity_NonExistantItem()
        {
            var repo = new NoSQLCartingRepository(_db);
            await repo.UpdateItemQuantityAsync(_existingCartId, 3, 3);

            var cart = await repo.GetCartAsync(_existingCartId);
            Assert.Equal(_existingCartId, cart.Id);
            Assert.Equal(2, cart.Items.Count);
            Assert.Equal(1, cart.Items.First(i => i.Id == 1).Quantity);
        }
        [Fact]
        public async Task UpdateItemQuantity_NonExistantCart()
        {
            var repo = new NoSQLCartingRepository(_db);
            var newGuid = Guid.NewGuid();
            await repo.UpdateItemQuantityAsync(newGuid, 1, 3);

            var cart = await repo.GetCartAsync(_existingCartId);
            Assert.Equal(_existingCartId, cart.Id);
            Assert.Equal(2, cart.Items.Count);
            Assert.Equal(1, cart.Items.First(i => i.Id == 1).Quantity);
        }

    }
}
