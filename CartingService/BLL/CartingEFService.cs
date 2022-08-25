using AutoMapper;
using CartingService.DAL;
using CartingService.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CartingService.BLL
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

        public async Task AddItem(Guid cartId, Item item)
        {
            var cartDAO = await _context.Carts.Include(c => c.Items)
                            .FirstOrDefaultAsync(c => c.Id == cartId);
            if (cartDAO == null)
            {
                var cart = await InitializeCart(cartId, item);
                cartDAO = _mapper.Map<CartDAO>(cart);
            }
            else if (item != null)
            {
                var existingItem = cartDAO.Items.Find(i => i.Item.Id == item.Id);
                if (existingItem != null)
                    existingItem.Quantity += item.Quantity;
                else if (await ExistsItem(item.Id))
                {
                    var itemToAdd = _mapper.Map<CartItemDAO>(item);
                    itemToAdd.Item = await GetItem(item.Id);
                    cartDAO.Items.Add(itemToAdd);
                }
                else
                {
                    throw new ArgumentException("Item doesn't exists.", nameof(Item));
                }
            }
            else
                throw new ArgumentException("Item can't be null.", nameof(Item));
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
        public async Task<bool> ExistsCart(Guid cartId)
        {
            return await _context.Carts.Where(c => c.Id == cartId).AnyAsync();
        }
        public async Task<bool> ExistsItemOnCart(Guid cartId, int itemId)
        {
            return await _context.Carts
                        .Where(c => c.Id == cartId &&
                                c.Items.Exists(i => i.Item.Id == itemId))
                        .AnyAsync();
        }
        public async Task<Cart> GetCart(Guid cartId)
        {
            var cartDAO = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.Id == cartId);
            if (cartDAO == null)
                return null;
            else
                return _mapper.Map<Cart>(cartDAO);

        }
        public async Task<Cart> InitializeCart(Guid cartId, Item? item)
        {
            var cartDAO = await _context.Carts.Include(c => c.Items)
                                .FirstOrDefaultAsync(c => c.Id == cartId);
            if (cartDAO == null)
            {
                cartDAO = new CartDAO() { Id = cartId };
                _context.Add(cartDAO);
            }
            if (item != null && await ExistsItem(item.Id))
            {
                var itemDAO = _mapper.Map<Item, CartItemDAO>(item);
                itemDAO.Item = await GetItem(item.Id);
                cartDAO.Items = new List<CartItemDAO>() { itemDAO };
            }
            else
                cartDAO.Items = new List<CartItemDAO>() { };
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
        public async Task RemoveItem(Guid cartId, int itemId)
        {
            var cartDAO = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.Id == cartId);
            if (cartDAO == null)
                return;
            var itemToRemove = cartDAO.Items.Find(i => i.Item.Id == itemId);
            if (itemToRemove != null)
            {
                _context.Remove(itemToRemove);
                await _context.SaveChangesAsync();
            }
        }
        public void ItemUpdated(MessagingService.Contracts.Item item)
        {
            var itemDAO = _context.Items.Find(item.Id);
            if (itemDAO == null)
            {
                var newItem = _mapper.Map<ItemDAO>(item);
                _context.Items.Add(newItem);
            }
            else
            {
                itemDAO.Name = item.Name;
                itemDAO.Image = item.Image;
                _context.Items.Update(itemDAO);
            }
            _context.SaveChanges();
        }
        public void ItemDeleted(int itemId)
        {
            var itemDAO = _context.Items.Find(itemId);
            if (itemDAO != null)
            {
                _context.Items.Remove(itemDAO);
            }
        }
        public async Task<bool> ExistsItem(int itemId)
        {
            var item = await _context.Items.FindAsync(itemId);
            return item != null;
        }

        private async Task<ItemDAO> GetItem(int itemId)
        {
            return await _context.Items.FindAsync(itemId);
        }
    }
}
