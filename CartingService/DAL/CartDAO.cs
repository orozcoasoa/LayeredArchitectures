using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartingService.DAL
{
    public class CartDAO
    {
        public Guid Id { get; set; }
        public List<ItemDAO> Items { get; set; }
    }
}
