using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartingService.Core.BLL
{
    public class Cart
    {
        public Guid Id { get; set; }
        public List<Item> Items { get; set; }
    }
}
