using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.BLL.Entities
{
    public class ItemDetails
    {
        public int ItemId { get; set; }
        public Dictionary<string, string> Details { get; set; } = new Dictionary<string, string>();
    }
}
