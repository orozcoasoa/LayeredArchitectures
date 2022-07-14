using System.ComponentModel.DataAnnotations;

namespace CartingService.BLL
{
    public class Item
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Image { get; set; }
        [Required]
        public decimal Price { get; set; }
        public double Quantity { get; set; }
    }
}
