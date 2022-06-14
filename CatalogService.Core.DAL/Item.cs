using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.Core.DAL
{
    public class Item
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Max allowed characters:{0}")]
        public string Name { get; set; }
        //TODO can contain html.
        public string Description { get; set; }
        //TODO URL.
        public string Image { get; set; }
        public int CategoryId { get; set; }
        [Required]
        public Category Category { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Amount { get; set; }
    }
}
