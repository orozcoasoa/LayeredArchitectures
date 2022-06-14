using CartingService.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartingService.BLL
{
    public interface ICartingService
    {
        Task<Cart> InitializeCartAsync(Guid cartId, Item? item);
        Task AddItemAsync(Guid cartId, Item item);
        Task RemoveItemAsync(Guid cartId, Item item);
        Task<IList<Item>> GetCartItemsAsync(Guid cartId);
    }
}
