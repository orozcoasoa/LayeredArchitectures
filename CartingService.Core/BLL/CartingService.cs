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
    public class CartingService : ICartingService
    {
        private readonly CartingDbContext _context;
        private readonly IMapper _mapper;

        public CartingService(CartingDbContext context, IMapper mapper)
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
            await _context.SaveChangesAsync();
        }
        public async Task<IList<Item>> GetCartItemsAsync(Guid cartId)
        {
            var cartDAO = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.Id == cartId);
            if (cartDAO == null)
                return new List<Item>();
            else
                return _mapper.Map<List<ItemDAO>,List<Item>>(cartDAO.Items);

        }
        public async Task<Cart> InitializeCartAsync(Guid cartId, Item? item)
        {
            var cartDAO = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.Id == cartId);
            if(cartDAO == null)
                cartDAO = new CartDAO() { Id = cartId };
            if (item != null)
            {
                var itemDAO = _mapper.Map<Item, ItemDAO>(item);
                cartDAO.Items = new List<ItemDAO>() { itemDAO};
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
        public async Task RemoveItemAsync(Guid cartId, Item item)
        {
            var cartDAO = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.Id == cartId);
            if (cartDAO == null)
                return;
            cartDAO.Items.Remove(_mapper.Map<ItemDAO>(item));
            await _context.SaveChangesAsync();
        }
    }
}
