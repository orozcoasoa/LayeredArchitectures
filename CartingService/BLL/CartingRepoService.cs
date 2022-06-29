using AutoMapper;
using CartingService.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                var cart = await InitializeCart(cartId, item);
            }
            else
            {
                var existingItem = cartDAO.Items.Find(i => i.Id == item.Id);
                if (existingItem != null)
                    await _repository.UpdateItemQuantity(cartId, existingItem.Id, existingItem.Quantity + item.Quantity);
                else
                    await _repository.AddItemToCart(cartId, _mapper.Map<ItemDAO>(item));

            }
        }
        public async Task<IList<Item>> GetCartItems(Guid cartId)
        {
            var cartDAO = await _repository.GetCart(cartId);
            if (cartDAO == null)
                return new List<Item>();
            else
                return _mapper.Map<List<ItemDAO>,List<Item>>(cartDAO.Items);

        }
        public async Task<Cart> InitializeCart(Guid cartId, Item? item)
        {
            var cartDAO = await _repository.GetCart(cartId);
            if(cartDAO == null)
            {
                cartDAO = await _repository.CreateCart(cartId);
            }
            if (item != null)
            {
                var itemDAO = _mapper.Map<Item, ItemDAO>(item);
                await _repository.AddItemToCart(cartId, itemDAO);
                cartDAO = await _repository.GetCart(cartId);
            }
            else
                cartDAO.Items = new List<ItemDAO>() { };
            
            return _mapper.Map<Cart>(cartDAO);
        }
        public async Task RemoveItem(Guid cartId, int itemId)
        {
            await _repository.RemoveItemFromCart(cartId, itemId);
        }
    }
}
