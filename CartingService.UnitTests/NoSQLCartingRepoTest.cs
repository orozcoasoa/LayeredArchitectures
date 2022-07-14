using CartingService.DAL.Entities;
using LiteDB;

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
            _db.DropCollection(NoSQLCartingRepository.cartitems);
            BsonMapper.Global.Entity<CartDAO>().Id(c => c.Id);
            BsonMapper.Global.Entity<ItemDAO>().Id(i => i.Id);
            BsonMapper.Global.Entity<CartItemDAO>().Id(c => c.Id, true);

            BsonMapper.Global.Entity<CartDAO>().DbRef(c => c.Items, NoSQLCartingRepository.cartitems);
            BsonMapper.Global.Entity<CartItemDAO>().DbRef(i => i.Cart, NoSQLCartingRepository.carts);
            BsonMapper.Global.Entity<CartItemDAO>().DbRef(i => i.Item, NoSQLCartingRepository.items);

            _existingCartId = Guid.NewGuid();
            var item1 = new ItemDAO() { Name = "Item1", Id = 1, Image = null, Price = 10};
            var item2 = new ItemDAO() { Name = "Item2", Id = 2, Image = null, Price = 20};

            var itemCart1 = new CartItemDAO
            {
                Item = item1,
                Quantity = 1
            };
            var itemCart2 = new CartItemDAO
            {
                Item = item2,
                Quantity = 2
            };
            var cart = new CartDAO()
            {
                Id = _existingCartId,
                Items = new List<CartItemDAO>()
                    {
                        itemCart1, itemCart2
                    }
            };
            var colItems = _db.GetCollection<ItemDAO>(NoSQLCartingRepository.items);
            colItems.Insert(item1);
            colItems.Insert(item2);
            itemCart1.Cart = cart;
            itemCart2.Cart = cart;
            var colCartItems = _db.GetCollection<CartItemDAO>(NoSQLCartingRepository.cartitems);
            colCartItems.Insert(itemCart1);
            colCartItems.Insert(itemCart2);
            var colCart = _db.GetCollection<CartDAO>(NoSQLCartingRepository.carts);
            colCart.Insert(cart);
        }

        [Fact]
        public async Task GetExistingCart()
        {
            var repo = new NoSQLCartingRepository(_db);
            var cart = await repo.GetCart(_existingCartId);
            Assert.Equal(_existingCartId, cart.Id);
            Assert.Equal(2, cart.Items.Count);
        }
        [Fact]
        public async Task GetNonExistingCart()
        {
            var repo = new NoSQLCartingRepository(_db);
            var newGuid = Guid.NewGuid();
            var cart = await repo.GetCart(newGuid);
            Assert.Null(cart);
        }
        [Fact]
        public async Task CreateCart_ExistingCart()
        {
            var repo = new NoSQLCartingRepository(_db);
            var cart = await repo.CreateCart(_existingCartId);
            Assert.Equal(_existingCartId, cart.Id);
            Assert.Equal(2, cart.Items.Count);
        }
        [Fact]
        public async Task CreateCart_NewCart()
        {
            var repo = new NoSQLCartingRepository(_db);
            var newGuid = Guid.NewGuid();
            var cart = await repo.CreateCart(newGuid);
            Assert.Equal(newGuid, cart.Id);
            Assert.Empty(cart.Items);
        }
        [Fact]
        public async Task AddItemToCart()
        {
            var repo = new NoSQLCartingRepository(_db);
            var item = new ItemDAO() { Id = 3, Name = "Item3", Image = null, Price = 30 };
            var cartItem = new CartItemDAO() { Id = 3, Item = item, Quantity = 3};
            await repo.AddItemToCart(_existingCartId, cartItem);

            var cart = await repo.GetCart(_existingCartId);
            Assert.Equal(_existingCartId, cart.Id);
            Assert.Equal(3, cart.Items.Count);
        }
        [Fact]
        public async Task AddItemToCart_NonExistingCart()
        {
            var repo = new NoSQLCartingRepository(_db);
            var item = new ItemDAO() { Id = 3, Name = "Item3", Image = null, Price = 30 };
            var cartItem = new CartItemDAO() { Item = item, Quantity = 3};
            var newGuid = Guid.NewGuid();
            await repo.AddItemToCart(newGuid, cartItem);

            var cart = await repo.GetCart(newGuid);
            Assert.Null(cart);
        }
        [Fact]
        public async Task AddItemToCart_AlreadyExistingItem()
        {
            var repo = new NoSQLCartingRepository(_db);
            var item = new ItemDAO() { Id = 2, Name = "Item2", Image = null, Price = 20 };
            var cartItem = new CartItemDAO { Item = item, Quantity = 2};
            await repo.AddItemToCart(_existingCartId, cartItem);

            var cart = await repo.GetCart(_existingCartId);
            Assert.Equal(_existingCartId, cart.Id);
            Assert.Equal(2, cart.Items.Count);
        }
        [Fact]
        public async Task RemoveItemFromCart()
        {
            var repo = new NoSQLCartingRepository(_db);
            await repo.RemoveItemFromCart(_existingCartId, 2);

            var cart = await repo.GetCart(_existingCartId);
            Assert.Equal(_existingCartId, cart.Id);
            Assert.Single(cart.Items);
        }
        [Fact]
        public async Task RemoveItemFromCart_NonExistantItem()
        {
            var repo = new NoSQLCartingRepository(_db);
            await repo.RemoveItemFromCart(_existingCartId, 3);

            var cart = await repo.GetCart(_existingCartId);
            Assert.Equal(_existingCartId, cart.Id);
            Assert.Equal(2, cart.Items.Count);
        }
        [Fact]
        public async Task RemoveItemFromCart_NonExistantCart()
        {
            var repo = new NoSQLCartingRepository(_db);
            var newGuid = Guid.NewGuid();
            await repo.RemoveItemFromCart(newGuid, 3);

            var cart = await repo.GetCart(_existingCartId);
            Assert.Equal(_existingCartId, cart.Id);
            Assert.Equal(2, cart.Items.Count);
        }
        [Fact]
        public async Task UpdateItemQuantity()
        {
            var repo = new NoSQLCartingRepository(_db);
            await repo.UpdateItemQuantity(_existingCartId, 1, 3);

            var cart = await repo.GetCart(_existingCartId);
            Assert.Equal(_existingCartId, cart.Id);
            Assert.Equal(2, cart.Items.Count);
            Assert.Equal(4, cart.Items.First(i => i.Item.Id == 1).Quantity);
        }
        [Fact]
        public async Task UpdateItemQuantity_NonExistantItem()
        {
            var repo = new NoSQLCartingRepository(_db);
            await repo.UpdateItemQuantity(_existingCartId, 3, 3);

            var cart = await repo.GetCart(_existingCartId);
            Assert.Equal(_existingCartId, cart.Id);
            Assert.Equal(2, cart.Items.Count);
            Assert.Equal(1, cart.Items.First(i => i.Item.Id == 1).Quantity);
        }
        [Fact]
        public async Task UpdateItemQuantity_NonExistantCart()
        {
            var repo = new NoSQLCartingRepository(_db);
            var newGuid = Guid.NewGuid();
            await repo.UpdateItemQuantity(newGuid, 1, 3);

            var cart = await repo.GetCart(_existingCartId);
            Assert.Equal(_existingCartId, cart.Id);
            Assert.Equal(2, cart.Items.Count);
            Assert.Equal(1, cart.Items.First(i => i.Item.Id == 1).Quantity);
        }
        [Fact]
        public void ItemUpdated_NewItem()
        {
            var itemToAdd = new ItemDAO() { Id = 3, Name = "Item3", Price = 30, Image = null };
            var repo = new NoSQLCartingRepository(_db);
            repo.ItemUpdated(itemToAdd);
            var col = _db.GetCollection<ItemDAO>(NoSQLCartingRepository.items);
            var numItems = col.Count();
            Assert.Equal(3, numItems);
            var itemAdded = col.Query().Where(i => i.Id == itemToAdd.Id).SingleOrDefault();
            Assert.Equal(itemToAdd.Name, itemAdded.Name);
            Assert.Equal(itemToAdd.Price, itemAdded.Price);
            Assert.Null(itemAdded.Image);
        }
        [Fact]
        public void ItemUpdated_ExistingItem()
        {
            var itemToUpdate = new ItemDAO() { Id = 2, Name = "Item3", Price = 30, Image = null };
            var repo = new NoSQLCartingRepository(_db);
            repo.ItemUpdated(itemToUpdate);
            var col = _db.GetCollection<ItemDAO>(NoSQLCartingRepository.items);
            var numItems = col.Count();
            Assert.Equal(2, numItems);
            var itemUpdated = col.Query().Where(i => i.Id == itemToUpdate.Id).SingleOrDefault();
            Assert.Equal(itemToUpdate.Name, itemUpdated.Name);
            Assert.Equal(itemToUpdate.Price, itemUpdated.Price);
            Assert.Null(itemUpdated.Image);
        }
        [Fact]
        public void ItemDeleted_ExistingItem()
        {
            var repo = new NoSQLCartingRepository(_db);
            repo.ItemDeleted(1);
            var col = _db.GetCollection<ItemDAO>(NoSQLCartingRepository.items);
            var numItems = col.Count();
            Assert.Equal(1, numItems);
        }
        [Fact]
        public void ItemDeleted_NonExistingItem()
        {
            var repo = new NoSQLCartingRepository(_db);
            repo.ItemDeleted(3);
            var col = _db.GetCollection<ItemDAO>(NoSQLCartingRepository.items);
            var numItems = col.Count();
            Assert.Equal(2, numItems);
        }
        [Fact]
        public void ExistsItem()
        {
            var repo = new NoSQLCartingRepository(_db);
            var exists = repo.ExistsItem(1);
            Assert.True(exists);
        }
        [Fact]
        public void ExistsItem_NonExistingItem()
        {
            var repo = new NoSQLCartingRepository(_db);
            var exists = repo.ExistsItem(5);
            Assert.False(exists);
        }
    }
}
