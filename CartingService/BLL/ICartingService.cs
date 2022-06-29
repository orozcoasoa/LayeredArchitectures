using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartingService.BLL
{
    public interface ICartingService
    {
        Task<Cart> InitializeCart(Guid cartId, Item? item);
        Task AddItem(Guid cartId, Item item);
        Task RemoveItem(Guid cartId, int itemId);
        Task<IList<Item>> GetCartItems(Guid cartId);
    }
}
