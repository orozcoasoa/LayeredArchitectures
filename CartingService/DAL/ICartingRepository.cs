using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartingService.DAL
{
    public interface ICartingRepository
    {
        Task<CartDAO> CreateCart(Guid id);
        Task<CartDAO> GetCart(Guid id);
        Task AddItemToCart(Guid id, ItemDAO item);
        Task RemoveItemFromCart(Guid id, int itemId);
        Task UpdateItemQuantity(Guid id, int itemId, double quantity);
    }
}
