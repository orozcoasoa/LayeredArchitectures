using AutoMapper;
using CartingService.DAL;
using CartingService.DAL.Entities;

namespace CartingService.BLL
{
    public class CartingRepoService : ICartingService
    {
        private readonly ICartingRepository _repository;
        private readonly IMapper _mapper;

        public CartingRepoService(ICartingRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task AddItem(Guid cartId, Item item)
        {
            var cartDAO = await _repository.GetCart(cartId);
            if (cartDAO == null)
            {
                if (_repository.ExistsItem(item.Id))
                    await InitializeCart(cartId, item);
                else
                    throw new ArgumentException("Item doesn't exists.", nameof(Item));
            }
            else if (item != null)
            {
                var existingItem = cartDAO.Items.Find(i => i.Item.Id == item.Id);
                if (existingItem != null)
                    await _repository.UpdateItemQuantity(cartId, existingItem.Item.Id, existingItem.Quantity + item.Quantity);
                else if (_repository.ExistsItem(item.Id))
                    await _repository.AddItemToCart(cartId, _mapper.Map<CartItemDAO>(item));
                else
                    throw new ArgumentException("Item doesn't exists.", nameof(Item));

            }
            else
                throw new ArgumentException("Item can't be null.", nameof(Item));
        }
        public async Task<bool> ExistsCart(Guid cartId)
        {
            var cart = await _repository.GetCart(cartId);
            return cart != null;
        }
        public async Task<bool> ExistsItemOnCart(Guid cartId, int itemId)
        {
            var cart = await _repository.GetCart(cartId);
            if (cart == null)
                return false;
            return cart.Items.Exists(i => i.Item.Id == itemId);
        }
        public async Task<Cart> GetCart(Guid cartId)
        {
            var cartDAO = await _repository.GetCart(cartId);
            if (cartDAO == null)
                return null;
            else
                return _mapper.Map<Cart>(cartDAO);

        }
        public async Task<Cart> InitializeCart(Guid cartId, Item? item)
        {
            CartDAO cartDAO;
            if (await _repository.ExistsCart(cartId))
                cartDAO = await _repository.GetCart(cartId);
            else
                cartDAO = await _repository.CreateCart(cartId);

            if (item != null)
            {
                var itemDAO = _mapper.Map<Item, CartItemDAO>(item);
                await _repository.AddItemToCart(cartId, itemDAO);
                cartDAO = await _repository.GetCart(cartId);
            }
            else
                cartDAO.Items = new List<CartItemDAO>() { };

            return _mapper.Map<Cart>(cartDAO);
        }
        public async Task RemoveItem(Guid cartId, int itemId)
        {
            await _repository.RemoveItemFromCart(cartId, itemId);
        }
        public async Task<bool> ExistsItem(int itemId)
        {
            return await Task.Run(() => _repository.ExistsItem(itemId));
        }
        public void ItemUpdated(MessagingService.Contracts.Item item)
        {
            var itemDAO = _mapper.Map<ItemDAO>(item);
            _repository.ItemUpdated(itemDAO);

        }
        public void ItemDeleted(int itemId)
        {
            _repository.ItemDeleted(itemId);
        }
    }
}
