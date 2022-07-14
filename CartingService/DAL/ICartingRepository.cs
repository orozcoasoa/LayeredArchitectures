using CartingService.DAL.Entities;

namespace CartingService.DAL
{
    public interface ICartingRepository
    {
        Task<CartDAO> CreateCart(Guid id);
        Task<CartDAO> GetCart(Guid id);
        Task<bool> ExistsCart(Guid id);
        Task AddItemToCart(Guid id, CartItemDAO item);
        Task RemoveItemFromCart(Guid id, int itemId);
        Task UpdateItemQuantity(Guid id, int itemId, double quantity);
        void ItemUpdated(ItemDAO item);
        void ItemDeleted(int itemId);
        bool ExistsItem(int itemId);
    }
}
