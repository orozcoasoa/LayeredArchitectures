using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.BLL.Entities
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //TODO can contain html.
        public string Description { get; set; }
        //TODO URL.
        public string Image { get; set; }
        public Category Category { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
    }
}
