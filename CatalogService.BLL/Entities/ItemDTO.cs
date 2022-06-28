using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.BLL.Entities
{
    public class ItemDTO
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        //TODO can contain html.
        public string? Description { get; set; }
        //TODO URL.
        public string? Image { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        [Range(0, (double)decimal.MaxValue)]
        public decimal Price { get; set; }
        [Required]
        [Range (1, int.MaxValue)]
        public int Amount { get; set; }
    }
}
