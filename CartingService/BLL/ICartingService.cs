namespace CartingService.BLL
{
    public interface ICartingService
    {
        Task<Cart> InitializeCart(Guid cartId, Item? item);
        Task AddItem(Guid cartId, Item item);
        Task RemoveItem(Guid cartId, int itemId);
        Task<Cart> GetCart(Guid cartId);
        Task<bool> ExistsCart(Guid cartId);
        Task<bool> ExistsItemOnCart(Guid cartId, int itemId);
        Task<bool> ExistsItem(int itemId);
        void ItemUpdated(MessagingService.Contracts.Item item);
        void ItemDeleted(int itemId);
    }
}
