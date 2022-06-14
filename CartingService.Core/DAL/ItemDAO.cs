using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartingService.Core.DAL
{
    public class ItemDAO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[]? Image { get; set; }
        public decimal Price { get; set; }
        public double Quantity { get; set; }
        public CartDAO Cart { get; set; }
    }
}
