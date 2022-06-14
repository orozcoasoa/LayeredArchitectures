using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartingService.DAL
{
    public class ItemDAO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public byte[]? Image { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public double Quantity { get; set; }
        //public Guid CartId { get; set; }
        //public CartDAO Cart { get; set; }
    }
}
