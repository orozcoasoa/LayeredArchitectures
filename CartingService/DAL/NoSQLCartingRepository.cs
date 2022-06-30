using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartingService.DAL
{
    public class NoSQLCartingRepository : ICartingRepository
    {
        private readonly ILiteDatabase _db;
        public const string carts = "Carts";
        public const string items = "Items";

        public NoSQLCartingRepository(ILiteDatabase db)
        {
            _db = db;
        }

        public async Task AddItemToCart(Guid id, ItemDAO item)
        {
            var cart = await GetCart(id);
            if (cart == null || cart.Items.Exists(i => i.Id == item.Id))
                return;

            cart.Items.Add(item);
            var col = _db.GetCollection<CartDAO>(carts);
            await Task.Run(() => col.Update(cart));
            var itemsCol = _db.GetCollection<ItemDAO>(items);
            item.Cart = cart;
            await Task.Run(() => itemsCol.Upsert(item));
        }
        public async Task<CartDAO> CreateCart(Guid id)
        {
            var cart = await GetCart(id);
            if (cart != null)
                return cart;

            var col = _db.GetCollection<CartDAO>(carts);
            var newCart = new CartDAO() { Id = id, Items = new List<ItemDAO>() };
            col.Insert(newCart);
            return newCart;
        }
        public async Task<CartDAO> GetCart(Guid id)
        {
            var col = _db.GetCollection<CartDAO>(carts);
            var result = await Task.Run(() => col.Query()
                                            .Include(c => c.Items)
                                            .Where(c => c.Id == id)
                                            .SingleOrDefault());
            return result;
        }
        public async Task RemoveItemFromCart(Guid id, int itemId)
        {
            var cart = await GetCart(id);
            if (cart == null || !cart.Items.Exists(i => i.Id == itemId))
                return;

            cart.Items.RemoveAll(i => i.Id == itemId);
            var col = _db.GetCollection<CartDAO>(carts);
            await Task.Run(() => col.Update(cart));
        }
        public async Task UpdateItemQuantity(Guid id, int itemId, double quantity)
        {
            var cart = await GetCart(id);
            if (cart == null)
                return;

            var item = cart.Items.FirstOrDefault(i => i.Id == itemId);
            if (item == null)
                return;
            item.Quantity += quantity;
            var col = _db.GetCollection<ItemDAO>(items);
            await Task.Run(() => col.Update(item));
        }
    }
}
