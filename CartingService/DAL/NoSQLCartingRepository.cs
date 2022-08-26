using CartingService.DAL.Entities;
using LiteDB;

namespace CartingService.DAL
{
    public class NoSQLCartingRepository : ICartingRepository
    {
        private readonly ILiteDatabase _db;
        public const string carts = "Carts";
        public const string items = "Items";
        public const string cartitems = "CartItems";

        public NoSQLCartingRepository(ILiteDatabase db)
        {
            _db = db;
        }

        public async Task AddItemToCart(Guid id, CartItemDAO item)
        {
            var cart = await GetCart(id);
            if (cart == null || cart.Items.Exists(i => i.Item.Id == item.Item.Id))
                return;

            var itemsCol = _db.GetCollection<CartItemDAO>(cartitems);
            item.Cart = cart;
            await Task.Run(() => itemsCol.Upsert(item));

            item = itemsCol.Query()
                .Where(i => i.Item.Id == item.Item.Id && i.Cart.Id == id)
                .FirstOrDefault();
            cart.Items.Add(item);
            var col = _db.GetCollection<CartDAO>(carts);
            await Task.Run(() => col.Update(cart));
        }
        public async Task<CartDAO> CreateCart(Guid id)
        {
            var cart = await GetCart(id);
            if (cart != null)
                return cart;

            var col = _db.GetCollection<CartDAO>(carts);
            var newCart = new CartDAO() { Id = id, Items = new List<CartItemDAO>() };
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
        public async Task<bool> ExistsCart(Guid id)
        {
            var cart = await GetCart(id);
            return cart != null;
        }
        public async Task RemoveItemFromCart(Guid id, int itemId)
        {
            var cart = await GetCart(id);
            if (cart == null || !cart.Items.Exists(i => i.Item.Id == itemId))
                return;

            cart.Items.RemoveAll(i => i.Item.Id == itemId);
            var col = _db.GetCollection<CartDAO>(carts);
            await Task.Run(() => col.Update(cart));
        }
        public async Task UpdateItemQuantity(Guid id, int itemId, double quantity)
        {
            var cart = await GetCart(id);
            if (cart == null)
                return;

            var item = cart.Items.FirstOrDefault(i => i.Item.Id == itemId);
            if (item == null)
                return;
            item.Quantity += quantity;
            var col = _db.GetCollection<CartItemDAO>(cartitems);
            await Task.Run(() => col.Update(item));
        }
        public void ItemUpdated(ItemDAO item)
        {
            var col = _db.GetCollection<ItemDAO>(items);
            var itm = col.Query().Where(i => i.Id == item.Id).SingleOrDefault();
            if (itm == null)
            {
                col.Insert(item);
            }
            else
            {
                itm.Name = item.Name;
                itm.Price = item.Price;
                itm.Image = item.Image;
                col.Update(item);
            }
        }
        public void ItemDeleted(int itemId)
        {
            var col = _db.GetCollection<ItemDAO>(items);
            col.Delete(itemId);
        }
        public bool ExistsItem(int itemId)
        {
            var col = _db.GetCollection<ItemDAO>(items);
            var itm = col.Query().Where(i => i.Id == itemId).SingleOrDefault();
            return itm != null;
        }
    }
}
