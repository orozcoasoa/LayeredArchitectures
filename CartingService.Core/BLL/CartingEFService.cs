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
    public class CartingEFService : ICartingService
    {
        private readonly CartingDbContext _context;
        private readonly IMapper _mapper;

        public CartingEFService(CartingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddItemAsync(Guid cartId, Item item)
        {
            var cartDAO = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.Id == cartId);
            if (cartDAO == null)
            {
                var cart = await InitializeCartAsync(cartId, item);
                cartDAO = _mapper.Map<CartDAO>(cart);
            }
            else
            {
                var existingItem = cartDAO.Items.Find(i => i.Id == item.Id);
                if (existingItem != null)
                    existingItem.Quantity += item.Quantity;
                else
                    cartDAO.Items.Add(_mapper.Map<ItemDAO>(item));

            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                entry.OriginalValues.SetValues(entry.CurrentValues);
            }
        }
        public async Task<IList<Item>> GetCartItemsAsync(Guid cartId)
        {
            var cartDAO = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.Id == cartId);
            if (cartDAO == null)
                return new List<Item>();
            else
                return _mapper.Map<List<ItemDAO>, List<Item>>(cartDAO.Items);

        }
        public async Task<Cart> InitializeCartAsync(Guid cartId, Item? item)
        {
            var cartDAO = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.Id == cartId);
            if (cartDAO == null)
            {
                cartDAO = new CartDAO() { Id = cartId };
                _context.Add(cartDAO);
            }
            if (item != null)
            {
                var itemDAO = _mapper.Map<Item, ItemDAO>(item);
                cartDAO.Items = new List<ItemDAO>() { itemDAO };
            }
            else
                cartDAO.Items = new List<ItemDAO>() { };
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                entry.OriginalValues.SetValues(entry.CurrentValues);
            }
            return _mapper.Map<Cart>(cartDAO);
        }
        public async Task RemoveItemAsync(Guid cartId, int itemId)
        {
            var cartDAO = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.Id == cartId);
            if (cartDAO == null)
                return;
            var itemToRemove = cartDAO.Items.Find(i => i.Id == itemId);
            if (itemToRemove != null)
            {
                _context.Remove(itemToRemove);
                await _context.SaveChangesAsync();
            }
        }
    }
}
