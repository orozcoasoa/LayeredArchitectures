using AutoMapper;
using CartingService.Core.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartingService.Core.BLL
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

        public async Task AddItemAsync(Guid cartId, Item item)
        {
            var cartDAO = await _repository.GetCartAsync(cartId);
            if (cartDAO == null)
            {
                var cart = await InitializeCartAsync(cartId, item);
            }
            else
            {
                var existingItem = cartDAO.Items.Find(i => i.Id == item.Id);
                if (existingItem != null)
                    await _repository.UpdateItemQuantityAsync(cartId, existingItem.Id, existingItem.Quantity + item.Quantity);
                else
                    await _repository.AddItemToCartAsync(cartId, _mapper.Map<ItemDAO>(item));

            }
        }
        public async Task<IList<Item>> GetCartItemsAsync(Guid cartId)
        {
            var cartDAO = await _repository.GetCartAsync(cartId);
            if (cartDAO == null)
                return new List<Item>();
            else
                return _mapper.Map<List<ItemDAO>,List<Item>>(cartDAO.Items);

        }
        public async Task<Cart> InitializeCartAsync(Guid cartId, Item? item)
        {
            var cartDAO = await _repository.GetCartAsync(cartId);
            if(cartDAO == null)
            {
                cartDAO = await _repository.CreateCartAsync(cartId);
            }
            if (item != null)
            {
                var itemDAO = _mapper.Map<Item, ItemDAO>(item);
                await _repository.AddItemToCartAsync(cartId, itemDAO);
                cartDAO = await _repository.GetCartAsync(cartId);
            }
            else
                cartDAO.Items = new List<ItemDAO>() { };
            
            return _mapper.Map<Cart>(cartDAO);
        }
        public async Task RemoveItemAsync(Guid cartId, int itemId)
        {
            await _repository.RemoveItemFromCartAsync(cartId, itemId);
        }
    }
}
