using System.ComponentModel.DataAnnotations;

namespace CatalogService.DAL
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
        public virtual int CategoryId { get; set; } 
        [Required]
        public virtual Category Category { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        [Required]
        [Range(1, double.MaxValue)]
        public int Amount { get; set; }
    }
}
