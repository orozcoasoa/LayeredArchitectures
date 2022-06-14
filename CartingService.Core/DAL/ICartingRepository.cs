using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartingService.Core.DAL
{
    public interface ICartingRepository
    {
        Task<CartDAO> CreateCartAsync(Guid id);
        Task<CartDAO> GetCartAsync(Guid id);
        Task AddItemToCartAsync(Guid id, ItemDAO item);
        Task RemoveItemFromCartAsync(Guid id, int itemId);
        Task UpdateItemQuantityAsync(Guid id, int itemId, double quantity);
    }
}
